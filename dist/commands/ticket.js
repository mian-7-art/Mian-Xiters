"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ticketCommand = void 0;
const discord_js_1 = require("discord.js");
const TicketService_js_1 = require("../services/TicketService.js");
exports.ticketCommand = {
    data: new discord_js_1.SlashCommandBuilder()
        .setName('ticket')
        .setDescription('Manage the server ticket system')
        .addSubcommand(sub => sub
        .setName('panel')
        .setDescription('Send the official interactive ticket creation panel')
        .addChannelOption(opt => opt
        .setName('channel')
        .setDescription('Channel to send the ticket panel to (defaults to current channel)')
        .setRequired(false)))
        .addSubcommand(sub => sub
        .setName('close')
        .setDescription('Close the current ticket channel'))
        .addSubcommand(sub => sub
        .setName('transcript')
        .setDescription('Export the transcript of the current ticket')),
    async execute(interaction) {
        const sub = interaction.options.getSubcommand();
        const guild = interaction.guild;
        if (!guild)
            return;
        if (sub === 'panel') {
            // Admin / Staff check
            const member = await guild.members.fetch(interaction.user.id);
            const isStaff = member.permissions.has(discord_js_1.PermissionFlagsBits.ManageChannels) ||
                member.permissions.has(discord_js_1.PermissionFlagsBits.Administrator) ||
                interaction.user.id === guild.ownerId;
            if (!isStaff) {
                await interaction.reply({
                    content: '❌ You do not have permission to deploy the ticket panel.',
                    ephemeral: true
                });
                return;
            }
            const targetChannel = interaction.options.getChannel('channel') ||
                interaction.channel;
            if (!targetChannel || !targetChannel.isTextBased()) {
                await interaction.reply({
                    content: '❌ Invalid text channel selected.',
                    ephemeral: true
                });
                return;
            }
            const embed = TicketService_js_1.ticketService.buildPanelEmbed(guild.name);
            const row = TicketService_js_1.ticketService.buildPanelActionRow();
            await targetChannel.send({
                embeds: [embed],
                components: [row]
            });
            await interaction.reply({
                content: `✅ Ticket panel successfully deployed in <#${targetChannel.id}>!`,
                ephemeral: true
            });
        }
        else if (sub === 'close') {
            const channel = interaction.channel;
            const record = TicketService_js_1.ticketService.getTicketByChannel(channel.id);
            if (!record) {
                await interaction.reply({
                    content: '❌ This channel is not an active ticket.',
                    ephemeral: true
                });
                return;
            }
            await TicketService_js_1.ticketService.closeTicket(interaction);
        }
        else if (sub === 'transcript') {
            const channel = interaction.channel;
            const record = TicketService_js_1.ticketService.getTicketByChannel(channel.id);
            if (!record) {
                await interaction.reply({
                    content: '❌ This channel is not an active ticket.',
                    ephemeral: true
                });
                return;
            }
            await TicketService_js_1.ticketService.exportTranscript(interaction);
        }
    }
};
