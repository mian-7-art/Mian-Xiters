import { Events, GuildMember, TextChannel, EmbedBuilder } from 'discord.js';
import { BOT_CONFIG } from '../config/config.js';
import { rateLimiter } from '../utils/rateLimiter.js';
import { whitelistManager } from '../config/whitelist.js';
import { watchlistService } from '../services/WatchlistService.js';
import { isolationService } from '../services/isolationService.js';
import { raidDetector } from '../services/raidDetector.js';
import { Logger } from '../utils/logger.js';

export default {
  name: Events.GuildMemberAdd,
  async execute(member: GuildMember): Promise<void> {
    const guild = member.guild;

    try {
      // 0. WATCHLIST SUSPECT JOIN TRIGGER
      if (watchlistService.isWatchlisted(member.id)) {
        Logger.threat('WATCHLIST MEMBER JOINED', `Suspect ${member.user.tag} (${member.id}) joined ${guild.name}. Quarantining immediately.`);
        await isolationService.quarantineMember(member, 'Monitored Watchlist Suspect');
        const logChannelId = whitelistManager.getLogChannel(guild.id);
        if (logChannelId) {
          const logChannel = guild.channels.cache.get(logChannelId) as TextChannel;
          if (logChannel && logChannel.isTextBased()) {
            const embed = new EmbedBuilder()
              .setColor(BOT_CONFIG.colors.danger)
              .setTitle('👁️ MIAN XITERS — WATCHLIST SUSPECT DETECTED')
              .setDescription(`High-risk monitored suspect <@${member.id}> (\`${member.user.tag}\`) has entered the server.\nAutomatic quarantine role applied.`)
              .setFooter({ text: `${BOT_CONFIG.name} Defense • Dev by ${BOT_CONFIG.author}` })
              .setTimestamp();
            await logChannel.send({ embeds: [embed] }).catch(() => {});
          }
        }
      }

      // 1. SWARM JOIN VELOCITY TRIGGER (>5 joins in 3 seconds)
      const key = `${guild.id}:mass_join_burst`;
      const { count, breached } = rateLimiter.track(key, 5, 3 * 1000);

      if (breached) {
        Logger.threat('SWARM JOIN RAID DETECTED', `${count} joins detected within 3 seconds! Engaging Server Panic Mode.`);

        if (!whitelistManager.isPanicMode(guild.id)) {
          // Trip Server Panic Mode (locks channels & halts invites)
          await isolationService.executePanicLockdown(guild);

          const logChannelId = whitelistManager.getLogChannel(guild.id);
          if (logChannelId) {
            const logChannel = guild.channels.cache.get(logChannelId) as TextChannel;
            if (logChannel && logChannel.isTextBased()) {
              const embed = new EmbedBuilder()
                .setColor(BOT_CONFIG.colors.danger)
                .setTitle('🛑 MIAN XITERS — AUTOMATED RAID PANIC ENGAGED')
                .setDescription(
                  `**${BOT_CONFIG.slogan}**\nA mass-join swarm was detected (**${count} joins in 3s**).\n` +
                  `Server is placed in Panic Mode: All channels locked, invites paused, raid shield active.\n\n` +
                  `*Panic Mode will auto-lift after 5 minutes of quiet traffic, or run \`/security panic action:restore\`.*`
                )
                .setFooter({ text: `${BOT_CONFIG.name} Defense • Dev by ${BOT_CONFIG.author}` })
                .setTimestamp();

              await logChannel.send({ embeds: [embed] }).catch(() => {});
            }
          }
        }
      }

      // 2. HEURISTIC RAID PATTERN ANALYSIS (BEEMO-GRADE HEURISTIC ENGINE)
      const raidAnalysis = raidDetector.registerAndAnalyze(member);
      if (raidAnalysis.isRaidWave) {
        Logger.threat('HEURISTIC RAID PATTERN', `${member.user.tag} matches swarm pattern: ${raidAnalysis.reason}`);
        if (!whitelistManager.isPanicMode(guild.id)) {
          await isolationService.executePanicLockdown(guild);
        }
        await isolationService.quarantineMember(member, `Heuristic Raid Wave: ${raidAnalysis.reason}`);
        if (member.kickable) {
          await member.kick(`[MIAN XITERS] Raid Wave Neutralization: ${raidAnalysis.reason}`).catch(() => {});
        }
      }

      // 3. ACCOUNT AGE (7 DAYS) & SUSPICIOUS PROFILE FILTER
      const accountCreatedMs = member.user.createdTimestamp;
      const accountAgeDays = (Date.now() - accountCreatedMs) / (1000 * 60 * 60 * 24);
      const hasDefaultAvatar = !member.user.avatar;
      const isPanicActive = whitelistManager.isPanicMode(guild.id);

      // During active raid or panic, quarantine accounts younger than 7 days or without avatars
      const isSuspicious = isPanicActive
        ? accountAgeDays < 7 || hasDefaultAvatar
        : accountAgeDays < 3;

      if (isSuspicious) {
        const quarantineSuccess = await isolationService.quarantineMember(
          member,
          `Account age ${accountAgeDays.toFixed(1)} days (Panic Threshold: 7d, DefaultAvatar: ${hasDefaultAvatar})`
        );

        if (!quarantineSuccess && isPanicActive && member.kickable) {
          await member.kick('[MIAN XITERS] Auto-Defense: Suspicious account during active raid').catch(() => {});
        }
      }
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[GUILD_MEMBER_ADD_ERROR] ${msg}`);
    }
  }
};
