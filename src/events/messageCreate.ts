import { Events, Message, EmbedBuilder, TextChannel } from 'discord.js';
import { BOT_CONFIG, THRESHOLDS } from '../config/config.js';
import { whitelistManager, SUPREME_OWNERS } from '../config/whitelist.js';
import { watchlistService } from '../services/WatchlistService.js';
import { Logger } from '../utils/logger.js';

const DISCORD_INVITE_REGEX = /(https?:\/\/)?(www\.)?(discord\.(gg|io|me|li)|discord(app)?\.com\/invite)\/[a-zA-Z0-9-]+/gi;
const PHISHING_REGEX = /(discoord|dlscord|discrod|discrd|nitro-gift|gift-nitro|free-nitro|steamcommunlty|steamcommunity-trade|discord-app\.net|discord-nitro|claim-nitro)/i;

// Track user violation counts (pre-seeded with beast_.here who has 1 violation)
const userViolations = new Map<string, number>();
userViolations.set('1492308506433290402', 1);

// Sliding window velocity tracker for @everyone / @here pings (5-second window)
const everyonePingTimestamps = new Map<string, number[]>();

export default {
  name: Events.MessageCreate,
  async execute(message: Message) {
    // Ignore bots and DM messages
    if (message.author.bot || !message.guild || !message.member) return;

    const guild = message.guild;
    const authorId = message.author.id;

    // Safety checks: Supreme Owners (Mian & Hashir), Server owner, bot, and whitelisted users are immune
    if (SUPREME_OWNERS.has(authorId) || authorId === guild.ownerId || authorId === guild.client.user?.id) return;
    if (whitelistManager.isWhitelisted(guild.id, authorId, guild.ownerId, guild.client.user?.id)) return;

    const isWatchlisted = watchlistService.isWatchlisted(authorId);
    const totalMentions = message.mentions.users.size + message.mentions.roles.size;
    const hasInvite = DISCORD_INVITE_REGEX.test(message.content);
    const hasPhishing = PHISHING_REGEX.test(message.content);

    // Cooldown Rate Limiting: Only flag if @everyone / @here is mentioned MORE THAN 3 times in 5 seconds
    // WATCHLIST EXCEPTION: Watchlisted suspects have ZERO tolerance (1 single ping = instant violation)
    let hasEveryoneSpam = false;
    if (message.mentions.everyone) {
      if (isWatchlisted) {
        hasEveryoneSpam = true;
      } else {
        const now = Date.now();
        const existingTimestamps = everyonePingTimestamps.get(authorId) || [];
        const recentPings = existingTimestamps.filter(t => now - t <= 5000);
        recentPings.push(now);
        everyonePingTimestamps.set(authorId, recentPings);

        if (recentPings.length > 3) {
          hasEveryoneSpam = true;
        }
      }
    }

    const isViolation = totalMentions > THRESHOLDS.maxMentionsPerMessage || hasInvite || hasPhishing || hasEveryoneSpam;

    if (isViolation) {
      let violationReason = '';
      if (hasPhishing) {
        violationReason = 'Malicious Phishing / Fake Nitro Scam Link';
      } else if (totalMentions > THRESHOLDS.maxMentionsPerMessage) {
        violationReason = `Mass Mention Attack (${totalMentions} mentions)`;
      } else if (hasInvite) {
        violationReason = 'Unauthorized Discord Invite Link Nuker';
      } else if (hasEveryoneSpam) {
        violationReason = isWatchlisted
          ? 'Watchlisted High-Risk Suspect Unauthorized @everyone / @here Ping'
          : 'Mass @everyone / @here Spam (>3 pings in 5s)';
      }

      // Track repeat offenses & update watchlist
      if (isWatchlisted) {
        watchlistService.recordStrike(authorId, violationReason);
      }
      const prevStrikes = userViolations.get(authorId) || (isWatchlisted ? 1 : 0);
      const currentStrikes = prevStrikes + 1;
      userViolations.set(authorId, currentStrikes);

      const isRepeatOffender = currentStrikes > 1 || isWatchlisted;
      const timeoutDuration = isRepeatOffender
        ? 7 * 24 * 60 * 60 * 1000 // 7 Days in milliseconds
        : 60 * 60 * 1000;          // 1 Hour in milliseconds

      const actionTitle = isWatchlisted ? '7-DAY WATCHLIST MAXIMUM TIMEOUT' : (isRepeatOffender ? '7-DAY ESCALATED TIMEOUT' : '1-Hour Timeout');
      const auditReason = isWatchlisted
        ? `[MIAN XITERS] WATCHLIST ZERO-TOLERANCE: ${violationReason} - 7-DAY TIMEOUT`
        : isRepeatOffender
        ? `[MIAN XITERS] Auto-Defense: REPEAT OFFENDER (${violationReason}) - 7-DAY TIMEOUT`
        : `[MIAN XITERS] Auto-Defense: ${violationReason}`;

      // 1. Delete message immediately
      if (message.deletable) {
        await message.delete().catch(() => {});
      }

      // 2. Apply Timeout (1 Hour or 7 Days for repeat/watchlist)
      if (message.member.moderatable) {
        try {
          await message.member.timeout(timeoutDuration, auditReason);
          Logger.mitigation(
            isWatchlisted ? 'WATCHLIST ZERO-TOLERANCE TIMEOUT' : (isRepeatOffender ? 'REPEAT NUKER 7-DAY TIMEOUT' : 'MESSAGE NUKER TIMEOUT'),
            message.author.tag,
            0
          );
        } catch (err: unknown) {
          const msg = err instanceof Error ? err.message : String(err);
          Logger.error(`[TIMEOUT_FAILED] Could not timeout ${message.author.tag}: ${msg}`);
        }
      }

      // 3. Send warning & detailed incident report exclusively in private audit channel (#mian-xiters-logs)
      const logChannelId = whitelistManager.getLogChannel(guild.id);
      if (logChannelId) {
        const logChannel = guild.channels.cache.get(logChannelId) as TextChannel;
        if (logChannel && logChannel.isTextBased()) {
          const embed = new EmbedBuilder()
            .setColor(BOT_CONFIG.colors.danger)
            .setTitle(
              isWatchlisted
                ? '👁️ MIAN XITERS — WATCHLIST INTERCEPTION'
                : isRepeatOffender
                ? '🚨 MIAN XITERS — REPEAT NUKER ESCALATION'
                : '🚨 MIAN XITERS — SPAM/NUKER MITIGATION'
            )
            .setDescription(`A message containing malicious content was suppressed.\n**Author:** <@${authorId}> (\`${authorId}\`)`)
            .addFields(
              { name: '⚠️ Violation', value: violationReason, inline: true },
              { name: '🛡️ Action Taken', value: `Message Purged + ${actionTitle}`, inline: true },
              { name: '💬 Channel', value: `<#${message.channelId}>`, inline: true },
              { name: '⚡ Strikes', value: `${currentStrikes} offense(s)`, inline: true }
            )
            .setFooter({ text: `${BOT_CONFIG.name} Shield • Dev by ${BOT_CONFIG.author}` })
            .setTimestamp();

          await logChannel.send({ embeds: [embed] }).catch(() => {});
        }
      }
    }
  }
};

