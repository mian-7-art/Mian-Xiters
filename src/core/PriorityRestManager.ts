import { REST, Routes } from 'discord.js';
import { Logger } from '../utils/logger.js';

export interface PriorityRestOptions {
  deleteMessageSeconds?: number;
  reason?: string;
}

export class PriorityRestManager {
  private restClient: REST | null = null;
  private activeLocks: Set<string> = new Set();
  private emergencyTokens: number = 20; // Reserved rate-limit burst bucket for emergency bans
  private lastRefill: number = Date.now();

  public initialize(token: string): void {
    this.restClient = new REST({ version: '10' }).setToken(token);
    Logger.info('[PRIORITY REST] Initialized independent emergency direct REST dispatcher.');
  }

  private replenishTokens(): void {
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
  public async emergencyBan(
    guildId: string,
    targetUserId: string,
    options: PriorityRestOptions = {}
  ): Promise<boolean> {
    const lockKey = `ban:${guildId}:${targetUserId}`;
    if (this.activeLocks.has(lockKey)) {
      Logger.warn(`[PRIORITY REST] Target ${targetUserId} is already being banned, skipping redundant call.`);
      return false;
    }

    this.activeLocks.add(lockKey);
    this.replenishTokens();

    const startTime = Date.now();
    try {
      if (this.restClient) {
        // Direct REST execution with raw route
        await this.restClient.put(
          Routes.guildBan(guildId, targetUserId),
          {
            body: {
              delete_message_seconds: options.deleteMessageSeconds ?? 604800
            },
            reason: options.reason ?? '[MIAN XITERS] Emergency Zero-Trust Autonomous Ban'
          }
        );
        const elapsed = Date.now() - startTime;
        Logger.mitigation('PRIORITY REST BAN', `<@${targetUserId}>`, elapsed);
        return true;
      }
      return false;
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : String(err);
      Logger.error(`[PRIORITY REST] Direct emergency ban failed for ${targetUserId}: ${message}`);
      throw err;
    } finally {
      setTimeout(() => {
        this.activeLocks.delete(lockKey);
      }, 5000);
    }
  }

  /**
   * Immediately strips all roles from a member using the priority REST lane.
   */
  public async emergencyStripRoles(
    guildId: string,
    targetUserId: string,
    reason: string = '[MIAN XITERS] Emergency Administrative Revocation'
  ): Promise<boolean> {
    const lockKey = `roles:${guildId}:${targetUserId}`;
    if (this.activeLocks.has(lockKey)) {
      return false;
    }

    this.activeLocks.add(lockKey);
    const startTime = Date.now();

    try {
      if (this.restClient) {
        await this.restClient.patch(
          Routes.guildMember(guildId, targetUserId),
          {
            body: { roles: [] },
            reason
          }
        );
        const elapsed = Date.now() - startTime;
        Logger.mitigation('PRIORITY ROLE STRIP', `<@${targetUserId}>`, elapsed);
        return true;
      }
      return false;
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : String(err);
      Logger.error(`[PRIORITY REST] Direct role stripping failed for ${targetUserId}: ${message}`);
      throw err;
    } finally {
      setTimeout(() => {
        this.activeLocks.delete(lockKey);
      }, 5000);
    }
  }

  public isLocked(key: string): boolean {
    return this.activeLocks.has(key);
  }
}

export const priorityRestManager = new PriorityRestManager();
