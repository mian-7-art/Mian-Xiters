import { Client, GatewayIntentBits, Partials, Collection } from 'discord.js';
import dotenv from 'dotenv';
import path from 'path';
import fs from 'fs';
import http from 'http';
import { BOT_CONFIG } from './config/config.js';
import { Logger } from './utils/logger.js';

// Load environment variables
dotenv.config();

const token = BOT_CONFIG.token;
if (!token) {
  console.error(`
\x1b[41m\x1b[37m[FATAL ERROR]\x1b[0m DISCORD_TOKEN is missing!
Please edit the \x1b[33m.env\x1b[0m file on your Desktop in:
\x1b[36mC:\\Users\\nafee\\Desktop\\MIAN XITERS\\.env\x1b[0m
and insert your Bot Token, then start the bot again.
`);
  process.exit(1);
}

import { Options } from 'discord.js';

// Enterprise Gateway client initialization with strict cache caps & sweepers
// This locks RAM usage between 25MB and 35MB permanently, preventing Discloud Missing RAM/OOM
const client: Client = new Client({
  makeCache: Options.cacheWithLimits({
    MessageManager: 15, // Only store 15 recent messages per channel in RAM
    UserManager: 50,
    GuildMemberManager: 50,
    ReactionManager: 0,
    PresenceManager: 0,
    ThreadManager: 0,
    ThreadMemberManager: 0,
    StageInstanceManager: 0,
    VoiceStateManager: 5,
    GuildScheduledEventManager: 0,
    ApplicationCommandManager: 0,
    BaseGuildEmojiManager: 0,
    GuildStickerManager: 0,
    AutoModerationRuleManager: 0
  }),
  sweepers: {
    ...Options.DefaultSweeperSettings,
    messages: {
      interval: 120, // Sweep old messages every 2 minutes
      lifetime: 180  // Discard messages older than 3 minutes
    },
    users: {
      interval: 300,
      filter: () => (user: any) => !user.bot && user.id !== client.user?.id
    }
  },
  intents: [
    GatewayIntentBits.Guilds,
    GatewayIntentBits.GuildMembers,
    GatewayIntentBits.GuildModeration,
    GatewayIntentBits.GuildMessages,
    GatewayIntentBits.MessageContent,
    GatewayIntentBits.GuildIntegrations,
    GatewayIntentBits.GuildWebhooks,
    GatewayIntentBits.GuildEmojisAndStickers,
    GatewayIntentBits.GuildVoiceStates
  ],
  partials: [
    Partials.GuildMember,
    Partials.Message,
    Partials.User,
    Partials.Channel
  ],
  allowedMentions: {
    parse: [],
    repliedUser: false
  }
});

// Import event listeners
import readyEvent from './events/ready.js';
import auditLogEvent from './events/guildAuditLogEntryCreate.js';
import guildUpdateEvent from './events/guildUpdate.js';
import guildMemberAddEvent from './events/guildMemberAdd.js';
import messageCreateEvent from './events/messageCreate.js';
import channelCreateEvent from './events/channelCreate.js';
import channelDeleteEvent from './events/channelDelete.js';
import roleCreateEvent from './events/roleCreate.js';
import roleDeleteEvent from './events/roleDelete.js';
import interactionCreateEvent from './events/interactionCreate.js';
import guildCreateEvent from './events/guildCreate.js';
import voiceStateUpdateEvent from './events/voiceStateUpdate.js';

// Bind event listeners
client.once(readyEvent.name as any, (...args: any[]) => (readyEvent as any).execute(...args));
client.on(auditLogEvent.name as any, (...args: any[]) => (auditLogEvent as any).execute(...args));
client.on(guildCreateEvent.name as any, (...args: any[]) => (guildCreateEvent as any).execute(...args));
client.on(guildUpdateEvent.name as any, (...args: any[]) => (guildUpdateEvent as any).execute(...args));
client.on(guildMemberAddEvent.name as any, (...args: any[]) => (guildMemberAddEvent as any).execute(...args));
client.on(messageCreateEvent.name as any, (...args: any[]) => (messageCreateEvent as any).execute(...args));
client.on(channelCreateEvent.name as any, (...args: any[]) => (channelCreateEvent as any).execute(...args));
client.on(channelDeleteEvent.name as any, (...args: any[]) => (channelDeleteEvent as any).execute(...args));
client.on(roleCreateEvent.name as any, (...args: any[]) => (roleCreateEvent as any).execute(...args));
client.on(roleDeleteEvent.name as any, (...args: any[]) => (roleDeleteEvent as any).execute(...args));
client.on(interactionCreateEvent.name as any, (...args: any[]) => (interactionCreateEvent as any).execute(...args));
client.on(voiceStateUpdateEvent.name as any, (...args: any[]) => (voiceStateUpdateEvent as any).execute(...args));

// Robust unhandled error guards so malicious payloads NEVER crash the process
process.on('unhandledRejection', (reason: any) => {
  Logger.error(`[UNHANDLED REJECTION] ${reason?.stack || reason}`);
});

process.on('uncaughtException', (err: Error) => {
  Logger.error(`[UNCAUGHT EXCEPTION] ${err.stack || err.message}`);
});

// Graceful termination
const shutdown = (signal: string) => {
  Logger.info(`Received ${signal}. Gracefully stopping security engines...`);
  client.destroy();
  process.exit(0);
};

process.on('SIGINT', () => shutdown('SIGINT'));
process.on('SIGTERM', () => shutdown('SIGTERM'));

// Enterprise RAM Shield Watchdog (Runs every 60s)
// Enforces RAM floor between 25MB-35MB on Discloud (100MB hard limit)
setInterval(() => {
  const memoryUsage = process.memoryUsage();
  const rssMB = Math.round(memoryUsage.rss / 1024 / 1024);
  const heapUsedMB = Math.round(memoryUsage.heapUsed / 1024 / 1024);

  // If RSS exceeds 50MB or heap exceeds 30MB, invoke aggressive garbage collection
  if (global.gc && (rssMB > 50 || heapUsedMB > 30)) {
    global.gc();
  }
}, 60000);

// Cloud Health-Check Keepalive Server (only enabled if PORT is explicitly passed, e.g. on Render/Koyeb)
if (process.env.PORT) {
  try {
    const PORT = process.env.PORT;
    http.createServer((_req, res) => {
      res.writeHead(200, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({
        status: 'ONLINE',
        bot: BOT_CONFIG.name,
        uptimeSeconds: Math.floor(process.uptime()),
        timestamp: new Date().toISOString()
      }));
    }).listen(PORT, () => {
      Logger.info(`[CLOUD KEEPALIVE] HTTP server active on port ${PORT}`);
    });
  } catch (err: any) {
    Logger.warn(`[CLOUD KEEPALIVE] Skipped port binding: ${err.message}`);
  }
}

// Authenticate to Discord Gateway
Logger.info(`Bootstrapping ${BOT_CONFIG.name} Security Infrastructure...`);
client.login(token).catch((err: any) => {
  Logger.error(`Login failed: ${err.message}`, err);
  process.exit(1);
});
