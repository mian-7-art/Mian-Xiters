"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.rateLimiter = exports.SlidingWindowRateLimiter = void 0;
class SlidingWindowRateLimiter {
    tracker = new Map();
    cleanupInterval;
    constructor() {
        // Periodic garbage collection to maintain ultra-low memory footprint
        this.cleanupInterval = setInterval(() => this.cleanup(), 60 * 1000);
    }
    /**
     * Tracks an action occurrence within a sliding window.
     * @param key Unique identifier, e.g. `${guildId}:${executorId}:${actionType}`
     * @param limit Maximum allowed events in window
     * @param windowMs Duration of window in milliseconds
     * @param metadata Optional payload (such as created entity IDs)
     */
    track(key, limit, windowMs, metadata) {
        const now = Date.now();
        const cutoff = now - windowMs;
        let entries = this.tracker.get(key) || [];
        // Filter out timestamps outside the sliding window
        entries = entries.filter(e => e.timestamp > cutoff);
        // Record the current event
        entries.push({ timestamp: now, metadata });
        this.tracker.set(key, entries);
        const count = entries.length;
        const breached = count > limit;
        const remaining = Math.max(0, limit - count);
        return { count, breached, remaining, entries };
    }
    /**
     * Retrieves all active entries in the current window without incrementing.
     */
    getEntries(key, windowMs) {
        const cutoff = Date.now() - windowMs;
        const entries = this.tracker.get(key) || [];
        return entries.filter(e => e.timestamp > cutoff);
    }
    /**
     * Clears a key after isolation or manual reset
     */
    reset(key) {
        this.tracker.delete(key);
    }
    cleanup() {
        const now = Date.now();
        for (const [key, entries] of this.tracker.entries()) {
            // Retain entries younger than 5 minutes for general tracking
            const valid = entries.filter(e => now - e.timestamp < 300000);
            if (valid.length === 0) {
                this.tracker.delete(key);
            }
            else {
                this.tracker.set(key, valid);
            }
        }
    }
    destroy() {
        clearInterval(this.cleanupInterval);
    }
}
exports.SlidingWindowRateLimiter = SlidingWindowRateLimiter;
exports.rateLimiter = new SlidingWindowRateLimiter();
