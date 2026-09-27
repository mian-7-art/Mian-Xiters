import fs from 'fs';
import path from 'path';
import { WhitelistData, WhitelistPermissionConfig } from '../types/index.js';
import { Logger } from '../utils/logger.js';

const DATA_DIR = path.resolve(process.cwd(), 'data');
const WHITELIST_FILE = path.join(DATA_DIR, 'whitelist.json');

class WhitelistManager {
  private data: WhitelistData = { guilds: {} };

  constructor() {
    this.init();
  }

  private init(): void {
    try {
      if (!fs.existsSync(DATA_DIR)) {
        fs.mkdirSync(DATA_DIR, { recursive: true });
      }
      if (fs.existsSync(WHITELIST_FILE)) {
        const raw = fs.readFileSync(WHITELIST_FILE, 'utf-8');
        this.data = JSON.parse(raw);
      } else {
        this.save();
      }
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[WHITELIST] Failed to initialize: ${msg}`);
      this.data = { guilds: {} };
    }
  }

  private save(): void {
    try {
      const tempPath = `${WHITELIST_FILE}.tmp`;
      fs.writeFileSync(tempPath, JSON.stringify(this.data, null, 2), 'utf-8');
      fs.renameSync(tempPath, WHITELIST_FILE);
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[WHITELIST] Failed to save: ${msg}`);
    }
  }

  private ensureGuild(guildId: string): void {
    if (!this.data.guilds[guildId]) {
      this.data.guilds[guildId] = {
        whitelistedUsers: {},
        lockdownActive: false,
        panicModeActive: false
      };
    }
  }

  public isAuthorized(
    guildId: string,
    userId: string,
    action: keyof WhitelistPermissionConfig,
    ownerId?: string,
    botId?: string
  ): boolean {
    if (ownerId && userId === ownerId) return true;
    if (botId && userId === botId) return true;

    this.ensureGuild(guildId);
    const userPerms = this.data.guilds[guildId].whitelistedUsers[userId];
    if (!userPerms) return false;
    if (userPerms.isFullSuperAdmin) return true;

    return !!userPerms[action];
  }

  public isWhitelisted(guildId: string, userId: string, ownerId?: string, botId?: string): boolean {
    if (ownerId && userId === ownerId) return true;
    if (botId && userId === botId) return true;

    this.ensureGuild(guildId);
    return !!this.data.guilds[guildId].whitelistedUsers[userId];
  }

  public setWhitelist(
    guildId: string,
    userId: string,
    permissions: WhitelistPermissionConfig = { isFullSuperAdmin: true }
  ): boolean {
    this.ensureGuild(guildId);
    this.data.guilds[guildId].whitelistedUsers[userId] = permissions;
    this.save();
    return true;
  }

  public removeWhitelist(guildId: string, userId: string): boolean {
    this.ensureGuild(guildId);
    if (!this.data.guilds[guildId].whitelistedUsers[userId]) {
      return false;
    }
    delete this.data.guilds[guildId].whitelistedUsers[userId];
    this.save();
    return true;
  }

  public getWhitelist(guildId: string): { [userId: string]: WhitelistPermissionConfig } {
    this.ensureGuild(guildId);
    return { ...this.data.guilds[guildId].whitelistedUsers };
  }

  public setLogChannel(guildId: string, channelId: string): void {
    this.ensureGuild(guildId);
    this.data.guilds[guildId].logChannelId = channelId;
    this.save();
  }

  public getLogChannel(guildId: string): string | undefined {
    return this.data.guilds[guildId]?.logChannelId;
  }

  public setQuarantineRole(guildId: string, roleId: string): void {
    this.ensureGuild(guildId);
    this.data.guilds[guildId].quarantineRoleId = roleId;
    this.save();
  }

  public getQuarantineRole(guildId: string): string | undefined {
    return this.data.guilds[guildId]?.quarantineRoleId;
  }

  public setLockdown(guildId: string, active: boolean): void {
    this.ensureGuild(guildId);
    this.data.guilds[guildId].lockdownActive = active;
    this.save();
  }

  public isLockdown(guildId: string): boolean {
    return !!this.data.guilds[guildId]?.lockdownActive;
  }

  public setPanicMode(guildId: string, active: boolean): void {
    this.ensureGuild(guildId);
    this.data.guilds[guildId].panicModeActive = active;
    this.data.guilds[guildId].panicTriggeredAt = active ? Date.now() : undefined;
    this.save();
  }

  public isPanicMode(guildId: string): boolean {
    const guildData = this.data.guilds[guildId];
    if (!guildData?.panicModeActive) return false;

    // Auto-lift panic mode after 5 minutes of quiet traffic
    if (guildData.panicTriggeredAt && Date.now() - guildData.panicTriggeredAt > 5 * 60 * 1000) {
      guildData.panicModeActive = false;
      this.save();
      return false;
    }

    return true;
  }
}

export const whitelistManager = new WhitelistManager();
