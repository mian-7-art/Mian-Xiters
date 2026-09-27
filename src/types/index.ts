import {
  ChannelType,
  OverwriteType,
  PermissionResolvable,
  PermissionsBitField
} from 'discord.js';

export enum ThreatType {
  MASS_CHANNEL_DELETE = 'MASS_CHANNEL_DELETE',
  MASS_CHANNEL_CREATE = 'MASS_CHANNEL_CREATE',
  MASS_ROLE_DELETE = 'MASS_ROLE_DELETE',
  MASS_ROLE_CREATE = 'MASS_ROLE_CREATE',
  MASS_ROLE_UPDATE_PERMISSIONS = 'MASS_ROLE_UPDATE_PERMISSIONS',
  UNAUTHORIZED_ROLE_ASSIGNMENT = 'UNAUTHORIZED_ROLE_ASSIGNMENT',
  MASS_BAN = 'MASS_BAN',
  MASS_KICK = 'MASS_KICK',
  MASS_PRUNE = 'MASS_PRUNE',
  WEBHOOK_TAMPER = 'WEBHOOK_TAMPER',
  UNAUTHORIZED_BOT_ADD = 'UNAUTHORIZED_BOT_ADD',
  GUILD_TAMPER = 'GUILD_TAMPER',
  MASS_EMOJI_DELETE = 'MASS_EMOJI_DELETE',
  MASS_STICKER_DELETE = 'MASS_STICKER_DELETE',
  RAID_JOIN_BURST = 'RAID_JOIN_BURST',
  RAID_ACCOUNT_AGE = 'RAID_ACCOUNT_AGE',
  MASS_MENTION_SPAM = 'MASS_MENTION_SPAM',
  MALICIOUS_LINK_SPAM = 'MALICIOUS_LINK_SPAM',
  GUILD_CIRCUIT_BREAKER = 'GUILD_CIRCUIT_BREAKER',
  INTEGRATION_TAMPER = 'INTEGRATION_TAMPER',
  INVITE_FLOOD = 'INVITE_FLOOD',
  SECURITY_INFRASTRUCTURE_TAMPER = 'SECURITY_INFRASTRUCTURE_TAMPER'
}

export interface SecurityThreat {
  type: ThreatType;
  executorId: string;
  executorTag?: string;
  targetId?: string;
  guildId: string;
  timestamp: number;
  reason: string;
  metadata?: Record<string, unknown>;
}

export interface CachedPermissionOverwrite {
  id: string;
  type: OverwriteType;
  allow: string;
  deny: string;
}

export interface CachedChannelState {
  id: string;
  name: string;
  type: ChannelType;
  parentId: string | null;
  position: number;
  topic: string | null;
  nsfw: boolean;
  rateLimitPerUser: number;
  bitrate?: number;
  userLimit?: number;
  permissionOverwrites: CachedPermissionOverwrite[];
}

export interface CachedRoleState {
  id: string;
  name: string;
  color: number;
  hoist: boolean;
  position: number;
  permissions: string;
  mentionable: boolean;
}

export interface GuildStateSnapshot {
  id: string;
  name: string;
  iconURL: string | null;
  bannerURL: string | null;
  splashURL: string | null;
  verificationLevel: number;
  explicitContentFilter: number;
  defaultMessageNotifications: number;
  rulesChannelId: string | null;
  publicUpdatesChannelId: string | null;
  systemChannelId: string | null;
  afkChannelId: string | null;
  afkTimeout: number;
  vanityURLCode: string | null;
  timestamp: number;
  channels: CachedChannelState[];
  roles: CachedRoleState[];
}

export interface WhitelistPermissionConfig {
  allowChannelManage?: boolean;
  allowRoleManage?: boolean;
  allowMemberManage?: boolean;
  allowBotAdd?: boolean;
  isFullSuperAdmin?: boolean;
}

export interface WhitelistData {
  guilds: {
    [guildId: string]: {
      whitelistedUsers: {
        [userId: string]: WhitelistPermissionConfig;
      };
      logChannelId?: string;
      quarantineRoleId?: string;
      lockdownActive?: boolean;
      panicModeActive?: boolean;
      panicTriggeredAt?: number;
    };
  };
}

export interface ThresholdConfig {
  channelDelete: { limit: number; windowMs: number };
  channelCreate: { limit: number; windowMs: number };
  roleDelete: { limit: number; windowMs: number };
  roleCreate: { limit: number; windowMs: number };
  ban: { limit: number; windowMs: number };
  kick: { limit: number; windowMs: number };
  webhook: { limit: number; windowMs: number };
  emojiDelete: { limit: number; windowMs: number };
  stickerDelete: { limit: number; windowMs: number };
  raidJoinBurst: { limit: number; windowMs: number };
  minAccountAgeDays: number;
  quarantineDefaultAvatar: boolean;
  maxMentionsPerMessage: number;
}
