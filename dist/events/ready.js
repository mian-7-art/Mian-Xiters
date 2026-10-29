"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const config_js_1 = require("../config/config.js");
const SnapshotService_js_1 = require("../services/SnapshotService.js");
const PriorityRestManager_js_1 = require("../core/PriorityRestManager.js");
const logger_js_1 = require("../utils/logger.js");
exports.default = {
    name: discord_js_1.Events.ClientReady,
    once: true,
    async execute(client) {
        console.log(`
\x1b[31m
  __  __ _____          _   _  __   _______ _______ ______ _____   _____ 
 |  \\/  |_   _|   /\\   | \\ | | \\ \\ / /_   _|__   __|  ____|  __ \\ / ____|
 | \\  / | | |    /  \\  |  \\| |  \\ V /  | |    | |  | |__  | |__) | (___  
 | |\\/| | | |   / /\\ \\ | . \` |   > <   | |    | |  |  __| |  _  / \\___ \\ 
 | |  | |_| |_ / ____ \\| |\\  |  / . \\ _| |_   | |  | |____| | \\ \\ ____) |
 |_|  |_|_____/_/    \\_\\_| \\_| /_/ \\_\\_____|  |_|  |______|_|  \\_\\_____/ 
\x1b[0m
\x1b[1m\x1b[37m===================================================================================\x1b[0m
\x1b[32m [ONLINE]\x1b[0m ${config_js_1.BOT_CONFIG.name} initialized as ${client.user?.tag}
\x1b[36m [AUTHOR]\x1b[0m Developed by ${config_js_1.BOT_CONFIG.author}
\x1b[35m [STATUS]\x1b[0m ${config_js_1.BOT_CONFIG.slogan}
\x1b[1m\x1b[37m===================================================================================\x1b[0m
`);
        // Set custom rich presence with dynamic developer attribution
        const activities = [
            { name: 'custom', type: discord_js_1.ActivityType.Custom, state: `dev by ${config_js_1.BOT_CONFIG.author}` },
            { name: 'custom', type: discord_js_1.ActivityType.Custom, state: config_js_1.BOT_CONFIG.slogan },
            { name: 'custom', type: discord_js_1.ActivityType.Custom, state: `🛡️ dev by ${config_js_1.BOT_CONFIG.author} • MIAN XITERS` }
        ];
        let currentActivity = 0;
        const updatePresence = () => {
            client.user?.setPresence({
                status: 'dnd',
                activities: [activities[currentActivity]]
            });
            currentActivity = (currentActivity + 1) % activities.length;
        };
        updatePresence();
        setInterval(updatePresence, 15000);
        // Initialize Priority REST lane for direct zero-latency emergency bans
        if (config_js_1.BOT_CONFIG.token) {
            PriorityRestManager_js_1.priorityRestManager.initialize(config_js_1.BOT_CONFIG.token);
        }
        // Start 15-minute atomic snapshot mirror cycle
        SnapshotService_js_1.snapshotService.startSynchronizationCycle(client);
        // Verify role hierarchy in all guilds
        for (const guild of client.guilds.cache.values()) {
            const botMember = guild.members.me;
            if (!botMember)
                continue;
            if (!botMember.permissions.has(discord_js_1.PermissionFlagsBits.Administrator)) {
                logger_js_1.Logger.warn(`[PERMISSIONS WARNING] In guild "${guild.name}" (${guild.id}), ${config_js_1.BOT_CONFIG.name} DOES NOT have Administrator permission! Anti-nuke capabilities may be hindered.`);
            }
            const highestBotRole = botMember.roles.highest;
            const highestGuildRole = guild.roles.highest;
            if (highestBotRole.id !== highestGuildRole.id) {
                logger_js_1.Logger.warn(`[HIERARCHY NOTICE] In guild "${guild.name}", role "${highestBotRole.name}" is not at the very top. Place ${config_js_1.BOT_CONFIG.name}'s role above all admin roles for 100% killswitch guarantee.`);
            }
        }
        logger_js_1.Logger.info(`Defense engines active across ${client.guilds.cache.size} server(s). Monitoring Gateway audit stream.`);
    }
};
