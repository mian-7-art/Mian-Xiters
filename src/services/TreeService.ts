import fs from 'fs';
import path from 'path';
import {
  EmbedBuilder,
  ButtonBuilder,
  ActionRowBuilder,
  ButtonStyle,
  ButtonInteraction,
  Guild,
  User
} from 'discord.js';
import { Logger } from '../utils/logger.js';
import { levelService } from './LevelService.js';

export interface GardenerData {
  userId: string;
  waters: number;
  lastWaterTimestamp: number;
  fruitsCollected: number;
}

export interface GuildTreeData {
  guildId: string;
  height: number; // in meters
  waterCount: number;
  leaves: number;
  fruits: number;
  stageIndex: number;
  lastWateredBy: string | null;
  lastWateredTimestamp: number;
  gardeners: { [userId: string]: GardenerData };
}

export interface TreeStage {
  name: string;
  emoji: string;
  minHeight: number;
  art: string;
  description: string;
}

export const TREE_STAGES: TreeStage[] = [
  {
    name: 'Dormant Acorn',
    emoji: '🌰',
    minHeight: 0,
    art: '        🌰\n     ░░░░░░░',
    description: 'A tiny magical seed resting in fertile soil, waiting for fresh water.'
  },
  {
    name: 'Tender Sprout',
    emoji: '🌱',
    minHeight: 1,
    art: '        🌱\n       ░░░░░',
    description: 'Two bright green leaves emerging towards the sunlight!'
  },
  {
    name: 'Young Sapling',
    emoji: '🪴',
    minHeight: 3,
    art: '        🌿\n        🌱\n       ░░░░░',
    description: 'Sturdy stems and vibrant foliage starting to take real shape.'
  },
  {
    name: 'Small Willow',
    emoji: '🎋',
    minHeight: 7,
    art: '       🍃🌿🍃\n         || \n       ░░░░░',
    description: 'Reaching upward with flexible branches dancing in the breeze.'
  },
  {
    name: 'Lush Oak Tree',
    emoji: '🌳',
    minHeight: 15,
    art: '      🌳🌳🌳\n     🌳🌳🌳🌳🌳\n         || \n         || \n       ░░░░░',
    description: 'A grand community oak providing cool shade and abundant leaves.'
  },
  {
    name: 'Great Forest Pine',
    emoji: '🌲',
    minHeight: 30,
    art: '         🌲\n        🌲🌲\n       🌲🌲🌲\n         ||\n       ░░░░░',
    description: 'A towering evergreen standing tall through any season or storm.'
  },
  {
    name: 'Sacred Blossom Tree',
    emoji: '🌸',
    minHeight: 55,
    art: '       🌸🌸🌸\n      🌸🌸🌸🌸🌸\n      🌸🌸🌸🌸🌸\n         || \n       ░░░░░',
    description: 'Enchanted pink cherry petals drifting gently across the server.'
  },
  {
    name: 'Eternal World Tree (Yggdrasil)',
    emoji: '🌟',
    minHeight: 90,
    art: '        ✨🌟✨\n      🌳🌳🌟🌳🌳\n     🌟🌳🌳🌳🌳🌟\n         |||| \n         |||| \n       ░░░░░░░░',
    description: 'The celestial community world tree, radiant with cosmic energy!'
  }
];

const DATA_DIR = path.resolve(process.cwd(), 'data');
const TREE_FILE = path.join(DATA_DIR, 'trees.json');

class TreeService {
  private data: { [guildId: string]: GuildTreeData } = {};
  public readonly WATER_COOLDOWN_MS = 3 * 60 * 1000; // 3-minute cooldown per person

  constructor() {
    this.init();
  }

  private init(): void {
    try {
      if (!fs.existsSync(DATA_DIR)) {
        fs.mkdirSync(DATA_DIR, { recursive: true });
      }
      if (fs.existsSync(TREE_FILE)) {
        const raw = fs.readFileSync(TREE_FILE, 'utf-8');
        this.data = JSON.parse(raw);
      } else {
        this.save();
      }
    } catch (err: any) {
      Logger.error(`[TREE] Failed to init tree db: ${err.message}`);
      this.data = {};
    }
  }

  private save(): void {
    try {
      const temp = `${TREE_FILE}.tmp`;
      fs.writeFileSync(temp, JSON.stringify(this.data, null, 2), 'utf-8');
      fs.renameSync(temp, TREE_FILE);
    } catch (err: any) {
      Logger.error(`[TREE] Failed to save tree db: ${err.message}`);
    }
  }

  public getTree(guildId: string): GuildTreeData {
    if (!this.data[guildId]) {
      this.data[guildId] = {
        guildId,
        height: 1.2,
        waterCount: 0,
        leaves: 15,
        fruits: 2,
        stageIndex: 1,
        lastWateredBy: null,
        lastWateredTimestamp: 0,
        gardeners: {}
      };
      this.save();
    }
    return this.data[guildId];
  }

  public getStage(height: number): TreeStage {
    let currentStage = TREE_STAGES[0];
    for (const stage of TREE_STAGES) {
      if (height >= stage.minHeight) {
        currentStage = stage;
      }
    }
    return currentStage;
  }

  public getNextStage(height: number): TreeStage | null {
    for (const stage of TREE_STAGES) {
      if (stage.minHeight > height) {
        return stage;
      }
    }
    return null;
  }

  public waterTree(guildId: string, user: User): {
    success: boolean;
    reason?: string;
    heightGain?: number;
    leavesGain?: number;
    xpEarned?: number;
    newStage?: TreeStage;
    tree?: GuildTreeData;
  } {
    const tree = this.getTree(guildId);
    const now = Date.now();

    if (!tree.gardeners[user.id]) {
      tree.gardeners[user.id] = {
        userId: user.id,
        waters: 0,
        lastWaterTimestamp: 0,
        fruitsCollected: 0
      };
    }

    const gardener = tree.gardeners[user.id];
    const diff = now - gardener.lastWaterTimestamp;

    if (diff < this.WATER_COOLDOWN_MS) {
      const remainingSeconds = Math.ceil((this.WATER_COOLDOWN_MS - diff) / 1000);
      return {
        success: false,
        reason: `⏳ Soil is still fresh! You can water again in **${remainingSeconds}s**.`
      };
    }

    const prevStage = this.getStage(tree.height);

    // Height gain: 0.1m - 0.3m (15% chance of a golden blessing +0.5m!)
    const isSuperWater = Math.random() < 0.15;
    const heightGain = isSuperWater ? 0.5 : parseFloat((0.1 + Math.random() * 0.15).toFixed(2));
    const leavesGain = Math.floor(Math.random() * 6) + 3;

    tree.height = parseFloat((tree.height + heightGain).toFixed(2));
    tree.waterCount++;
    tree.leaves += leavesGain;
    tree.lastWateredBy = user.id;
    tree.lastWateredTimestamp = now;

    gardener.waters++;
    gardener.lastWaterTimestamp = now;

    // Chance to sprout a golden fruit if tree is Stage 3+
    if (tree.height >= 7 && Math.random() < 0.25) {
      tree.fruits++;
    }

    // Award leveling XP for gardening activity!
    const xpEarned = isSuperWater ? 40 : 25;
    const userLevelData = levelService.getUser(guildId, user.id);
    userLevelData.xp += xpEarned;
    userLevelData.totalMessages++;

    const newStage = this.getStage(tree.height);
    const stageUp = newStage.name !== prevStage.name ? newStage : undefined;

    this.save();

    return {
      success: true,
      heightGain,
      leavesGain,
      xpEarned,
      newStage: stageUp,
      tree
    };
  }

  public pickFruit(guildId: string, user: User): {
    success: boolean;
    reason?: string;
    xpEarned?: number;
    fruitsLeft?: number;
  } {
    const tree = this.getTree(guildId);
    if (tree.fruits <= 0) {
      return {
        success: false,
        reason: '🍎 No ripe fruits on the tree right now! Keep watering to grow new fruits.'
      };
    }

    tree.fruits--;
    if (!tree.gardeners[user.id]) {
      tree.gardeners[user.id] = {
        userId: user.id,
        waters: 0,
        lastWaterTimestamp: 0,
        fruitsCollected: 0
      };
    }
    tree.gardeners[user.id].fruitsCollected++;

    // Harvesting grants 50 bonus XP
    const xpEarned = 50;
    const userLevelData = levelService.getUser(guildId, user.id);
    userLevelData.xp += xpEarned;

    this.save();

    return {
      success: true,
      xpEarned,
      fruitsLeft: tree.fruits
    };
  }

  public buildTreeEmbed(guild: Guild, lastActionText?: string): EmbedBuilder {
    const tree = this.getTree(guild.id);
    const currentStage = this.getStage(tree.height);
    const nextStage = this.getNextStage(tree.height);

    let progressText = '🌟 **MAXIMUM WORLD TREE REACHED**';
    if (nextStage) {
      const prevMin = currentStage.minHeight;
      const nextMin = nextStage.minHeight;
      const pct = Math.min(Math.max((tree.height - prevMin) / (nextMin - prevMin), 0), 1);
      const filled = Math.round(pct * 10);
      const bar = '🟩'.repeat(filled) + '⬛'.repeat(10 - filled);
      progressText = `${bar} **${Math.round(pct * 100)}%**\n> Next: **${nextStage.name}** at \`${nextStage.minHeight}m\``;
    }

    const lastWateredText = tree.lastWateredBy
      ? `<@${tree.lastWateredBy}> (<t:${Math.floor(tree.lastWateredTimestamp / 1000)}:R>)`
      : 'No one yet';

    // Top gardener
    const topGardener = Object.values(tree.gardeners).sort((a, b) => b.waters - a.waters)[0];
    const topGardenerText = topGardener
      ? `<@${topGardener.userId}> (\`${topGardener.waters} waters\`)`
      : 'None yet';

    const embed = new EmbedBuilder()
      .setColor(0x2ECC71) // Nature emerald green
      .setTitle(`🌱 ${guild.name} — Community Tree Garden`)
      .setDescription(
        `\`\`\`text\n${currentStage.art}\n\`\`\`\n` +
        `### ${currentStage.emoji} ${currentStage.name}\n` +
        `*${currentStage.description}*\n\n` +
        `**📈 Stage Growth Progress:**\n${progressText}`
      )
      .addFields(
        {
          name: '📏 Tree Height',
          value: `**${tree.height}m**`,
          inline: true
        },
        {
          name: '💧 Total Waters',
          value: `\`${tree.waterCount.toLocaleString()} times\``,
          inline: true
        },
        {
          name: '🍃 Leaves & Fruits',
          value: `\`🍃 ${tree.leaves.toLocaleString()}\` • \`🍎 ${tree.fruits} ripe\``,
          inline: true
        },
        {
          name: '⏳ Last Watered',
          value: lastWateredText,
          inline: true
        },
        {
          name: '🏆 Top Gardener',
          value: topGardenerText,
          inline: true
        },
        {
          name: '🎁 Gardening XP',
          value: `\`+25 XP\` per water`,
          inline: true
        }
      )
      .setFooter({
        text: lastActionText || `${guild.name} • Click the buttons below to care for the tree!`,
        iconURL: guild.iconURL() || undefined
      })
      .setTimestamp();

    return embed;
  }

  public buildTreeActionRow(): ActionRowBuilder<ButtonBuilder> {
    const waterBtn = new ButtonBuilder()
      .setCustomId('tree_water')
      .setLabel('Water Tree')
      .setEmoji('💧')
      .setStyle(ButtonStyle.Success);

    const harvestBtn = new ButtonBuilder()
      .setCustomId('tree_harvest')
      .setLabel('Pick Fruit')
      .setEmoji('🍎')
      .setStyle(ButtonStyle.Primary);

    const leaderboardBtn = new ButtonBuilder()
      .setCustomId('tree_leaderboard')
      .setLabel('Gardeners')
      .setEmoji('🏆')
      .setStyle(ButtonStyle.Secondary);

    const refreshBtn = new ButtonBuilder()
      .setCustomId('tree_refresh')
      .setLabel('Refresh')
      .setEmoji('🔄')
      .setStyle(ButtonStyle.Secondary);

    return new ActionRowBuilder<ButtonBuilder>().addComponents(
      waterBtn,
      harvestBtn,
      leaderboardBtn,
      refreshBtn
    );
  }

  public getGardenersLeaderboard(guildId: string, limit = 10): (GardenerData & { rank: number })[] {
    const tree = this.getTree(guildId);
    const sorted = Object.values(tree.gardeners).sort((a, b) => b.waters - a.waters);
    return sorted.slice(0, limit).map((g, i) => ({
      ...g,
      rank: i + 1
    }));
  }
}

export const treeService = new TreeService();
