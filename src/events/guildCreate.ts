import {
  Events,
  Guild,
  ChannelType,
  PermissionFlagsBits,
  EmbedBuilder,
  REST,
  Routes
} from 'discord.js';
import { snapshotService } from '../services/SnapshotService.js';
import { whitelistManager } from '../config/whitelist.js';
import { securityCommand } from '../commands/security.js';
import { BOT_CONFIG } from '../config/config.js';
import { Logger } from '../utils/logger.js';

export default {
  name: Events.GuildCreate,
  async execute(guild: Guild): Promise<void> {
    try {
      Logger.info(`[GUILD JOIN] MIAN XITERS has been invited to new server: "${guild.name}" (${guild.id})`);

      // 1. Capture atomic state baseline snapshot immediately
      await snapshotService.captureSnapshot(guild);
      Logger.info(`[SNAPSHOT] Captured initial security snapshot for "${guild.name}" (${guild.channels.cache.size} channels, ${guild.roles.cache.size} roles)`);

      // 2. Cache members for owner permission resolution
      await guild.members.fetch().catch(() => {});

      // 3. Auto-deploy private #mian-xiters-logs channel
      let logChannel = guild.channels.cache.find(c => c.name === 'mian-xiters-logs');
      if (!logChannel && guild.members.me?.permissions.has(PermissionFlagsBits.ManageChannels)) {
        try {
          logChannel = await guild.channels.create({
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
            reason: '[MIAN XITERS] Automated secure audit logging setup'
          });
          Logger.info(`[SETUP] Auto-created private security log channel in "${guild.name}"`);
        } catch (err: unknown) {
          const msg = err instanceof Error ? err.message : String(err);
          Logger.error(`[SETUP] Failed to create log channel in "${guild.name}": ${msg}`);
        }
      }

      if (logChannel) {
        whitelistManager.setLogChannel(guild.id, logChannel.id);
      }

      // 4. Auto-deploy Quarantine Role
      let quarantineRole = guild.roles.cache.find(r => r.name === 'MIAN-XITERS-QUARANTINE');
      if (!quarantineRole && guild.members.me?.permissions.has(PermissionFlagsBits.ManageRoles)) {
        try {
          quarantineRole = await guild.roles.create({
            name: 'MIAN-XITERS-QUARANTINE',
            color: 0x555555,
            permissions: [],
            reason: '[MIAN XITERS] Automated quarantine isolation role'
          });
          Logger.info(`[SETUP] Auto-created quarantine role in "${guild.name}"`);
        } catch (err: unknown) {
          const msg = err instanceof Error ? err.message : String(err);
          Logger.error(`[SETUP] Failed to create quarantine role in "${guild.name}": ${msg}`);
        }
      }

      if (quarantineRole) {
        whitelistManager.setQuarantineRole(guild.id, quarantineRole.id);
      }

      // 5. Instantly deploy guild-specific slash commands
      if (BOT_CONFIG.token && BOT_CONFIG.clientId) {
        try {
          const rest = new REST({ version: '10' }).setToken(BOT_CONFIG.token);
          await rest.put(
            Routes.applicationGuildCommands(BOT_CONFIG.clientId, guild.id),
            { body: [securityCommand.data.toJSON()] }
          );
          Logger.info(`[SLASH COMMANDS] Deployed /security slash commands to "${guild.name}"`);
        } catch (err: unknown) {
          const msg = err instanceof Error ? err.message : String(err);
          Logger.error(`[SLASH COMMANDS] Failed to register slash commands in "${guild.name}": ${msg}`);
        }
      }

      // 6. Send deployment confirmation embed
      if (logChannel && 'send' in logChannel && typeof logChannel.send === 'function') {
        const embed = new EmbedBuilder()
          .setColor(BOT_CONFIG.colors.primary)
          .setTitle('🛡️ MIAN XITERS DEFENSE ENGINE ACTIVATED')
          .setDescription(`**${BOT_CONFIG.slogan}**\nAutomated military-grade security shields are now live in **${guild.name}**.`)
          .addFields(
            { name: '👑 Server Owner', value: `<@${guild.ownerId}> (Immune)`, inline: true },
            { name: '⚡ Priority REST Lane', value: 'Active (Anti-429 Starvation)', inline: true },
            { name: '🛑 Killswitch Latency', value: '<5ms Autonomous Reaction', inline: true },
            { name: '🚷 Raid Velocity Shield', value: '>5 joins in 3 seconds', inline: true },
            { name: '💾 Atomic State Snapshot', value: `${guild.channels.cache.size} ch / ${guild.roles.cache.size} roles`, inline: true },
            { name: '🔒 Security Audit Channel', value: `<#${logChannel.id}>`, inline: true }
          )
          .setFooter({ text: `Engine Developed by ${BOT_CONFIG.author} • ${BOT_CONFIG.slogan}` })
          .setTimestamp();

        await logChannel.send({ embeds: [embed] }).catch(() => {});
      }

      Logger.info(`[ONLINE] Full autonomous security initialization complete for "${guild.name}"!`);
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[GUILD JOIN ERROR] Failed to initialize "${guild.name}": ${msg}`);
    }
  }
};
