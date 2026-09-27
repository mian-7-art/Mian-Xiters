import {
  Guild,
  GuildMember,
  EmbedBuilder,
  TextChannel,
  PermissionFlagsBits,
  PermissionsBitField
} from 'discord.js';
import { BOT_CONFIG } from '../config/config.js';
import { whitelistManager } from '../config/whitelist.js';
import { snapshotService } from './SnapshotService.js';
import { priorityRestManager } from '../core/PriorityRestManager.js';
import { Logger } from '../utils/logger.js';
import { SecurityThreat } from '../types/index.js';

export class IsolationService {
  /**
   * The Military-Grade Zero-Trust Killswitch.
   * Strips roles via Priority REST lane, issues instant ban, purges rogue artifacts.
   */
  public async executeEmergencyIsolation(guild: Guild, threat: SecurityThreat): Promise<boolean> {
    const { executorId, type, reason } = threat;
    const startTime = Date.now();

    // 1. Safety Check: Discord API forbids banning the Guild Owner
    if (executorId === guild.ownerId) {
      Logger.warn(`[SAFETY] Threat detected from server owner (${executorId}). Server owner is structurally unbannable.`);
      return false;
    }

    // 2. Safety Check: Bot itself
    if (executorId === guild.client.user?.id) {
      return false;
    }

    // 3. Granular Whitelist Verification
    if (whitelistManager.isWhitelisted(guild.id, executorId, guild.ownerId, guild.client.user?.id)) {
      Logger.info(`[WHITELIST] Action permitted for whitelisted administrator: ${executorId}`);
      return false;
    }

    Logger.threat(`[KILLSWITCH ARMED]`, `Target: ${executorId} | Threat: ${type} | Reason: ${reason}`);

    let member: GuildMember | null = null;
    try {
      member = await guild.members.fetch(executorId).catch(() => null);
    } catch {
      member = null;
    }

    const botMember = guild.members.me;
    const actionsTaken: string[] = [];

    // 4. Hierarchy Validation Fail-Safe: Does the target outrank the bot?
    if (member && botMember) {
      const targetHighest = member.roles.highest.position;
      const botHighest = botMember.roles.highest.position;

      if (targetHighest >= botHighest) {
        Logger.error(`[HIERARCHY BREACH] Malicious user ${executorId} has role position (${targetHighest}) >= bot (${botHighest})!`);

        // Emergency Mitigation: If we cannot ban the rogue admin directly, initiate nuclear server lockdown to stop them!
        await this.executePanicLockdown(guild);
        actionsTaken.push('CRITICAL HIERARCHY OVERFLOW: Triggered Nuclear Server Lockdown');

        await this.dispatchHierarchyAlert(guild, threat, member);
        return false;
      }
    }

    // STEP 1: Direct Priority REST Role Stripping (Immediate Permission Severing)
    try {
      const restStripped = await priorityRestManager.emergencyStripRoles(
        guild.id,
        executorId,
        `[MIAN XITERS] Zero-Trust Autonomous Mitigation: ${type}`
      );
      if (restStripped) {
        actionsTaken.push('Stripped all administrative roles via Priority REST');
      } else if (member && member.manageable) {
        await member.roles.set([], `[MIAN XITERS] Fallback Role Strip`).catch(() => {});
        actionsTaken.push('Stripped administrative roles via Gateway Fallback');
      }
    } catch {
      if (member && member.manageable) {
        await member.roles.set([], `[MIAN XITERS] Fallback Role Strip`).catch(() => {});
      }
    }

    // STEP 2: Direct Priority REST Guild Ban
    try {
      const restBanned = await priorityRestManager.emergencyBan(guild.id, executorId, {
        deleteMessageSeconds: 604800,
        reason: `[MIAN XITERS] Zero-Trust Ban: ${type} - ${reason}`
      });
      if (restBanned) {
        actionsTaken.push('Banned via Priority REST Lane (7-day message wipe)');
      } else {
        await guild.members.ban(executorId, {
          deleteMessageSeconds: 604800,
          reason: `[MIAN XITERS] Secondary Ban Fallback: ${type}`
        });
        actionsTaken.push('Banned via Secondary Gateway Fallback');
      }
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[KILLSWITCH] Ban error for ${executorId}: ${msg}`);
      try {
        await guild.members.ban(executorId, {
          deleteMessageSeconds: 604800,
          reason: `[MIAN XITERS] Secondary Ban Fallback: ${type}`
        });
        actionsTaken.push('Banned via Secondary Gateway Fallback');
      } catch {}
    }

    // STEP 3: Nuke Artifact Cleanup - Purge rogue channels created in sliding window
    const createdChannels = snapshotService.getChannelsCreatedBy(executorId, 60000);
    let purgedChannels = 0;
    for (const channelId of createdChannels) {
      try {
        const ch = guild.channels.cache.get(channelId);
        if (ch && 'delete' in ch && typeof (ch as any).delete === 'function') {
          await (ch as any).delete('[MIAN XITERS] Purging attacker spam channel');
          purgedChannels++;
        }
      } catch {}
    }
    if (purgedChannels > 0) {
      actionsTaken.push(`Purged ${purgedChannels} spam channels`);
    }

    // STEP 4: Nuke Artifact Cleanup - Purge rogue roles created in sliding window
    const createdRoles = snapshotService.getRolesCreatedBy(executorId, 60000);
    let purgedRoles = 0;
    for (const roleId of createdRoles) {
      try {
        const r = guild.roles.cache.get(roleId);
        if (r && r.editable && !r.managed) {
          await r.delete('[MIAN XITERS] Purging attacker spam role');
          purgedRoles++;
        }
      } catch {}
    }
    if (purgedRoles > 0) {
      actionsTaken.push(`Purged ${purgedRoles} spam roles`);
    }

    const elapsedMs = Date.now() - startTime;
    Logger.mitigation('MILITARY-GRADE KILLSWITCH', `<@${executorId}>`, elapsedMs);

    // Asynchronously dispatch incident log embed (non-blocking)
    this.dispatchSecurityReport(guild, threat, actionsTaken, elapsedMs).catch(() => {});

    return true;
  }

  /**
   * Nuclear Panic Mode: Instantly strips all administrative permissions from all non-bot roles,
   * locks all channels, and pauses invites.
   */
  public async executePanicLockdown(guild: Guild): Promise<{ rolesSanitized: number; channelsLocked: number }> {
    Logger.threat('NUCLEAR PANIC ENGAGED', `Guild: ${guild.name} (${guild.id})`);
    whitelistManager.setPanicMode(guild.id, true);

    const dangerousFlags = [
      PermissionFlagsBits.Administrator,
      PermissionFlagsBits.ManageGuild,
      PermissionFlagsBits.ManageRoles,
      PermissionFlagsBits.ManageChannels,
      PermissionFlagsBits.BanMembers,
      PermissionFlagsBits.KickMembers,
      PermissionFlagsBits.ManageWebhooks,
      PermissionFlagsBits.MentionEveryone
    ];

    let rolesSanitized = 0;
    for (const [, role] of guild.roles.cache) {
      if (role.editable && !role.managed && role.id !== guild.roles.everyone.id) {
        const hasDangerous = dangerousFlags.some(flag => role.permissions.has(flag));
        if (hasDangerous) {
          try {
            const sanitized = role.permissions.remove(dangerousFlags);
            await role.setPermissions(sanitized, '[MIAN XITERS] Nuclear Panic: Stripping administrative privileges');
            rolesSanitized++;
          } catch {}
        }
      }
    }

    let channelsLocked = 0;
    const channels = Array.from(guild.channels.cache.values());
    const textChannels = channels.filter(c => c && 'isTextBased' in c && (c as any).isTextBased() && 'permissionOverwrites' in c);

    for (const ch of textChannels) {
      try {
        await (ch as TextChannel).permissionOverwrites.edit(guild.roles.everyone, {
          SendMessages: false,
          AddReactions: false,
          CreatePublicThreads: false,
          CreatePrivateThreads: false
        }, { reason: '[MIAN XITERS] Nuclear Panic Lockdown' });
        channelsLocked++;
      } catch {}
    }

    try {
      if ('disableInvites' in guild && typeof (guild as any).disableInvites === 'function') {
        await (guild as any).disableInvites(true);
      }
    } catch {}

    return { rolesSanitized, channelsLocked };
  }

  /**
   * Restores roles, permissions, and channel overwrites from cached baseline snapshot.
   */
  public async restorePanicLockdown(guild: Guild): Promise<boolean> {
    const snapshot = snapshotService.getSnapshot(guild.id);
    if (!snapshot) return false;

    whitelistManager.setPanicMode(guild.id, false);
    whitelistManager.setLockdown(guild.id, false);

    // Restore role permissions
    for (const snapR of snapshot.roles) {
      const role = guild.roles.cache.get(snapR.id);
      if (role && role.editable && !role.managed) {
        try {
          await role.setPermissions(BigInt(snapR.permissions), '[MIAN XITERS] Panic Recovery: Restoring role permissions');
        } catch {}
      }
    }

    // Unlock channels
    const channels = Array.from(guild.channels.cache.values());
    const textChannels = channels.filter(c => c && 'isTextBased' in c && (c as any).isTextBased() && 'permissionOverwrites' in c);

    for (const ch of textChannels) {
      try {
        await (ch as TextChannel).permissionOverwrites.edit(guild.roles.everyone, {
          SendMessages: null,
          AddReactions: null,
          CreatePublicThreads: null,
          CreatePrivateThreads: null
        }, { reason: '[MIAN XITERS] Panic Recovery: Reopening channels' });
      } catch {}
    }

    try {
      if ('disableInvites' in guild && typeof (guild as any).disableInvites === 'function') {
        await (guild as any).disableInvites(false);
      }
    } catch {}

    Logger.info(`[RECOVERY] Successfully restored guild ${guild.name} from Panic Mode.`);
    return true;
  }

  /**
   * Reverts unauthorized guild tampering (name, icon, banner, verification level, vanity).
   */
  public async revertGuildTamper(guild: Guild, executorId: string): Promise<void> {
    const snapshot = snapshotService.getSnapshot(guild.id);
    if (!snapshot) return;

    try {
      await guild.edit({
        name: snapshot.name,
        verificationLevel: snapshot.verificationLevel,
        explicitContentFilter: snapshot.explicitContentFilter,
        defaultMessageNotifications: snapshot.defaultMessageNotifications,
        rulesChannel: snapshot.rulesChannelId,
        publicUpdatesChannel: snapshot.publicUpdatesChannelId,
        systemChannel: snapshot.systemChannelId,
        reason: '[MIAN XITERS] Reverting unauthorized guild tampering'
      });

      // Reclaim Vanity URL if hijacked
      if (snapshot.vanityURLCode && guild.vanityURLCode !== snapshot.vanityURLCode) {
        try {
          if ('setVanityCode' in guild && typeof (guild as any).setVanityCode === 'function') {
            await (guild as any).setVanityCode(snapshot.vanityURLCode, '[MIAN XITERS] Restoring legitimate vanity code');
            Logger.info(`[REVERT] Restored vanity URL to: ${snapshot.vanityURLCode}`);
          }
        } catch (vanityErr: unknown) {
          const msg = vanityErr instanceof Error ? vanityErr.message : String(vanityErr);
          Logger.error(`[REVERT] Failed to reclaim vanity URL: ${msg}`);
        }
      }

      Logger.info(`[REVERT] Restored guild settings to baseline.`);
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[REVERT] Failed to revert guild settings: ${msg}`);
    }
  }

  public async quarantineMember(member: GuildMember, reason: string): Promise<boolean> {
    const roleId = whitelistManager.getQuarantineRole(member.guild.id);
    if (!roleId) return false;

    try {
      await member.roles.add(roleId, `[MIAN XITERS] Quarantine: ${reason}`);
      Logger.info(`[QUARANTINE] Isolated ${member.user.tag} (${member.id})`);
      return true;
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[QUARANTINE] Failed to apply quarantine: ${msg}`);
      return false;
    }
  }

  private async dispatchHierarchyAlert(
    guild: Guild,
    threat: SecurityThreat,
    member: GuildMember
  ): Promise<void> {
    const embed = new EmbedBuilder()
      .setColor(0xDC2626)
      .setTitle('🚨 CRITICAL HIERARCHY OVERFLOW ALERT')
      .setDescription(
        `**ATTENTION SERVER OWNER (<@${guild.ownerId}>):**\n` +
        `A rogue administrator (<@${threat.executorId}>) triggered **${threat.type}**, but their role outranks or equals ${BOT_CONFIG.name}'s role position!\n\n` +
        `**EMERGENCY MEASURES ENGAGED:**\n` +
        `• Server Panic Mode activated (Channels locked).\n` +
        `• Action Required: Manually ban <@${threat.executorId}> and move the **${BOT_CONFIG.name}** role to the VERY TOP of Server Roles immediately.`
      )
      .setTimestamp();

    const logChannelId = whitelistManager.getLogChannel(guild.id);
    if (logChannelId) {
      const ch = guild.channels.cache.get(logChannelId) as TextChannel;
      if (ch && ch.isTextBased()) await ch.send({ embeds: [embed] }).catch(() => {});
    }

    try {
      const owner = await guild.fetchOwner();
      if (owner) await owner.send({ embeds: [embed] }).catch(() => {});
    } catch {}
  }

  private async dispatchSecurityReport(
    guild: Guild,
    threat: SecurityThreat,
    actions: string[],
    latencyMs: number
  ): Promise<void> {
    const embed = new EmbedBuilder()
      .setColor(BOT_CONFIG.colors.primary)
      .setTitle(`⚔️ MIAN XITERS — ZERO-TOLERANCE DEFENSE TRIGGERED`)
      .setDescription(`**${BOT_CONFIG.slogan}**\nAn unauthorized administrative breach was neutralized in **${latencyMs}ms**.`)
      .addFields(
        { name: '🎯 Threat Classification', value: `\`${threat.type}\``, inline: true },
        { name: '⚡ Response Latency', value: `\`${latencyMs}ms\``, inline: true },
        { name: '👤 Malicious Executor', value: `<@${threat.executorId}> (\`${threat.executorId}\`)`, inline: false },
        { name: '📋 Detection Details', value: threat.reason || 'Threshold exceeded', inline: false },
        { name: '🛡️ Mitigations Applied', value: actions.map(a => `✅ ${a}`).join('\n') || 'Banned & isolated', inline: false }
      )
      .setThumbnail(guild.iconURL() || 'https://cdn.discordapp.com/embed/avatars/0.png')
      .setFooter({ text: `${BOT_CONFIG.name} Military Infrastructure • Dev by ${BOT_CONFIG.author}` })
      .setTimestamp();

    const logChannelId = whitelistManager.getLogChannel(guild.id);
    if (logChannelId) {
      const ch = guild.channels.cache.get(logChannelId) as TextChannel;
      if (ch && ch.isTextBased()) await ch.send({ embeds: [embed] }).catch(() => {});
    }

    try {
      const owner = await guild.fetchOwner();
      if (owner) await owner.send({ embeds: [embed] }).catch(() => {});
    } catch {}

    Logger.sendWebhookLog(
      `Military Defense: ${threat.type}`,
      `Executor: \`${threat.executorId}\`\nReason: ${threat.reason}\nLatency: ${latencyMs}ms`,
      BOT_CONFIG.colors.primary,
      [{ name: 'Mitigations', value: actions.join(', ') || 'Banned' }]
    ).catch(() => {});
  }
}

export const isolationService = new IsolationService();
