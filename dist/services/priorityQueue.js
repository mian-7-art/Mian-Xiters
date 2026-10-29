"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.priorityDispatcher = exports.PriorityQueueDispatcher = void 0;
const logger_js_1 = require("../utils/logger.js");
class PriorityQueueDispatcher {
    activeJobs = new Set();
    /**
     * Dispatches an emergency mitigation task with zero delay.
     * Ensures identical targets are not hammered repeatedly to conserve Discord REST rate limit buckets.
     */
    async dispatchEmergency(targetKey, task) {
        if (this.activeJobs.has(targetKey)) {
            logger_js_1.Logger.warn(`[DISPATCHER] Mitigation already active for target: ${targetKey}, skipping redundant invocation.`);
            return null;
        }
        this.activeJobs.add(targetKey);
        const start = Date.now();
        try {
            const result = await task();
            return result;
        }
        catch (error) {
            logger_js_1.Logger.error(`[DISPATCHER] Emergency mitigation failed for ${targetKey}: ${error?.message || error}`);
            throw error;
        }
        finally {
            // Hold the target in lock for 5 seconds to prevent multi-event spam from triggering repeated bans
            setTimeout(() => {
                this.activeJobs.delete(targetKey);
            }, 5000);
        }
    }
    isLocked(targetKey) {
        return this.activeJobs.has(targetKey);
    }
}
exports.PriorityQueueDispatcher = PriorityQueueDispatcher;
exports.priorityDispatcher = new PriorityQueueDispatcher();
