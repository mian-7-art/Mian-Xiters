"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.treeCommand = void 0;
const discord_js_1 = require("discord.js");
const TreeService_js_1 = require("../services/TreeService.js");
exports.treeCommand = {
    data: new discord_js_1.SlashCommandBuilder()
        .setName('tree')
        .setDescription('🌱 View, water, and grow the community server tree!'),
    async execute(interaction) {
        await interaction.deferReply();
        const guild = interaction.guild;
        if (!guild) {
            return interaction.editReply({
                content: '❌ This command can only be executed within a Discord server.'
            });
        }
        const embed = TreeService_js_1.treeService.buildTreeEmbed(guild);
        const row = TreeService_js_1.treeService.buildTreeActionRow();
        await interaction.editReply({
            embeds: [embed],
            components: [row]
        });
    }
};
