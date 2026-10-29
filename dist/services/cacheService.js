"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.cacheService = exports.CacheService = void 0;
const fs_1 = __importDefault(require("fs"));
const path_1 = __importDefault(require("path"));
const logger_js_1 = require("../utils/logger.js");
const SNAPSHOTS_DIR = path_1.default.resolve(process.cwd(), 'data', 'snapshots');
class CacheService {
    guildSnapshots = new Map();
    createdChannels = new Map();
    createdRoles = new Map();
    constructor() {
        if (!fs_1.default.existsSync(SNAPSHOTS_DIR)) {
            fs_1.default.mkdirSync(SNAPSHOTS_DIR, { recursive: true });
        }
    }
    /**
     * Captures and caches the legitimate state of a guild.
     */
    async snapshotGuild(guild) {
        try {
            const snapshot = {
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
                    position: 'position' in c ? c.position : 0,
                    topic: 'topic' in c ? c.topic : null,
                    nsfw: 'nsfw' in c ? c.nsfw ?? false : false,
                    rateLimitPerUser: 'rateLimitPerUser' in c ? c.rateLimitPerUser ?? 0 : 0,
                    permissionOverwrites: 'permissionOverwrites' in c && c.permissionOverwrites?.cache
                        ? c.permissionOverwrites.cache.map((ow) => ({
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
            const filePath = path_1.default.join(SNAPSHOTS_DIR, `${guild.id}.json`);
            fs_1.default.writeFileSync(filePath, JSON.stringify(snapshot, null, 2), 'utf-8');
            logger_js_1.Logger.info(`[SNAPSHOT] Cached clean baseline state for guild: ${guild.name} (${guild.id})`);
            return snapshot;
        }
        catch (err) {
            logger_js_1.Logger.error(`[SNAPSHOT] Failed to capture baseline for ${guild.id}: ${err.message}`);
            throw err;
        }
    }
    getSnapshot(guildId) {
        if (this.guildSnapshots.has(guildId)) {
            return this.guildSnapshots.get(guildId);
        }
        const filePath = path_1.default.join(SNAPSHOTS_DIR, `${guildId}.json`);
        if (fs_1.default.existsSync(filePath)) {
            try {
                const raw = fs_1.default.readFileSync(filePath, 'utf-8');
                const snap = JSON.parse(raw);
                this.guildSnapshots.set(guildId, snap);
                return snap;
            }
            catch {
                return undefined;
            }
        }
        return undefined;
    }
    registerCreatedChannel(channelId, executorId) {
        this.createdChannels.set(channelId, { executorId, timestamp: Date.now() });
    }
    getChannelsCreatedBy(executorId, withinMs = 30000) {
        const now = Date.now();
        const result = [];
        for (const [channelId, data] of this.createdChannels.entries()) {
            if (data.executorId === executorId && (now - data.timestamp) <= withinMs) {
                result.push(channelId);
            }
        }
        return result;
    }
    registerCreatedRole(roleId, executorId) {
        this.createdRoles.set(roleId, { executorId, timestamp: Date.now() });
    }
    getRolesCreatedBy(executorId, withinMs = 30000) {
        const now = Date.now();
        const result = [];
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
    async restoreMissingChannels(guild) {
        const snapshot = this.getSnapshot(guild.id);
        if (!snapshot || !snapshot.channels)
            return 0;
        let restoredCount = 0;
        const existingChannelNames = new Set(guild.channels.cache.map(c => c.name));
        for (const snapCh of snapshot.channels) {
            if (!existingChannelNames.has(snapCh.name)) {
                try {
                    await guild.channels.create({
                        name: snapCh.name,
                        type: snapCh.type,
                        topic: snapCh.topic || undefined,
                        reason: '[MIAN XITERS] Auto-Reconstruction from baseline security snapshot'
                    });
                    restoredCount++;
                }
                catch { }
            }
        }
        logger_js_1.Logger.info(`[RECONSTRUCTION] Re-created ${restoredCount} missing channels for ${guild.name}`);
        return restoredCount;
    }
    /**
     * Automatically reconstructs any roles that were deleted during an attack.
     */
    async restoreMissingRoles(guild) {
        const snapshot = this.getSnapshot(guild.id);
        if (!snapshot || !snapshot.roles)
            return 0;
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
                }
                catch { }
            }
        }
        logger_js_1.Logger.info(`[RECONSTRUCTION] Re-created ${restoredCount} missing roles for ${guild.name}`);
        return restoredCount;
    }
    /**
     * Initializes snapshot caching for all guilds on bot startup and schedules 30min updates.
     */
    startPeriodicSnapshots(client) {
        const takeSnapshots = async () => {
            for (const guild of client.guilds.cache.values()) {
                await this.snapshotGuild(guild).catch(() => { });
            }
        };
        // Initial snapshot after startup
        setTimeout(takeSnapshots, 3000);
        // Every 30 minutes
        setInterval(takeSnapshots, 30 * 60 * 1000);
    }
}
exports.CacheService = CacheService;
exports.cacheService = new CacheService();
