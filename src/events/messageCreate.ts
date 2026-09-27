import { Events, Message, EmbedBuilder, TextChannel } from 'discord.js';
import { BOT_CONFIG, THRESHOLDS } from '../config/config.js';
import { whitelistManager } from '../config/whitelist.js';
import { Logger } from '../utils/logger.js';

const DISCORD_INVITE_REGEX = /(https?:\/\/)?(www\.)?(discord\.(gg|io|me|li)|discord(app)?\.com\/invite)\/[a-zA-Z0-9-]+/gi;
const PHISHING_REGEX = /(discoord|dlscord|discrod|discrd|nitro-gift|gift-nitro|free-nitro|steamcommunlty|steamcommunity-trade|discord-app\.net|discord-nitro|claim-nitro)/i;

// Track user violation counts (pre-seeded with beast_.here who has 1 violation)
const userViolations = new Map<string, number>();
userViolations.set('1492308506433290402', 1);

export default {
  name: Events.MessageCreate,
  async execute(message: Message) {
    // Ignore bots and DM messages
    if (message.author.bot || !message.guild || !message.member) return;

    const guild = message.guild;
    const authorId = message.author.id;

    // Safety checks: Server owner, bot, and whitelisted users are immune
    if (authorId === guild.ownerId || authorId === guild.client.user?.id) return;
    if (whitelistManager.isWhitelisted(guild.id, authorId, guild.ownerId, guild.client.user?.id)) return;

    const totalMentions = message.mentions.users.size + message.mentions.roles.size;
    const hasInvite = DISCORD_INVITE_REGEX.test(message.content);
    const hasPhishing = PHISHING_REGEX.test(message.content);
    const hasEveryoneMention = message.mentions.everyone;

    const isViolation = totalMentions > THRESHOLDS.maxMentionsPerMessage || hasInvite || hasPhishing || hasEveryoneMention;

    if (isViolation) {
      let violationReason = '';
      if (hasPhishing) {
        violationReason = 'Malicious Phishing / Fake Nitro Scam Link';
      } else if (totalMentions > THRESHOLDS.maxMentionsPerMessage) {
        violationReason = `Mass Mention Attack (${totalMentions} mentions)`;
      } else if (hasInvite) {
        violationReason = 'Unauthorized Discord Invite Link Nuker';
      } else if (hasEveryoneMention) {
        violationReason = 'Unauthorized @everyone / @here Ping';
      }

      // Track repeat offenses
      const prevStrikes = userViolations.get(authorId) || 0;
      const currentStrikes = prevStrikes + 1;
      userViolations.set(authorId, currentStrikes);

      const isRepeatOffender = currentStrikes > 1;
      const timeoutDuration = isRepeatOffender
        ? 7 * 24 * 60 * 60 * 1000 // 7 Days in milliseconds
        : 60 * 60 * 1000;          // 1 Hour in milliseconds

      const actionTitle = isRepeatOffender ? '7-DAY ESCALATED TIMEOUT' : '1-Hour Timeout';
      const auditReason = isRepeatOffender
        ? `[MIAN XITERS] Auto-Defense: REPEAT OFFENDER (${violationReason}) - 7-DAY TIMEOUT`
        : `[MIAN XITERS] Auto-Defense: ${violationReason}`;

      // 1. Delete message immediately
      if (message.deletable) {
        await message.delete().catch(() => {});
      }

      // 2. Apply Timeout (1 Hour or 7 Days for repeat)
      if (message.member.moderatable) {
        try {
          await message.member.timeout(timeoutDuration, auditReason);
          Logger.mitigation(
            isRepeatOffender ? 'REPEAT NUKER 7-DAY TIMEOUT' : 'MESSAGE NUKER TIMEOUT',
            message.author.tag,
            0
          );
        } catch (err: unknown) {
          const msg = err instanceof Error ? err.message : String(err);
          Logger.error(`[TIMEOUT_FAILED] Could not timeout ${message.author.tag}: ${msg}`);
        }
      }

      // 3. Send immediate warning in the channel where the attempt happened
      if (message.channel && 'send' in message.channel) {
        const warningMsg = isRepeatOffender
          ? `🚨 <@${authorId}> **TRIED IT AGAIN.** You were warned that MIAN's detection system is GOATED. Enjoy your **7-DAY TIMEOUT**.`
          : `⚠️ <@${authorId}> received a **1-Hour Security Timeout** for \`${violationReason}\`. Do that again and MIAN XITERS will give you an automatic **7-DAY TIMEOUT**.`;
        
        await (message.channel as TextChannel).send(warningMsg).catch(() => {});
      }

      // 4. Send warning & log in private audit channel
      const logChannelId = whitelistManager.getLogChannel(guild.id);
      if (logChannelId) {
        const logChannel = guild.channels.cache.get(logChannelId) as TextChannel;
        if (logChannel && logChannel.isTextBased()) {
          const embed = new EmbedBuilder()
            .setColor(BOT_CONFIG.colors.danger)
            .setTitle(isRepeatOffender ? '🚨 MIAN XITERS — REPEAT NUKER ESCALATION' : '🚨 MIAN XITERS — SPAM/NUKER MITIGATION')
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

