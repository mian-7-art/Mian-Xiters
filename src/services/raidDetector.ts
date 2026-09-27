import { GuildMember } from 'discord.js';
import { Logger } from '../utils/logger.js';

export interface RaidFingerprint {
  similarUsernameCount: number;
  burstCount: number;
  suspiciousRatio: number;
}

export class AdvancedRaidDetector {
  private recentJoins: { id: string; username: string; joinedAt: number; hasAvatar: boolean; accountAgeDays: number }[] = [];

  /**
   * Evaluates incoming member against historical raid patterns in memory.
   */
  public registerAndAnalyze(member: GuildMember): { isRaidWave: boolean; confidence: number; reason: string } {
    const now = Date.now();
    const accountAgeDays = (now - member.user.createdTimestamp) / (1000 * 60 * 60 * 24);
    const hasAvatar = !!member.user.avatar;

    // Prune joins older than 15 seconds
    this.recentJoins = this.recentJoins.filter(j => now - j.joinedAt < 15000);

    this.recentJoins.push({
      id: member.id,
      username: member.user.username.toLowerCase(),
      joinedAt: now,
      hasAvatar,
      accountAgeDays
    });

    const windowCount = this.recentJoins.length;
    if (windowCount < 3) {
      return { isRaidWave: false, confidence: 0, reason: '' };
    }

    // 1. Calculate default avatar ratio
    const defaultAvatarCount = this.recentJoins.filter(j => !j.hasAvatar).length;
    const defaultAvatarRatio = defaultAvatarCount / windowCount;

    // 2. Calculate brand-new accounts (< 3 days) ratio
    const youngAccountsCount = this.recentJoins.filter(j => j.accountAgeDays < 3).length;
    const youngAccountsRatio = youngAccountsCount / windowCount;

    // 3. Detect username pattern repetition (e.g. raid bots sharing prefixes or numbers)
    let similarNameMatches = 0;
    const currentName = member.user.username.toLowerCase();
    for (const prior of this.recentJoins) {
      if (prior.id !== member.id) {
        if (this.calculateSimilarity(currentName, prior.username) > 0.75) {
          similarNameMatches++;
        }
      }
    }

    // Heuristic threat score
    let threatScore = 0;
    if (windowCount >= 5) threatScore += 40;
    if (defaultAvatarRatio >= 0.6) threatScore += 30;
    if (youngAccountsRatio >= 0.5) threatScore += 30;
    if (similarNameMatches >= 2) threatScore += 35;

    const isRaidWave = threatScore >= 60;
    const reason = `Threat Score: ${threatScore}/100 [Burst: ${windowCount}/15s, DefaultAvatar: ${(defaultAvatarRatio * 100).toFixed(0)}%, YoungAccs: ${(youngAccountsRatio * 100).toFixed(0)}%, NamePattern: ${similarNameMatches}]`;

    return {
      isRaidWave,
      confidence: Math.min(100, threatScore),
      reason
    };
  }

  /**
   * Fast Jaro-Winkler string similarity approximation
   */
  private calculateSimilarity(s1: string, s2: string): number {
    if (s1 === s2) return 1.0;
    if (s1.length === 0 || s2.length === 0) return 0.0;

    let matchCount = 0;
    const minLen = Math.min(s1.length, s2.length);
    for (let i = 0; i < minLen; i++) {
      if (s1[i] === s2[i]) matchCount++;
    }
    return matchCount / Math.max(s1.length, s2.length);
  }

  public reset(): void {
    this.recentJoins = [];
  }
}

export const raidDetector = new AdvancedRaidDetector();
