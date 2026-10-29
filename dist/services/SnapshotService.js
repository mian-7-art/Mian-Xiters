"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.snapshotService = exports.SnapshotService = void 0;
const fs_1 = __importDefault(require("fs"));
const path_1 = __importDefault(require("path"));
const logger_js_1 = require("../utils/logger.js");
const SNAPSHOTS_DIR = path_1.default.resolve(process.cwd(), 'data', 'snapshots');
class SnapshotService {
    guildSnapshots = new Map();
    createdChannels = new Map();
    createdRoles = new Map();
    constructor() {
        if (!fs_1.default.existsSync(SNAPSHOTS_DIR)) {
            fs_1.default.mkdirSync(SNAPSHOTS_DIR, { recursive: true });
        }
    }
    /**
     * Captures a synchronized atomic mirror of the guild state.
     */
    async captureSnapshot(guild) {
        try {
            const channels = [];
            for (const [, channel] of guild.channels.cache) {
                let overwrites = [];
                if ('permissionOverwrites' in channel && channel.permissionOverwrites?.cache) {
                    overwrites = channel.permissionOverwrites.cache.map((ow) => ({
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
                    position: 'position' in channel ? channel.position : 0,
                    topic: 'topic' in channel ? channel.topic ?? null : null,
                    nsfw: 'nsfw' in channel ? channel.nsfw ?? false : false,
                    rateLimitPerUser: 'rateLimitPerUser' in channel ? channel.rateLimitPerUser ?? 0 : 0,
                    bitrate: isVoice ? channel.bitrate : undefined,
                    userLimit: isVoice ? channel.userLimit : undefined,
                    permissionOverwrites: overwrites
                });
            }
            const roles = Array.from(guild.roles.cache.values()).map(role => ({
                id: role.id,
                name: role.name,
                color: role.color,
                hoist: role.hoist,
                position: role.position,
                permissions: role.permissions?.bitfield ? role.permissions.bitfield.toString() : '0',
                mentionable: role.mentionable ?? false
            }));
            const snapshot = {
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
            const filePath = path_1.default.join(SNAPSHOTS_DIR, `${guild.id}.json`);
            fs_1.default.writeFileSync(filePath, JSON.stringify(snapshot, null, 2), 'utf-8');
            logger_js_1.Logger.info(`[SNAPSHOT] Refreshed atomic state mirror for: ${guild.name} (${channels.length} ch, ${roles.length} roles)`);
            return snapshot;
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[SNAPSHOT] Failed to capture state for ${guild.id}: ${msg}`);
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
    /**
     * Recreates a specific deleted channel instantly from cached state,
     * restoring type, category parenting, topic, and exact permission overwrites.
     */
    async recreateChannelFromCache(guild, channelId) {
        const snapshot = this.getSnapshot(guild.id);
        if (!snapshot)
            return false;
        const cached = snapshot.channels.find(c => c.id === channelId);
        if (!cached)
            return false;
        try {
            const overwrites = cached.permissionOverwrites.map(ow => ({
                id: ow.id,
                allow: BigInt(ow.allow),
                deny: BigInt(ow.deny),
                type: ow.type
            }));
            await guild.channels.create({
                name: cached.name,
                type: cached.type,
                topic: cached.topic ?? undefined,
                parent: cached.parentId ?? undefined,
                position: cached.position,
                permissionOverwrites: overwrites,
                reason: '[MIAN XITERS] Autonomous Re-Creation of Nuked Channel'
            });
            logger_js_1.Logger.mitigation('CHANNEL RECONSTRUCTED', `#${cached.name}`, 0);
            return true;
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[RECONSTRUCT] Failed to rebuild #${cached.name}: ${msg}`);
            return false;
        }
    }
    /**
     * Recreates all missing channels from snapshot.
     */
    async restoreMissingChannels(guild) {
        const snapshot = this.getSnapshot(guild.id);
        if (!snapshot || !snapshot.channels)
            return 0;
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
                        type: snapCh.type,
                        topic: snapCh.topic ?? undefined,
                        parent: snapCh.parentId ?? undefined,
                        permissionOverwrites: overwrites,
                        reason: '[MIAN XITERS] Autonomous Reconstruction from Security Baseline'
                    });
                    count++;
                }
                catch { }
            }
        }
        return count;
    }
    /**
     * Recreates all missing roles from snapshot with exact bitfields and hierarchy.
     */
    async restoreMissingRoles(guild) {
        const snapshot = this.getSnapshot(guild.id);
        if (!snapshot || !snapshot.roles)
            return 0;
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
                }
                catch { }
            }
        }
        return count;
    }
    registerCreatedChannel(channelId, executorId) {
        this.createdChannels.set(channelId, { executorId, timestamp: Date.now() });
    }
    getChannelsCreatedBy(executorId, withinMs = 30000) {
        const now = Date.now();
        const result = [];
        for (const [id, data] of this.createdChannels.entries()) {
            if (data.executorId === executorId && (now - data.timestamp) <= withinMs) {
                result.push(id);
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
    startSynchronizationCycle(client) {
        const refreshAll = async () => {
            for (const guild of client.guilds.cache.values()) {
                await this.captureSnapshot(guild).catch(() => { });
            }
        };
        setTimeout(refreshAll, 2000);
        // Every 15 minutes as per specification
        setInterval(refreshAll, 15 * 60 * 1000);
    }
}
exports.SnapshotService = SnapshotService;
exports.snapshotService = new SnapshotService();
