"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const config_js_1 = require("../config/config.js");
const rateLimiter_js_1 = require("../utils/rateLimiter.js");
const whitelist_js_1 = require("../config/whitelist.js");
const WatchlistService_js_1 = require("../services/WatchlistService.js");
const isolationService_js_1 = require("../services/isolationService.js");
const raidDetector_js_1 = require("../services/raidDetector.js");
const logger_js_1 = require("../utils/logger.js");
exports.default = {
    name: discord_js_1.Events.GuildMemberAdd,
    async execute(member) {
        const guild = member.guild;
        try {
            // 0. WATCHLIST SUSPECT JOIN TRIGGER
            if (WatchlistService_js_1.watchlistService.isWatchlisted(member.id)) {
                logger_js_1.Logger.threat('WATCHLIST MEMBER JOINED', `Suspect ${member.user.tag} (${member.id}) joined ${guild.name}. Quarantining immediately.`);
                await isolationService_js_1.isolationService.quarantineMember(member, 'Monitored Watchlist Suspect');
                const logChannelId = whitelist_js_1.whitelistManager.getLogChannel(guild.id);
                if (logChannelId) {
                    const logChannel = guild.channels.cache.get(logChannelId);
                    if (logChannel && logChannel.isTextBased()) {
                        const embed = new discord_js_1.EmbedBuilder()
                            .setColor(config_js_1.BOT_CONFIG.colors.danger)
                            .setTitle('👁️ MIAN XITERS — WATCHLIST SUSPECT DETECTED')
                            .setDescription(`High-risk monitored suspect <@${member.id}> (\`${member.user.tag}\`) has entered the server.\nAutomatic quarantine role applied.`)
                            .setFooter({ text: `${config_js_1.BOT_CONFIG.name} Defense • Dev by ${config_js_1.BOT_CONFIG.author}` })
                            .setTimestamp();
                        await logChannel.send({ embeds: [embed] }).catch(() => { });
                    }
                }
            }
            // 1. SWARM JOIN VELOCITY TRIGGER (>5 joins in 3 seconds)
            const key = `${guild.id}:mass_join_burst`;
            const { count, breached } = rateLimiter_js_1.rateLimiter.track(key, 5, 3 * 1000);
            if (breached) {
                logger_js_1.Logger.threat('SWARM JOIN RAID DETECTED', `${count} joins detected within 3 seconds! Engaging Server Panic Mode.`);
                if (!whitelist_js_1.whitelistManager.isPanicMode(guild.id)) {
                    // Trip Server Panic Mode (locks channels & halts invites)
                    await isolationService_js_1.isolationService.executePanicLockdown(guild);
                    const logChannelId = whitelist_js_1.whitelistManager.getLogChannel(guild.id);
                    if (logChannelId) {
                        const logChannel = guild.channels.cache.get(logChannelId);
                        if (logChannel && logChannel.isTextBased()) {
                            const embed = new discord_js_1.EmbedBuilder()
                                .setColor(config_js_1.BOT_CONFIG.colors.danger)
                                .setTitle('🛑 MIAN XITERS — AUTOMATED RAID PANIC ENGAGED')
                                .setDescription(`**${config_js_1.BOT_CONFIG.slogan}**\nA mass-join swarm was detected (**${count} joins in 3s**).\n` +
                                `Server is placed in Panic Mode: All channels locked, invites paused, raid shield active.\n\n` +
                                `*Panic Mode will auto-lift after 5 minutes of quiet traffic, or run \`/security panic action:restore\`.*`)
                                .setFooter({ text: `${config_js_1.BOT_CONFIG.name} Defense • Dev by ${config_js_1.BOT_CONFIG.author}` })
                                .setTimestamp();
                            await logChannel.send({ embeds: [embed] }).catch(() => { });
                        }
                    }
                }
            }
            // 2. HEURISTIC RAID PATTERN ANALYSIS (BEEMO-GRADE HEURISTIC ENGINE)
            const raidAnalysis = raidDetector_js_1.raidDetector.registerAndAnalyze(member);
            if (raidAnalysis.isRaidWave) {
                logger_js_1.Logger.threat('HEURISTIC RAID PATTERN', `${member.user.tag} matches swarm pattern: ${raidAnalysis.reason}`);
                if (!whitelist_js_1.whitelistManager.isPanicMode(guild.id)) {
                    await isolationService_js_1.isolationService.executePanicLockdown(guild);
                }
                await isolationService_js_1.isolationService.quarantineMember(member, `Heuristic Raid Wave: ${raidAnalysis.reason}`);
                if (member.kickable) {
                    await member.kick(`[MIAN XITERS] Raid Wave Neutralization: ${raidAnalysis.reason}`).catch(() => { });
                }
            }
            // 3. ACCOUNT AGE (7 DAYS) & SUSPICIOUS PROFILE FILTER
            const accountCreatedMs = member.user.createdTimestamp;
            const accountAgeDays = (Date.now() - accountCreatedMs) / (1000 * 60 * 60 * 24);
            const hasDefaultAvatar = !member.user.avatar;
            const isPanicActive = whitelist_js_1.whitelistManager.isPanicMode(guild.id);
            // During active raid or panic, quarantine accounts younger than 7 days or without avatars
            const isSuspicious = isPanicActive
                ? accountAgeDays < 7 || hasDefaultAvatar
                : accountAgeDays < 3;
            if (isSuspicious) {
                const quarantineSuccess = await isolationService_js_1.isolationService.quarantineMember(member, `Account age ${accountAgeDays.toFixed(1)} days (Panic Threshold: 7d, DefaultAvatar: ${hasDefaultAvatar})`);
                if (!quarantineSuccess && isPanicActive && member.kickable) {
                    await member.kick('[MIAN XITERS] Auto-Defense: Suspicious account during active raid').catch(() => { });
                }
            }
            // 4. DYNAMIC MEMBER WELCOME ANNOUNCEMENT (JENNY PLAYZ)
            if (guild.id === '1457401315415298193' && !member.user.bot) {
                const welcomeCh = guild.channels.cache.get('1565026826189934642');
                if (welcomeCh && welcomeCh.isTextBased()) {
                    const embed = new discord_js_1.EmbedBuilder()
                        .setColor(0xE74C3C)
                        .setAuthor({
                        name: `New Legend Joined!`,
                        iconURL: member.user.displayAvatarURL()
                    })
                        .setDescription(`👑 Welcome to **${guild.name}**, <@${member.id}>!\n\n` +
                        `> Grab your game roles in <#1565027249319575582> to unlock chats.\n` +
                        `> Read the rules in <#1565026963263852634>.\n` +
                        `> You are our **#${guild.memberCount}** member! Enjoy your stay! 🎉`)
                        .setThumbnail(member.user.displayAvatarURL())
                        .setTimestamp();
                    await welcomeCh.send({ content: `👋 Welcome <@${member.id}>!`, embeds: [embed] }).catch(() => { });
                }
            }
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[GUILD_MEMBER_ADD_ERROR] ${msg}`);
        }
    }
};
