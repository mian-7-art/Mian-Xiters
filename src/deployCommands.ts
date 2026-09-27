import { REST, Routes } from 'discord.js';
import dotenv from 'dotenv';
import { securityCommand } from './commands/security.js';
import { BOT_CONFIG } from './config/config.js';
import { Logger } from './utils/logger.js';

dotenv.config();

const token = BOT_CONFIG.token;
const clientId = BOT_CONFIG.clientId;
const guildId = process.env.GUILD_ID;

if (!token || !clientId) {
  console.error('\x1b[31m[ERROR] DISCORD_TOKEN and CLIENT_ID are required in .env to deploy commands.\x1b[0m');
  process.exit(1);
}

const rest = new REST({ version: '10' }).setToken(token);

(async () => {
  try {
    Logger.info(`Started refreshing application (/) commands for ${BOT_CONFIG.name}...`);

    const commands = [securityCommand.data.toJSON()];

    Logger.info('Deploying commands globally across all guilds...');
    await rest.put(Routes.applicationCommands(clientId), { body: commands });
    Logger.info('Successfully deployed commands globally!');

    if (guildId) {
      try {
        Logger.info(`Deploying commands to target guild: ${guildId}`);
        await rest.put(Routes.applicationGuildCommands(clientId, guildId), { body: commands });
        Logger.info(`Successfully deployed commands to guild ${guildId}!`);
      } catch (gErr: any) {
        Logger.warn(`Guild-specific deployment pending (Bot not in guild yet). Global commands are active.`);
      }
    }
  } catch (error: any) {
    Logger.error('Failed to deploy slash commands:', error);
  }
})();
