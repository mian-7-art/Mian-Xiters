"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.FONT_STYLES = void 0;
const discord_js_1 = require("discord.js");
const dotenv_1 = __importDefault(require("dotenv"));
const config_js_1 = require("../config/config.js");
const logger_js_1 = require("../utils/logger.js");
dotenv_1.default.config();
const token = config_js_1.BOT_CONFIG.token;
const client = new discord_js_1.Client({
    intents: [discord_js_1.GatewayIntentBits.Guilds]
});
// Provide various bold unicode font styles
exports.FONT_STYLES = {
    sansBold: '𝗠𝗜𝗔𝗡 𝗫𝗜𝗧𝗘𝗥𝗦', // Modern Clean Heavy Sans
    serifBold: '𝐌𝐈𝐀𝐍 𝐗𝐈𝐓𝐄𝐑𝐒', // Classic Bold Serif
    gothicBold: '𝕸𝕴𝕬𝕹 𝖃𝕴𝕿𝕰𝕽𝕾', // Aggressive Gothic Fraktur
    smallCaps: 'ᴍɪᴀɴ xɪᴛᴇʀs', // Clean Aesthetic Small Caps
    shielded: '🛡️ 𝗠𝗜𝗔𝗡 𝗫𝗜𝗧𝗘𝗥𝗦' // Shielded Heavy Sans
};
const chosenStyle = process.argv[2] || 'sansBold';
const newName = exports.FONT_STYLES[chosenStyle] || exports.FONT_STYLES.sansBold;
client.once('ready', async () => {
    try {
        logger_js_1.Logger.info(`Connected as ${client.user?.tag}`);
        // 1. Update Global Username if possible
        logger_js_1.Logger.info(`Attempting to set global username to: "${newName}"...`);
        await client.user?.setUsername(newName).catch(err => {
            logger_js_1.Logger.warn(`Global username update note: ${err.message} (Discord allows 2 username changes per hour).`);
        });
        // 2. Update Guild Nickname across all guilds
        for (const guild of client.guilds.cache.values()) {
            try {
                const me = guild.members.me || await guild.members.fetchMe();
                await me.setNickname(newName);
                logger_js_1.Logger.info(`✅ Successfully updated nickname in server "${guild.name}" to: "${newName}"`);
            }
            catch (nickErr) {
                logger_js_1.Logger.warn(`Could not set nickname in "${guild.name}": ${nickErr.message}`);
            }
        }
        process.exit(0);
    }
    catch (err) {
        logger_js_1.Logger.error(`Error: ${err.message}`, err);
        process.exit(1);
    }
});
client.login(token);
