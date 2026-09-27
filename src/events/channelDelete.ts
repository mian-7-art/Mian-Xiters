import { Events, GuildChannel, ChannelType, PermissionFlagsBits } from 'discord.js';
import { whitelistManager } from '../config/whitelist.js';
import { Logger } from '../utils/logger.js';

export default {
  name: Events.ChannelDelete,
  async execute(channel: GuildChannel) {
    const guild = channel.guild;
    if (!guild) return;

    // Self-Healing Defense: Detect if attacker deleted the bot's private security audit channel
    const currentLogChannelId = whitelistManager.getLogChannel(guild.id);
    if (currentLogChannelId && channel.id === currentLogChannelId) {
      Logger.threat('CRITICAL DEFENSE TAMPERING', `Attacker deleted security audit log channel (${channel.name})! Self-healing activated.`);

      try {
        const recreated = await guild.channels.create({
          name: 'mian-xiters-logs',
          type: ChannelType.GuildText,
          permissionOverwrites: [
            {
              id: guild.roles.everyone.id,
              deny: [PermissionFlagsBits.ViewChannel]
            },
            {
              id: guild.client.user?.id || '',
              allow: [PermissionFlagsBits.ViewChannel, PermissionFlagsBits.SendMessages, PermissionFlagsBits.EmbedLinks]
            },
            {
              id: guild.ownerId,
              allow: [PermissionFlagsBits.ViewChannel, PermissionFlagsBits.ReadMessageHistory]
            }
          ],
          reason: '[MIAN XITERS] Self-Healing: Re-creating deleted audit log channel'
        });

        whitelistManager.setLogChannel(guild.id, recreated.id);
        Logger.mitigation('LOG CHANNEL RECONSTRUCTED', recreated.name, 0);
      } catch (err: any) {
        Logger.error(`[SELF_HEAL_FAILED] Could not reconstruct log channel: ${err.message}`);
      }
    }
  }
};
