import { Client, GatewayIntentBits, Partials, Collection } from 'discord.js';
import dotenv from 'dotenv';
import path from 'path';
import fs from 'fs';
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

// Enterprise Gateway client initialization with all required anti-nuke intents
const client = new Client({
  intents: [
    GatewayIntentBits.Guilds,
    GatewayIntentBits.GuildMembers,
    GatewayIntentBits.GuildModeration,
    GatewayIntentBits.GuildMessages,
    GatewayIntentBits.MessageContent,
    GatewayIntentBits.GuildIntegrations,
    GatewayIntentBits.GuildWebhooks,
    GatewayIntentBits.GuildEmojisAndStickers
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

// Bind event listeners
client.once(readyEvent.name as any, (...args) => (readyEvent as any).execute(...args));
client.on(auditLogEvent.name as any, (...args) => (auditLogEvent as any).execute(...args));
client.on(guildCreateEvent.name as any, (...args) => (guildCreateEvent as any).execute(...args));
client.on(guildUpdateEvent.name as any, (...args) => (guildUpdateEvent as any).execute(...args));
client.on(guildMemberAddEvent.name as any, (...args) => (guildMemberAddEvent as any).execute(...args));
client.on(messageCreateEvent.name as any, (...args) => (messageCreateEvent as any).execute(...args));
client.on(channelCreateEvent.name as any, (...args) => (channelCreateEvent as any).execute(...args));
client.on(channelDeleteEvent.name as any, (...args) => (channelDeleteEvent as any).execute(...args));
client.on(roleCreateEvent.name as any, (...args) => (roleCreateEvent as any).execute(...args));
client.on(roleDeleteEvent.name as any, (...args) => (roleDeleteEvent as any).execute(...args));
client.on(interactionCreateEvent.name as any, (...args) => (interactionCreateEvent as any).execute(...args));

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

// Authenticate to Discord Gateway
Logger.info(`Bootstrapping ${BOT_CONFIG.name} Security Infrastructure...`);
client.login(token).catch(err => {
  Logger.error(`Login failed: ${err.message}`, err);
  process.exit(1);
});
