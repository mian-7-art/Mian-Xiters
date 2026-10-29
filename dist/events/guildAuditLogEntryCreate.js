"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const AuditDispatcher_js_1 = require("../services/AuditDispatcher.js");
const logger_js_1 = require("../utils/logger.js");
exports.default = {
    name: discord_js_1.Events.GuildAuditLogEntryCreate,
    async execute(auditLogEntry, guild) {
        try {
            await AuditDispatcher_js_1.auditDispatcher.dispatchAuditEntry(auditLogEntry, guild);
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[AUDIT_LOG_GATEWAY_ERROR] ${msg}`);
        }
    }
};
