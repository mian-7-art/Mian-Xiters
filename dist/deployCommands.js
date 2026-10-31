"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const dotenv_1 = __importDefault(require("dotenv"));
const security_js_1 = require("./commands/security.js");
const ban_js_1 = require("./commands/ban.js");
const rank_js_1 = require("./commands/rank.js");
const leaderboard_js_1 = require("./commands/leaderboard.js");
const tree_js_1 = require("./commands/tree.js");
const water_js_1 = require("./commands/water.js");
const config_js_1 = require("./config/config.js");
const logger_js_1 = require("./utils/logger.js");
dotenv_1.default.config();
const token = config_js_1.BOT_CONFIG.token;
const clientId = config_js_1.BOT_CONFIG.clientId;
const guildId = process.env.GUILD_ID;
if (!token || !clientId) {
    console.error('\x1b[31m[ERROR] DISCORD_TOKEN and CLIENT_ID are required in .env to deploy commands.\x1b[0m');
    process.exit(1);
}
const rest = new discord_js_1.REST({ version: '10' }).setToken(token);
(async () => {
    try {
        logger_js_1.Logger.info(`Started refreshing application (/) commands for ${config_js_1.BOT_CONFIG.name}...`);
        const commands = [
            security_js_1.securityCommand.data.toJSON(),
            ban_js_1.banCommand.data.toJSON(),
            rank_js_1.rankCommand.data.toJSON(),
            leaderboard_js_1.leaderboardCommand.data.toJSON(),
            tree_js_1.treeCommand.data.toJSON(),
            water_js_1.waterCommand.data.toJSON()
        ];
        logger_js_1.Logger.info('Deploying commands globally across all guilds...');
        await rest.put(discord_js_1.Routes.applicationCommands(clientId), { body: commands });
        logger_js_1.Logger.info('Successfully deployed commands globally!');
        if (guildId) {
            try {
                logger_js_1.Logger.info(`Deploying commands to target guild: ${guildId}`);
                await rest.put(discord_js_1.Routes.applicationGuildCommands(clientId, guildId), { body: commands });
                logger_js_1.Logger.info(`Successfully deployed commands to guild ${guildId}!`);
            }
            catch (gErr) {
                logger_js_1.Logger.warn(`Guild-specific deployment pending (Bot not in guild yet). Global commands are active.`);
            }
        }
    }
    catch (error) {
        logger_js_1.Logger.error('Failed to deploy slash commands:', error);
    }
})();
