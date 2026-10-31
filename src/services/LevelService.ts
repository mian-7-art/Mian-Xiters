import fs from 'fs';
import path from 'path';
import { Message, EmbedBuilder, GuildMember } from 'discord.js';
import { Logger } from '../utils/logger.js';

export interface UserLevelData {
  userId: string;
  xp: number;
  level: number;
  totalMessages: number;
  lastXpTimestamp: number;
}

export interface LevelsDatabase {
  [guildId: string]: {
    [userId: string]: UserLevelData;
  };
}

const DATA_DIR = path.resolve(process.cwd(), 'data');
const LEVELS_FILE = path.join(DATA_DIR, 'levels.json');

// Milestone Role Rewards for JENNY PLAYZ (Guild: 1457401315415298193)
const ROLE_REWARDS: { [level: number]: string } = {
  5: '1565044668939505824',  // Level 5 -> 👑・𝑴𝑬𝑴𝑩𝑬𝑹
  10: '1565044582717333524'  // Level 10 -> 💎・𝑽𝑰𝑷 𝑳𝑬𝑮𝑬𝑵𝑫
};

class LevelService {
  private data: LevelsDatabase = {};
  private readonly COOLDOWN_MS = 60 * 1000; // 1 minute XP cooldown (standard Arcane/MEE6)

  constructor() {
    this.init();
  }

  private init(): void {
    try {
      if (!fs.existsSync(DATA_DIR)) {
        fs.mkdirSync(DATA_DIR, { recursive: true });
      }
      if (fs.existsSync(LEVELS_FILE)) {
        const raw = fs.readFileSync(LEVELS_FILE, 'utf-8');
        this.data = JSON.parse(raw);
      } else {
        this.save();
      }
    } catch (err: any) {
      Logger.error(`[LEVELS] Failed to init levels db: ${err.message}`);
      this.data = {};
    }
  }

  private save(): void {
    try {
      const temp = `${LEVELS_FILE}.tmp`;
      fs.writeFileSync(temp, JSON.stringify(this.data, null, 2), 'utf-8');
      fs.renameSync(temp, LEVELS_FILE);
    } catch (err: any) {
      Logger.error(`[LEVELS] Failed to save levels db: ${err.message}`);
    }
  }

  private ensureGuild(guildId: string): void {
    if (!this.data[guildId]) {
      this.data[guildId] = {};
    }
  }

  public getXpForNextLevel(level: number): number {
    // Arcane / MEE6 leveling formula: 5 * L^2 + 50 * L + 100
    return 5 * Math.pow(level, 2) + 50 * level + 100;
  }

  public getUser(guildId: string, userId: string): UserLevelData {
    this.ensureGuild(guildId);
    if (!this.data[guildId][userId]) {
      this.data[guildId][userId] = {
        userId,
        xp: 0,
        level: 0,
        totalMessages: 0,
        lastXpTimestamp: 0
      };
    }
    return this.data[guildId][userId];
  }

  public async handleMessage(message: Message): Promise<void> {
    if (!message.guild || message.author.bot) return;

    const guildId = message.guild.id;
    const userId = message.author.id;
    const now = Date.now();

    const userData = this.getUser(guildId, userId);
    userData.totalMessages++;

    // Check 60-second cooldown
    if (now - userData.lastXpTimestamp < this.COOLDOWN_MS) {
      this.save();
      return;
    }

    // Award random XP between 15 and 25 (standard Arcane rate)
    const earnedXp = Math.floor(Math.random() * 11) + 15;
    userData.xp += earnedXp;
    userData.lastXpTimestamp = now;

    // Check for Level Up
    let xpNeeded = this.getXpForNextLevel(userData.level);
    let leveledUp = false;

    while (userData.xp >= xpNeeded) {
      userData.xp -= xpNeeded;
      userData.level++;
      leveledUp = true;
      xpNeeded = this.getXpForNextLevel(userData.level);
    }

    this.save();

    if (leveledUp) {
      await this.sendLevelUpNotification(message, userData.level);
    }
  }

  private async sendLevelUpNotification(message: Message, newLevel: number): Promise<void> {
    if (!message.guild || !message.channel.isTextBased() || !('send' in message.channel)) return;

    const member = message.member;
    const guild = message.guild;

    // Role reward check
    let rewardText = '';
    const rewardRoleId = ROLE_REWARDS[newLevel];
    if (rewardRoleId && member) {
      try {
        const role = guild.roles.cache.get(rewardRoleId);
        if (role && !member.roles.cache.has(rewardRoleId)) {
          await member.roles.add(rewardRoleId, `Level ${newLevel} role reward milestone`);
          rewardText = `\n🎁 **Unlocked Role Reward:** <@&${rewardRoleId}>!`;
        }
      } catch (err: any) {
        Logger.warn(`[LEVELS] Failed to assign role reward: ${err.message}`);
      }
    }

    const embed = new EmbedBuilder()
      .setColor(0xE03C3C)
      .setAuthor({
        name: `🎉 LEVEL UP!`,
        iconURL: message.author.displayAvatarURL({ size: 128 })
      })
      .setDescription(
        `GG <@${message.author.id}>! You just leveled up to **Level ${newLevel}**! 🚀${rewardText}\n\n` +
        `> Keep chatting in the community to climb the server leaderboard!`
      )
      .setFooter({
        text: `${guild.name} • Arcane Leveling Engine`,
        iconURL: guild.iconURL() || undefined
      })
      .setTimestamp();

    await (message.channel as any).send({
      content: `🎉 Congratulations <@${message.author.id}>!`,
      embeds: [embed]
    }).catch(() => {});
  }

  public getUserRank(guildId: string, userId: string): number {
    this.ensureGuild(guildId);
    const users = Object.values(this.data[guildId]).sort((a, b) => {
      if (b.level !== a.level) return b.level - a.level;
      return b.xp - a.xp;
    });

    const index = users.findIndex(u => u.userId === userId);
    return index !== -1 ? index + 1 : users.length + 1;
  }

  public getLeaderboard(guildId: string, limit = 10): (UserLevelData & { rank: number })[] {
    this.ensureGuild(guildId);
    const sorted = Object.values(this.data[guildId]).sort((a, b) => {
      if (b.level !== a.level) return b.level - a.level;
      return b.xp - a.xp;
    });

    return sorted.slice(0, limit).map((u, i) => ({
      ...u,
      rank: i + 1
    }));
  }

  public createProgressBar(current: number, total: number, barSize = 10): string {
    const progress = Math.min(Math.max(current / total, 0), 1);
    const filledCount = Math.round(progress * barSize);
    const emptyCount = barSize - filledCount;

    const filledBar = '🟩'.repeat(filledCount);
    const emptyBar = '⬛'.repeat(emptyCount);

    return `${filledBar}${emptyBar}`;
  }
}

export const levelService = new LevelService();
