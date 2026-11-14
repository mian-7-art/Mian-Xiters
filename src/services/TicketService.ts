import fs from 'fs';
import path from 'path';
import {
  ChannelType,
  PermissionsBitField,
  ButtonInteraction,
  CommandInteraction,
  EmbedBuilder,
  ActionRowBuilder,
  ButtonBuilder,
  ButtonStyle,
  TextChannel,
  Guild,
  User,
  GuildMember,
  AttachmentBuilder
} from 'discord.js';
import { BOT_CONFIG } from '../config/config.js';
import { Logger } from '../utils/logger.js';
import { whitelistManager } from '../config/whitelist.js';

interface TicketRecord {
  id: number;
  channelId: string;
  guildId: string;
  userId: string;
  userTag: string;
  status: 'open' | 'closed';
  createdAt: number;
  closedAt?: number;
  claimedBy?: string;
}

interface TicketData {
  counter: Record<string, number>;
  tickets: Record<string, TicketRecord>; // key: channelId
}

const DATA_DIR = path.resolve(process.cwd(), 'data');
const TICKETS_FILE = path.join(DATA_DIR, 'tickets.json');

class TicketService {
  private data: TicketData = {
    counter: {},
    tickets: {}
  };

  constructor() {
    this.init();
  }

  private init(): void {
    try {
      if (!fs.existsSync(DATA_DIR)) {
        fs.mkdirSync(DATA_DIR, { recursive: true });
      }
      if (fs.existsSync(TICKETS_FILE)) {
        const raw = fs.readFileSync(TICKETS_FILE, 'utf-8');
        this.data = JSON.parse(raw);
      } else {
        this.save();
      }
    } catch (err: any) {
      Logger.error(`[TICKET_SERVICE] Failed to initialize: ${err.message}`);
      this.data = { counter: {}, tickets: {} };
    }
  }

  private save(): void {
    try {
      const temp = `${TICKETS_FILE}.tmp`;
      fs.writeFileSync(temp, JSON.stringify(this.data, null, 2), 'utf-8');
      fs.renameSync(temp, TICKETS_FILE);
    } catch (err: any) {
      Logger.error(`[TICKET_SERVICE] Failed to save: ${err.message}`);
    }
  }

  private getNextTicketNumber(guildId: string): number {
    if (!this.data.counter[guildId]) {
      this.data.counter[guildId] = 0;
    }
    this.data.counter[guildId]++;
    this.save();
    return this.data.counter[guildId];
  }

  public getTicketByChannel(channelId: string): TicketRecord | undefined {
    return this.data.tickets[channelId];
  }

  public getUserActiveTicket(guildId: string, userId: string): TicketRecord | undefined {
    return Object.values(this.data.tickets).find(
      t => t.guildId === guildId && t.userId === userId && t.status === 'open'
    );
  }

  public buildPanelEmbed(guildName: string): EmbedBuilder {
    return new EmbedBuilder()
      .setColor(BOT_CONFIG.colors.primary)
      .setTitle(`🎫 ${guildName} — OFFICIAL SUPPORT & SALES DESK`)
      .setDescription(
        `Welcome to our official ticket portal!\n\n` +
        `**Need assistance, purchasing a key, or encountering technical issues?**\n` +
        `Click the button below to open a private ticket. Our staff team will respond promptly.\n\n` +
        `> 🛒 **Purchases & Orders:** Keys, bypasses, panels, custom packages.\n` +
        `> 🛠️ **Technical Support:** Setup guides, loader assistance, emulator tuning.\n` +
        `> 💡 **General Inquiries:** Partnerships, feedback, questions.\n\n` +
        `*Please be patient after opening your ticket. Avoid spamming staff.*`
      )
      .setFooter({
        text: `${guildName} • Powered by ${BOT_CONFIG.name}`,
        iconURL: undefined
      })
      .setTimestamp();
  }

  public buildPanelActionRow(): ActionRowBuilder<ButtonBuilder> {
    const button = new ButtonBuilder()
      .setCustomId('ticket_create')
      .setLabel('Create Ticket')
      .setEmoji('🎫')
      .setStyle(ButtonStyle.Success);

    return new ActionRowBuilder<ButtonBuilder>().addComponents(button);
  }

  public async createTicket(interaction: ButtonInteraction): Promise<void> {
    const guild = interaction.guild;
    const user = interaction.user;
    if (!guild) return;

    await interaction.deferReply({ ephemeral: true });

    // Check if user already has an active open ticket
    const existing = this.getUserActiveTicket(guild.id, user.id);
    if (existing) {
      const channel = guild.channels.cache.get(existing.channelId);
      if (channel) {
        await interaction.editReply({
          content: `⚠️ You already have an open ticket in <#${existing.channelId}>! Please resolve it before opening a new one.`
        });
        return;
      }
    }

    const ticketNumber = this.getNextTicketNumber(guild.id);
    const padded = String(ticketNumber).padStart(4, '0');
    const channelName = `ticket-${padded}`;

    // Find staff and owner roles to grant visibility
    const staffRoles = guild.roles.cache.filter(r => {
      const n = r.name.toLowerCase();
      return (
        n.includes('owner') ||
        n.includes('support') ||
        n.includes('developer') ||
        n.includes('admin') ||
        n.includes('moderator')
      );
    });

    const permissionOverwrites: any[] = [
      {
        id: guild.roles.everyone.id,
        deny: [PermissionsBitField.Flags.ViewChannel]
      },
      {
        id: user.id,
        allow: [
          PermissionsBitField.Flags.ViewChannel,
          PermissionsBitField.Flags.SendMessages,
          PermissionsBitField.Flags.ReadMessageHistory,
          PermissionsBitField.Flags.AttachFiles,
          PermissionsBitField.Flags.EmbedLinks
        ]
      },
      {
        id: guild.members.me?.id || BOT_CONFIG.clientId,
        allow: [
          PermissionsBitField.Flags.ViewChannel,
          PermissionsBitField.Flags.SendMessages,
          PermissionsBitField.Flags.ManageChannels,
          PermissionsBitField.Flags.ManageMessages,
          PermissionsBitField.Flags.ReadMessageHistory,
          PermissionsBitField.Flags.AttachFiles
        ]
      }
    ];

    for (const r of staffRoles.values()) {
      permissionOverwrites.push({
        id: r.id,
        allow: [
          PermissionsBitField.Flags.ViewChannel,
          PermissionsBitField.Flags.SendMessages,
          PermissionsBitField.Flags.ReadMessageHistory,
          PermissionsBitField.Flags.AttachFiles,
          PermissionsBitField.Flags.EmbedLinks
        ]
      });
    }

    // Look for category '🎟️ TICKETS' or '🎟️ TICKET'
    let parentCategory = guild.channels.cache.find(
      c => c.type === ChannelType.GuildCategory && c.name.toLowerCase().includes('ticket')
    );

    try {
      const ticketChannel = await guild.channels.create({
        name: channelName,
        type: ChannelType.GuildText,
        parent: parentCategory?.id,
        topic: `Ticket #${padded} opened by ${user.tag} (${user.id})`,
        permissionOverwrites
      });

      // Save ticket state
      this.data.tickets[ticketChannel.id] = {
        id: ticketNumber,
        channelId: ticketChannel.id,
        guildId: guild.id,
        userId: user.id,
        userTag: user.tag,
        status: 'open',
        createdAt: Date.now()
      };
      this.save();

      // Post initial ticket greeting & control buttons
      const ticketEmbed = new EmbedBuilder()
        .setColor(BOT_CONFIG.colors.primary)
        .setTitle(`🎫 TICKET #${padded} — ${guild.name}`)
        .setDescription(
          `Welcome <@${user.id}>! Staff has been notified and will assist you shortly.\n\n` +
          `**Please provide details regarding your inquiry:**\n` +
          `• For **purchases**: state your preferred payment method & duration.\n` +
          `• For **support**: describe the issue, emulator version, and attach screenshots if applicable.\n\n` +
          `━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━`
        )
        .addFields(
          { name: '👤 Opener', value: `<@${user.id}> (\`${user.tag}\`)`, inline: true },
          { name: '🆔 User ID', value: `\`${user.id}\``, inline: true },
          { name: '⏰ Created', value: `<t:${Math.floor(Date.now() / 1000)}:R>`, inline: true }
        )
        .setFooter({ text: `${guild.name} Ticket System • Use buttons below to manage` })
        .setTimestamp();

      const actionRow = new ActionRowBuilder<ButtonBuilder>().addComponents(
        new ButtonBuilder()
          .setCustomId('ticket_close')
          .setLabel('Close')
          .setEmoji('🔒')
          .setStyle(ButtonStyle.Danger),
        new ButtonBuilder()
          .setCustomId('ticket_claim')
          .setLabel('Claim')
          .setEmoji('📋')
          .setStyle(ButtonStyle.Primary)
      );

      await ticketChannel.send({
        content: `👋 Hello <@${user.id}>, welcome to your private ticket! Staff will be with you shortly.`,
        embeds: [ticketEmbed],
        components: [actionRow]
      });

      await interaction.editReply({
        content: `✅ Your ticket has been created! Head over to <#${ticketChannel.id}>.`
      });

      Logger.info(`[TICKET] Created ticket #${padded} (${ticketChannel.id}) for ${user.tag} in ${guild.name}`);
    } catch (err: any) {
      Logger.error(`[TICKET_ERROR] Failed to create ticket channel: ${err.message}`);
      await interaction.editReply({
        content: `❌ Failed to create ticket channel: ${err.message}`
      });
    }
  }

  public async closeTicket(interaction: ButtonInteraction): Promise<void> {
    const channel = interaction.channel as TextChannel;
    if (!channel) return;

    const record = this.data.tickets[channel.id];
    if (!record) {
      await interaction.reply({
        content: '❌ This channel is not recognized as an active ticket.',
        ephemeral: true
      });
      return;
    }

    if (record.status === 'closed') {
      await interaction.reply({
        content: '⚠️ This ticket is already closed.',
        ephemeral: true
      });
      return;
    }

    await interaction.deferReply();

    record.status = 'closed';
    record.closedAt = Date.now();
    this.save();

    // Revoke send permission from ticket opener
    try {
      await channel.permissionOverwrites.edit(record.userId, {
        SendMessages: false,
        ViewChannel: true
      });
    } catch (err: any) {
      Logger.warn(`Could not revoke ticket opener permissions: ${err.message}`);
    }

    const closedEmbed = new EmbedBuilder()
      .setColor(BOT_CONFIG.colors.warning)
      .setTitle('🔒 TICKET CLOSED')
      .setDescription(
        `This ticket was closed by <@${interaction.user.id}>.\n` +
        `Staff can reopen this ticket, export a transcript, or delete the channel permanently.`
      )
      .setTimestamp();

    const controlRow = new ActionRowBuilder<ButtonBuilder>().addComponents(
      new ButtonBuilder()
        .setCustomId('ticket_reopen')
        .setLabel('Reopen')
        .setEmoji('🔓')
        .setStyle(ButtonStyle.Secondary),
      new ButtonBuilder()
        .setCustomId('ticket_transcript')
        .setLabel('Transcript')
        .setEmoji('📜')
        .setStyle(ButtonStyle.Primary),
      new ButtonBuilder()
        .setCustomId('ticket_delete')
        .setLabel('Delete')
        .setEmoji('🗑️')
        .setStyle(ButtonStyle.Danger)
    );

    await interaction.editReply({
      embeds: [closedEmbed],
      components: [controlRow]
    });
  }

  public async reopenTicket(interaction: ButtonInteraction): Promise<void> {
    const channel = interaction.channel as TextChannel;
    if (!channel) return;

    const record = this.data.tickets[channel.id];
    if (!record) return;

    await interaction.deferReply();

    record.status = 'open';
    this.save();

    try {
      await channel.permissionOverwrites.edit(record.userId, {
        SendMessages: true,
        ViewChannel: true
      });
    } catch (err: any) {
      Logger.warn(`Could not restore ticket opener permissions: ${err.message}`);
    }

    const reopenEmbed = new EmbedBuilder()
      .setColor(BOT_CONFIG.colors.success)
      .setTitle('🔓 TICKET REOPENED')
      .setDescription(`Ticket was reopened by <@${interaction.user.id}>.`)
      .setTimestamp();

    await interaction.editReply({ embeds: [reopenEmbed] });
  }

  public async claimTicket(interaction: ButtonInteraction): Promise<void> {
    const channel = interaction.channel as TextChannel;
    if (!channel) return;

    const record = this.data.tickets[channel.id];
    if (!record) return;

    if (record.claimedBy) {
      await interaction.reply({
        content: `⚠️ This ticket has already been claimed by <@${record.claimedBy}>.`,
        ephemeral: true
      });
      return;
    }

    record.claimedBy = interaction.user.id;
    this.save();

    const claimEmbed = new EmbedBuilder()
      .setColor(BOT_CONFIG.colors.success)
      .setTitle('📋 TICKET CLAIMED')
      .setDescription(`<@${interaction.user.id}> has claimed this ticket and will handle your request!`)
      .setTimestamp();

    await interaction.reply({ embeds: [claimEmbed] });
  }

  public async generateTranscript(channel: TextChannel): Promise<AttachmentBuilder> {
    const fetched = await channel.messages.fetch({ limit: 100 });
    const sorted = [...fetched.values()].reverse();

    let transcript = `====================================================\n`;
    transcript += `TRANSCRIPT FOR: #${channel.name} (${channel.id})\n`;
    transcript += `GENERATED AT: ${new Date().toISOString()}\n`;
    transcript += `====================================================\n\n`;

    for (const msg of sorted) {
      const time = msg.createdAt.toISOString();
      transcript += `[${time}] ${msg.author.tag} (${msg.author.id}):\n`;
      if (msg.content) transcript += `  ${msg.content}\n`;
      if (msg.embeds.length > 0) {
        msg.embeds.forEach(e => {
          transcript += `  [EMBED] ${e.title || ''}: ${e.description || ''}\n`;
        });
      }
      if (msg.attachments.size > 0) {
        msg.attachments.forEach(a => {
          transcript += `  [ATTACHMENT] ${a.url}\n`;
        });
      }
      transcript += `\n`;
    }

    const buffer = Buffer.from(transcript, 'utf-8');
    return new AttachmentBuilder(buffer, { name: `${channel.name}-transcript.txt` });
  }

  public async exportTranscript(interaction: ButtonInteraction): Promise<void> {
    const channel = interaction.channel as TextChannel;
    if (!channel) return;

    await interaction.deferReply();
    const attachment = await this.generateTranscript(channel);
    await interaction.editReply({
      content: `📜 Transcript exported for **#${channel.name}**:`,
      files: [attachment]
    });
  }

  public async deleteTicket(interaction: ButtonInteraction): Promise<void> {
    const channel = interaction.channel as TextChannel;
    const guild = interaction.guild;
    if (!channel || !guild) return;

    const record = this.data.tickets[channel.id];

    await interaction.reply({
      content: '🗑️ Ticket will be deleted in 5 seconds... Saving transcript to logs.'
    });

    try {
      const attachment = await this.generateTranscript(channel);

      // Send to log channel if one is configured
      const logChannelId = whitelistManager.getLogChannel(guild.id);
      if (logChannelId) {
        const logChannel = guild.channels.cache.get(logChannelId) as TextChannel;
        if (logChannel && logChannel.isTextBased()) {
          const logEmbed = new EmbedBuilder()
            .setColor(BOT_CONFIG.colors.dark)
            .setTitle(`🗑️ TICKET CLOSED & ARCHIVED: #${channel.name}`)
            .addFields(
              { name: '👤 Opener', value: `<@${record?.userId || 'unknown'}>`, inline: true },
              { name: '🔨 Deleted By', value: `<@${interaction.user.id}>`, inline: true },
              { name: '🆔 Channel ID', value: `\`${channel.id}\``, inline: true }
            )
            .setFooter({ text: `${BOT_CONFIG.name} Ticket Archive` })
            .setTimestamp();

          await logChannel.send({ embeds: [logEmbed], files: [attachment] }).catch(() => {});
        }
      }

      delete this.data.tickets[channel.id];
      this.save();

      setTimeout(async () => {
        await channel.delete('Ticket closed and deleted').catch(() => {});
      }, 5000);
    } catch (err: any) {
      Logger.error(`[TICKET_DELETE_ERROR] ${err.message}`);
    }
  }
}

export const ticketService = new TicketService();

