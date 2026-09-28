import {
  GuildAuditLogsEntry,
  AuditLogEvent,
  Guild,
  PermissionFlagsBits,
  PermissionsBitField,
  Role,
  GuildChannel
} from 'discord.js';
import { THRESHOLDS } from '../config/config.js';
import { whitelistManager, SUPREME_OWNERS } from '../config/whitelist.js';
import { watchlistService } from './WatchlistService.js';
import { rateLimiter } from '../utils/rateLimiter.js';
import { isolationService } from './isolationService.js';
import { snapshotService } from './SnapshotService.js';
import { Logger } from '../utils/logger.js';
import { ThreatType } from '../types/index.js';

export class AuditDispatcher {
  /**
   * High-frequency WebSocket Gateway event dispatcher.
   * Processes audit logs in sub-millisecond timeframes without REST rate-limit starvation.
   */
  public async dispatchAuditEntry(entry: GuildAuditLogsEntry, guild: Guild): Promise<void> {
    const executorId = entry.executorId;
    if (!executorId) return;

    // Hardcoded Immunity: Ignore Supreme Owners (Mian & Hashir), Server Owner, and Bot Client ID
    if (SUPREME_OWNERS.has(executorId) || executorId === guild.client.user?.id || executorId === guild.ownerId) {
      return;
    }

    const action = entry.action;

    // WATCHLIST SURVEILLANCE: Zero Tolerance for Monitored Suspects
    if (watchlistService.isWatchlisted(executorId)) {
      Logger.threat(
        'WATCHLIST SUSPECT SENSITIVE ACTION DETECTED',
        `Monitored suspect ${executorId} attempted administrative action (${action})! Immediate killswitch engaging.`
      );
      await isolationService.executeEmergencyIsolation(guild, {
        type: ThreatType.UNAUTHORIZED_ROLE_ASSIGNMENT,
        executorId,
        guildId: guild.id,
        timestamp: Date.now(),
        reason: `Watchlist High-Risk Suspect Attempted Administrative Action (${action})`
      });
      return;
    }

    switch (action) {
      // -----------------------------------------------------------------------
      // A. MASS PRUNE DEFENSE (THE SILENT NUKE)
      // -----------------------------------------------------------------------
      case AuditLogEvent.MemberPrune: {
        Logger.threat('MASS PRUNE DETECTED', `Executor: ${executorId} attempted member prune!`);
        await isolationService.executeEmergencyIsolation(guild, {
          type: ThreatType.MASS_PRUNE,
          executorId,
          guildId: guild.id,
          timestamp: Date.now(),
          reason: 'Unauthorized mass member prune attempt'
        });
        break;
      }

      // -----------------------------------------------------------------------
      // B. PERMISSION ESCALATION & ROLE TAMPERING GUARD
      // -----------------------------------------------------------------------
      case AuditLogEvent.RoleUpdate: {
        if (whitelistManager.isAuthorized(guild.id, executorId, 'allowRoleManage', guild.ownerId, guild.client.user?.id)) {
          return;
        }

        const targetRoleId = entry.targetId;
        const permChanges = entry.changes.find(c => c.key === 'permissions');

        if (permChanges && targetRoleId) {
          const newPerms = new PermissionsBitField(BigInt(permChanges.new as string));
          const dangerousFlags = [
            PermissionFlagsBits.Administrator,
            PermissionFlagsBits.ManageGuild,
            PermissionFlagsBits.ManageRoles,
            PermissionFlagsBits.ManageChannels,
            PermissionFlagsBits.BanMembers,
            PermissionFlagsBits.KickMembers,
            PermissionFlagsBits.MentionEveryone
          ];

          const hasDangerous = dangerousFlags.some(flag => newPerms.has(flag));
          if (hasDangerous) {
            Logger.threat('ADMINISTRATIVE ESCALATION DETECTED', `Executor ${executorId} granted dangerous perms to role ${targetRoleId}`);

            // 1. Revert role permissions immediately using cached state
            const snapshot = snapshotService.getSnapshot(guild.id);
            const cachedRole = snapshot?.roles.find(r => r.id === targetRoleId);
            const targetRole = guild.roles.cache.get(targetRoleId);

            if (targetRole && targetRole.editable) {
              if (cachedRole) {
                await targetRole.setPermissions(BigInt(cachedRole.permissions), '[MIAN XITERS] Reverting unauthorized privilege escalation').catch(() => {});
              } else {
                const sanitized = newPerms.remove(dangerousFlags);
                await targetRole.setPermissions(sanitized, '[MIAN XITERS] Stripping dangerous flags').catch(() => {});
              }
            }

            // 2. Isolate executor immediately
            await isolationService.executeEmergencyIsolation(guild, {
              type: ThreatType.MASS_ROLE_UPDATE_PERMISSIONS,
              executorId,
              targetId: targetRoleId,
              guildId: guild.id,
              timestamp: Date.now(),
              reason: 'Unauthorized injection of administrative privileges into role'
            });
          }
        }
        break;
      }

      // -----------------------------------------------------------------------
      // MASS ROLE DELETION & AUTO-RECONSTRUCTION
      // -----------------------------------------------------------------------
      case AuditLogEvent.RoleDelete: {
        if (whitelistManager.isAuthorized(guild.id, executorId, 'allowRoleManage', guild.ownerId, guild.client.user?.id)) {
          return;
        }

        const key = `${guild.id}:${executorId}:role_delete`;
        const { count, breached } = rateLimiter.track(
          key,
          THRESHOLDS.roleDelete.limit,
          THRESHOLDS.roleDelete.windowMs
        );

        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_ROLE_DELETE,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded role deletion threshold (${count} roles deleted)`
          });
          // Auto-reconstruct deleted roles from snapshot
          await snapshotService.restoreMissingRoles(guild);
        }

        // Global Circuit Breaker for Sybil Attacks
        const globalKey = `${guild.id}:global:role_delete`;
        const globalTrack = rateLimiter.track(globalKey, 4, 15 * 1000);
        if (globalTrack.breached) {
          await isolationService.executePanicLockdown(guild);
          await snapshotService.restoreMissingRoles(guild);
        }
        break;
      }

      // -----------------------------------------------------------------------
      // MASS ROLE CREATION
      // -----------------------------------------------------------------------
      case AuditLogEvent.RoleCreate: {
        if (whitelistManager.isAuthorized(guild.id, executorId, 'allowRoleManage', guild.ownerId, guild.client.user?.id)) {
          return;
        }

        const roleId = entry.targetId;
        if (roleId) snapshotService.registerCreatedRole(roleId, executorId);

        const key = `${guild.id}:${executorId}:role_create`;
        const { count, breached } = rateLimiter.track(key, 2, 10 * 1000, { roleId });
        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_ROLE_CREATE,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Role spam creation detected (${count} created in 10s)`
          });
        }
        break;
      }

      // -----------------------------------------------------------------------
      // C. CHANNEL DESTRUCTION & SPAM FLOOD GUARD
      // -----------------------------------------------------------------------
      case AuditLogEvent.ChannelDelete: {
        if (whitelistManager.isAuthorized(guild.id, executorId, 'allowChannelManage', guild.ownerId, guild.client.user?.id)) {
          return;
        }

        const channelId = entry.targetId;
        const key = `${guild.id}:${executorId}:channel_delete`;
        const { count, breached } = rateLimiter.track(key, 2, 10 * 1000);

        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_CHANNEL_DELETE,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Channel deletion velocity exceeded (${count} deleted in 10s)`
          });

          // Instantly reconstruct the deleted channel from memory cache
          if (channelId) {
            await snapshotService.recreateChannelFromCache(guild, channelId);
          }
          await snapshotService.restoreMissingChannels(guild);
        }

        // Global Guild Circuit Breaker (Sybil Multi-Account Distributed Attack Defense)
        const globalKey = `${guild.id}:global:channel_delete`;
        const globalTrack = rateLimiter.track(globalKey, 4, 15 * 1000);
        if (globalTrack.breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_CHANNEL_DELETE,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Global channel deletion circuit breaker tripped (${globalTrack.count} deletions in 15s across distributed accounts)`
          });
          await isolationService.executePanicLockdown(guild);
          await snapshotService.restoreMissingChannels(guild);
        }
        break;
      }

      case AuditLogEvent.ChannelCreate: {
        if (whitelistManager.isAuthorized(guild.id, executorId, 'allowChannelManage', guild.ownerId, guild.client.user?.id)) {
          return;
        }

        const channelId = entry.targetId;
        if (channelId) snapshotService.registerCreatedChannel(channelId, executorId);

        const key = `${guild.id}:${executorId}:channel_create`;
        const { count, breached } = rateLimiter.track(key, 2, 10 * 1000, { channelId });

        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_CHANNEL_CREATE,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Channel spam creation detected (${count} created in 10s)`
          });
        }
        break;
      }

      // -----------------------------------------------------------------------
      // D. UNAUTHORIZED BOT ADDITIONS (DOUBLE-TOKEN TAKEOVER)
      // -----------------------------------------------------------------------
      case AuditLogEvent.BotAdd: {
        const addedBotId = entry.targetId;
        const isBotAllowed = whitelistManager.isAuthorized(guild.id, executorId, 'allowBotAdd', guild.ownerId, guild.client.user?.id);

        if (!isBotAllowed) {
          Logger.threat('UNAUTHORIZED BOT INTEGRATION', `Executor: ${executorId} added Bot: ${addedBotId}`);

          // 1. Isolate the human executor who added the bot
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.UNAUTHORIZED_BOT_ADD,
            executorId,
            targetId: addedBotId ?? undefined,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Unauthorized addition of external bot (${addedBotId})`
          });

          // 2. Instantly ban the added rogue bot
          if (addedBotId) {
            try {
              await guild.members.ban(addedBotId, {
                deleteMessageSeconds: 604800,
                reason: `[MIAN XITERS] Rogue bot banned (Added by unauthorized executor ${executorId})`
              });
              Logger.mitigation('ROGUE BOT PURGED', addedBotId, 0);
            } catch (botBanErr: unknown) {
              const msg = botBanErr instanceof Error ? botBanErr.message : String(botBanErr);
              Logger.error(`[AUDIT] Failed to ban rogue bot ${addedBotId}: ${msg}`);
            }
          }
        }
        break;
      }

      // -----------------------------------------------------------------------
      // E. WEBHOOK HIJACKING & ABUSE GUARD
      // -----------------------------------------------------------------------
      case AuditLogEvent.WebhookCreate:
      case AuditLogEvent.WebhookUpdate: {
        if (whitelistManager.isWhitelisted(guild.id, executorId, guild.ownerId, guild.client.user?.id)) {
          return;
        }

        const key = `${guild.id}:${executorId}:webhook`;
        const { count, breached } = rateLimiter.track(key, 1, 15 * 1000);

        if (breached) {
          // Delete unauthorized webhook
          if (entry.targetId) {
            try {
              const webhooks = await guild.fetchWebhooks();
              const rogue = webhooks.get(entry.targetId);
              if (rogue) await rogue.delete('[MIAN XITERS] Purging unauthorized webhook');
            } catch {}
          }

          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.WEBHOOK_TAMPER,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Unauthorized webhook action burst (${count} in 15s)`
          });
        }
        break;
      }

      // -----------------------------------------------------------------------
      // F. GUILD PROFILE & VANITY URL HIJACK GUARD
      // -----------------------------------------------------------------------
      case AuditLogEvent.GuildUpdate: {
        if (!whitelistManager.isWhitelisted(guild.id, executorId, guild.ownerId, guild.client.user?.id)) {
          Logger.threat('GUILD PROFILE TAMPERING', `Executor: ${executorId} modified server settings`);
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.GUILD_TAMPER,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: 'Unauthorized modification of server configuration'
          });
          await isolationService.revertGuildTamper(guild, executorId);
        }
        break;
      }

      // -----------------------------------------------------------------------
      // MASS BANS & KICKS
      // -----------------------------------------------------------------------
      case AuditLogEvent.MemberBanAdd: {
        if (whitelistManager.isAuthorized(guild.id, executorId, 'allowMemberManage', guild.ownerId, guild.client.user?.id)) {
          return;
        }

        const key = `${guild.id}:${executorId}:ban`;
        const { count, breached } = rateLimiter.track(key, 3, 10 * 1000);
        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_BAN,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Mass ban velocity limit exceeded (${count} bans in 10s)`
          });
        }
        break;
      }

      case AuditLogEvent.MemberKick: {
        if (whitelistManager.isAuthorized(guild.id, executorId, 'allowMemberManage', guild.ownerId, guild.client.user?.id)) {
          return;
        }

        const key = `${guild.id}:${executorId}:kick`;
        const { count, breached } = rateLimiter.track(key, 3, 10 * 1000);
        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_KICK,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Mass kick velocity limit exceeded (${count} kicks in 10s)`
          });
        }
        break;
      }

      // -----------------------------------------------------------------------
      // STEALTH ROLE ASSIGNMENT DEFENSE
      // -----------------------------------------------------------------------
      case AuditLogEvent.MemberRoleUpdate: {
        if (whitelistManager.isAuthorized(guild.id, executorId, 'allowRoleManage', guild.ownerId, guild.client.user?.id)) {
          return;
        }

        const targetUserId = entry.targetId;
        const addChanges = entry.changes.find(c => c.key === '$add');
        if (addChanges && Array.isArray(addChanges.new) && targetUserId) {
          const dangerousFlags = [
            PermissionFlagsBits.Administrator,
            PermissionFlagsBits.ManageGuild,
            PermissionFlagsBits.ManageRoles,
            PermissionFlagsBits.ManageChannels,
            PermissionFlagsBits.BanMembers,
            PermissionFlagsBits.KickMembers
          ];

          for (const roleObj of addChanges.new as { id: string; name: string }[]) {
            const role = guild.roles.cache.get(roleObj.id);
            if (role && dangerousFlags.some(flag => role.permissions.has(flag))) {
              try {
                const targetMember = await guild.members.fetch(targetUserId);
                if (targetMember && targetMember.manageable) {
                  await targetMember.roles.remove(role.id, '[MIAN XITERS] Revoking unauthorized admin role assignment');
                }
              } catch {}

              await isolationService.executeEmergencyIsolation(guild, {
                type: ThreatType.UNAUTHORIZED_ROLE_ASSIGNMENT,
                executorId,
                targetId: targetUserId,
                guildId: guild.id,
                timestamp: Date.now(),
                reason: `Unauthorized assignment of high-privilege role (${role.name})`
              });
              break;
            }
          }
        }
        break;
      }

      // -----------------------------------------------------------------------
      // CHANNEL OVERWRITES & AUTOMOD
      // -----------------------------------------------------------------------
      case AuditLogEvent.ChannelOverwriteCreate:
      case AuditLogEvent.ChannelOverwriteUpdate:
      case AuditLogEvent.ChannelOverwriteDelete: {
        if (whitelistManager.isAuthorized(guild.id, executorId, 'allowChannelManage', guild.ownerId, guild.client.user?.id)) {
          return;
        }
        const key = `${guild.id}:${executorId}:channel_overwrite`;
        const { count, breached } = rateLimiter.track(key, 3, 10 * 1000);
        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.GUILD_TAMPER,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Channel permission overwrite velocity exceeded (${count} in 10s)`
          });
        }
        break;
      }

      case AuditLogEvent.AutoModerationRuleDelete: {
        if (!whitelistManager.isWhitelisted(guild.id, executorId, guild.ownerId, guild.client.user?.id)) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.GUILD_TAMPER,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: 'Unauthorized deletion of server AutoModeration rule'
          });
        }
        break;
      }

      default:
        break;
    }
  }
}

export const auditDispatcher = new AuditDispatcher();
