import { Client, GatewayIntentBits } from 'discord.js';
import dotenv from 'dotenv';
import { BOT_CONFIG } from '../config/config.js';
import { Logger } from '../utils/logger.js';

dotenv.config();

const token = BOT_CONFIG.token;
const client = new Client({
  intents: [GatewayIntentBits.Guilds]
});

// Provide various bold unicode font styles
export const FONT_STYLES: Record<string, string> = {
  sansBold: '𝗠𝗜𝗔𝗡 𝗫𝗜𝗧𝗘𝗥𝗦',      // Modern Clean Heavy Sans
  serifBold: '𝐌𝐈𝐀𝐍 𝐗𝐈𝐓𝐄𝐑𝐒',     // Classic Bold Serif
  gothicBold: '𝕸𝕴𝕬𝕹 𝖃𝕴𝕿𝕰𝕽𝕾',    // Aggressive Gothic Fraktur
  smallCaps: 'ᴍɪᴀɴ xɪᴛᴇʀs',       // Clean Aesthetic Small Caps
  shielded: '🛡️ 𝗠𝗜𝗔𝗡 𝗫𝗜𝗧𝗘𝗥𝗦'     // Shielded Heavy Sans
};

const chosenStyle = process.argv[2] || 'sansBold';
const newName = FONT_STYLES[chosenStyle] || FONT_STYLES.sansBold;

client.once('ready', async () => {
  try {
    Logger.info(`Connected as ${client.user?.tag}`);

    // 1. Update Global Username if possible
    Logger.info(`Attempting to set global username to: "${newName}"...`);
    await client.user?.setUsername(newName).catch(err => {
      Logger.warn(`Global username update note: ${err.message} (Discord allows 2 username changes per hour).`);
    });

    // 2. Update Guild Nickname across all guilds
    for (const guild of client.guilds.cache.values()) {
      try {
        const me = guild.members.me || await guild.members.fetchMe();
        await me.setNickname(newName);
        Logger.info(`✅ Successfully updated nickname in server "${guild.name}" to: "${newName}"`);
      } catch (nickErr: any) {
        Logger.warn(`Could not set nickname in "${guild.name}": ${nickErr.message}`);
      }
    }

    process.exit(0);
  } catch (err: any) {
    Logger.error(`Error: ${err.message}`, err);
    process.exit(1);
  }
});

client.login(token);
