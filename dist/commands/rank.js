"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.rankCommand = void 0;
const discord_js_1 = require("discord.js");
const LevelService_js_1 = require("../services/LevelService.js");
exports.rankCommand = {
    data: new discord_js_1.SlashCommandBuilder()
        .setName('rank')
        .setDescription('📊 Check your current server level, XP, and rank (Arcane Style)')
        .addUserOption(opt => opt
        .setName('user')
        .setDescription('Target member to check rank for (optional)')
        .setRequired(false)),
    async execute(interaction) {
        const guild = interaction.guild;
        if (!guild) {
            return interaction.reply({
                content: '❌ This command can only be executed within a Discord server.',
                ephemeral: true
            });
        }
        const targetUser = interaction.options.getUser('user') || interaction.user;
        if (targetUser.bot) {
            return interaction.reply({
                content: '❌ Bots do not earn XP or level up.',
                ephemeral: true
            });
        }
        const userData = LevelService_js_1.levelService.getUser(guild.id, targetUser.id);
        const rank = LevelService_js_1.levelService.getUserRank(guild.id, targetUser.id);
        const isMax = userData.level >= LevelService_js_1.levelService.MAX_LEVEL;
        const xpNeeded = isMax ? 0 : LevelService_js_1.levelService.getXpForNextLevel(userData.level);
        const progressBar = isMax ? '🟩'.repeat(10) : LevelService_js_1.levelService.createProgressBar(userData.xp, xpNeeded, 10);
        const percentage = isMax ? 100 : Math.round((userData.xp / xpNeeded) * 100);
        const treeStage = LevelService_js_1.levelService.getLevelTreeStage(userData.level);
        const embed = new discord_js_1.EmbedBuilder()
            .setColor(0x2ECC71)
            .setAuthor({
            name: `${targetUser.username}'s Level & Tree Card`,
            iconURL: targetUser.displayAvatarURL({ size: 256 })
        })
            .setThumbnail(targetUser.displayAvatarURL({ size: 512 }))
            .addFields({
            name: '🏆 Server Rank',
            value: `**#${rank}**`,
            inline: true
        }, {
            name: '⭐ Level',
            value: isMax ? `**Level 10 (MAX LEVEL 👑)**` : `**Level ${userData.level}**`,
            inline: true
        }, {
            name: '🌳 Tree Stage',
            value: `**${treeStage.emoji} ${treeStage.stage}**`,
            inline: true
        }, {
            name: '💬 Total Messages',
            value: `\`${userData.totalMessages}\``,
            inline: true
        }, {
            name: '📈 Level Progress',
            value: isMax
                ? `${progressBar} **100%**\n👑 **MAXIMUM PRESTIGE REACHED**`
                : `${progressBar} **${percentage}%**\n\`${userData.xp.toLocaleString()}\` / \`${xpNeeded.toLocaleString()} XP\``,
            inline: false
        })
            .setFooter({
            text: `${guild.name} • Arcane Leveling Engine`,
            iconURL: guild.iconURL() || undefined
        })
            .setTimestamp();
        await interaction.reply({ embeds: [embed] });
    }
};
