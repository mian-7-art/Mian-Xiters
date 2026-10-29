"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.watchlistService = void 0;
const fs_1 = __importDefault(require("fs"));
const path_1 = __importDefault(require("path"));
const logger_js_1 = require("../utils/logger.js");
const DATA_DIR = path_1.default.resolve(process.cwd(), 'data');
const WATCHLIST_FILE = path_1.default.join(DATA_DIR, 'watchlist.json');
class WatchlistService {
    entries = {};
    constructor() {
        this.init();
    }
    init() {
        try {
            if (!fs_1.default.existsSync(DATA_DIR)) {
                fs_1.default.mkdirSync(DATA_DIR, { recursive: true });
            }
            if (fs_1.default.existsSync(WATCHLIST_FILE)) {
                const raw = fs_1.default.readFileSync(WATCHLIST_FILE, 'utf-8');
                this.entries = JSON.parse(raw);
                // Ensure beast_.here is registered
                if (!this.entries['1492308506433290402']) {
                    this.entries['1492308506433290402'] = {
                        userId: '1492308506433290402',
                        tag: 'beast_.here',
                        reason: 'High-risk suspect: attempted unauthorized @everyone ping raid & previous nuker activity',
                        addedAt: Date.now(),
                        addedBy: '1nOnlyMian',
                        strictMode: true,
                        strikes: 1,
                        lastInfraction: 'Unauthorized @everyone / @here Ping',
                        lastInfractionAt: Date.now()
                    };
                    this.save();
                }
                // Ensure walterwhite05726 (Nᴏʙɪ Tʜᴇ Fᴏx) is registered
                if (!this.entries['1535730040803696742']) {
                    this.entries['1535730040803696742'] = {
                        userId: '1535730040803696742',
                        tag: 'walterwhite05726 (Nᴏʙɪ Tʜᴇ Fᴏx)',
                        reason: 'High-risk suspect under close surveillance requested by 1nOnlyMian',
                        addedAt: Date.now(),
                        addedBy: '1nOnlyMian',
                        strictMode: true,
                        strikes: 0
                    };
                    this.save();
                }
            }
            else {
                // Seed default watchlist with beast_.here and walterwhite05726
                this.entries = {
                    '1492308506433290402': {
                        userId: '1492308506433290402',
                        tag: 'beast_.here',
                        reason: 'High-risk suspect: attempted unauthorized @everyone ping raid & previous nuker activity',
                        addedAt: Date.now(),
                        addedBy: '1nOnlyMian',
                        strictMode: true,
                        strikes: 1,
                        lastInfraction: 'Unauthorized @everyone / @here Ping',
                        lastInfractionAt: Date.now()
                    },
                    '1535730040803696742': {
                        userId: '1535730040803696742',
                        tag: 'walterwhite05726 (Nᴏʙɪ Tʜᴇ Fᴏx)',
                        reason: 'High-risk suspect under close surveillance requested by 1nOnlyMian',
                        addedAt: Date.now(),
                        addedBy: '1nOnlyMian',
                        strictMode: true,
                        strikes: 0
                    }
                };
                this.save();
            }
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[WATCHLIST] Init error: ${msg}`);
            this.entries = {};
        }
    }
    save() {
        try {
            const tempPath = `${WATCHLIST_FILE}.tmp`;
            fs_1.default.writeFileSync(tempPath, JSON.stringify(this.entries, null, 2), 'utf-8');
            fs_1.default.renameSync(tempPath, WATCHLIST_FILE);
        }
        catch (err) {
            const msg = err instanceof Error ? err.message : String(err);
            logger_js_1.Logger.error(`[WATCHLIST] Save error: ${msg}`);
        }
    }
    isWatchlisted(userId) {
        return !!this.entries[userId];
    }
    getEntry(userId) {
        return this.entries[userId];
    }
    addSuspect(userId, tag, reason, addedBy = '1nOnlyMian') {
        const existing = this.entries[userId];
        const entry = {
            userId,
            tag,
            reason,
            addedAt: existing ? existing.addedAt : Date.now(),
            addedBy,
            strictMode: true,
            strikes: existing ? existing.strikes + 1 : 1,
            lastInfraction: existing ? existing.lastInfraction : undefined,
            lastInfractionAt: existing ? existing.lastInfractionAt : undefined
        };
        this.entries[userId] = entry;
        this.save();
        logger_js_1.Logger.threat('WATCHLIST SUSPECT ADDED', `Added ${tag} (${userId}) under close surveillance.`);
        return entry;
    }
    removeSuspect(userId) {
        if (!this.entries[userId])
            return false;
        delete this.entries[userId];
        this.save();
        return true;
    }
    recordStrike(userId, infraction) {
        if (!this.entries[userId]) {
            this.addSuspect(userId, 'Unknown', infraction);
        }
        const entry = this.entries[userId];
        entry.strikes += 1;
        entry.lastInfraction = infraction;
        entry.lastInfractionAt = Date.now();
        this.save();
        return entry.strikes;
    }
    getAll() {
        return Object.values(this.entries);
    }
}
exports.watchlistService = new WatchlistService();
