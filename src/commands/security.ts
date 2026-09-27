import {
  SlashCommandBuilder,
  ChatInputCommandInteraction,
  EmbedBuilder,
  PermissionFlagsBits,
  TextChannel,
  ChannelType
} from 'discord.js';
import { BOT_CONFIG, THRESHOLDS } from '../config/config.js';
import { whitelistManager } from '../config/whitelist.js';
import { snapshotService } from '../services/SnapshotService.js';
import { isolationService } from '../services/isolationService.js';
import { Logger } from '../utils/logger.js';

export const securityCommand = {
  data: new SlashCommandBuilder()
    .setName('security')
    .setDescription('MIAN XITERS — Ultimate Autonomous Security Control Center')
    .addSubcommand(sub =>
      sub
        .setName('status')
        .setDescription('View live anti-nuke defense status and health metrics')
    )
    .addSubcommandGroup(group =>
      group
        .setName('whitelist')
        .setDescription('Manage zero-trust user ID whitelist (Server Owner Only)')
        .addSubcommand(sub =>
          sub
            .setName('add')
            .setDescription('Add a trusted user to the whitelist')
            .addUserOption(opt =>
              opt.setName('user').setDescription('User to whitelist').setRequired(true)
            )
        )
        .addSubcommand(sub =>
          sub
            .setName('remove')
            .setDescription('Remove a user from the whitelist')
            .addUserOption(opt =>
              opt.setName('user').setDescription('User to remove').setRequired(true)
            )
        )
        .addSubcommand(sub =>
          sub
            .setName('list')
            .setDescription('List all whitelisted user IDs')
        )
    )
    .addSubcommand(sub =>
      sub
        .setName('lockdown')
        .setDescription('Toggle emergency server lockdown')
        .addStringOption(opt =>
          opt
            .setName('action')
            .setDescription('Lockdown state')
            .setRequired(true)
            .addChoices(
              { name: 'Enable Emergency Lockdown', value: 'enable' },
              { name: 'Disable / Release Lockdown', value: 'disable' }
            )
        )
    )
    .addSubcommand(sub =>
      sub
        .setName('setup')
        .setDescription('Auto-deploy #mian-xiters-logs channel & @Quarantine role')
    )
    .addSubcommand(sub =>
      sub
        .setName('snapshot')
        .setDescription('Manage server baseline backup')
        .addStringOption(opt =>
          opt
            .setName('action')
            .setDescription('Snapshot action')
            .setRequired(true)
            .addChoices(
              { name: 'Capture Current Baseline', value: 'create' },
              { name: 'View Saved Baseline Info', value: 'info' },
              { name: 'Auto-Reconstruct Missing Channels', value: 'restore_channels' },
              { name: 'Auto-Reconstruct Missing Roles', value: 'restore_roles' }
            )
        )
    )
    .addSubcommand(sub =>
      sub
        .setName('quarantine')
        .setDescription('Manually isolate or release a member')
        .addUserOption(opt =>
          opt.setName('user').setDescription('Target member').setRequired(true)
        )
        .addStringOption(opt =>
          opt
            .setName('action')
            .setDescription('Quarantine action')
            .setRequired(true)
            .addChoices(
              { name: 'Apply Quarantine', value: 'apply' },
              { name: 'Release Quarantine', value: 'release' }
            )
        )
    )
    .addSubcommand(sub =>
      sub
        .setName('panic')
        .setDescription('Nuclear Panic Mode: Neutralize admin perms & lock server (Owner Only)')
        .addStringOption(opt =>
          opt
            .setName('action')
            .setDescription('Trigger or restore panic state')
            .setRequired(true)
            .addChoices(
              { name: 'Engage Nuclear Panic (Strip Admin Perms & Lock)', value: 'trigger' },
              { name: 'Restore From Snapshot Baseline', value: 'restore' }
            )
        )
    ),

  async execute(interaction: ChatInputCommandInteraction): Promise<any> {
    const guild = interaction.guild;
    if (!guild) {
      return interaction.reply({ content: '❌ This command can only be executed in a guild.', ephemeral: true });
    }

    const isOwner = interaction.user.id === guild.ownerId;
    const subGroup = interaction.options.getSubcommandGroup(false);
    const subCommand = interaction.options.getSubcommand();

    // 1. STATUS COMMAND
    if (subCommand === 'status' && !subGroup) {
      const whitelistedUsers = Object.keys(whitelistManager.getWhitelist(guild.id));
      const isLockdown = whitelistManager.isLockdown(guild.id);
      const isPanic = whitelistManager.isPanicMode(guild.id);
      const logChannelId = whitelistManager.getLogChannel(guild.id);
      const quarantineRoleId = whitelistManager.getQuarantineRole(guild.id);
      const snapshot = snapshotService.getSnapshot(guild.id);

      const embed = new EmbedBuilder()
        .setColor(BOT_CONFIG.colors.primary)
        .setTitle(`🛡️ ${BOT_CONFIG.name} — SECURITY CORE STATUS`)
        .setDescription(`**${BOT_CONFIG.slogan}**\nEnterprise-Grade Autonomous Gateway Defense Engine\nDeveloper: **${BOT_CONFIG.author}**`)
        .addFields(
          {
            name: '⚡ Core Engine',
            value: `• **Gateway Latency:** \`${guild.client.ws.ping}ms\`\n• **Threat State:** ${isPanic ? '🛑 **PANIC MODE ACTIVE**' : isLockdown ? '🔴 **LOCKDOWN ACTIVE**' : '🟢 **NORMAL DEFENSE**'}\n• **Server Owner:** <@${guild.ownerId}>`,
            inline: false
          },
          {
            name: '🛡️ Active Defense Shields',
            value: [
              '✅ **Anti-Channel Nuke & Instant Re-Creation Pipeline**',
              '✅ **Anti-Role Nuke & Admin Escalation Reversion**',
              '✅ **Zero-Trust Autonomous Killswitch (Priority REST)**',
              '✅ **Rogue Bot & Integration Dual-Ban Engine**',
              '✅ **Swarm Join (3s Trigger) & 7-Day Raid Shield**',
              '✅ **Anti-Tamper Guild Settings & Vanity Reclaim Engine**',
              '✅ **Global Guild Circuit Breakers (Anti-Sybil)**',
              '✅ **Infrastructure Self-Healing (Logs & Quarantine)**'
            ].join('\n'),
            inline: false
          },
          {
            name: '⚙️ Configuration',
            value: `• **Whitelisted Users:** \`${whitelistedUsers.length}\`\n• **Log Channel:** ${logChannelId ? `<#${logChannelId}>` : '⚠️ *Not configured (Run `/security setup`)*'}\n• **Quarantine Role:** ${quarantineRoleId ? `<@&${quarantineRoleId}>` : '⚠️ *Not configured*'}\n• **Last Snapshot:** ${snapshot ? `<t:${Math.floor(snapshot.timestamp / 1000)}:R>` : '⚠️ *Pending*'}`
          }
        )
        .setFooter({ text: `${BOT_CONFIG.name} v${BOT_CONFIG.version} • Autonomous Defense` })
        .setTimestamp();

      return interaction.reply({ embeds: [embed] });
    }

    // 2. WHITELIST COMMANDS (STRICT OWNER ONLY)
    if (subGroup === 'whitelist') {
      if (!isOwner) {
        return interaction.reply({
          content: '⛔ **Access Denied:** Only the **Server Owner** has authority to modify the zero-trust whitelist.',
          ephemeral: true
        });
      }

      if (subCommand === 'add') {
        const targetUser = interaction.options.getUser('user', true);
        if (targetUser.id === guild.ownerId || targetUser.id === guild.client.user?.id) {
          return interaction.reply({
            content: 'ℹ️ The server owner and bot are permanently immune and whitelisted by default.',
            ephemeral: true
          });
        }

        const added = whitelistManager.setWhitelist(guild.id, targetUser.id, { isFullSuperAdmin: true });
        if (!added) {
          return interaction.reply({
            content: `⚠️ <@${targetUser.id}> is already on the security whitelist.`,
            ephemeral: true
          });
        }

        return interaction.reply({
          content: `✅ **Granted:** Added <@${targetUser.id}> (\`${targetUser.id}\`) to the trusted security whitelist.`,
          ephemeral: false
        });
      }

      if (subCommand === 'remove') {
        const targetUser = interaction.options.getUser('user', true);
        const removed = whitelistManager.removeWhitelist(guild.id, targetUser.id);
        if (!removed) {
          return interaction.reply({
            content: `⚠️ <@${targetUser.id}> is not in the whitelist.`,
            ephemeral: true
          });
        }

        return interaction.reply({
          content: `🔒 **Revoked:** Removed <@${targetUser.id}> (\`${targetUser.id}\`) from the security whitelist.`,
          ephemeral: false
        });
      }

      if (subCommand === 'list') {
        const listObj = whitelistManager.getWhitelist(guild.id);
        const userIds = Object.keys(listObj);
        const listFormatted = userIds.length > 0
          ? userIds.map(id => `• <@${id}> (\`${id}\`) [Full Admin]`).join('\n')
          : '*No additional users whitelisted.*';

        const embed = new EmbedBuilder()
          .setColor(BOT_CONFIG.colors.primary)
          .setTitle('📋 Zero-Trust Whitelist Matrix')
          .setDescription(`**Owner (Immune):** <@${guild.ownerId}>\n**Bot (Immune):** <@${guild.client.user?.id}>\n\n**Explicitly Whitelisted Admins:**\n${listFormatted}`)
          .setFooter({ text: 'Role-based whitelisting is strictly prohibited for security integrity.' });

        return interaction.reply({ embeds: [embed], ephemeral: true });
      }
    }

    // 3. LOCKDOWN TOGGLE
    if (subCommand === 'lockdown') {
      if (!isOwner && !interaction.memberPermissions?.has(PermissionFlagsBits.Administrator)) {
        return interaction.reply({ content: '⛔ Administrator permission required.', ephemeral: true });
      }

      await interaction.deferReply();
      const action = interaction.options.getString('action', true);

      if (action === 'enable') {
        whitelistManager.setLockdown(guild.id, true);
        const textChannels = guild.channels.cache.filter(c => c.isTextBased() && 'permissionOverwrites' in c);

        for (const [, ch] of textChannels) {
          const channel = ch as TextChannel;
          await channel.permissionOverwrites.edit(guild.roles.everyone, {
            SendMessages: false,
            AddReactions: false
          }).catch(() => {});
        }

        return interaction.editReply({
          content: '🚨 **EMERGENCY LOCKDOWN ACTIVATED:** All text channels locked for `@everyone`. Invites secured.'
        });
      } else {
        whitelistManager.setLockdown(guild.id, false);
        const textChannels = guild.channels.cache.filter(c => c.isTextBased() && 'permissionOverwrites' in c);

        for (const [, ch] of textChannels) {
          const channel = ch as TextChannel;
          await channel.permissionOverwrites.edit(guild.roles.everyone, {
            SendMessages: null,
            AddReactions: null
          }).catch(() => {});
        }

        return interaction.editReply({
          content: '🟢 **LOCKDOWN RELEASED:** Normal server operations and messaging restored.'
        });
      }
    }

    // 4. ONE-CLICK SECURITY SETUP
    if (subCommand === 'setup') {
      if (!isOwner && !interaction.memberPermissions?.has(PermissionFlagsBits.Administrator)) {
        return interaction.reply({ content: '⛔ Administrator permission required.', ephemeral: true });
      }

      await interaction.deferReply();

      // Create or find #mian-xiters-logs
      let logChannel = guild.channels.cache.find(c => c.name === 'mian-xiters-logs') as TextChannel;
      if (!logChannel) {
        logChannel = await guild.channels.create({
          name: 'mian-xiters-logs',
          type: ChannelType.GuildText,
          permissionOverwrites: [
            {
              id: guild.roles.everyone.id,
              deny: [PermissionFlagsBits.ViewChannel]
            },
            {
              id: guild.client.user?.id || '',
              allow: [PermissionFlagsBits.ViewChannel, PermissionFlagsBits.SendMessages, PermissionFlagsBits.EmbedLinks]
            },
            {
              id: guild.ownerId,
              allow: [PermissionFlagsBits.ViewChannel, PermissionFlagsBits.ReadMessageHistory]
            }
          ],
          reason: '[MIAN XITERS] Automated secure audit logging setup'
        });
      }

      whitelistManager.setLogChannel(guild.id, logChannel.id);

      // Create or find Quarantine Role
      let quarantineRole = guild.roles.cache.find(r => r.name === 'MIAN-XITERS-QUARANTINE');
      if (!quarantineRole) {
        quarantineRole = await guild.roles.create({
          name: 'MIAN-XITERS-QUARANTINE',
          color: 0x555555,
          permissions: [],
          reason: '[MIAN XITERS] Automated quarantine isolation role'
        });

        // Set overwrites across all channels to deny viewing
        for (const [, ch] of guild.channels.cache) {
          if ('permissionOverwrites' in ch) {
            await (ch as TextChannel).permissionOverwrites.edit(quarantineRole, {
              ViewChannel: false,
              SendMessages: false,
              Connect: false
            }).catch(() => {});
          }
        }
      }

      whitelistManager.setQuarantineRole(guild.id, quarantineRole.id);
      await snapshotService.captureSnapshot(guild);

      const setupEmbed = new EmbedBuilder()
        .setColor(BOT_CONFIG.colors.success)
        .setTitle('✅ MIAN XITERS Automated Setup Completed')
        .setDescription(`**${BOT_CONFIG.slogan}**\nDefense infrastructure has been successfully configured.`)
        .addFields(
          { name: '📜 Log Channel', value: `<#${logChannel.id}>`, inline: true },
          { name: '🚷 Quarantine Role', value: `<@&${quarantineRole.id}>`, inline: true },
          { name: '💾 Baseline Snapshot', value: 'Captured & Cached', inline: true }
        )
        .setFooter({ text: `${BOT_CONFIG.name} Security Infrastructure` });

      return interaction.editReply({ embeds: [setupEmbed] });
    }

    // 5. SNAPSHOT COMMAND
    if (subCommand === 'snapshot') {
      if (!isOwner && !interaction.memberPermissions?.has(PermissionFlagsBits.Administrator)) {
        return interaction.reply({ content: '⛔ Administrator permission required.', ephemeral: true });
      }

      const action = interaction.options.getString('action', true);
      if (action === 'create') {
        await interaction.deferReply();
        const snap = await snapshotService.captureSnapshot(guild);
        return interaction.editReply({
          content: `✅ Baseline snapshot captured successfully. Indexed **${snap.channels?.length || 0} channels** and **${snap.roles?.length || 0} roles**.`
        });
      } else if (action === 'restore_channels') {
        await interaction.deferReply();
        const count = await snapshotService.restoreMissingChannels(guild);
        return interaction.editReply({
          content: `✅ Successfully restored/reconstructed **${count}** missing channels from snapshot.`
        });
      } else if (action === 'restore_roles') {
        await interaction.deferReply();
        const count = await snapshotService.restoreMissingRoles(guild);
        return interaction.editReply({
          content: `✅ Successfully restored/reconstructed **${count}** missing roles from snapshot.`
        });
      } else {
        const snap = snapshotService.getSnapshot(guild.id);
        if (!snap) {
          return interaction.reply({ content: '⚠️ No snapshot found. Run `/security snapshot action:create` first.', ephemeral: true });
        }
        return interaction.reply({
          content: `📊 Baseline snapshot details:\n• Guild Name: **${snap.name}**\n• Captured: <t:${Math.floor(snap.timestamp / 1000)}:F>\n• Channels: **${snap.channels?.length || 0}**\n• Roles: **${snap.roles?.length || 0}**`,
          ephemeral: true
        });
      }
    }

    // 6. MANUAL QUARANTINE
    if (subCommand === 'quarantine') {
      if (!isOwner && !interaction.memberPermissions?.has(PermissionFlagsBits.Administrator)) {
        return interaction.reply({ content: '⛔ Administrator permission required.', ephemeral: true });
      }

      const target = interaction.options.getUser('user', true);
      const action = interaction.options.getString('action', true);
      const member = await guild.members.fetch(target.id).catch(() => null);

      if (!member) {
        return interaction.reply({ content: '❌ Member not found in this guild.', ephemeral: true });
      }

      const qRoleId = whitelistManager.getQuarantineRole(guild.id);
      if (!qRoleId) {
        return interaction.reply({ content: '⚠️ Quarantine role not configured. Run `/security setup` first.', ephemeral: true });
      }

      if (action === 'apply') {
        await member.roles.add(qRoleId, `[MIAN XITERS] Manual quarantine by ${interaction.user.tag}`);
        return interaction.reply({ content: `🔒 Applied quarantine isolation to <@${target.id}>.` });
      } else {
        await member.roles.remove(qRoleId, `[MIAN XITERS] Quarantine released by ${interaction.user.tag}`);
        return interaction.reply({ content: `🟢 Released <@${target.id}> from quarantine isolation.` });
      }
    }

    // 7. NUCLEAR PANIC MODE (OWNER ONLY)
    if (subCommand === 'panic') {
      if (!isOwner) {
        return interaction.reply({
          content: '⛔ **Access Denied:** Only the **Server Owner** has authority to trigger Nuclear Panic Mode.',
          ephemeral: true
        });
      }

      await interaction.deferReply();
      const action = interaction.options.getString('action', true);

      if (action === 'trigger') {
        const result = await isolationService.executePanicLockdown(guild);
        return interaction.editReply({
          content: `🚨 **NUCLEAR PANIC MODE ENGAGED:**\n• **${result.rolesSanitized}** roles stripped of dangerous permissions.\n• **${result.channelsLocked}** channels locked.\n• Server invites paused.\n\nRun \`/security panic action:restore\` when the server is secure.`
        });
      } else {
        const success = await isolationService.restorePanicLockdown(guild);
        if (success) {
          return interaction.editReply({
            content: '🟢 **PANIC MODE RESTORED:** Role permissions and channel overwrites successfully recovered from baseline snapshot.'
          });
        } else {
          return interaction.editReply({
            content: '⚠️ Unable to restore: no valid baseline snapshot found.'
          });
        }
      }
    }

    return interaction.reply({ content: 'Invalid security subcommand.', ephemeral: true });
  }
};
