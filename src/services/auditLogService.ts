import {
  GuildAuditLogsEntry,
  AuditLogEvent,
  Guild,
  PermissionFlagsBits,
  PermissionsBitField,
  ChannelType
} from 'discord.js';
import { THRESHOLDS } from '../config/config.js';
import { whitelistManager } from '../config/whitelist.js';
import { rateLimiter } from '../utils/rateLimiter.js';
import { isolationService } from './isolationService.js';
import { cacheService } from './cacheService.js';
import { Logger } from '../utils/logger.js';
import { ThreatType } from '../types/index.js';

export class AuditLogService {
  /**
   * Primary entry point for real-time audit log events pushed via WebSocket Gateway.
   * Zero REST cost, lowest possible latency.
   */
  public async handleAuditLogEntry(entry: GuildAuditLogsEntry, guild: Guild): Promise<void> {
    const executorId = entry.executorId;
    if (!executorId) return;

    // Hard safety checks: Ignore bot itself and server owner
    if (executorId === guild.client.user?.id || executorId === guild.ownerId) {
      return;
    }

    // Check whitelist
    const isWhitelisted = whitelistManager.isWhitelisted(
      guild.id,
      executorId,
      guild.ownerId,
      guild.client.user?.id
    );

    // If executor is whitelisted, they are authorized for standard maintenance
    if (isWhitelisted) {
      return;
    }

    const action = entry.action;

    switch (action) {
      // 1. CHANNEL DELETION
      case AuditLogEvent.ChannelDelete: {
        const key = `${guild.id}:${executorId}:channel_delete`;
        const { count, breached } = rateLimiter.track(
          key,
          THRESHOLDS.channelDelete.limit,
          THRESHOLDS.channelDelete.windowMs
        );

        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_CHANNEL_DELETE,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded channel deletion velocity limit (${count} deleted in ${THRESHOLDS.channelDelete.windowMs / 1000}s)`
          });
        }

        // Global Guild Circuit Breaker (Sybil Multi-Account Nuke Attack Defense)
        const globalKey = `${guild.id}:global:channel_delete`;
        const globalTrack = rateLimiter.track(globalKey, 4, 15 * 1000);
        if (globalTrack.breached) {
          Logger.threat('GLOBAL CIRCUIT BREAKER TRIPPED', `Multi-account channel deletion attack detected (${globalTrack.count} channels deleted in 15s)`);
          await isolationService.executePanicLockdown(guild);
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.GUILD_CIRCUIT_BREAKER,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Global Circuit Breaker Tripped: ${globalTrack.count} channels deleted across multiple accounts`
          });
          await cacheService.restoreMissingChannels(guild);
        }
        break;
      }

      // 2. CHANNEL CREATION
      case AuditLogEvent.ChannelCreate: {
        const channelId = entry.targetId;
        if (channelId) {
          cacheService.registerCreatedChannel(channelId, executorId);
        }

        const key = `${guild.id}:${executorId}:channel_create`;
        const { count, breached } = rateLimiter.track(
          key,
          THRESHOLDS.channelCreate.limit,
          THRESHOLDS.channelCreate.windowMs,
          { channelId }
        );

        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_CHANNEL_CREATE,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded channel creation velocity limit (${count} created in ${THRESHOLDS.channelCreate.windowMs / 1000}s)`
          });
        }
        break;
      }

      // 3. ROLE DELETION
      case AuditLogEvent.RoleDelete: {
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
            reason: `Exceeded role deletion velocity limit (${count} deleted in ${THRESHOLDS.roleDelete.windowMs / 1000}s)`
          });
        }

        // Global Guild Circuit Breaker for Roles
        const globalRoleKey = `${guild.id}:global:role_delete`;
        const globalRoleTrack = rateLimiter.track(globalRoleKey, 4, 15 * 1000);
        if (globalRoleTrack.breached) {
          Logger.threat('GLOBAL CIRCUIT BREAKER TRIPPED', `Multi-account role deletion attack detected (${globalRoleTrack.count} roles deleted in 15s)`);
          await isolationService.executePanicLockdown(guild);
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.GUILD_CIRCUIT_BREAKER,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Global Circuit Breaker Tripped: ${globalRoleTrack.count} roles deleted across multiple accounts`
          });
          await cacheService.restoreMissingRoles(guild);
        }
        break;
      }

      // 4. ROLE CREATION
      case AuditLogEvent.RoleCreate: {
        const roleId = entry.targetId;
        if (roleId) {
          cacheService.registerCreatedRole(roleId, executorId);
        }

        const key = `${guild.id}:${executorId}:role_create`;
        const { count, breached } = rateLimiter.track(
          key,
          THRESHOLDS.roleCreate.limit,
          THRESHOLDS.roleCreate.windowMs,
          { roleId }
        );

        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_ROLE_CREATE,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded role creation velocity limit (${count} created in ${THRESHOLDS.roleCreate.windowMs / 1000}s)`
          });
        }
        break;
      }

      // 5. ROLE UPDATE & PRIVILEGE ESCALATION VULNERABILITY PREVENTION
      case AuditLogEvent.RoleUpdate: {
        const targetRoleId = entry.targetId;
        // Check if permissions were changed to grant dangerous admin rights
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

          const hasDangerousPerm = dangerousFlags.some(flag => newPerms.has(flag));
          if (hasDangerousPerm) {
            // Immediate isolation for granting dangerous permissions
            await isolationService.executeEmergencyIsolation(guild, {
              type: ThreatType.MASS_ROLE_UPDATE_PERMISSIONS,
              executorId,
              targetId: targetRoleId,
              guildId: guild.id,
              timestamp: Date.now(),
              reason: 'Unauthorized elevation of administrative permissions on role'
            });

            // Strip dangerous permissions from the modified role
            try {
              const role = guild.roles.cache.get(targetRoleId);
              if (role && role.editable) {
                const sanitized = newPerms.remove(dangerousFlags);
                await role.setPermissions(sanitized, '[MIAN XITERS] Revoking unauthorized admin permissions');
              }
            } catch {}
          }
        }
        break;
      }

      // 6. MEMBER BANS
      case AuditLogEvent.MemberBanAdd: {
        const key = `${guild.id}:${executorId}:ban`;
        const { count, breached } = rateLimiter.track(
          key,
          THRESHOLDS.ban.limit,
          THRESHOLDS.ban.windowMs
        );

        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_BAN,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded member ban velocity limit (${count} bans in ${THRESHOLDS.ban.windowMs / 1000}s)`
          });
        }
        break;
      }

      // 7. MEMBER KICKS
      case AuditLogEvent.MemberKick: {
        const key = `${guild.id}:${executorId}:kick`;
        const { count, breached } = rateLimiter.track(
          key,
          THRESHOLDS.kick.limit,
          THRESHOLDS.kick.windowMs
        );

        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_KICK,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded member kick velocity limit (${count} kicks in ${THRESHOLDS.kick.windowMs / 1000}s)`
          });
        }
        break;
      }

      // 8. MEMBER PRUNING (CRITICAL VULNERABILITY)
      case AuditLogEvent.MemberPrune: {
        await isolationService.executeEmergencyIsolation(guild, {
          type: ThreatType.MASS_PRUNE,
          executorId,
          guildId: guild.id,
          timestamp: Date.now(),
          reason: 'Unauthorized member prune attempt'
        });
        break;
      }

      // 9. WEBHOOK CREATION & DELETION
      case AuditLogEvent.WebhookCreate:
      case AuditLogEvent.WebhookDelete: {
        const key = `${guild.id}:${executorId}:webhook`;
        const { count, breached } = rateLimiter.track(
          key,
          THRESHOLDS.webhook.limit,
          THRESHOLDS.webhook.windowMs
        );

        if (breached) {
          // If webhook was created, purge it
          if (action === AuditLogEvent.WebhookCreate && entry.targetId) {
            try {
              const webhooks = await guild.fetchWebhooks();
              const rogue = webhooks.get(entry.targetId);
              if (rogue) await rogue.delete('[MIAN XITERS] Purging rogue webhook');
            } catch {}
          }

          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.WEBHOOK_TAMPER,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded webhook action limit (${count} actions in ${THRESHOLDS.webhook.windowMs / 1000}s)`
          });
        }
        break;
      }

      // 10. UNAUTHORIZED BOT INTEGRATION
      case AuditLogEvent.BotAdd: {
        const addedBotId = entry.targetId;
        Logger.threat('UNAUTHORIZED BOT INTEGRATION', `Executor: ${executorId} added Bot: ${addedBotId}`);

        // Immediately isolate executor
        await isolationService.executeEmergencyIsolation(guild, {
          type: ThreatType.UNAUTHORIZED_BOT_ADD,
          executorId,
          targetId: addedBotId || undefined,
          guildId: guild.id,
          timestamp: Date.now(),
          reason: `Unauthorized addition of external bot (${addedBotId})`
        });

        // Immediately ban the added rogue bot
        if (addedBotId) {
          try {
            await guild.members.ban(addedBotId, {
              deleteMessageSeconds: 604800,
              reason: `[MIAN XITERS] Rogue bot automatically banned (Added by unauthorized user ${executorId})`
            });
            Logger.mitigation('ROGUE BOT PURGED', addedBotId, 0);
          } catch (err: any) {
            Logger.error(`[AUDIT] Failed to ban added bot ${addedBotId}: ${err.message}`);
          }
        }
        break;
      }

      // 11. GUILD UPDATE TAMPERING
      case AuditLogEvent.GuildUpdate: {
        Logger.threat('GUILD TAMPERING DETECTED', `Executor: ${executorId}`);
        await isolationService.executeEmergencyIsolation(guild, {
          type: ThreatType.GUILD_TAMPER,
          executorId,
          guildId: guild.id,
          timestamp: Date.now(),
          reason: 'Unauthorized modification of server configuration'
        });
        await isolationService.revertGuildTamper(guild, executorId);
        break;
      }

      // 12. EMOJI & STICKER NUKES
      case AuditLogEvent.EmojiDelete:
      case AuditLogEvent.StickerDelete: {
        const key = `${guild.id}:${executorId}:asset_delete`;
        const { count, breached } = rateLimiter.track(
          key,
          THRESHOLDS.emojiDelete.limit,
          THRESHOLDS.emojiDelete.windowMs
        );

        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_EMOJI_DELETE,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded asset deletion velocity limit (${count} deleted)`
          });
        }
        break;
      }

      // 13. STEALTH ROLE ASSIGNMENT DEFENSE (PREVENTS PRIVILEGE INJECTION)
      case AuditLogEvent.MemberRoleUpdate: {
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

          for (const addedRoleInfo of addChanges.new) {
            const role = guild.roles.cache.get(addedRoleInfo.id);
            if (role) {
              const hasDangerous = dangerousFlags.some(flag => role.permissions.has(flag));
              if (hasDangerous) {
                Logger.threat('STEALTH ROLE INJECTION', `Executor ${executorId} assigned dangerous role ${role.name} to ${targetUserId}`);

                // Strip role from target user
                try {
                  const targetMember = await guild.members.fetch(targetUserId);
                  if (targetMember && targetMember.manageable) {
                    await targetMember.roles.remove(role.id, '[MIAN XITERS] Revoking unauthorized admin role assignment');
                  }
                } catch {}

                // Isolate rogue executor
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
        }
        break;
      }

      // 14. CHANNEL PERMISSION OVERWRITE TAMPERING
      case AuditLogEvent.ChannelOverwriteCreate:
      case AuditLogEvent.ChannelOverwriteUpdate:
      case AuditLogEvent.ChannelOverwriteDelete: {
        const key = `${guild.id}:${executorId}:channel_overwrite`;
        const { count, breached } = rateLimiter.track(key, 3, 10 * 1000);

        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.GUILD_TAMPER,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded channel permission overwrite velocity limit (${count} in 10s)`
          });
        }
        break;
      }

      // 15. THREAD DELETION NUKING
      case AuditLogEvent.ThreadDelete: {
        const key = `${guild.id}:${executorId}:channel_delete`;
        const { count, breached } = rateLimiter.track(
          key,
          THRESHOLDS.channelDelete.limit,
          THRESHOLDS.channelDelete.windowMs
        );

        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_CHANNEL_DELETE,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded thread deletion velocity limit (${count} deleted in 10s)`
          });
        }
        break;
      }

      // 16. AUTOMOD RULE TAMPERING
      case AuditLogEvent.AutoModerationRuleDelete: {
        await isolationService.executeEmergencyIsolation(guild, {
          type: ThreatType.GUILD_TAMPER,
          executorId,
          guildId: guild.id,
          timestamp: Date.now(),
          reason: 'Unauthorized deletion of server AutoModeration rule'
        });
        break;
      }

      // 17. THIRD-PARTY INTEGRATION TAMPERING (OAUTH2 APPS & WEBHOOKS)
      case AuditLogEvent.IntegrationCreate:
      case AuditLogEvent.IntegrationDelete: {
        await isolationService.executeEmergencyIsolation(guild, {
          type: ThreatType.INTEGRATION_TAMPER,
          executorId,
          guildId: guild.id,
          timestamp: Date.now(),
          reason: 'Unauthorized creation or modification of third-party server integration'
        });
        break;
      }

      // 18. BACKDOOR INVITE FLOOD SHIELD
      case AuditLogEvent.InviteCreate: {
        const key = `${guild.id}:${executorId}:invite_create`;
        const { count, breached } = rateLimiter.track(key, 3, 10 * 1000);
        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.INVITE_FLOOD,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded invite generation velocity (${count} invites in 10s)`
          });
        }
        break;
      }

      // 19. EMOJI & STICKER SPAM CREATION ATTACK
      case AuditLogEvent.EmojiCreate:
      case AuditLogEvent.StickerCreate: {
        const key = `${guild.id}:${executorId}:asset_create`;
        const { count, breached } = rateLimiter.track(key, 4, 10 * 1000);
        if (breached) {
          await isolationService.executeEmergencyIsolation(guild, {
            type: ThreatType.MASS_EMOJI_DELETE,
            executorId,
            guildId: guild.id,
            timestamp: Date.now(),
            reason: `Exceeded asset creation velocity limit (${count} created in 10s)`
          });
        }
        break;
      }

      default:
        break;
    }
  }
}

export const auditLogService = new AuditLogService();
