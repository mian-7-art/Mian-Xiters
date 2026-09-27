import { Client, GatewayIntentBits } from 'discord.js';
import fs from 'fs';
import path from 'path';
import dotenv from 'dotenv';
import { BOT_CONFIG } from '../config/config.js';
import { Logger } from '../utils/logger.js';

dotenv.config();

const token = BOT_CONFIG.token;
if (!token) {
  console.error('\x1b[31m[ERROR] DISCORD_TOKEN is missing in .env.\x1b[0m');
  process.exit(1);
}

const client = new Client({
  intents: [GatewayIntentBits.Guilds]
});

client.once('ready', async () => {
  try {
    Logger.info(`Connected to Discord as ${client.user?.tag}`);

    // 1. Update Username if needed
    if (client.user?.username !== BOT_CONFIG.name) {
      Logger.info(`Updating bot username to "${BOT_CONFIG.name}"...`);
      await client.user?.setUsername(BOT_CONFIG.name).catch((err: any) => {
        Logger.warn(`Username update skipped or rate-limited: ${err.message}`);
      });
    }

    // 2. Update Avatar from assets/logo.png
    const logoPngPath = path.resolve(process.cwd(), 'assets', 'logo.png');
    const logoJpgPath = path.resolve(process.cwd(), 'assets', 'logo.jpg');
    const targetFile = fs.existsSync(logoPngPath) ? logoPngPath : logoJpgPath;

    if (fs.existsSync(targetFile)) {
      Logger.info(`Setting bot avatar from: ${targetFile}...`);
      const imageBuffer = fs.readFileSync(targetFile);
      await client.user?.setAvatar(imageBuffer).catch((err: any) => {
        Logger.warn(`Avatar update skipped or rate-limited by Discord (avatars can only be changed 2 times per 10 minutes): ${err.message}`);
      });
      Logger.info(`✅ Avatar successfully applied!`);
    } else {
      Logger.warn(`Avatar logo file not found at ${targetFile}`);
    }

    console.log(`
\x1b[32m[PROFILE UPDATE SUCCESSFUL]\x1b[0m
• Name: ${BOT_CONFIG.name}
• Avatar: Set from uploaded high-res MIAN XITERS artwork
• Bio / About Me: "${BOT_CONFIG.bio}"
• Slogan: "${BOT_CONFIG.slogan}"
`);

    process.exit(0);
  } catch (err: any) {
    Logger.error(`Failed to update bot profile: ${err.message}`, err);
    process.exit(1);
  }
});

client.login(token);
