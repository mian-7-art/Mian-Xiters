"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const dotenv_1 = __importDefault(require("dotenv"));
const http_1 = __importDefault(require("http"));
const config_js_1 = require("./config/config.js");
const logger_js_1 = require("./utils/logger.js");
// Load environment variables
dotenv_1.default.config();
const token = config_js_1.BOT_CONFIG.token;
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
const client = new discord_js_1.Client({
    intents: [
        discord_js_1.GatewayIntentBits.Guilds,
        discord_js_1.GatewayIntentBits.GuildMembers,
        discord_js_1.GatewayIntentBits.GuildModeration,
        discord_js_1.GatewayIntentBits.GuildMessages,
        discord_js_1.GatewayIntentBits.MessageContent,
        discord_js_1.GatewayIntentBits.GuildIntegrations,
        discord_js_1.GatewayIntentBits.GuildWebhooks,
        discord_js_1.GatewayIntentBits.GuildEmojisAndStickers,
        discord_js_1.GatewayIntentBits.GuildVoiceStates
    ],
    partials: [
        discord_js_1.Partials.GuildMember,
        discord_js_1.Partials.Message,
        discord_js_1.Partials.User,
        discord_js_1.Partials.Channel
    ],
    allowedMentions: {
        parse: [],
        repliedUser: false
    }
});
// Import event listeners
const ready_js_1 = __importDefault(require("./events/ready.js"));
const guildAuditLogEntryCreate_js_1 = __importDefault(require("./events/guildAuditLogEntryCreate.js"));
const guildUpdate_js_1 = __importDefault(require("./events/guildUpdate.js"));
const guildMemberAdd_js_1 = __importDefault(require("./events/guildMemberAdd.js"));
const messageCreate_js_1 = __importDefault(require("./events/messageCreate.js"));
const channelCreate_js_1 = __importDefault(require("./events/channelCreate.js"));
const channelDelete_js_1 = __importDefault(require("./events/channelDelete.js"));
const roleCreate_js_1 = __importDefault(require("./events/roleCreate.js"));
const roleDelete_js_1 = __importDefault(require("./events/roleDelete.js"));
const interactionCreate_js_1 = __importDefault(require("./events/interactionCreate.js"));
const guildCreate_js_1 = __importDefault(require("./events/guildCreate.js"));
const voiceStateUpdate_js_1 = __importDefault(require("./events/voiceStateUpdate.js"));
// Bind event listeners
client.once(ready_js_1.default.name, (...args) => ready_js_1.default.execute(...args));
client.on(guildAuditLogEntryCreate_js_1.default.name, (...args) => guildAuditLogEntryCreate_js_1.default.execute(...args));
client.on(guildCreate_js_1.default.name, (...args) => guildCreate_js_1.default.execute(...args));
client.on(guildUpdate_js_1.default.name, (...args) => guildUpdate_js_1.default.execute(...args));
client.on(guildMemberAdd_js_1.default.name, (...args) => guildMemberAdd_js_1.default.execute(...args));
client.on(messageCreate_js_1.default.name, (...args) => messageCreate_js_1.default.execute(...args));
client.on(channelCreate_js_1.default.name, (...args) => channelCreate_js_1.default.execute(...args));
client.on(channelDelete_js_1.default.name, (...args) => channelDelete_js_1.default.execute(...args));
client.on(roleCreate_js_1.default.name, (...args) => roleCreate_js_1.default.execute(...args));
client.on(roleDelete_js_1.default.name, (...args) => roleDelete_js_1.default.execute(...args));
client.on(interactionCreate_js_1.default.name, (...args) => interactionCreate_js_1.default.execute(...args));
client.on(voiceStateUpdate_js_1.default.name, (...args) => voiceStateUpdate_js_1.default.execute(...args));
// Robust unhandled error guards so malicious payloads NEVER crash the process
process.on('unhandledRejection', (reason) => {
    logger_js_1.Logger.error(`[UNHANDLED REJECTION] ${reason?.stack || reason}`);
});
process.on('uncaughtException', (err) => {
    logger_js_1.Logger.error(`[UNCAUGHT EXCEPTION] ${err.stack || err.message}`);
});
// Graceful termination
const shutdown = (signal) => {
    logger_js_1.Logger.info(`Received ${signal}. Gracefully stopping security engines...`);
    client.destroy();
    process.exit(0);
};
process.on('SIGINT', () => shutdown('SIGINT'));
process.on('SIGTERM', () => shutdown('SIGTERM'));
// Cloud Health-Check Keepalive Server (enables 100% free 24/7 cloud hosting)
const PORT = process.env.PORT || 3000;
http_1.default.createServer((_req, res) => {
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({
        status: 'ONLINE',
        bot: config_js_1.BOT_CONFIG.name,
        uptimeSeconds: Math.floor(process.uptime()),
        timestamp: new Date().toISOString()
    }));
}).listen(PORT, () => {
    logger_js_1.Logger.info(`[CLOUD KEEPALIVE] HTTP server active on port ${PORT}`);
});
// Authenticate to Discord Gateway
logger_js_1.Logger.info(`Bootstrapping ${config_js_1.BOT_CONFIG.name} Security Infrastructure...`);
client.login(token).catch(err => {
    logger_js_1.Logger.error(`Login failed: ${err.message}`, err);
    process.exit(1);
});
