import {
  SlashCommandBuilder,
  ChatInputCommandInteraction,
  EmbedBuilder
} from 'discord.js';
import { levelService } from '../services/LevelService.js';
import { BOT_CONFIG } from '../config/config.js';

export const leaderboardCommand = {
  data: new SlashCommandBuilder()
    .setName('leaderboard')
    .setDescription('🏆 Display the top 10 most active members on the server'),

  async execute(interaction: ChatInputCommandInteraction): Promise<any> {
    const guild = interaction.guild;
    if (!guild) {
      return interaction.reply({
        content: '❌ This command can only be executed within a Discord server.',
        ephemeral: true
      });
    }

    const topUsers = levelService.getLeaderboard(guild.id, 10);

    if (topUsers.length === 0) {
      return interaction.reply({
        content: '📊 The leaderboard is currently empty! Start chatting in public channels to earn XP and level up!',
        ephemeral: true
      });
    }

    const medals = ['🥇', '🥈', '🥉'];

    const leaderboardLines = topUsers.map((u, i) => {
      const medal = i < 3 ? medals[i] : `\`#${i + 1}\``;
      const isMax = u.level >= levelService.MAX_LEVEL;
      if (isMax) {
        return `${medal} <@${u.userId}> — **Level 10 (MAX 👑)** (\`MAXIMUM PRESTIGE\` • \`${u.totalMessages} msgs\`)`;
      }
      const xpNeeded = levelService.getXpForNextLevel(u.level);
      return `${medal} <@${u.userId}> — **Level ${u.level}** (\`${u.xp.toLocaleString()} / ${xpNeeded.toLocaleString()} XP\` • \`${u.totalMessages} msgs\`)`;
    });

    const embed = new EmbedBuilder()
      .setColor(0xE03C3C)
      .setTitle(`🏆 ${guild.name} — Activity Leaderboard`)
      .setDescription(
        `Top members ranked by message activity and levels:\n\n` +
        leaderboardLines.join('\n\n')
      )
      .setFooter({
        text: `${BOT_CONFIG.name} Arcane Leveling • Chat to climb the ranks`,
        iconURL: guild.iconURL() || undefined
      })
      .setTimestamp();

    await interaction.reply({ embeds: [embed] });
  }
};
