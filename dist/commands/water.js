"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.waterCommand = void 0;
const discord_js_1 = require("discord.js");
const TreeService_js_1 = require("../services/TreeService.js");
exports.waterCommand = {
    data: new discord_js_1.SlashCommandBuilder()
        .setName('water')
        .setDescription('💧 Water the server tree to increase its height and earn XP!'),
    async execute(interaction) {
        await interaction.deferReply();
        const guild = interaction.guild;
        if (!guild) {
            return interaction.editReply({
                content: '❌ This command can only be executed within a Discord server.'
            });
        }
        const res = TreeService_js_1.treeService.waterTree(guild.id, interaction.user);
        if (!res.success) {
            return interaction.editReply({
                content: res.reason || '❌ Cannot water right now.'
            });
        }
        const stageUpText = res.newStage
            ? `\n🎉 **EVOLUTION!** The server tree evolved into **${res.newStage.emoji} ${res.newStage.name}**! 🚀`
            : '';
        const embed = TreeService_js_1.treeService.buildTreeEmbed(guild);
        const row = TreeService_js_1.treeService.buildTreeActionRow();
        await interaction.editReply({
            content: `💧 <@${interaction.user.id}> watered the tree!\n` +
                `> 📏 **+${res.heightGain}m** height • 🍃 **+${res.leavesGain}** leaves • ⭐ **+${res.xpEarned} XP**!${stageUpText}`,
            embeds: [embed],
            components: [row]
        });
    }
};
