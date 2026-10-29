"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const whitelist_js_1 = require("../config/whitelist.js");
const logger_js_1 = require("../utils/logger.js");
exports.default = {
    name: discord_js_1.Events.GuildRoleDelete,
    async execute(role) {
        const guild = role.guild;
        if (!guild)
            return;
        // Self-Healing Defense: Detect if attacker deleted the bot's Quarantine role
        const currentQuarantineRoleId = whitelist_js_1.whitelistManager.getQuarantineRole(guild.id);
        if (currentQuarantineRoleId && role.id === currentQuarantineRoleId) {
            logger_js_1.Logger.threat('CRITICAL DEFENSE TAMPERING', `Attacker deleted Quarantine isolation role (${role.name})! Self-healing activated.`);
            try {
                const recreated = await guild.roles.create({
                    name: 'MIAN-XITERS-QUARANTINE',
                    color: 0x555555,
                    permissions: [],
                    reason: '[MIAN XITERS] Self-Healing: Re-creating deleted Quarantine role'
                });
                // Re-apply channel isolation overwrites
                for (const [, ch] of guild.channels.cache) {
                    if ('permissionOverwrites' in ch) {
                        await ch.permissionOverwrites.edit(recreated, {
                            ViewChannel: false,
                            SendMessages: false,
                            Connect: false
                        }).catch(() => { });
                    }
                }
                whitelist_js_1.whitelistManager.setQuarantineRole(guild.id, recreated.id);
                logger_js_1.Logger.mitigation('QUARANTINE ROLE RECONSTRUCTED', recreated.name, 0);
            }
            catch (err) {
                logger_js_1.Logger.error(`[SELF_HEAL_FAILED] Could not reconstruct quarantine role: ${err.message}`);
            }
        }
    }
};
