import {
  SlashCommandBuilder,
  ChatInputCommandInteraction,
  PermissionFlagsBits,
  TextChannel
} from 'discord.js';
import { ticketService } from '../services/TicketService.js';
import { BOT_CONFIG } from '../config/config.js';

export const ticketCommand = {
  data: new SlashCommandBuilder()
    .setName('ticket')
    .setDescription('Manage the server ticket system')
    .addSubcommand(sub =>
      sub
        .setName('panel')
        .setDescription('Send the official interactive ticket creation panel')
        .addChannelOption(opt =>
          opt
            .setName('channel')
            .setDescription('Channel to send the ticket panel to (defaults to current channel)')
            .setRequired(false)
        )
    )
    .addSubcommand(sub =>
      sub
        .setName('close')
        .setDescription('Close the current ticket channel')
    )
    .addSubcommand(sub =>
      sub
        .setName('transcript')
        .setDescription('Export the transcript of the current ticket')
    ),

  async execute(interaction: ChatInputCommandInteraction): Promise<void> {
    const sub = interaction.options.getSubcommand();
    const guild = interaction.guild;
    if (!guild) return;

    if (sub === 'panel') {
      // Admin / Staff check
      const member = await guild.members.fetch(interaction.user.id);
      const isStaff =
        member.permissions.has(PermissionFlagsBits.ManageChannels) ||
        member.permissions.has(PermissionFlagsBits.Administrator) ||
        interaction.user.id === guild.ownerId;

      if (!isStaff) {
        await interaction.reply({
          content: '❌ You do not have permission to deploy the ticket panel.',
          ephemeral: true
        });
        return;
      }

      const targetChannel =
        (interaction.options.getChannel('channel') as TextChannel) ||
        (interaction.channel as TextChannel);

      if (!targetChannel || !targetChannel.isTextBased()) {
        await interaction.reply({
          content: '❌ Invalid text channel selected.',
          ephemeral: true
        });
        return;
      }

      const embed = ticketService.buildPanelEmbed(guild.name);
      const row = ticketService.buildPanelActionRow();

      await targetChannel.send({
        embeds: [embed],
        components: [row]
      });

      await interaction.reply({
        content: `✅ Ticket panel successfully deployed in <#${targetChannel.id}>!`,
        ephemeral: true
      });

    } else if (sub === 'close') {
      const channel = interaction.channel as TextChannel;
      const record = ticketService.getTicketByChannel(channel.id);
      if (!record) {
        await interaction.reply({
          content: '❌ This channel is not an active ticket.',
          ephemeral: true
        });
        return;
      }
      await ticketService.closeTicket(interaction as any);

    } else if (sub === 'transcript') {
      const channel = interaction.channel as TextChannel;
      const record = ticketService.getTicketByChannel(channel.id);
      if (!record) {
        await interaction.reply({
          content: '❌ This channel is not an active ticket.',
          ephemeral: true
        });
        return;
      }
      await ticketService.exportTranscript(interaction as any);
    }
  }
};

