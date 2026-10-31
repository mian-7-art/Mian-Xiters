import {
  SlashCommandBuilder,
  ChatInputCommandInteraction
} from 'discord.js';
import { treeService } from '../services/TreeService.js';

export const waterCommand = {
  data: new SlashCommandBuilder()
    .setName('water')
    .setDescription('💧 Water the server tree to increase its height and earn XP!'),

  async execute(interaction: ChatInputCommandInteraction): Promise<any> {
    await interaction.deferReply();

    const guild = interaction.guild;
    if (!guild) {
      return interaction.editReply({
        content: '❌ This command can only be executed within a Discord server.'
      });
    }

    const res = treeService.waterTree(guild.id, interaction.user);
    if (!res.success) {
      return interaction.editReply({
        content: res.reason || '❌ Cannot water right now.'
      });
    }

    const stageUpText = res.newStage
      ? `\n🎉 **EVOLUTION!** The server tree evolved into **${res.newStage.emoji} ${res.newStage.name}**! 🚀`
      : '';

    const embed = treeService.buildTreeEmbed(guild);
    const row = treeService.buildTreeActionRow();

    await interaction.editReply({
      content:
        `💧 <@${interaction.user.id}> watered the tree!\n` +
        `> 📏 **+${res.heightGain}m** height • 🍃 **+${res.leavesGain}** leaves • ⭐ **+${res.xpEarned} XP**!${stageUpText}`,
      embeds: [embed],
      components: [row]
    });
  }
};
