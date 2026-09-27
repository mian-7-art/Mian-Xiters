import { Events, Guild, AuditLogEvent } from 'discord.js';
import { snapshotService } from '../services/SnapshotService.js';
import { isolationService } from '../services/isolationService.js';
import { whitelistManager } from '../config/whitelist.js';
import { ThreatType } from '../types/index.js';
import { Logger } from '../utils/logger.js';

export default {
  name: Events.GuildUpdate,
  async execute(oldGuild: Guild, newGuild: Guild): Promise<void> {
    try {
      const snapshot = snapshotService.getSnapshot(newGuild.id);
      if (!snapshot) return;

      const nameChanged = oldGuild.name !== newGuild.name;
      const iconChanged = oldGuild.icon !== newGuild.icon;
      const verificationChanged = oldGuild.verificationLevel !== newGuild.verificationLevel;
      const vanityChanged = oldGuild.vanityURLCode !== newGuild.vanityURLCode;

      if (!nameChanged && !iconChanged && !verificationChanged && !vanityChanged) {
        return;
      }

      // Fetch audit entry to identify executor
      const fetchedLogs = await newGuild.fetchAuditLogs({
        limit: 1,
        type: AuditLogEvent.GuildUpdate
      }).catch(() => null);

      const logEntry = fetchedLogs?.entries.first();
      if (!logEntry) return;

      const executorId = logEntry.executorId;
      if (!executorId || executorId === newGuild.ownerId || executorId === newGuild.client.user?.id) {
        return;
      }

      const isWhitelisted = whitelistManager.isWhitelisted(
        newGuild.id,
        executorId,
        newGuild.ownerId,
        newGuild.client.user?.id
      );

      if (!isWhitelisted) {
        Logger.threat('GUILD TAMPERING EVENT', `Executor: ${executorId} changed guild core attributes`);

        await isolationService.executeEmergencyIsolation(newGuild, {
          type: ThreatType.GUILD_TAMPER,
          executorId,
          guildId: newGuild.id,
          timestamp: Date.now(),
          reason: 'Unauthorized modification of guild identity and settings'
        });

        await isolationService.revertGuildTamper(newGuild, executorId);
      }
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[GUILD_UPDATE_ERROR] ${msg}`);
    }
  }
};
