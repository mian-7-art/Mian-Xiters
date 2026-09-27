import { Events, Message, EmbedBuilder, TextChannel } from 'discord.js';
import { BOT_CONFIG, THRESHOLDS } from '../config/config.js';
import { whitelistManager } from '../config/whitelist.js';
import { Logger } from '../utils/logger.js';

const DISCORD_INVITE_REGEX = /(https?:\/\/)?(www\.)?(discord\.(gg|io|me|li)|discord(app)?\.com\/invite)\/[a-zA-Z0-9-]+/gi;
const PHISHING_REGEX = /(discoord|dlscord|discrod|discrd|nitro-gift|gift-nitro|free-nitro|steamcommunlty|steamcommunity-trade|discord-app\.net|discord-nitro|claim-nitro)/i;

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

      // 1. Delete message immediately
      if (message.deletable) {
        await message.delete().catch(() => {});
      }

      // 2. Apply 1-Hour Security Timeout
      if (message.member.moderatable) {
        try {
          await message.member.timeout(
            60 * 60 * 1000, // 1 hour timeout
            `[MIAN XITERS] Auto-Defense: ${violationReason}`
          );
          Logger.mitigation('MESSAGE NUKER TIMEOUT', message.author.tag, 0);
        } catch (err: unknown) {
          const msg = err instanceof Error ? err.message : String(err);
          Logger.error(`[TIMEOUT_FAILED] Could not timeout ${message.author.tag}: ${msg}`);
        }
      }

      // 3. Send warning & log
      const logChannelId = whitelistManager.getLogChannel(guild.id);
      if (logChannelId) {
        const logChannel = guild.channels.cache.get(logChannelId) as TextChannel;
        if (logChannel && logChannel.isTextBased()) {
          const embed = new EmbedBuilder()
            .setColor(BOT_CONFIG.colors.danger)
            .setTitle('🚨 MIAN XITERS — SPAM/NUKER MITIGATION')
            .setDescription(`A message containing malicious content was suppressed.\n**Author:** <@${authorId}> (\`${authorId}\`)`)
            .addFields(
              { name: '⚠️ Violation', value: violationReason, inline: true },
              { name: '🛡️ Action', value: 'Message Purged + 1 Hour Timeout', inline: true },
              { name: '💬 Channel', value: `<#${message.channelId}>`, inline: true }
            )
            .setFooter({ text: `${BOT_CONFIG.name} Shield • Dev by ${BOT_CONFIG.author}` })
            .setTimestamp();

          await logChannel.send({ embeds: [embed] }).catch(() => {});
        }
      }
    }
  }
};
