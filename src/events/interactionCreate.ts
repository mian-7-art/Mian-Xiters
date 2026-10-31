import { Events, Interaction } from 'discord.js';
import { securityCommand } from '../commands/security.js';
import { banCommand } from '../commands/ban.js';
import { Logger } from '../utils/logger.js';

export default {
  name: Events.InteractionCreate,
  async execute(interaction: Interaction) {
    if (interaction.isStringSelectMenu()) {
      await handleRoleSelect(interaction);
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
    }
  }
};

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
    '1565044501498560553',
    '1565044414475145347'
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
