import { Logger } from '../utils/logger.js';

export interface PriorityTask<T = any> {
  id: string;
  priority: 'EMERGENCY' | 'STANDARD';
  execute: () => Promise<T>;
  onSuccess?: (res: T) => void;
  onError?: (err: any) => void;
}

export class PriorityQueueDispatcher {
  private activeJobs: Set<string> = new Set();

  /**
   * Dispatches an emergency mitigation task with zero delay.
   * Ensures identical targets are not hammered repeatedly to conserve Discord REST rate limit buckets.
   */
  public async dispatchEmergency<T>(targetKey: string, task: () => Promise<T>): Promise<T | null> {
    if (this.activeJobs.has(targetKey)) {
      Logger.warn(`[DISPATCHER] Mitigation already active for target: ${targetKey}, skipping redundant invocation.`);
      return null;
    }

    this.activeJobs.add(targetKey);
    const start = Date.now();

    try {
      const result = await task();
      return result;
    } catch (error: any) {
      Logger.error(`[DISPATCHER] Emergency mitigation failed for ${targetKey}: ${error?.message || error}`);
      throw error;
    } finally {
      // Hold the target in lock for 5 seconds to prevent multi-event spam from triggering repeated bans
      setTimeout(() => {
        this.activeJobs.delete(targetKey);
      }, 5000);
    }
  }

  public isLocked(targetKey: string): boolean {
    return this.activeJobs.has(targetKey);
  }
}

export const priorityDispatcher = new PriorityQueueDispatcher();
