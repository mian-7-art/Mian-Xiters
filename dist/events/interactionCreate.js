"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const security_js_1 = require("../commands/security.js");
const ban_js_1 = require("../commands/ban.js");
const logger_js_1 = require("../utils/logger.js");
exports.default = {
    name: discord_js_1.Events.InteractionCreate,
    async execute(interaction) {
        if (!interaction.isChatInputCommand())
            return;
        if (interaction.commandName === 'security') {
            try {
                await security_js_1.securityCommand.execute(interaction);
            }
            catch (err) {
                logger_js_1.Logger.error(`[COMMAND_ERROR] /security failed: ${err.message}`, err);
                const replyPayload = {
                    content: '❌ An unexpected error occurred while executing the security command.',
                    ephemeral: true
                };
                if (interaction.deferred || interaction.replied) {
                    await interaction.editReply(replyPayload).catch(() => { });
                }
                else {
                    await interaction.reply(replyPayload).catch(() => { });
                }
            }
        }
        else if (interaction.commandName === 'ban') {
            try {
                await ban_js_1.banCommand.execute(interaction);
            }
            catch (err) {
                logger_js_1.Logger.error(`[COMMAND_ERROR] /ban failed: ${err.message}`, err);
                const replyPayload = {
                    content: '❌ An unexpected error occurred while executing the ban command.',
                    ephemeral: true
                };
                if (interaction.deferred || interaction.replied) {
                    await interaction.editReply(replyPayload).catch(() => { });
                }
                else {
                    await interaction.reply(replyPayload).catch(() => { });
                }
            }
        }
    }
};
