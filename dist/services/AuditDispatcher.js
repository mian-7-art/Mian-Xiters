"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.auditDispatcher = exports.AuditDispatcher = void 0;
const discord_js_1 = require("discord.js");
const config_js_1 = require("../config/config.js");
const whitelist_js_1 = require("../config/whitelist.js");
const WatchlistService_js_1 = require("./WatchlistService.js");
const rateLimiter_js_1 = require("../utils/rateLimiter.js");
const isolationService_js_1 = require("./isolationService.js");
const SnapshotService_js_1 = require("./SnapshotService.js");
const logger_js_1 = require("../utils/logger.js");
const index_js_1 = require("../types/index.js");
class AuditDispatcher {
    /**
     * High-frequency WebSocket Gateway event dispatcher.
     * Processes audit logs in sub-millisecond timeframes without REST rate-limit starvation.
     */
    async dispatchAuditEntry(entry, guild) {
        const executorId = entry.executorId;
        if (!executorId)
            return;
        // Hardcoded Immunity: Ignore Supreme Owners (Mian & Hashir), Server Owner, and Bot Client ID
        if (whitelist_js_1.SUPREME_OWNERS.has(executorId) || executorId === guild.client.user?.id || executorId === guild.ownerId) {
            return;
        }
        const action = entry.action;
        // WATCHLIST SURVEILLANCE: Zero Tolerance for Monitored Suspects
        if (WatchlistService_js_1.watchlistService.isWatchlisted(executorId)) {
            logger_js_1.Logger.threat('WATCHLIST SUSPECT SENSITIVE ACTION DETECTED', `Monitored suspect ${executorId} attempted administrative action (${action})! Immediate killswitch engaging.`);
            await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                type: index_js_1.ThreatType.UNAUTHORIZED_ROLE_ASSIGNMENT,
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
            case discord_js_1.AuditLogEvent.MemberPrune: {
                logger_js_1.Logger.threat('MASS PRUNE DETECTED', `Executor: ${executorId} attempted member prune!`);
                await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                    type: index_js_1.ThreatType.MASS_PRUNE,
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
            case discord_js_1.AuditLogEvent.RoleUpdate: {
                if (whitelist_js_1.whitelistManager.isAuthorized(guild.id, executorId, 'allowRoleManage', guild.ownerId, guild.client.user?.id)) {
                    return;
                }
                const targetRoleId = entry.targetId;
                const permChanges = entry.changes.find(c => c.key === 'permissions');
                if (permChanges && targetRoleId) {
                    const newPerms = new discord_js_1.PermissionsBitField(BigInt(permChanges.new));
                    const dangerousFlags = [
                        discord_js_1.PermissionFlagsBits.Administrator,
                        discord_js_1.PermissionFlagsBits.ManageGuild,
                        discord_js_1.PermissionFlagsBits.ManageRoles,
                        discord_js_1.PermissionFlagsBits.ManageChannels,
                        discord_js_1.PermissionFlagsBits.BanMembers,
                        discord_js_1.PermissionFlagsBits.KickMembers,
                        discord_js_1.PermissionFlagsBits.MentionEveryone
                    ];
                    const hasDangerous = dangerousFlags.some(flag => newPerms.has(flag));
                    if (hasDangerous) {
                        logger_js_1.Logger.threat('ADMINISTRATIVE ESCALATION DETECTED', `Executor ${executorId} granted dangerous perms to role ${targetRoleId}`);
                        // 1. Revert role permissions immediately using cached state
                        const snapshot = SnapshotService_js_1.snapshotService.getSnapshot(guild.id);
                        const cachedRole = snapshot?.roles.find(r => r.id === targetRoleId);
                        const targetRole = guild.roles.cache.get(targetRoleId);
                        if (targetRole && targetRole.editable) {
                            if (cachedRole) {
                                await targetRole.setPermissions(BigInt(cachedRole.permissions), '[MIAN XITERS] Reverting unauthorized privilege escalation').catch(() => { });
                            }
                            else {
                                const sanitized = newPerms.remove(dangerousFlags);
                                await targetRole.setPermissions(sanitized, '[MIAN XITERS] Stripping dangerous flags').catch(() => { });
                            }
                        }
                        // 2. Isolate executor immediately
                        await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                            type: index_js_1.ThreatType.MASS_ROLE_UPDATE_PERMISSIONS,
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
            case discord_js_1.AuditLogEvent.RoleDelete: {
                if (whitelist_js_1.whitelistManager.isAuthorized(guild.id, executorId, 'allowRoleManage', guild.ownerId, guild.client.user?.id)) {
                    return;
                }
                const key = `${guild.id}:${executorId}:role_delete`;
                const { count, breached } = rateLimiter_js_1.rateLimiter.track(key, config_js_1.THRESHOLDS.roleDelete.limit, config_js_1.THRESHOLDS.roleDelete.windowMs);
                if (breached) {
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.MASS_ROLE_DELETE,
                        executorId,
                        guildId: guild.id,
                        timestamp: Date.now(),
                        reason: `Exceeded role deletion threshold (${count} roles deleted)`
                    });
                    // Auto-reconstruct deleted roles from snapshot
                    await SnapshotService_js_1.snapshotService.restoreMissingRoles(guild);
                }
                // Global Circuit Breaker for Sybil Attacks
                const globalKey = `${guild.id}:global:role_delete`;
                const globalTrack = rateLimiter_js_1.rateLimiter.track(globalKey, 4, 15 * 1000);
                if (globalTrack.breached) {
                    await isolationService_js_1.isolationService.executePanicLockdown(guild);
                    await SnapshotService_js_1.snapshotService.restoreMissingRoles(guild);
                }
                break;
            }
            // -----------------------------------------------------------------------
            // MASS ROLE CREATION
            // -----------------------------------------------------------------------
            case discord_js_1.AuditLogEvent.RoleCreate: {
                if (whitelist_js_1.whitelistManager.isAuthorized(guild.id, executorId, 'allowRoleManage', guild.ownerId, guild.client.user?.id)) {
                    return;
                }
                const roleId = entry.targetId;
                if (roleId)
                    SnapshotService_js_1.snapshotService.registerCreatedRole(roleId, executorId);
                const key = `${guild.id}:${executorId}:role_create`;
                const { count, breached } = rateLimiter_js_1.rateLimiter.track(key, 2, 10 * 1000, { roleId });
                if (breached) {
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.MASS_ROLE_CREATE,
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
            case discord_js_1.AuditLogEvent.ChannelDelete: {
                if (whitelist_js_1.whitelistManager.isAuthorized(guild.id, executorId, 'allowChannelManage', guild.ownerId, guild.client.user?.id)) {
                    return;
                }
                const channelId = entry.targetId;
                const key = `${guild.id}:${executorId}:channel_delete`;
                const { count, breached } = rateLimiter_js_1.rateLimiter.track(key, 2, 10 * 1000);
                if (breached) {
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.MASS_CHANNEL_DELETE,
                        executorId,
                        guildId: guild.id,
                        timestamp: Date.now(),
                        reason: `Channel deletion velocity exceeded (${count} deleted in 10s)`
                    });
                    // Instantly reconstruct the deleted channel from memory cache
                    if (channelId) {
                        await SnapshotService_js_1.snapshotService.recreateChannelFromCache(guild, channelId);
                    }
                    await SnapshotService_js_1.snapshotService.restoreMissingChannels(guild);
                }
                // Global Guild Circuit Breaker (Sybil Multi-Account Distributed Attack Defense)
                const globalKey = `${guild.id}:global:channel_delete`;
                const globalTrack = rateLimiter_js_1.rateLimiter.track(globalKey, 4, 15 * 1000);
                if (globalTrack.breached) {
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.MASS_CHANNEL_DELETE,
                        executorId,
                        guildId: guild.id,
                        timestamp: Date.now(),
                        reason: `Global channel deletion circuit breaker tripped (${globalTrack.count} deletions in 15s across distributed accounts)`
                    });
                    await isolationService_js_1.isolationService.executePanicLockdown(guild);
                    await SnapshotService_js_1.snapshotService.restoreMissingChannels(guild);
                }
                break;
            }
            case discord_js_1.AuditLogEvent.ChannelCreate: {
                if (whitelist_js_1.whitelistManager.isAuthorized(guild.id, executorId, 'allowChannelManage', guild.ownerId, guild.client.user?.id)) {
                    return;
                }
                const channelId = entry.targetId;
                if (channelId)
                    SnapshotService_js_1.snapshotService.registerCreatedChannel(channelId, executorId);
                const key = `${guild.id}:${executorId}:channel_create`;
                const { count, breached } = rateLimiter_js_1.rateLimiter.track(key, 2, 10 * 1000, { channelId });
                if (breached) {
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.MASS_CHANNEL_CREATE,
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
            case discord_js_1.AuditLogEvent.BotAdd: {
                const addedBotId = entry.targetId;
                const isBotAllowed = whitelist_js_1.whitelistManager.isAuthorized(guild.id, executorId, 'allowBotAdd', guild.ownerId, guild.client.user?.id);
                if (!isBotAllowed) {
                    logger_js_1.Logger.threat('UNAUTHORIZED BOT INTEGRATION', `Executor: ${executorId} added Bot: ${addedBotId}`);
                    // 1. Isolate the human executor who added the bot
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.UNAUTHORIZED_BOT_ADD,
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
                            logger_js_1.Logger.mitigation('ROGUE BOT PURGED', addedBotId, 0);
                        }
                        catch (botBanErr) {
                            const msg = botBanErr instanceof Error ? botBanErr.message : String(botBanErr);
                            logger_js_1.Logger.error(`[AUDIT] Failed to ban rogue bot ${addedBotId}: ${msg}`);
                        }
                    }
                }
                break;
            }
            // -----------------------------------------------------------------------
            // E. WEBHOOK HIJACKING & ABUSE GUARD
            // -----------------------------------------------------------------------
            case discord_js_1.AuditLogEvent.WebhookCreate:
            case discord_js_1.AuditLogEvent.WebhookUpdate: {
                if (whitelist_js_1.whitelistManager.isWhitelisted(guild.id, executorId, guild.ownerId, guild.client.user?.id)) {
                    return;
                }
                const key = `${guild.id}:${executorId}:webhook`;
                const { count, breached } = rateLimiter_js_1.rateLimiter.track(key, 1, 15 * 1000);
                if (breached) {
                    // Delete unauthorized webhook
                    if (entry.targetId) {
                        try {
                            const webhooks = await guild.fetchWebhooks();
                            const rogue = webhooks.get(entry.targetId);
                            if (rogue)
                                await rogue.delete('[MIAN XITERS] Purging unauthorized webhook');
                        }
                        catch { }
                    }
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.WEBHOOK_TAMPER,
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
            case discord_js_1.AuditLogEvent.GuildUpdate: {
                if (!whitelist_js_1.whitelistManager.isWhitelisted(guild.id, executorId, guild.ownerId, guild.client.user?.id)) {
                    logger_js_1.Logger.threat('GUILD PROFILE TAMPERING', `Executor: ${executorId} modified server settings`);
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.GUILD_TAMPER,
                        executorId,
                        guildId: guild.id,
                        timestamp: Date.now(),
                        reason: 'Unauthorized modification of server configuration'
                    });
                    await isolationService_js_1.isolationService.revertGuildTamper(guild, executorId);
                }
                break;
            }
            // -----------------------------------------------------------------------
            // MASS BANS & KICKS
            // -----------------------------------------------------------------------
            case discord_js_1.AuditLogEvent.MemberBanAdd: {
                if (whitelist_js_1.whitelistManager.isAuthorized(guild.id, executorId, 'allowMemberManage', guild.ownerId, guild.client.user?.id)) {
                    return;
                }
                const key = `${guild.id}:${executorId}:ban`;
                const { count, breached } = rateLimiter_js_1.rateLimiter.track(key, 3, 10 * 1000);
                if (breached) {
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.MASS_BAN,
                        executorId,
                        guildId: guild.id,
                        timestamp: Date.now(),
                        reason: `Mass ban velocity limit exceeded (${count} bans in 10s)`
                    });
                }
                break;
            }
            case discord_js_1.AuditLogEvent.MemberKick: {
                if (whitelist_js_1.whitelistManager.isAuthorized(guild.id, executorId, 'allowMemberManage', guild.ownerId, guild.client.user?.id)) {
                    return;
                }
                const key = `${guild.id}:${executorId}:kick`;
                const { count, breached } = rateLimiter_js_1.rateLimiter.track(key, 3, 10 * 1000);
                if (breached) {
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.MASS_KICK,
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
            case discord_js_1.AuditLogEvent.MemberRoleUpdate: {
                if (whitelist_js_1.whitelistManager.isAuthorized(guild.id, executorId, 'allowRoleManage', guild.ownerId, guild.client.user?.id)) {
                    return;
                }
                const targetUserId = entry.targetId;
                const addChanges = entry.changes.find(c => c.key === '$add');
                if (addChanges && Array.isArray(addChanges.new) && targetUserId) {
                    const dangerousFlags = [
                        discord_js_1.PermissionFlagsBits.Administrator,
                        discord_js_1.PermissionFlagsBits.ManageGuild,
                        discord_js_1.PermissionFlagsBits.ManageRoles,
                        discord_js_1.PermissionFlagsBits.ManageChannels,
                        discord_js_1.PermissionFlagsBits.BanMembers,
                        discord_js_1.PermissionFlagsBits.KickMembers
                    ];
                    for (const roleObj of addChanges.new) {
                        const role = guild.roles.cache.get(roleObj.id);
                        if (role && dangerousFlags.some(flag => role.permissions.has(flag))) {
                            try {
                                const targetMember = await guild.members.fetch(targetUserId);
                                if (targetMember && targetMember.manageable) {
                                    await targetMember.roles.remove(role.id, '[MIAN XITERS] Revoking unauthorized admin role assignment');
                                }
                            }
                            catch { }
                            await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                                type: index_js_1.ThreatType.UNAUTHORIZED_ROLE_ASSIGNMENT,
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
            case discord_js_1.AuditLogEvent.ChannelOverwriteCreate:
            case discord_js_1.AuditLogEvent.ChannelOverwriteUpdate:
            case discord_js_1.AuditLogEvent.ChannelOverwriteDelete: {
                if (whitelist_js_1.whitelistManager.isAuthorized(guild.id, executorId, 'allowChannelManage', guild.ownerId, guild.client.user?.id)) {
                    return;
                }
                const key = `${guild.id}:${executorId}:channel_overwrite`;
                const { count, breached } = rateLimiter_js_1.rateLimiter.track(key, 3, 10 * 1000);
                if (breached) {
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.GUILD_TAMPER,
                        executorId,
                        guildId: guild.id,
                        timestamp: Date.now(),
                        reason: `Channel permission overwrite velocity exceeded (${count} in 10s)`
                    });
                }
                break;
            }
            case discord_js_1.AuditLogEvent.AutoModerationRuleDelete: {
                if (!whitelist_js_1.whitelistManager.isWhitelisted(guild.id, executorId, guild.ownerId, guild.client.user?.id)) {
                    await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                        type: index_js_1.ThreatType.GUILD_TAMPER,
                        executorId,
                        guildId: guild.id,
                        timestamp: Date.now(),
                        reason: 'Unauthorized deletion of server AutoModeration rule'
                    });
                }
                break;
            }
            // -----------------------------------------------------------------------
            // OWNER VOICE PROTECTION (ANTI-DRAG, ANTI-MOVE, ANTI-MUTE)
            // -----------------------------------------------------------------------
            case discord_js_1.AuditLogEvent.MemberMove: {
                const targetUserId = entry.targetId;
                if (targetUserId && (whitelist_js_1.SUPREME_OWNERS.has(targetUserId) || targetUserId === guild.ownerId)) {
                    if (executorId !== guild.client.user?.id && !whitelist_js_1.SUPREME_OWNERS.has(executorId)) {
                        logger_js_1.Logger.threat('OWNER DRAGGED IN VOICE', `Executor ${executorId} dragged Owner ${targetUserId}!`);
                        await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                            type: index_js_1.ThreatType.SECURITY_INFRASTRUCTURE_TAMPER,
                            executorId,
                            targetId: targetUserId,
                            guildId: guild.id,
                            timestamp: Date.now(),
                            reason: 'Unauthorized attempt to drag/move the Owner in Voice'
                        });
                    }
                }
                break;
            }
            case discord_js_1.AuditLogEvent.MemberDisconnect: {
                const targetUserId = entry.targetId;
                if (targetUserId && (whitelist_js_1.SUPREME_OWNERS.has(targetUserId) || targetUserId === guild.ownerId)) {
                    if (executorId !== guild.client.user?.id && !whitelist_js_1.SUPREME_OWNERS.has(executorId)) {
                        logger_js_1.Logger.threat('OWNER DISCONNECTED FROM VOICE', `Executor ${executorId} disconnected Owner ${targetUserId}!`);
                        await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                            type: index_js_1.ThreatType.SECURITY_INFRASTRUCTURE_TAMPER,
                            executorId,
                            targetId: targetUserId,
                            guildId: guild.id,
                            timestamp: Date.now(),
                            reason: 'Unauthorized attempt to disconnect the Owner from Voice'
                        });
                    }
                }
                break;
            }
            case discord_js_1.AuditLogEvent.MemberUpdate: {
                const targetUserId = entry.targetId;
                if (targetUserId && (whitelist_js_1.SUPREME_OWNERS.has(targetUserId) || targetUserId === guild.ownerId)) {
                    const muteChange = entry.changes.find(c => c.key === 'mute' && c.new === true);
                    const deafChange = entry.changes.find(c => c.key === 'deaf' && c.new === true);
                    if ((muteChange || deafChange) && executorId !== guild.client.user?.id && !whitelist_js_1.SUPREME_OWNERS.has(executorId)) {
                        logger_js_1.Logger.threat('OWNER MUTED/DEAFENED IN VOICE', `Executor ${executorId} muted/deafened Owner ${targetUserId}!`);
                        try {
                            const targetMember = await guild.members.fetch(targetUserId).catch(() => null);
                            if (targetMember?.voice) {
                                if (muteChange)
                                    await targetMember.voice.setMute(false, '[ONLY SECURITY] Reverting unauthorized mute on Owner');
                                if (deafChange)
                                    await targetMember.voice.setDeaf(false, '[ONLY SECURITY] Reverting unauthorized deafen on Owner');
                            }
                        }
                        catch { }
                        await isolationService_js_1.isolationService.executeEmergencyIsolation(guild, {
                            type: index_js_1.ThreatType.SECURITY_INFRASTRUCTURE_TAMPER,
                            executorId,
                            targetId: targetUserId,
                            guildId: guild.id,
                            timestamp: Date.now(),
                            reason: 'Unauthorized attempt to server-mute/deafen the Owner'
                        });
                    }
                }
                break;
            }
            default:
                break;
        }
    }
}
exports.AuditDispatcher = AuditDispatcher;
exports.auditDispatcher = new AuditDispatcher();
