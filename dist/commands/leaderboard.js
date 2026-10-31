"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.leaderboardCommand = void 0;
const discord_js_1 = require("discord.js");
const LevelService_js_1 = require("../services/LevelService.js");
const config_js_1 = require("../config/config.js");
exports.leaderboardCommand = {
    data: new discord_js_1.SlashCommandBuilder()
        .setName('leaderboard')
        .setDescription('🏆 Display the top 10 most active members on the server'),
    async execute(interaction) {
        const guild = interaction.guild;
        if (!guild) {
            return interaction.reply({
                content: '❌ This command can only be executed within a Discord server.',
                ephemeral: true
            });
        }
        const topUsers = LevelService_js_1.levelService.getLeaderboard(guild.id, 10);
        if (topUsers.length === 0) {
            return interaction.reply({
                content: '📊 The leaderboard is currently empty! Start chatting in public channels to earn XP and level up!',
                ephemeral: true
            });
        }
        const medals = ['🥇', '🥈', '🥉'];
        const leaderboardLines = topUsers.map((u, i) => {
            const medal = i < 3 ? medals[i] : `\`#${i + 1}\``;
            const isMax = u.level >= LevelService_js_1.levelService.MAX_LEVEL;
            if (isMax) {
                return `${medal} <@${u.userId}> — **Level 10 (MAX 👑)** (\`MAXIMUM PRESTIGE\` • \`${u.totalMessages} msgs\`)`;
            }
            const xpNeeded = LevelService_js_1.levelService.getXpForNextLevel(u.level);
            return `${medal} <@${u.userId}> — **Level ${u.level}** (\`${u.xp.toLocaleString()} / ${xpNeeded.toLocaleString()} XP\` • \`${u.totalMessages} msgs\`)`;
        });
        const embed = new discord_js_1.EmbedBuilder()
            .setColor(0xE03C3C)
            .setTitle(`🏆 ${guild.name} — Activity Leaderboard`)
            .setDescription(`Top members ranked by message activity and levels:\n\n` +
            leaderboardLines.join('\n\n'))
            .setFooter({
            text: `${config_js_1.BOT_CONFIG.name} Arcane Leveling • Chat to climb the ranks`,
            iconURL: guild.iconURL() || undefined
        })
            .setTimestamp();
        await interaction.reply({ embeds: [embed] });
    }
};
