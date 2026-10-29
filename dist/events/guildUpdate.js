"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const SnapshotService_js_1 = require("../services/SnapshotService.js");
const isolationService_js_1 = require("../services/isolationService.js");
const whitelist_js_1 = require("../config/whitelist.js");
const index_js_1 = require("../types/index.js");
const logger_js_1 = require("../utils/logger.js");
exports.default = {
    name: discord_js_1.Events.GuildUpdate,
    async execute(oldGuild, newGuild) {
        try {
            const snapshot = SnapshotService_js_1.snapshotService.getSnapshot(newGuild.id);
            if (!snapshot)
                return;
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
                type: discord_js_1.AuditLogEvent.GuildUpdate
            }).catch(() => null);
            const logEntry = fetchedLogs?.entries.first();
            if (!logEntry)
                return;
            const executorId = logEntry.executorId;
            if (!executorId || executorId === newGuild.ownerId || executorId === newGuild.client.user?.id) {
                return;
            }
            const isWhitelisted = whitelist_js_1.whitelistManager.isWhitelisted(newGuild.id, executorId, newGuild.ownerId, newGuild.client.user?.id);
            if (!isWhitelisted) {
                logger_js_1.Logger.threat('GUILD TAMPERING EVENT', `Executor: ${executorId} changed guild core attributes`);
                await isolationService_js_1.isolationService.executeEmergencyIsolation(newGuild, {
                    type: index_js_1.ThreatType.GUILD_TAMPER,
                    executorId,
                    guildId: newGuild.id,
                    timestamp: Date.now(),
                    reason: 'Unauthorized modification of guild identity and settings'
                });
                await isolationService_js_1.isolationService.revertGuildTamper(newGuild, executorId);
            }
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[GUILD_UPDATE_ERROR] ${msg}`);
        }
    }
};
