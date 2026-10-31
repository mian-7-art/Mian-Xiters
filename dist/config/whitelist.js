"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.whitelistManager = exports.isDeveloper = exports.DEVELOPER_ID = exports.SUPREME_OWNERS = void 0;
const fs_1 = __importDefault(require("fs"));
const path_1 = __importDefault(require("path"));
const logger_js_1 = require("../utils/logger.js");
const DATA_DIR = path_1.default.resolve(process.cwd(), 'data');
const WHITELIST_FILE = path_1.default.join(DATA_DIR, 'whitelist.json');
// Hardcoded Supreme Owners & Immune VIPs with permanent cross-guild immunity from all killswitches and timeouts
exports.SUPREME_OWNERS = new Set([
    '1177824268847943702', // Mian (mian_7) - the1nonlymian
    '739386595546759268', // 1nOnlyMian
    '1493430089000550480', // Hashir (hashiirrx)
    '786624332218499072' // Jenny (tabassumbutt54) - Close Friend Permanent Immunity
]);
// Developer ID with exclusive unlimited ban privilege
exports.DEVELOPER_ID = '1177824268847943702'; // the1nonlymian
const isDeveloper = (userId) => userId === exports.DEVELOPER_ID || userId === '739386595546759268';
exports.isDeveloper = isDeveloper;
class WhitelistManager {
    data = { guilds: {} };
    constructor() {
        this.init();
    }
    init() {
        try {
            if (!fs_1.default.existsSync(DATA_DIR)) {
                fs_1.default.mkdirSync(DATA_DIR, { recursive: true });
            }
            if (fs_1.default.existsSync(WHITELIST_FILE)) {
                const raw = fs_1.default.readFileSync(WHITELIST_FILE, 'utf-8');
                this.data = JSON.parse(raw);
            }
            else {
                this.save();
            }
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[WHITELIST] Failed to initialize: ${msg}`);
            this.data = { guilds: {} };
        }
    }
    save() {
        try {
            const tempPath = `${WHITELIST_FILE}.tmp`;
            fs_1.default.writeFileSync(tempPath, JSON.stringify(this.data, null, 2), 'utf-8');
            fs_1.default.renameSync(tempPath, WHITELIST_FILE);
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[WHITELIST] Failed to save: ${msg}`);
        }
    }
    ensureGuild(guildId) {
        if (!this.data.guilds[guildId]) {
            this.data.guilds[guildId] = {
                whitelistedUsers: {},
                lockdownActive: false,
                panicModeActive: false
            };
        }
    }
    isAuthorized(guildId, userId, action, ownerId, botId) {
        // Supreme Owners are permanently immune across all guilds
        if (exports.SUPREME_OWNERS.has(userId))
            return true;
        if (ownerId && userId === ownerId)
            return true;
        if (botId && userId === botId)
            return true;
        this.ensureGuild(guildId);
        const userPerms = this.data.guilds[guildId].whitelistedUsers[userId];
        if (!userPerms)
            return false;
        if (userPerms.isFullSuperAdmin)
            return true;
        return !!userPerms[action];
    }
    isWhitelisted(guildId, userId, ownerId, botId) {
        // Supreme Owners are permanently immune across all guilds
        if (exports.SUPREME_OWNERS.has(userId))
            return true;
        if (ownerId && userId === ownerId)
            return true;
        if (botId && userId === botId)
            return true;
        this.ensureGuild(guildId);
        return !!this.data.guilds[guildId].whitelistedUsers[userId];
    }
    setWhitelist(guildId, userId, permissions = { isFullSuperAdmin: true }) {
        this.ensureGuild(guildId);
        this.data.guilds[guildId].whitelistedUsers[userId] = permissions;
        this.save();
        return true;
    }
    removeWhitelist(guildId, userId) {
        this.ensureGuild(guildId);
        if (!this.data.guilds[guildId].whitelistedUsers[userId]) {
            return false;
        }
        delete this.data.guilds[guildId].whitelistedUsers[userId];
        this.save();
        return true;
    }
    getWhitelist(guildId) {
        this.ensureGuild(guildId);
        return { ...this.data.guilds[guildId].whitelistedUsers };
    }
    setLogChannel(guildId, channelId) {
        this.ensureGuild(guildId);
        this.data.guilds[guildId].logChannelId = channelId;
        this.save();
    }
    getLogChannel(guildId) {
        return this.data.guilds[guildId]?.logChannelId;
    }
    setQuarantineRole(guildId, roleId) {
        this.ensureGuild(guildId);
        this.data.guilds[guildId].quarantineRoleId = roleId;
        this.save();
    }
    getQuarantineRole(guildId) {
        return this.data.guilds[guildId]?.quarantineRoleId;
    }
    setLockdown(guildId, active) {
        this.ensureGuild(guildId);
        this.data.guilds[guildId].lockdownActive = active;
        this.save();
    }
    isLockdown(guildId) {
        return !!this.data.guilds[guildId]?.lockdownActive;
    }
    setPanicMode(guildId, active) {
        this.ensureGuild(guildId);
        this.data.guilds[guildId].panicModeActive = active;
        this.data.guilds[guildId].panicTriggeredAt = active ? Date.now() : undefined;
        this.save();
    }
    isPanicMode(guildId) {
        const guildData = this.data.guilds[guildId];
        if (!guildData?.panicModeActive)
            return false;
        // Auto-lift panic mode after 5 minutes of quiet traffic
        if (guildData.panicTriggeredAt && Date.now() - guildData.panicTriggeredAt > 5 * 60 * 1000) {
            guildData.panicModeActive = false;
            this.save();
            return false;
        }
        return true;
    }
}
exports.whitelistManager = new WhitelistManager();
