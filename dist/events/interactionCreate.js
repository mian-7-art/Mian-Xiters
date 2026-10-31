"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.handleRoleSelect = handleRoleSelect;
const discord_js_1 = require("discord.js");
const security_js_1 = require("../commands/security.js");
const ban_js_1 = require("../commands/ban.js");
const rank_js_1 = require("../commands/rank.js");
const leaderboard_js_1 = require("../commands/leaderboard.js");
const logger_js_1 = require("../utils/logger.js");
exports.default = {
    name: discord_js_1.Events.InteractionCreate,
    async execute(interaction) {
        if (interaction.isStringSelectMenu()) {
            await handleRoleSelect(interaction);
            return;
        }
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
        else if (interaction.commandName === 'rank') {
            try {
                await rank_js_1.rankCommand.execute(interaction);
            }
            catch (err) {
                logger_js_1.Logger.error(`[COMMAND_ERROR] /rank failed: ${err.message}`, err);
            }
        }
        else if (interaction.commandName === 'leaderboard') {
            try {
                await leaderboard_js_1.leaderboardCommand.execute(interaction);
            }
            catch (err) {
                logger_js_1.Logger.error(`[COMMAND_ERROR] /leaderboard failed: ${err.message}`, err);
            }
        }
    }
};
async function handleRoleSelect(interaction) {
    if (!interaction.isStringSelectMenu())
        return;
    if (!['select_game_roles', 'select_pronoun_roles'].includes(interaction.customId))
        return;
    const member = interaction.member;
    if (!member)
        return;
    const selectedValues = interaction.values;
    const isPronoun = interaction.customId === 'select_pronoun_roles';
    const allPronounIds = [
        '1565097046858530936',
        '1565097047739080824',
        '1565097048506892388',
        '1565097050373365881',
        '1565097051774128221'
    ];
    const allGameIds = [
        '1565044501498560553', // Minecraft
        '1565044414475145347', // Free Fire
        '1566079735879172277', // Roblox
        '1566079740979314849', // Among Us
        '1566079745601310836' // Other Games
    ];
    try {
        if (isPronoun) {
            // Remove other pronouns first if any selected
            for (const pId of allPronounIds) {
                if (!selectedValues.includes(pId) && member.roles.cache.has(pId)) {
                    await member.roles.remove(pId).catch(() => { });
                }
            }
            for (const pId of selectedValues) {
                if (!member.roles.cache.has(pId)) {
                    await member.roles.add(pId).catch(() => { });
                }
            }
        }
        else {
            for (const gId of allGameIds) {
                if (selectedValues.includes(gId)) {
                    if (!member.roles.cache.has(gId))
                        await member.roles.add(gId).catch(() => { });
                }
                else {
                    if (member.roles.cache.has(gId))
                        await member.roles.remove(gId).catch(() => { });
                }
            }
        }
        await interaction.reply({
            content: '✅ Your roles have been updated successfully!',
            ephemeral: true
        }).catch(() => { });
    }
    catch (err) {
        await interaction.reply({
            content: '❌ Failed to update your roles. Please contact staff.',
            ephemeral: true
        }).catch(() => { });
    }
}
