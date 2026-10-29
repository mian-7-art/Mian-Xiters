"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.isolationService = exports.IsolationService = void 0;
const discord_js_1 = require("discord.js");
const config_js_1 = require("../config/config.js");
const whitelist_js_1 = require("../config/whitelist.js");
const SnapshotService_js_1 = require("./SnapshotService.js");
const PriorityRestManager_js_1 = require("../core/PriorityRestManager.js");
const logger_js_1 = require("../utils/logger.js");
class IsolationService {
    /**
     * The Military-Grade Zero-Trust Killswitch.
     * Strips roles via Priority REST lane, issues instant ban, purges rogue artifacts.
     */
    async executeEmergencyIsolation(guild, threat) {
        const { executorId, type, reason } = threat;
        const startTime = Date.now();
        // 0. Safety Check: Supreme Owners (Mian & Hashir) have permanent immunity
        if (whitelist_js_1.SUPREME_OWNERS.has(executorId)) {
            logger_js_1.Logger.info(`[SUPREME IMMUNITY] Action permitted for Supreme Owner: ${executorId}`);
            return false;
        }
        // 1. Safety Check: Discord API forbids banning the Guild Owner
        if (executorId === guild.ownerId) {
            logger_js_1.Logger.warn(`[SAFETY] Threat detected from server owner (${executorId}). Server owner is structurally unbannable.`);
            return false;
        }
        // 2. Safety Check: Bot itself
        if (executorId === guild.client.user?.id) {
            return false;
        }
        // 3. Granular Whitelist Verification
        if (whitelist_js_1.whitelistManager.isWhitelisted(guild.id, executorId, guild.ownerId, guild.client.user?.id)) {
            logger_js_1.Logger.info(`[WHITELIST] Action permitted for whitelisted administrator: ${executorId}`);
            return false;
        }
        logger_js_1.Logger.threat(`[KILLSWITCH ARMED]`, `Target: ${executorId} | Threat: ${type} | Reason: ${reason}`);
        let member = null;
        try {
            member = await guild.members.fetch(executorId).catch(() => null);
        }
        catch {
            member = null;
        }
        const botMember = guild.members.me;
        const actionsTaken = [];
        // 4. Hierarchy Validation Fail-Safe: Does the target outrank the bot?
        if (member && botMember) {
            const targetHighest = member.roles.highest.position;
            const botHighest = botMember.roles.highest.position;
            if (targetHighest >= botHighest) {
                logger_js_1.Logger.error(`[HIERARCHY BREACH] Malicious user ${executorId} has role position (${targetHighest}) >= bot (${botHighest})!`);
                // Emergency Mitigation: If we cannot ban the rogue admin directly, initiate nuclear server lockdown to stop them!
                await this.executePanicLockdown(guild);
                actionsTaken.push('CRITICAL HIERARCHY OVERFLOW: Triggered Nuclear Server Lockdown');
                await this.dispatchHierarchyAlert(guild, threat, member);
                return false;
            }
        }
        // STEP 1: Direct Priority REST Role Stripping (Immediate Permission Severing)
        try {
            const restStripped = await PriorityRestManager_js_1.priorityRestManager.emergencyStripRoles(guild.id, executorId, `[MIAN XITERS] Zero-Trust Autonomous Mitigation: ${type}`);
            if (restStripped) {
                actionsTaken.push('Stripped all administrative roles via Priority REST');
            }
            else if (member && member.manageable) {
                await member.roles.set([], `[MIAN XITERS] Fallback Role Strip`).catch(() => { });
                actionsTaken.push('Stripped administrative roles via Gateway Fallback');
            }
        }
        catch {
            if (member && member.manageable) {
                await member.roles.set([], `[MIAN XITERS] Fallback Role Strip`).catch(() => { });
            }
        }
        // STEP 2: Direct Priority REST Guild Ban
        try {
            const restBanned = await PriorityRestManager_js_1.priorityRestManager.emergencyBan(guild.id, executorId, {
                deleteMessageSeconds: 604800,
                reason: `[MIAN XITERS] Zero-Trust Ban: ${type} - ${reason}`
            });
            if (restBanned) {
                actionsTaken.push('Banned via Priority REST Lane (7-day message wipe)');
            }
            else {
                await guild.members.ban(executorId, {
                    deleteMessageSeconds: 604800,
                    reason: `[MIAN XITERS] Secondary Ban Fallback: ${type}`
                });
                actionsTaken.push('Banned via Secondary Gateway Fallback');
            }
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[KILLSWITCH] Ban error for ${executorId}: ${msg}`);
            try {
                await guild.members.ban(executorId, {
                    deleteMessageSeconds: 604800,
                    reason: `[MIAN XITERS] Secondary Ban Fallback: ${type}`
                });
                actionsTaken.push('Banned via Secondary Gateway Fallback');
            }
            catch { }
        }
        // STEP 3: Nuke Artifact Cleanup - Purge rogue channels created in sliding window
        const createdChannels = SnapshotService_js_1.snapshotService.getChannelsCreatedBy(executorId, 60000);
        let purgedChannels = 0;
        for (const channelId of createdChannels) {
            try {
                const ch = guild.channels.cache.get(channelId);
                if (ch && 'delete' in ch && typeof ch.delete === 'function') {
                    await ch.delete('[MIAN XITERS] Purging attacker spam channel');
                    purgedChannels++;
                }
            }
            catch { }
        }
        if (purgedChannels > 0) {
            actionsTaken.push(`Purged ${purgedChannels} spam channels`);
        }
        // STEP 4: Nuke Artifact Cleanup - Purge rogue roles created in sliding window
        const createdRoles = SnapshotService_js_1.snapshotService.getRolesCreatedBy(executorId, 60000);
        let purgedRoles = 0;
        for (const roleId of createdRoles) {
            try {
                const r = guild.roles.cache.get(roleId);
                if (r && r.editable && !r.managed) {
                    await r.delete('[MIAN XITERS] Purging attacker spam role');
                    purgedRoles++;
                }
            }
            catch { }
        }
        if (purgedRoles > 0) {
            actionsTaken.push(`Purged ${purgedRoles} spam roles`);
        }
        const elapsedMs = Date.now() - startTime;
        logger_js_1.Logger.mitigation('MILITARY-GRADE KILLSWITCH', `<@${executorId}>`, elapsedMs);
        // Asynchronously dispatch incident log embed (non-blocking)
        this.dispatchSecurityReport(guild, threat, actionsTaken, elapsedMs).catch(() => { });
        return true;
    }
    /**
     * Nuclear Panic Mode: Instantly strips all administrative permissions from all non-bot roles,
     * locks all channels, and pauses invites.
     */
    async executePanicLockdown(guild) {
        logger_js_1.Logger.threat('NUCLEAR PANIC ENGAGED', `Guild: ${guild.name} (${guild.id})`);
        whitelist_js_1.whitelistManager.setPanicMode(guild.id, true);
        const dangerousFlags = [
            discord_js_1.PermissionFlagsBits.Administrator,
            discord_js_1.PermissionFlagsBits.ManageGuild,
            discord_js_1.PermissionFlagsBits.ManageRoles,
            discord_js_1.PermissionFlagsBits.ManageChannels,
            discord_js_1.PermissionFlagsBits.BanMembers,
            discord_js_1.PermissionFlagsBits.KickMembers,
            discord_js_1.PermissionFlagsBits.ManageWebhooks,
            discord_js_1.PermissionFlagsBits.MentionEveryone
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
                    }
                    catch { }
                }
            }
        }
        let channelsLocked = 0;
        const channels = Array.from(guild.channels.cache.values());
        const textChannels = channels.filter(c => c && 'isTextBased' in c && c.isTextBased() && 'permissionOverwrites' in c);
        for (const ch of textChannels) {
            try {
                await ch.permissionOverwrites.edit(guild.roles.everyone, {
                    SendMessages: false,
                    AddReactions: false,
                    CreatePublicThreads: false,
                    CreatePrivateThreads: false
                }, { reason: '[MIAN XITERS] Nuclear Panic Lockdown' });
                channelsLocked++;
            }
            catch { }
        }
        try {
            if ('disableInvites' in guild && typeof guild.disableInvites === 'function') {
                await guild.disableInvites(true);
            }
        }
        catch { }
        return { rolesSanitized, channelsLocked };
    }
    /**
     * Restores roles, permissions, and channel overwrites from cached baseline snapshot.
     */
    async restorePanicLockdown(guild) {
        const snapshot = SnapshotService_js_1.snapshotService.getSnapshot(guild.id);
        if (!snapshot)
            return false;
        whitelist_js_1.whitelistManager.setPanicMode(guild.id, false);
        whitelist_js_1.whitelistManager.setLockdown(guild.id, false);
        // Restore role permissions
        for (const snapR of snapshot.roles) {
            const role = guild.roles.cache.get(snapR.id);
            if (role && role.editable && !role.managed) {
                try {
                    await role.setPermissions(BigInt(snapR.permissions), '[MIAN XITERS] Panic Recovery: Restoring role permissions');
                }
                catch { }
            }
        }
        // Unlock channels
        const channels = Array.from(guild.channels.cache.values());
        const textChannels = channels.filter(c => c && 'isTextBased' in c && c.isTextBased() && 'permissionOverwrites' in c);
        for (const ch of textChannels) {
            try {
                await ch.permissionOverwrites.edit(guild.roles.everyone, {
                    SendMessages: null,
                    AddReactions: null,
                    CreatePublicThreads: null,
                    CreatePrivateThreads: null
                }, { reason: '[MIAN XITERS] Panic Recovery: Reopening channels' });
            }
            catch { }
        }
        try {
            if ('disableInvites' in guild && typeof guild.disableInvites === 'function') {
                await guild.disableInvites(false);
            }
        }
        catch { }
        logger_js_1.Logger.info(`[RECOVERY] Successfully restored guild ${guild.name} from Panic Mode.`);
        return true;
    }
    /**
     * Reverts unauthorized guild tampering (name, icon, banner, verification level, vanity).
     */
    async revertGuildTamper(guild, executorId) {
        const snapshot = SnapshotService_js_1.snapshotService.getSnapshot(guild.id);
        if (!snapshot)
            return;
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
                    if ('setVanityCode' in guild && typeof guild.setVanityCode === 'function') {
                        await guild.setVanityCode(snapshot.vanityURLCode, '[MIAN XITERS] Restoring legitimate vanity code');
                        logger_js_1.Logger.info(`[REVERT] Restored vanity URL to: ${snapshot.vanityURLCode}`);
                    }
                }
                catch (vanityErr) {
                    const msg = vanityErr instanceof Error ? vanityErr.message : String(vanityErr);
                    logger_js_1.Logger.error(`[REVERT] Failed to reclaim vanity URL: ${msg}`);
                }
            }
            logger_js_1.Logger.info(`[REVERT] Restored guild settings to baseline.`);
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[REVERT] Failed to revert guild settings: ${msg}`);
        }
    }
    async quarantineMember(member, reason) {
        const roleId = whitelist_js_1.whitelistManager.getQuarantineRole(member.guild.id);
        if (!roleId)
            return false;
        try {
            await member.roles.add(roleId, `[MIAN XITERS] Quarantine: ${reason}`);
            logger_js_1.Logger.info(`[QUARANTINE] Isolated ${member.user.tag} (${member.id})`);
            return true;
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[QUARANTINE] Failed to apply quarantine: ${msg}`);
            return false;
        }
    }
    async dispatchHierarchyAlert(guild, threat, member) {
        const embed = new discord_js_1.EmbedBuilder()
            .setColor(0xDC2626)
            .setTitle('🚨 CRITICAL HIERARCHY OVERFLOW ALERT')
            .setDescription(`**ATTENTION SERVER OWNER (<@${guild.ownerId}>):**\n` +
            `A rogue administrator (<@${threat.executorId}>) triggered **${threat.type}**, but their role outranks or equals ${config_js_1.BOT_CONFIG.name}'s role position!\n\n` +
            `**EMERGENCY MEASURES ENGAGED:**\n` +
            `• Server Panic Mode activated (Channels locked).\n` +
            `• Action Required: Manually ban <@${threat.executorId}> and move the **${config_js_1.BOT_CONFIG.name}** role to the VERY TOP of Server Roles immediately.`)
            .setTimestamp();
        const logChannelId = whitelist_js_1.whitelistManager.getLogChannel(guild.id);
        if (logChannelId) {
            const ch = guild.channels.cache.get(logChannelId);
            if (ch && ch.isTextBased())
                await ch.send({ embeds: [embed] }).catch(() => { });
        }
        try {
            const owner = await guild.fetchOwner();
            if (owner)
                await owner.send({ embeds: [embed] }).catch(() => { });
        }
        catch { }
    }
    async dispatchSecurityReport(guild, threat, actions, latencyMs) {
        const embed = new discord_js_1.EmbedBuilder()
            .setColor(config_js_1.BOT_CONFIG.colors.primary)
            .setTitle(`⚔️ MIAN XITERS — ZERO-TOLERANCE DEFENSE TRIGGERED`)
            .setDescription(`**${config_js_1.BOT_CONFIG.slogan}**\nAn unauthorized administrative breach was neutralized in **${latencyMs}ms**.`)
            .addFields({ name: '🎯 Threat Classification', value: `\`${threat.type}\``, inline: true }, { name: '⚡ Response Latency', value: `\`${latencyMs}ms\``, inline: true }, { name: '👤 Malicious Executor', value: `<@${threat.executorId}> (\`${threat.executorId}\`)`, inline: false }, { name: '📋 Detection Details', value: threat.reason || 'Threshold exceeded', inline: false }, { name: '🛡️ Mitigations Applied', value: actions.map(a => `✅ ${a}`).join('\n') || 'Banned & isolated', inline: false })
            .setThumbnail(guild.iconURL() || 'https://cdn.discordapp.com/embed/avatars/0.png')
            .setFooter({ text: `${config_js_1.BOT_CONFIG.name} Military Infrastructure • Dev by ${config_js_1.BOT_CONFIG.author}` })
            .setTimestamp();
        const logChannelId = whitelist_js_1.whitelistManager.getLogChannel(guild.id);
        if (logChannelId) {
            const ch = guild.channels.cache.get(logChannelId);
            if (ch && ch.isTextBased())
                await ch.send({ embeds: [embed] }).catch(() => { });
        }
        try {
            const owner = await guild.fetchOwner();
            if (owner)
                await owner.send({ embeds: [embed] }).catch(() => { });
        }
        catch { }
        logger_js_1.Logger.sendWebhookLog(`Military Defense: ${threat.type}`, `Executor: \`${threat.executorId}\`\nReason: ${threat.reason}\nLatency: ${latencyMs}ms`, config_js_1.BOT_CONFIG.colors.primary, [{ name: 'Mitigations', value: actions.join(', ') || 'Banned' }]).catch(() => { });
    }
}
exports.IsolationService = IsolationService;
exports.isolationService = new IsolationService();
