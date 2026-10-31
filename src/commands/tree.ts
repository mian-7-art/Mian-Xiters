import {
  SlashCommandBuilder,
  ChatInputCommandInteraction
} from 'discord.js';
import { treeService } from '../services/TreeService.js';

export const treeCommand = {
  data: new SlashCommandBuilder()
    .setName('tree')
    .setDescription('🌱 View, water, and grow the community server tree!'),

  async execute(interaction: ChatInputCommandInteraction): Promise<any> {
    await interaction.deferReply();

    const guild = interaction.guild;
    if (!guild) {
      return interaction.editReply({
        content: '❌ This command can only be executed within a Discord server.'
      });
    }

    const embed = treeService.buildTreeEmbed(guild);
    const row = treeService.buildTreeActionRow();

    await interaction.editReply({
      embeds: [embed],
      components: [row]
    });
  }
};
