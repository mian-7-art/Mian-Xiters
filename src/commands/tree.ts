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
    const guild = interaction.guild;
    if (!guild) {
      return interaction.reply({
        content: '❌ This command can only be executed within a Discord server.',
        ephemeral: true
      });
    }

    const embed = treeService.buildTreeEmbed(guild);
    const row = treeService.buildTreeActionRow();

    await interaction.reply({
      embeds: [embed],
      components: [row]
    });
  }
};
