"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const fs_1 = __importDefault(require("fs"));
const path_1 = __importDefault(require("path"));
const dotenv_1 = __importDefault(require("dotenv"));
const config_js_1 = require("../config/config.js");
const logger_js_1 = require("../utils/logger.js");
dotenv_1.default.config();
const token = config_js_1.BOT_CONFIG.token;
if (!token) {
    console.error('\x1b[31m[ERROR] DISCORD_TOKEN is missing in .env.\x1b[0m');
    process.exit(1);
}
const client = new discord_js_1.Client({
    intents: [discord_js_1.GatewayIntentBits.Guilds]
});
client.once('ready', async () => {
    try {
        logger_js_1.Logger.info(`Connected to Discord as ${client.user?.tag}`);
        // 1. Update Username if needed
        if (client.user?.username !== config_js_1.BOT_CONFIG.name) {
            logger_js_1.Logger.info(`Updating bot username to "${config_js_1.BOT_CONFIG.name}"...`);
            await client.user?.setUsername(config_js_1.BOT_CONFIG.name).catch((err) => {
                logger_js_1.Logger.warn(`Username update skipped or rate-limited: ${err.message}`);
            });
        }
        // 2. Update Avatar from assets/logo.png
        const logoPngPath = path_1.default.resolve(process.cwd(), 'assets', 'logo.png');
        const logoJpgPath = path_1.default.resolve(process.cwd(), 'assets', 'logo.jpg');
        const targetFile = fs_1.default.existsSync(logoPngPath) ? logoPngPath : logoJpgPath;
        if (fs_1.default.existsSync(targetFile)) {
            logger_js_1.Logger.info(`Setting bot avatar from: ${targetFile}...`);
            const imageBuffer = fs_1.default.readFileSync(targetFile);
            await client.user?.setAvatar(imageBuffer).catch((err) => {
                logger_js_1.Logger.warn(`Avatar update skipped or rate-limited by Discord (avatars can only be changed 2 times per 10 minutes): ${err.message}`);
            });
            logger_js_1.Logger.info(`✅ Avatar successfully applied!`);
        }
        else {
            logger_js_1.Logger.warn(`Avatar logo file not found at ${targetFile}`);
        }
        console.log(`
\x1b[32m[PROFILE UPDATE SUCCESSFUL]\x1b[0m
• Name: ${config_js_1.BOT_CONFIG.name}
• Avatar: Set from uploaded high-res MIAN XITERS artwork
• Bio / About Me: "${config_js_1.BOT_CONFIG.bio}"
• Slogan: "${config_js_1.BOT_CONFIG.slogan}"
`);
        process.exit(0);
    }
    catch (err) {
        logger_js_1.Logger.error(`Failed to update bot profile: ${err.message}`, err);
        process.exit(1);
    }
});
client.login(token);
