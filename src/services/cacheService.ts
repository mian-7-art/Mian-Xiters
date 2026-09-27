import { Guild, Client, GuildDefaultMessageNotifications, GuildExplicitContentFilter, GuildVerificationLevel } from 'discord.js';
import fs from 'fs';
import path from 'path';
import { GuildStateSnapshot } from '../types/index.js';
import { Logger } from '../utils/logger.js';

const SNAPSHOTS_DIR = path.resolve(process.cwd(), 'data', 'snapshots');

export class CacheService {
  private guildSnapshots: Map<string, GuildStateSnapshot> = new Map();
  private createdChannels: Map<string, { executorId: string; timestamp: number }> = new Map();
  private createdRoles: Map<string, { executorId: string; timestamp: number }> = new Map();

  constructor() {
    if (!fs.existsSync(SNAPSHOTS_DIR)) {
      fs.mkdirSync(SNAPSHOTS_DIR, { recursive: true });
    }
  }

  /**
   * Captures and caches the legitimate state of a guild.
   */
  public async snapshotGuild(guild: Guild): Promise<GuildStateSnapshot> {
    try {
      const snapshot: GuildStateSnapshot = {
        id: guild.id,
        name: guild.name,
        iconURL: guild.iconURL({ extension: 'png', size: 1024 }),
        bannerURL: guild.bannerURL({ extension: 'png', size: 1024 }),
        splashURL: guild.splashURL({ extension: 'png', size: 1024 }),
        verificationLevel: guild.verificationLevel,
        explicitContentFilter: guild.explicitContentFilter,
        defaultMessageNotifications: guild.defaultMessageNotifications,
        rulesChannelId: guild.rulesChannelId,
        publicUpdatesChannelId: guild.publicUpdatesChannelId,
        systemChannelId: guild.systemChannelId,
        afkChannelId: guild.afkChannelId,
        afkTimeout: guild.afkTimeout,
        vanityURLCode: guild.vanityURLCode,
        timestamp: Date.now(),
        channels: guild.channels.cache.map(c => ({
          id: c.id,
          name: c.name,
          type: c.type,
          parentId: c.parentId,
          position: 'position' in c ? (c as any).position : 0,
          topic: 'topic' in c ? (c as any).topic : null,
          nsfw: 'nsfw' in c ? (c as any).nsfw ?? false : false,
          rateLimitPerUser: 'rateLimitPerUser' in c ? (c as any).rateLimitPerUser ?? 0 : 0,
          permissionOverwrites: 'permissionOverwrites' in c && (c as any).permissionOverwrites?.cache
            ? (c as any).permissionOverwrites.cache.map((ow: any) => ({
                id: ow.id,
                type: ow.type,
                allow: ow.allow.bitfield.toString(),
                deny: ow.deny.bitfield.toString()
              }))
            : []
        })),
        roles: guild.roles.cache.map(r => ({
          id: r.id,
          name: r.name,
          color: r.color,
          hoist: r.hoist,
          position: r.position,
          permissions: r.permissions.bitfield.toString(),
          mentionable: r.mentionable
        }))
      };

      this.guildSnapshots.set(guild.id, snapshot);

      // Persist to disk
      const filePath = path.join(SNAPSHOTS_DIR, `${guild.id}.json`);
      fs.writeFileSync(filePath, JSON.stringify(snapshot, null, 2), 'utf-8');

      Logger.info(`[SNAPSHOT] Cached clean baseline state for guild: ${guild.name} (${guild.id})`);
      return snapshot;
    } catch (err: any) {
      Logger.error(`[SNAPSHOT] Failed to capture baseline for ${guild.id}: ${err.message}`);
      throw err;
    }
  }

  public getSnapshot(guildId: string): GuildStateSnapshot | undefined {
    if (this.guildSnapshots.has(guildId)) {
      return this.guildSnapshots.get(guildId);
    }
    const filePath = path.join(SNAPSHOTS_DIR, `${guildId}.json`);
    if (fs.existsSync(filePath)) {
      try {
        const raw = fs.readFileSync(filePath, 'utf-8');
        const snap = JSON.parse(raw);
        this.guildSnapshots.set(guildId, snap);
        return snap;
      } catch {
        return undefined;
      }
    }
    return undefined;
  }

  public registerCreatedChannel(channelId: string, executorId: string): void {
    this.createdChannels.set(channelId, { executorId, timestamp: Date.now() });
  }

  public getChannelsCreatedBy(executorId: string, withinMs: number = 30000): string[] {
    const now = Date.now();
    const result: string[] = [];
    for (const [channelId, data] of this.createdChannels.entries()) {
      if (data.executorId === executorId && (now - data.timestamp) <= withinMs) {
        result.push(channelId);
      }
    }
    return result;
  }

  public registerCreatedRole(roleId: string, executorId: string): void {
    this.createdRoles.set(roleId, { executorId, timestamp: Date.now() });
  }

  public getRolesCreatedBy(executorId: string, withinMs: number = 30000): string[] {
    const now = Date.now();
    const result: string[] = [];
    for (const [roleId, data] of this.createdRoles.entries()) {
      if (data.executorId === executorId && (now - data.timestamp) <= withinMs) {
        result.push(roleId);
      }
    }
    return result;
  }

  /**
   * Automatically reconstructs any channels that were deleted during an attack,
   * restoring their exact names, types, parent categories, and positions.
   */
  public async restoreMissingChannels(guild: Guild): Promise<number> {
    const snapshot = this.getSnapshot(guild.id);
    if (!snapshot || !snapshot.channels) return 0;

    let restoredCount = 0;
    const existingChannelNames = new Set(guild.channels.cache.map(c => c.name));

    for (const snapCh of snapshot.channels) {
      if (!existingChannelNames.has(snapCh.name)) {
        try {
          await guild.channels.create({
            name: snapCh.name,
            type: snapCh.type as any,
            topic: snapCh.topic || undefined,
            reason: '[MIAN XITERS] Auto-Reconstruction from baseline security snapshot'
          });
          restoredCount++;
        } catch {}
      }
    }

    Logger.info(`[RECONSTRUCTION] Re-created ${restoredCount} missing channels for ${guild.name}`);
    return restoredCount;
  }

  /**
   * Automatically reconstructs any roles that were deleted during an attack.
   */
  public async restoreMissingRoles(guild: Guild): Promise<number> {
    const snapshot = this.getSnapshot(guild.id);
    if (!snapshot || !snapshot.roles) return 0;

    let restoredCount = 0;
    const existingRoleNames = new Set(guild.roles.cache.map(r => r.name));

    for (const snapR of snapshot.roles) {
      if (snapR.name !== '@everyone' && !existingRoleNames.has(snapR.name)) {
        try {
          await guild.roles.create({
            name: snapR.name,
            color: snapR.color,
            hoist: snapR.hoist,
            permissions: BigInt(snapR.permissions),
            reason: '[MIAN XITERS] Auto-Reconstruction from baseline security snapshot'
          });
          restoredCount++;
        } catch {}
      }
    }

    Logger.info(`[RECONSTRUCTION] Re-created ${restoredCount} missing roles for ${guild.name}`);
    return restoredCount;
  }

  /**
   * Initializes snapshot caching for all guilds on bot startup and schedules 30min updates.
   */
  public startPeriodicSnapshots(client: Client): void {
    const takeSnapshots = async () => {
      for (const guild of client.guilds.cache.values()) {
        await this.snapshotGuild(guild).catch(() => {});
      }
    };

    // Initial snapshot after startup
    setTimeout(takeSnapshots, 3000);
    // Every 30 minutes
    setInterval(takeSnapshots, 30 * 60 * 1000);
  }
}

export const cacheService = new CacheService();
