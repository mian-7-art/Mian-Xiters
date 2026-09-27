import { Events, GuildAuditLogsEntry, Guild } from 'discord.js';
import { auditDispatcher } from '../services/AuditDispatcher.js';
import { Logger } from '../utils/logger.js';

export default {
  name: Events.GuildAuditLogEntryCreate,
  async execute(auditLogEntry: GuildAuditLogsEntry, guild: Guild): Promise<void> {
    try {
      await auditDispatcher.dispatchAuditEntry(auditLogEntry, guild);
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[AUDIT_LOG_GATEWAY_ERROR] ${msg}`);
    }
  }
};
