import { Events, VoiceState, AuditLogEvent } from 'discord.js';
import { SUPREME_OWNERS } from '../config/whitelist.js';
import { Logger } from '../utils/logger.js';
import { isolationService } from '../services/isolationService.js';
import { ThreatType } from '../types/index.js';

const OWNER_ROLE_ID = '1557423753418510346';

export default {
  name: Events.VoiceStateUpdate,
  async execute(oldState: VoiceState, newState: VoiceState): Promise<void> {
    const guild = newState.guild || oldState.guild;
    const member = newState.member || oldState.member;
    if (!guild || !member) return;

    // Check if the affected member is the Owner
    const isOwner =
      SUPREME_OWNERS.has(member.id) ||
      member.id === guild.ownerId ||
      member.roles.cache.has(OWNER_ROLE_ID);

    if (!isOwner) return;

    const botId = guild.client.user?.id;

    // 1. ANTI-SERVER MUTE & ANTI-SERVER DEAFEN DEFENSE
    if (newState.serverMute && !oldState.serverMute) {
      Logger.threat(
        '[VOICE SHIELD] UNAUTHORIZED SERVER MUTE ON OWNER DETECTED',
        `Owner ${member.user.tag} (${member.id}) was server muted! Reverting instantly...`
      );
      try {
        await newState.setMute(false, '[ONLY SECURITY] Protected Owner from unauthorized server mute');
        Logger.info(`[VOICE SHIELD] Successfully unmuted Owner ${member.user.tag}`);
      } catch (err: unknown) {
        const msg = err instanceof Error ? err.message : String(err);
        Logger.error(`[VOICE SHIELD] Failed to unmute Owner: ${msg}`);
      }
    }

    if (newState.serverDeaf && !oldState.serverDeaf) {
      Logger.threat(
        '[VOICE SHIELD] UNAUTHORIZED SERVER DEAFEN ON OWNER DETECTED',
        `Owner ${member.user.tag} (${member.id}) was server deafened! Reverting instantly...`
      );
      try {
        await newState.setDeaf(false, '[ONLY SECURITY] Protected Owner from unauthorized server deafen');
        Logger.info(`[VOICE SHIELD] Successfully undeafened Owner ${member.user.tag}`);
      } catch (err: unknown) {
        const msg = err instanceof Error ? err.message : String(err);
        Logger.error(`[VOICE SHIELD] Failed to undeafen Owner: ${msg}`);
      }
    }

    // 2. ANTI-DRAG / ANTI-MOVE DEFENSE
    // When owner changes voice channel (from old channel to new channel)
    if (oldState.channelId && newState.channelId && oldState.channelId !== newState.channelId) {
      try {
        // Wait 250ms for Discord Gateway audit log ingestion
        await new Promise(r => setTimeout(r, 250));

        const auditLogs = await guild.fetchAuditLogs({
          type: AuditLogEvent.MemberMove,
          limit: 1
        }).catch(() => null);

        const latestMove = auditLogs?.entries.first();
        if (latestMove) {
          const timeDiff = Date.now() - latestMove.createdTimestamp;
          // If within last 4 seconds and target is the owner
          if (timeDiff < 4000 && latestMove.targetId === member.id) {
            const executorId = latestMove.executorId;
            // If the executor is NOT the bot itself and NOT the owner themselves
            if (executorId && executorId !== botId && !SUPREME_OWNERS.has(executorId)) {
              Logger.threat(
                '[VOICE SHIELD] UNAUTHORIZED OWNER DRAG DETECTED',
                `Executor ${executorId} dragged Owner ${member.user.tag} from ${oldState.channel?.name} to ${newState.channel?.name}! Reverting...`
              );

              // Instantly move the owner back
              await newState.setChannel(oldState.channelId, '[ONLY SECURITY] Reverting unauthorized drag of Owner').catch(err => {
                const msg = err instanceof Error ? err.message : String(err);
                Logger.error(`[VOICE SHIELD] Failed to revert owner voice channel: ${msg}`);
              });

              // Punish rogue executor: Disconnect / Mute in voice
              try {
                const rogueMember = await guild.members.fetch(executorId).catch(() => null);
                if (rogueMember && rogueMember.voice.channelId) {
                  await rogueMember.voice.disconnect('[ONLY SECURITY] Disconnected for attempting to drag the Owner').catch(() => {});
                  await rogueMember.voice.setMute(true, '[ONLY SECURITY] Muted for attempting to drag the Owner').catch(() => {});
                }
              } catch {}

              // Dispatch emergency isolation alert
              await isolationService.executeEmergencyIsolation(guild, {
                type: ThreatType.SECURITY_INFRASTRUCTURE_TAMPER,
                executorId,
                targetId: member.id,
                guildId: guild.id,
                timestamp: Date.now(),
                reason: `Unauthorized attempt to drag/move the Owner in Voice Channel`
              });
            }
          }
        }
      } catch (err: unknown) {
        const msg = err instanceof Error ? err.message : String(err);
        Logger.error(`[VOICE SHIELD] Error inspecting drag audit logs: ${msg}`);
      }
    }

    // 3. ANTI-DISCONNECT DEFENSE
    if (oldState.channelId && !newState.channelId) {
      try {
        await new Promise(r => setTimeout(r, 250));
        const auditLogs = await guild.fetchAuditLogs({
          type: AuditLogEvent.MemberDisconnect,
          limit: 1
        }).catch(() => null);

        const latestDisconnect = auditLogs?.entries.first();
        if (latestDisconnect) {
          const timeDiff = Date.now() - latestDisconnect.createdTimestamp;
          if (timeDiff < 4000 && latestDisconnect.targetId === member.id) {
            const executorId = latestDisconnect.executorId;
            if (executorId && executorId !== botId && !SUPREME_OWNERS.has(executorId)) {
              Logger.threat(
                '[VOICE SHIELD] UNAUTHORIZED OWNER DISCONNECT DETECTED',
                `Executor ${executorId} disconnected Owner ${member.user.tag} from voice!`
              );

              await isolationService.executeEmergencyIsolation(guild, {
                type: ThreatType.SECURITY_INFRASTRUCTURE_TAMPER,
                executorId,
                targetId: member.id,
                guildId: guild.id,
                timestamp: Date.now(),
                reason: `Unauthorized attempt to disconnect the Owner from Voice Channel`
              });
            }
          }
        }
      } catch (err: unknown) {
        const msg = err instanceof Error ? err.message : String(err);
        Logger.error(`[VOICE SHIELD] Error inspecting disconnect audit logs: ${msg}`);
      }
    }
  }
};
