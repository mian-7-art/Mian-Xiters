import {
  Guild,
  Client,
  ChannelType,
  GuildChannel,
  Role,
  CategoryChannel,
  GuildTextBasedChannel,
  VoiceBasedChannel,
  PermissionsBitField
} from 'discord.js';
import fs from 'fs';
import path from 'path';
import { GuildStateSnapshot, CachedChannelState, CachedRoleState } from '../types/index.js';
import { Logger } from '../utils/logger.js';

const SNAPSHOTS_DIR = path.resolve(process.cwd(), 'data', 'snapshots');

export class SnapshotService {
  private guildSnapshots: Map<string, GuildStateSnapshot> = new Map();
  private createdChannels: Map<string, { executorId: string; timestamp: number }> = new Map();
  private createdRoles: Map<string, { executorId: string; timestamp: number }> = new Map();

  constructor() {
    if (!fs.existsSync(SNAPSHOTS_DIR)) {
      fs.mkdirSync(SNAPSHOTS_DIR, { recursive: true });
    }
  }

  /**
   * Captures a synchronized atomic mirror of the guild state.
   */
  public async captureSnapshot(guild: Guild): Promise<GuildStateSnapshot> {
    try {
      const channels: CachedChannelState[] = [];

      for (const [, channel] of guild.channels.cache) {
        let overwrites: CachedChannelState['permissionOverwrites'] = [];
        if ('permissionOverwrites' in channel && (channel as any).permissionOverwrites?.cache) {
          overwrites = (channel as any).permissionOverwrites.cache.map((ow: any) => ({
            id: ow.id,
            type: ow.type,
            allow: ow.allow.bitfield.toString(),
            deny: ow.deny.bitfield.toString()
          }));
        }

        const isVoice = channel.isVoiceBased();
        const isText = channel.isTextBased();

        channels.push({
          id: channel.id,
          name: channel.name,
          type: channel.type,
          parentId: channel.parentId,
          position: 'position' in channel ? (channel as GuildChannel).position : 0,
          topic: 'topic' in channel ? (channel as any).topic ?? null : null,
          nsfw: 'nsfw' in channel ? (channel as any).nsfw ?? false : false,
          rateLimitPerUser: 'rateLimitPerUser' in channel ? (channel as any).rateLimitPerUser ?? 0 : 0,
          bitrate: isVoice ? (channel as VoiceBasedChannel).bitrate : undefined,
          userLimit: isVoice ? (channel as VoiceBasedChannel).userLimit : undefined,
          permissionOverwrites: overwrites
        });
      }

      const roles: CachedRoleState[] = Array.from(guild.roles.cache.values()).map(role => ({
        id: role.id,
        name: role.name,
        color: role.color,
        hoist: role.hoist,
        position: role.position,
        permissions: role.permissions?.bitfield ? role.permissions.bitfield.toString() : '0',
        mentionable: role.mentionable ?? false
      }));

      const snapshot: GuildStateSnapshot = {
        id: guild.id,
        name: guild.name,
        iconURL: typeof guild.iconURL === 'function' ? guild.iconURL({ extension: 'png', size: 1024 }) : null,
        bannerURL: typeof guild.bannerURL === 'function' ? guild.bannerURL({ extension: 'png', size: 1024 }) : null,
        splashURL: typeof guild.splashURL === 'function' ? guild.splashURL({ extension: 'png', size: 1024 }) : null,
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
        channels,
        roles
      };

      this.guildSnapshots.set(guild.id, snapshot);

      // Save to disk for crash persistence
      const filePath = path.join(SNAPSHOTS_DIR, `${guild.id}.json`);
      fs.writeFileSync(filePath, JSON.stringify(snapshot, null, 2), 'utf-8');

      Logger.info(`[SNAPSHOT] Refreshed atomic state mirror for: ${guild.name} (${channels.length} ch, ${roles.length} roles)`);
      return snapshot;
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[SNAPSHOT] Failed to capture state for ${guild.id}: ${msg}`);
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
        const snap = JSON.parse(raw) as GuildStateSnapshot;
        this.guildSnapshots.set(guildId, snap);
        return snap;
      } catch {
        return undefined;
      }
    }
    return undefined;
  }

  /**
   * Recreates a specific deleted channel instantly from cached state,
   * restoring type, category parenting, topic, and exact permission overwrites.
   */
  public async recreateChannelFromCache(guild: Guild, channelId: string): Promise<boolean> {
    const snapshot = this.getSnapshot(guild.id);
    if (!snapshot) return false;

    const cached = snapshot.channels.find(c => c.id === channelId);
    if (!cached) return false;

    try {
      const overwrites = cached.permissionOverwrites.map(ow => ({
        id: ow.id,
        allow: BigInt(ow.allow),
        deny: BigInt(ow.deny),
        type: ow.type
      }));

      await guild.channels.create({
        name: cached.name,
        type: cached.type as any,
        topic: cached.topic ?? undefined,
        parent: cached.parentId ?? undefined,
        position: cached.position,
        permissionOverwrites: overwrites,
        reason: '[MIAN XITERS] Autonomous Re-Creation of Nuked Channel'
      });

      Logger.mitigation('CHANNEL RECONSTRUCTED', `#${cached.name}`, 0);
      return true;
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[RECONSTRUCT] Failed to rebuild #${cached.name}: ${msg}`);
      return false;
    }
  }

  /**
   * Recreates all missing channels from snapshot.
   */
  public async restoreMissingChannels(guild: Guild): Promise<number> {
    const snapshot = this.getSnapshot(guild.id);
    if (!snapshot || !snapshot.channels) return 0;

    let count = 0;
    const existingNames = new Set(guild.channels.cache.map(c => c.name));

    for (const snapCh of snapshot.channels) {
      if (!existingNames.has(snapCh.name)) {
        try {
          const overwrites = snapCh.permissionOverwrites.map(ow => ({
            id: ow.id,
            allow: BigInt(ow.allow),
            deny: BigInt(ow.deny),
            type: ow.type
          }));

          await guild.channels.create({
            name: snapCh.name,
            type: snapCh.type as any,
            topic: snapCh.topic ?? undefined,
            parent: snapCh.parentId ?? undefined,
            permissionOverwrites: overwrites,
            reason: '[MIAN XITERS] Autonomous Reconstruction from Security Baseline'
          });
          count++;
        } catch {}
      }
    }
    return count;
  }

  /**
   * Recreates all missing roles from snapshot with exact bitfields and hierarchy.
   */
  public async restoreMissingRoles(guild: Guild): Promise<number> {
    const snapshot = this.getSnapshot(guild.id);
    if (!snapshot || !snapshot.roles) return 0;

    let count = 0;
    const existingNames = new Set(guild.roles.cache.map(r => r.name));

    for (const snapR of snapshot.roles) {
      if (snapR.name !== '@everyone' && !existingNames.has(snapR.name)) {
        try {
          await guild.roles.create({
            name: snapR.name,
            color: snapR.color,
            hoist: snapR.hoist,
            position: snapR.position,
            permissions: BigInt(snapR.permissions),
            mentionable: snapR.mentionable,
            reason: '[MIAN XITERS] Autonomous Role Reconstruction from Security Baseline'
          });
          count++;
        } catch {}
      }
    }
    return count;
  }

  public registerCreatedChannel(channelId: string, executorId: string): void {
    this.createdChannels.set(channelId, { executorId, timestamp: Date.now() });
  }

  public getChannelsCreatedBy(executorId: string, withinMs: number = 30000): string[] {
    const now = Date.now();
    const result: string[] = [];
    for (const [id, data] of this.createdChannels.entries()) {
      if (data.executorId === executorId && (now - data.timestamp) <= withinMs) {
        result.push(id);
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
    for (const [id, data] of this.createdRoles.entries()) {
      if (data.executorId === executorId && (now - data.timestamp) <= withinMs) {
        result.push(id);
      }
    }
    return result;
  }

  /**
   * Starts the 15-minute synchronization cycle.
   */
  public startSynchronizationCycle(client: Client): void {
    const refreshAll = async () => {
      for (const guild of client.guilds.cache.values()) {
        await this.captureSnapshot(guild).catch(() => {});
      }
    };

    setTimeout(refreshAll, 2000);
    // Every 15 minutes as per specification
    setInterval(refreshAll, 15 * 60 * 1000);
  }
}

export const snapshotService = new SnapshotService();
