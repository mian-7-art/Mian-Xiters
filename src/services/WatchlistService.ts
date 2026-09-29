import fs from 'fs';
import path from 'path';
import { Logger } from '../utils/logger.js';

export interface WatchlistEntry {
  userId: string;
  tag: string;
  reason: string;
  addedAt: number;
  addedBy: string;
  strictMode: boolean; // Zero tolerance: any infraction results in immediate maximum penalty
  strikes: number;
  lastInfraction?: string;
  lastInfractionAt?: number;
}

export interface WatchlistData {
  [userId: string]: WatchlistEntry;
}

const DATA_DIR = path.resolve(process.cwd(), 'data');
const WATCHLIST_FILE = path.join(DATA_DIR, 'watchlist.json');

class WatchlistService {
  private entries: WatchlistData = {};

  constructor() {
    this.init();
  }

  private init(): void {
    try {
      if (!fs.existsSync(DATA_DIR)) {
        fs.mkdirSync(DATA_DIR, { recursive: true });
      }
      if (fs.existsSync(WATCHLIST_FILE)) {
        const raw = fs.readFileSync(WATCHLIST_FILE, 'utf-8');
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
      } else {
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
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[WATCHLIST] Init error: ${msg}`);
      this.entries = {};
    }
  }

  private save(): void {
    try {
      const tempPath = `${WATCHLIST_FILE}.tmp`;
      fs.writeFileSync(tempPath, JSON.stringify(this.entries, null, 2), 'utf-8');
      fs.renameSync(tempPath, WATCHLIST_FILE);
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[WATCHLIST] Save error: ${msg}`);
    }
  }

  public isWatchlisted(userId: string): boolean {
    return !!this.entries[userId];
  }

  public getEntry(userId: string): WatchlistEntry | undefined {
    return this.entries[userId];
  }

  public addSuspect(
    userId: string,
    tag: string,
    reason: string,
    addedBy: string = '1nOnlyMian'
  ): WatchlistEntry {
    const existing = this.entries[userId];
    const entry: WatchlistEntry = {
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
    Logger.threat('WATCHLIST SUSPECT ADDED', `Added ${tag} (${userId}) under close surveillance.`);
    return entry;
  }

  public removeSuspect(userId: string): boolean {
    if (!this.entries[userId]) return false;
    delete this.entries[userId];
    this.save();
    return true;
  }

  public recordStrike(userId: string, infraction: string): number {
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

  public getAll(): WatchlistEntry[] {
    return Object.values(this.entries);
  }
}

export const watchlistService = new WatchlistService();
