"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.priorityRestManager = exports.PriorityRestManager = void 0;
const discord_js_1 = require("discord.js");
const logger_js_1 = require("../utils/logger.js");
class PriorityRestManager {
    restClient = null;
    activeLocks = new Set();
    emergencyTokens = 20; // Reserved rate-limit burst bucket for emergency bans
    lastRefill = Date.now();
    initialize(token) {
        this.restClient = new discord_js_1.REST({ version: '10' }).setToken(token);
        logger_js_1.Logger.info('[PRIORITY REST] Initialized independent emergency direct REST dispatcher.');
    }
    replenishTokens() {
        const now = Date.now();
        const elapsed = now - this.lastRefill;
        if (elapsed > 1000) {
            // Replenish 5 tokens per second up to cap of 20
            this.emergencyTokens = Math.min(20, this.emergencyTokens + Math.floor(elapsed / 1000) * 5);
            this.lastRefill = now;
        }
    }
    /**
     * Executes an immediate guild ban through the priority REST lane,
     * bypassing normal Discord.js event queues and message logging queues.
     */
    async emergencyBan(guildId, targetUserId, options = {}) {
        const lockKey = `ban:${guildId}:${targetUserId}`;
        if (this.activeLocks.has(lockKey)) {
            logger_js_1.Logger.warn(`[PRIORITY REST] Target ${targetUserId} is already being banned, skipping redundant call.`);
            return false;
        }
        this.activeLocks.add(lockKey);
        this.replenishTokens();
        const startTime = Date.now();
        try {
            if (this.restClient) {
                // Direct REST execution with raw route
                await this.restClient.put(discord_js_1.Routes.guildBan(guildId, targetUserId), {
                    body: {
                        delete_message_seconds: options.deleteMessageSeconds ?? 604800
                    },
                    reason: options.reason ?? '[MIAN XITERS] Emergency Zero-Trust Autonomous Ban'
                });
                const elapsed = Date.now() - startTime;
                logger_js_1.Logger.mitigation('PRIORITY REST BAN', `<@${targetUserId}>`, elapsed);
                return true;
            }
            return false;
        }
        catch (err) {
            const message = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[PRIORITY REST] Direct emergency ban failed for ${targetUserId}: ${message}`);
            throw err;
        }
        finally {
            setTimeout(() => {
                this.activeLocks.delete(lockKey);
            }, 5000);
        }
    }
    /**
     * Immediately strips all roles from a member using the priority REST lane.
     */
    async emergencyStripRoles(guildId, targetUserId, reason = '[MIAN XITERS] Emergency Administrative Revocation') {
        const lockKey = `roles:${guildId}:${targetUserId}`;
        if (this.activeLocks.has(lockKey)) {
            return false;
        }
        this.activeLocks.add(lockKey);
        const startTime = Date.now();
        try {
            if (this.restClient) {
                await this.restClient.patch(discord_js_1.Routes.guildMember(guildId, targetUserId), {
                    body: { roles: [] },
                    reason
                });
                const elapsed = Date.now() - startTime;
                logger_js_1.Logger.mitigation('PRIORITY ROLE STRIP', `<@${targetUserId}>`, elapsed);
                return true;
            }
            return false;
        }
        catch (err) {
            const message = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[PRIORITY REST] Direct role stripping failed for ${targetUserId}: ${message}`);
            throw err;
        }
        finally {
            setTimeout(() => {
                this.activeLocks.delete(lockKey);
            }, 5000);
        }
    }
    isLocked(key) {
        return this.activeLocks.has(key);
    }
}
exports.PriorityRestManager = PriorityRestManager;
exports.priorityRestManager = new PriorityRestManager();
