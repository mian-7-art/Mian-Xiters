import { Client, Events, ActivityType, PermissionFlagsBits } from 'discord.js';
import { BOT_CONFIG } from '../config/config.js';
import { snapshotService } from '../services/SnapshotService.js';
import { priorityRestManager } from '../core/PriorityRestManager.js';
import { Logger } from '../utils/logger.js';

export default {
  name: Events.ClientReady,
  once: true,
  async execute(client: Client) {
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
\x1b[32m [ONLINE]\x1b[0m ${BOT_CONFIG.name} initialized as ${client.user?.tag}
\x1b[36m [AUTHOR]\x1b[0m Developed by ${BOT_CONFIG.author}
\x1b[35m [STATUS]\x1b[0m ${BOT_CONFIG.slogan}
\x1b[1m\x1b[37m===================================================================================\x1b[0m
`);

    // Set custom rich presence with dynamic developer attribution
    const activities = [
      { name: 'custom', type: ActivityType.Custom, state: `dev by ${BOT_CONFIG.author}` },
      { name: 'custom', type: ActivityType.Custom, state: BOT_CONFIG.slogan },
      { name: 'custom', type: ActivityType.Custom, state: `🛡️ dev by ${BOT_CONFIG.author} • MIAN XITERS` }
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
    if (BOT_CONFIG.token) {
      priorityRestManager.initialize(BOT_CONFIG.token);
    }

    // Start 15-minute atomic snapshot mirror cycle
    snapshotService.startSynchronizationCycle(client);

    // Verify role hierarchy in all guilds
    for (const guild of client.guilds.cache.values()) {
      const botMember = guild.members.me;
      if (!botMember) continue;

      if (!botMember.permissions.has(PermissionFlagsBits.Administrator)) {
        Logger.warn(`[PERMISSIONS WARNING] In guild "${guild.name}" (${guild.id}), ${BOT_CONFIG.name} DOES NOT have Administrator permission! Anti-nuke capabilities may be hindered.`);
      }

      const highestBotRole = botMember.roles.highest;
      const highestGuildRole = guild.roles.highest;
      if (highestBotRole.id !== highestGuildRole.id) {
        Logger.warn(`[HIERARCHY NOTICE] In guild "${guild.name}", role "${highestBotRole.name}" is not at the very top. Place ${BOT_CONFIG.name}'s role above all admin roles for 100% killswitch guarantee.`);
      }
    }

    Logger.info(`Defense engines active across ${client.guilds.cache.size} server(s). Monitoring Gateway audit stream.`);
  }
};
