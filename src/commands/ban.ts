import {
  SlashCommandBuilder,
  ChatInputCommandInteraction,
  EmbedBuilder,
  PermissionFlagsBits,
  TextChannel,
  GuildMember
} from 'discord.js';
import { BOT_CONFIG, THRESHOLDS } from '../config/config.js';
import { SUPREME_OWNERS, whitelistManager } from '../config/whitelist.js';
import { rateLimiter } from '../utils/rateLimiter.js';
import { Logger } from '../utils/logger.js';

export const banCommand = {
  data: new SlashCommandBuilder()
    .setName('ban')
    .setDescription('🔨 MIAN XITERS — Executive Server Ban Engine (Server Owner Only)')
    .addUserOption(opt =>
      opt
        .setName('user')
        .setDescription('Target member to ban from this server')
        .setRequired(true)
    )
    .addStringOption(opt =>
      opt
        .setName('reason')
        .setDescription('Official reason for this executive ban')
        .setRequired(false)
    )
    .addIntegerOption(opt =>
      opt
        .setName('delete_messages')
        .setDescription('Delete message history of the banned user')
        .setRequired(false)
        .addChoices(
          { name: "Don't delete any messages", value: 0 },
          { name: 'Previous 24 Hours', value: 86400 },
          { name: 'Previous 7 Days', value: 604800 }
        )
    ),

  async execute(interaction: ChatInputCommandInteraction): Promise<any> {
    const guild = interaction.guild;
    if (!guild) {
      return interaction.reply({
        content: '❌ This command can only be executed within a Discord server.',
        ephemeral: true
      });
    }

    // 1. STRICT PERMISSION CHECK: OWNER ONLY - LOW ROLES / NON-OWNERS BLOCKED
    const isOwner = interaction.user.id === guild.ownerId || SUPREME_OWNERS.has(interaction.user.id);
    if (!isOwner) {
      const deniedEmbed = new EmbedBuilder()
        .setColor(BOT_CONFIG.colors.danger)
        .setTitle('⛔ Access Denied | Executive Permission Required')
        .setDescription(
          `**Unauthorized Execution:**\n` +
          `Only the **Server Owner** (<@${guild.ownerId}>) has permission to execute the \`/ban\` command.\n\n` +
          `Lower roles, administrators, and moderators are strictly prohibited from using this command.`
        )
        .addFields({
          name: '🛡️ Security Policy',
          value: 'Zero-Trust Protocol Active • Unauthorized attempts are audited.'
        })
        .setFooter({ text: `${BOT_CONFIG.name} Security Shield` })
        .setTimestamp();

      Logger.warn(
        `[SECURITY] Unauthorized /ban attempt by ${interaction.user.tag} (${interaction.user.id}) in guild ${guild.name} (${guild.id})`
      );

      return interaction.reply({ embeds: [deniedEmbed], ephemeral: true });
    }

    const targetUser = interaction.options.getUser('user', true);
    const reason = interaction.options.getString('reason') || 'No reason provided by Server Owner';
    const deleteMessageSeconds = interaction.options.getInteger('delete_messages') ?? 0;

    // 2. SELF-BAN & BOT-BAN VALIDATIONS
    if (targetUser.id === interaction.user.id) {
      return interaction.reply({
        content: '❌ **Operation Aborted:** You cannot ban yourself.',
        ephemeral: true
      });
    }

    if (targetUser.id === guild.ownerId || SUPREME_OWNERS.has(targetUser.id)) {
      return interaction.reply({
        content: '🛡️ **Immunity Triggered:** The Server Owner and Supreme Authority accounts cannot be banned.',
        ephemeral: true
      });
    }

    if (targetUser.id === guild.client.user?.id) {
      return interaction.reply({
        content: '❌ **Operation Aborted:** You cannot ban the security bot.',
        ephemeral: true
      });
    }

    // 3. BOT PERMISSION & HIERARCHY CHECK
    const botMember = guild.members.me;
    if (!botMember || !botMember.permissions.has(PermissionFlagsBits.BanMembers)) {
      return interaction.reply({
        content: '❌ **Configuration Error:** The bot lacks the `Ban Members` permission in this server.',
        ephemeral: true
      });
    }

    let targetMember: GuildMember | null = null;
    try {
      targetMember = await guild.members.fetch(targetUser.id);
    } catch {
      targetMember = null;
    }

    if (targetMember && !targetMember.bannable) {
      return interaction.reply({
        content:
          `❌ **Role Hierarchy Error:** Cannot ban <@${targetUser.id}> because their highest role is equal to or higher than the bot's highest role.\n` +
          `*Please move the **${BOT_CONFIG.name}** role higher in Server Settings > Roles.*`,
        ephemeral: true
      });
    }

    // 4. RATE LIMIT & BAN QUOTA TRACKING (HOW MANY BANS LEFT)
    const key = `${guild.id}:${interaction.user.id}:ban`;
    const limit = THRESHOLDS.ban.limit; // e.g. 3
    const windowMs = THRESHOLDS.ban.windowMs; // 10s

    const { count, remaining } = rateLimiter.track(key, limit, windowMs, {
      targetId: targetUser.id,
      targetTag: targetUser.tag,
      reason
    });

    // 5. EXECUTE THE BAN VIA GUILD BANS API
    try {
      await guild.bans.create(targetUser.id, {
        reason: `[${BOT_CONFIG.name} Executive Ban] By ${interaction.user.tag}: ${reason}`,
        deleteMessageSeconds
      });

      Logger.info(
        `[BAN] Successfully banned ${targetUser.tag} (${targetUser.id}) by Owner ${interaction.user.tag} (${interaction.user.id}) in ${guild.name}`
      );

      // 6. PUBLIC CHAT EMBED DISPLAY
      const banEmbed = new EmbedBuilder()
        .setColor(BOT_CONFIG.colors.primary)
        .setTitle('🔨 MEMBER BANNED')
        .setDescription(`**${BOT_CONFIG.slogan}**\nExecutive punishment enforced by Server Owner.`)
        .setThumbnail(targetUser.displayAvatarURL({ size: 256 }))
        .addFields(
          {
            name: '👤 Banned User',
            value: `• **Mention:** <@${targetUser.id}>\n• **Tag:** \`${targetUser.tag}\`\n• **ID:** \`${targetUser.id}\``,
            inline: false
          },
          {
            name: '👑 Executed By',
            value: `<@${interaction.user.id}> \`(${interaction.user.tag})\`\n*(Server Owner)*`,
            inline: true
          },
          {
            name: '⚡ Bans Remaining',
            value: `**${remaining}** of **${limit}** rapid bans left\n*(Safety window: ${windowMs / 1000}s)*`,
            inline: true
          },
          {
            name: '📝 Reason',
            value: `\`${reason}\``,
            inline: false
          }
        )
        .setFooter({
          text: `${BOT_CONFIG.name} Enterprise Defense • Velocity: ${count}/${limit}`,
          iconURL: guild.iconURL() || undefined
        })
        .setTimestamp();

      // Reply in chat publicly so everyone sees it
      await interaction.reply({ embeds: [banEmbed] });

      // 7. DISPATCH LOG TO DESIGNATED SECURITY LOG CHANNEL (IF CONFIGURED)
      const logChannelId = whitelistManager.getLogChannel(guild.id);
      if (logChannelId && logChannelId !== interaction.channelId) {
        const logChannel = guild.channels.cache.get(logChannelId) as TextChannel | undefined;
        if (logChannel && logChannel.isTextBased()) {
          await logChannel.send({ embeds: [banEmbed] }).catch(() => {});
        }
      }
    } catch (banError: any) {
      Logger.error(`[BAN_ERROR] Failed to ban ${targetUser.id}: ${banError.message}`, banError);
      return interaction.reply({
        content: `❌ **Ban Execution Failed:** ${banError.message || 'An unexpected API error occurred.'}`,
        ephemeral: true
      });
    }
  }
};
