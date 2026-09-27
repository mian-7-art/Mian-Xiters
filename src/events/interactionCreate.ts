import { Events, Interaction } from 'discord.js';
import { securityCommand } from '../commands/security.js';
import { Logger } from '../utils/logger.js';

export default {
  name: Events.InteractionCreate,
  async execute(interaction: Interaction) {
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
    }
  }
};
