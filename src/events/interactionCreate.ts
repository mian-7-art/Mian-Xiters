import { Events, Interaction, EmbedBuilder } from 'discord.js';
import { securityCommand } from '../commands/security.js';
import { banCommand } from '../commands/ban.js';
import { rankCommand } from '../commands/rank.js';
import { leaderboardCommand } from '../commands/leaderboard.js';
import { treeCommand } from '../commands/tree.js';
import { waterCommand } from '../commands/water.js';
import { treeService } from '../services/TreeService.js';
import { Logger } from '../utils/logger.js';

export default {
  name: Events.InteractionCreate,
  async execute(interaction: Interaction) {
    if (interaction.isStringSelectMenu()) {
      await handleRoleSelect(interaction);
      return;
    }

    if (interaction.isButton()) {
      await handleTreeButton(interaction);
      return;
    }

    if (!interaction.isChatInputCommand()) return;

    if (interaction.commandName === 'security') {
      try {
        await securityCommand.execute(interaction);
      } catch (err: any) {
        Logger.error(`[COMMAND_ERROR] /security failed: ${err.message}`, err);
        const replyPayload = {
          content: '❌ An unexpected error occurred while executing the security command.',
          ephemeral: true
        };
        if (interaction.deferred || interaction.replied) {
          await interaction.editReply(replyPayload).catch(() => {});
        } else {
          await interaction.reply(replyPayload).catch(() => {});
        }
      }
    } else if (interaction.commandName === 'ban') {
      try {
        await banCommand.execute(interaction);
      } catch (err: any) {
        Logger.error(`[COMMAND_ERROR] /ban failed: ${err.message}`, err);
        const replyPayload = {
          content: '❌ An unexpected error occurred while executing the ban command.',
          ephemeral: true
        };
        if (interaction.deferred || interaction.replied) {
          await interaction.editReply(replyPayload).catch(() => {});
        } else {
          await interaction.reply(replyPayload).catch(() => {});
        }
      }
    } else if (interaction.commandName === 'rank') {
      try {
        await rankCommand.execute(interaction);
      } catch (err: any) {
        Logger.error(`[COMMAND_ERROR] /rank failed: ${err.message}`, err);
      }
    } else if (interaction.commandName === 'leaderboard') {
      try {
        await leaderboardCommand.execute(interaction);
      } catch (err: any) {
        Logger.error(`[COMMAND_ERROR] /leaderboard failed: ${err.message}`, err);
      }
    } else if (interaction.commandName === 'tree') {
      try {
        await treeCommand.execute(interaction);
      } catch (err: any) {
        Logger.error(`[COMMAND_ERROR] /tree failed: ${err.message}`, err);
      }
    } else if (interaction.commandName === 'water') {
      try {
        await waterCommand.execute(interaction);
      } catch (err: any) {
        Logger.error(`[COMMAND_ERROR] /water failed: ${err.message}`, err);
      }
    }
  }
};

async function handleTreeButton(interaction: any) {
  if (!interaction.guild) return;
  const customId = interaction.customId;

  if (customId === 'tree_water') {
    const res = treeService.waterTree(interaction.guild.id, interaction.user);
    if (!res.success) {
      return interaction.reply({
        content: res.reason,
        ephemeral: true
      });
    }

    const embed = treeService.buildTreeEmbed(interaction.guild);
    const row = treeService.buildTreeActionRow();

    const stageUpText = res.newStage
      ? `\n🎉 **EVOLUTION!** The server tree evolved into **${res.newStage.emoji} ${res.newStage.name}**! 🚀`
      : '';

    await interaction.update({
      embeds: [embed],
      components: [row]
    }).catch(() => {});

    await interaction.followUp({
      content: `💧 <@${interaction.user.id}> watered the tree!\n> 📏 **+${res.heightGain}m** • 🍃 **+${res.leavesGain} leaves** • ⭐ **+${res.xpEarned} XP**!${stageUpText}`,
      ephemeral: true
    }).catch(() => {});

  } else if (customId === 'tree_harvest') {
    const res = treeService.pickFruit(interaction.guild.id, interaction.user);
    if (!res.success) {
      return interaction.reply({
        content: res.reason,
        ephemeral: true
      });
    }

    const embed = treeService.buildTreeEmbed(interaction.guild);
    const row = treeService.buildTreeActionRow();

    await interaction.update({
      embeds: [embed],
      components: [row]
    }).catch(() => {});

    await interaction.followUp({
      content: `🍎 <@${interaction.user.id}> harvested a golden fruit! **+${res.xpEarned} bonus XP** added to your rank! 🌿`,
      ephemeral: true
    }).catch(() => {});

  } else if (customId === 'tree_leaderboard') {
    const gardeners = treeService.getGardenersLeaderboard(interaction.guild.id, 10);
    if (gardeners.length === 0) {
      return interaction.reply({
        content: '🌱 No gardeners have watered the tree yet! Be the first to click **Water Tree**!',
        ephemeral: true
      });
    }

    const medals = ['🥇', '🥈', '🥉'];
    const lines = gardeners.map((g, i) => {
      const badge = i < 3 ? medals[i] : `\`#${i + 1}\``;
      return `${badge} <@${g.userId}> — **${g.waters} waters** (\`🍎 ${g.fruitsCollected} fruits\`)`;
    });

    const embed = new EmbedBuilder()
      .setColor(0x2ECC71)
      .setTitle(`🏆 ${interaction.guild.name} — Top Gardeners Leaderboard`)
      .setDescription(`Members who contributed the most water to grow the community tree:\n\n${lines.join('\n')}`)
      .setTimestamp();

    await interaction.reply({
      embeds: [embed],
      ephemeral: true
    });

  } else if (customId === 'tree_refresh') {
    const embed = treeService.buildTreeEmbed(interaction.guild);
    const row = treeService.buildTreeActionRow();
    await interaction.update({
      embeds: [embed],
      components: [row]
    }).catch(() => {});
  }
}

export async function handleRoleSelect(interaction: any) {
  if (!interaction.isStringSelectMenu()) return;
  if (!['select_game_roles', 'select_pronoun_roles'].includes(interaction.customId)) return;

  const member = interaction.member;
  if (!member) return;

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
    '1566079745601310836'  // Other Games
  ];

  try {
    if (isPronoun) {
      // Remove other pronouns first if any selected
      for (const pId of allPronounIds) {
        if (!selectedValues.includes(pId) && member.roles.cache.has(pId)) {
          await member.roles.remove(pId).catch(() => {});
        }
      }
      for (const pId of selectedValues) {
        if (!member.roles.cache.has(pId)) {
          await member.roles.add(pId).catch(() => {});
        }
      }
    } else {
      for (const gId of allGameIds) {
        if (selectedValues.includes(gId)) {
          if (!member.roles.cache.has(gId)) await member.roles.add(gId).catch(() => {});
        } else {
          if (member.roles.cache.has(gId)) await member.roles.remove(gId).catch(() => {});
        }
      }
    }

    await interaction.reply({
      content: '✅ Your roles have been updated successfully!',
      ephemeral: true
    }).catch(() => {});
  } catch (err: any) {
    await interaction.reply({
      content: '❌ Failed to update your roles. Please contact staff.',
      ephemeral: true
    }).catch(() => {});
  }
}
