import dotenv from 'dotenv';
import path from 'path';
import { ThresholdConfig } from '../types/index.js';

dotenv.config();

export const BOT_CONFIG = {
  name: 'MIAN XITERS',
  author: '1nOnlyMian',
  bio: 'MIAN XITERS is an Advance all rounder security bot dev by 1nOnlyMian . MIAN XITERS ULTIMATE DOMINANCE !',
  slogan: 'MIAN XITERS ULTIMATE DOMINANCE !',
  version: '2.0.0-PROD',
  token: process.env.DISCORD_TOKEN || '',
  clientId: process.env.CLIENT_ID || '',
  logWebhookUrl: process.env.LOG_WEBHOOK_URL || '',
  colors: {
    primary: 0xFF0033, // Crimson Red from logo
    danger: 0xDC2626,  // Threat red
    success: 0x10B981, // Defense green
    warning: 0xF59E0B, // Alert yellow
    dark: 0x0E1015     // Carbon dark
  },
  assets: {
    logoPath: path.resolve(process.cwd(), 'assets', 'logo.png')
  }
};

export const THRESHOLDS: ThresholdConfig = {
  channelDelete: {
    limit: parseInt(process.env.LIMIT_CHANNEL_DELETE || '2', 10),
    windowMs: 10 * 1000 // 10 seconds
  },
  channelCreate: {
    limit: parseInt(process.env.LIMIT_CHANNEL_CREATE || '3', 10),
    windowMs: 10 * 1000
  },
  roleDelete: {
    limit: parseInt(process.env.LIMIT_ROLE_DELETE || '2', 10),
    windowMs: 10 * 1000
  },
  roleCreate: {
    limit: parseInt(process.env.LIMIT_ROLE_CREATE || '2', 10),
    windowMs: 10 * 1000
  },
  ban: {
    limit: parseInt(process.env.LIMIT_BAN || '3', 10),
    windowMs: 10 * 1000
  },
  kick: {
    limit: parseInt(process.env.LIMIT_KICK || '3', 10),
    windowMs: 10 * 1000
  },
  webhook: {
    limit: parseInt(process.env.LIMIT_WEBHOOK || '1', 10),
    windowMs: 10 * 1000
  },
  emojiDelete: {
    limit: parseInt(process.env.LIMIT_EMOJI_DELETE || '3', 10),
    windowMs: 10 * 1000
  },
  stickerDelete: {
    limit: parseInt(process.env.LIMIT_STICKER_DELETE || '3', 10),
    windowMs: 10 * 1000
  },
  raidJoinBurst: {
    limit: parseInt(process.env.LIMIT_RAID_JOINS || '5', 10),
    windowMs: 4 * 1000 // 5 joins in 4 seconds
  },
  minAccountAgeDays: parseInt(process.env.MIN_ACCOUNT_AGE_DAYS || '3', 10),
  quarantineDefaultAvatar: process.env.QUARANTINE_DEFAULT_AVATAR !== 'false',
  maxMentionsPerMessage: parseInt(process.env.MAX_MENTIONS || '4', 10)
};
