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

export const LEVEL_TREE_STAGES: { [level: number]: { stage: string; emoji: string } } = {
  0: { stage: 'Dormant Acorn', emoji: '🌰' },
  1: { stage: 'Tender Sprout', emoji: '🌱' },
  2: { stage: 'Little Stem', emoji: '🌿' },
  3: { stage: 'Young Sapling', emoji: '🪴' },
  4: { stage: 'Small Willow', emoji: '🎋' },
  5: { stage: 'Lush Oak Tree', emoji: '🌳' },
  6: { stage: 'Forest Giant Pine', emoji: '🌲' },
  7: { stage: 'Golden Autumn Maple', emoji: '🍁' },
  8: { stage: 'Sacred Sakura Blossom', emoji: '🌸' },
  9: { stage: 'Mythic Spirit Tree', emoji: '🔮' },
  10: { stage: 'Eternal World Tree (Yggdrasil 👑)', emoji: '🌟' }
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

  public readonly MAX_LEVEL = 10;

  public getXpForNextLevel(level: number): number {
    if (level >= this.MAX_LEVEL) return Infinity;
    // Hardcore leveling curve: Level 1 requires 470 XP, scaling to 16,000+ XP for Level 10!
    // Total XP required across all levels is over 60,000 XP (thousands of active chat sessions over months).
    return Math.floor(120 * Math.pow(level + 1, 2) + 350 * (level + 1));
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

    // If user has already achieved Max Level 10, they remain immortalized at max level
    if (userData.level >= this.MAX_LEVEL) {
      this.save();
      return;
    }

    // Check 60-second cooldown
    if (now - userData.lastXpTimestamp < this.COOLDOWN_MS) {
      this.save();
      return;
    }

    // Award random XP between 15 and 25 (standard Arcane rate)
    const earnedXp = Math.floor(Math.random() * 11) + 15;
    userData.xp += earnedXp;
    userData.lastXpTimestamp = now;

    // Check for Level Up (capped at MAX_LEVEL 10)
    let xpNeeded = this.getXpForNextLevel(userData.level);
    let leveledUp = false;

    while (userData.xp >= xpNeeded && userData.level < this.MAX_LEVEL) {
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

  public getLevelTreeStage(level: number): { stage: string; emoji: string } {
    return LEVEL_TREE_STAGES[Math.min(Math.max(level, 0), 10)] || LEVEL_TREE_STAGES[0];
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

    const treeStage = this.getLevelTreeStage(newLevel);

    const embed = new EmbedBuilder()
      .setColor(0x2ECC71)
      .setAuthor({
        name: `🎉 LEVEL UP & TREE EVOLUTION!`,
        iconURL: message.author.displayAvatarURL({ size: 128 })
      })
      .setDescription(
        `GG <@${message.author.id}>! You leveled up to **Level ${newLevel}**! 🚀\n` +
        `🌱 **Tree Growth:** Your personal tree evolved into **${treeStage.emoji} ${treeStage.stage}**!${rewardText}\n\n` +
        `> Keep chatting and watering the community tree with \`/tree\` to climb the ranks!`
      )
      .setFooter({
        text: `${guild.name} • Arcane Leveling & Tree Growth Engine`,
        iconURL: guild.iconURL() || undefined
      })
      .setTimestamp();

    await (message.channel as any).send({
      content: `🎉 Congratulations <@${message.author.id}>! Your tree has grown!`,
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
