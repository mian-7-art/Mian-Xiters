export interface ActionEntry {
  timestamp: number;
  metadata?: any;
}

export class SlidingWindowRateLimiter {
  private tracker: Map<string, ActionEntry[]> = new Map();
  private cleanupInterval: NodeJS.Timeout;

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
  public track(
    key: string,
    limit: number,
    windowMs: number,
    metadata?: any
  ): { count: number; breached: boolean; remaining: number; entries: ActionEntry[] } {
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
  public getEntries(key: string, windowMs: number): ActionEntry[] {
    const cutoff = Date.now() - windowMs;
    const entries = this.tracker.get(key) || [];
    return entries.filter(e => e.timestamp > cutoff);
  }

  /**
   * Clears a key after isolation or manual reset
   */
  public reset(key: string): void {
    this.tracker.delete(key);
  }

  private cleanup(): void {
    const now = Date.now();
    for (const [key, entries] of this.tracker.entries()) {
      // Retain entries younger than 5 minutes for general tracking
      const valid = entries.filter(e => now - e.timestamp < 300000);
      if (valid.length === 0) {
        this.tracker.delete(key);
      } else {
        this.tracker.set(key, valid);
      }
    }
  }

  public destroy(): void {
    clearInterval(this.cleanupInterval);
  }
}

export const rateLimiter = new SlidingWindowRateLimiter();
