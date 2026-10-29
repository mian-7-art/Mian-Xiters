"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const whitelist_js_1 = require("../config/whitelist.js");
const logger_js_1 = require("../utils/logger.js");
exports.default = {
    name: discord_js_1.Events.ChannelDelete,
    async execute(channel) {
        const guild = channel.guild;
        if (!guild)
            return;
        // Self-Healing Defense: Detect if attacker deleted the bot's private security audit channel
        const currentLogChannelId = whitelist_js_1.whitelistManager.getLogChannel(guild.id);
        if (currentLogChannelId && channel.id === currentLogChannelId) {
            logger_js_1.Logger.threat('CRITICAL DEFENSE TAMPERING', `Attacker deleted security audit log channel (${channel.name})! Self-healing activated.`);
            try {
                const recreated = await guild.channels.create({
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
                        }
                    ],
                    reason: '[MIAN XITERS] Self-Healing: Re-creating deleted audit log channel'
                });
                whitelist_js_1.whitelistManager.setLogChannel(guild.id, recreated.id);
                logger_js_1.Logger.mitigation('LOG CHANNEL RECONSTRUCTED', recreated.name, 0);
            }
            catch (err) {
                logger_js_1.Logger.error(`[SELF_HEAL_FAILED] Could not reconstruct log channel: ${err.message}`);
            }
        }
    }
};
