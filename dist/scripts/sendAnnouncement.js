"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const dotenv_1 = __importDefault(require("dotenv"));
dotenv_1.default.config();
const TOKEN = process.env.DISCORD_TOKEN;
const ANNOUNCEMENTS_CHANNEL_ID = '1483580605986902167'; // 📢┃announcements in MIAN XITERS
if (!TOKEN) {
    console.error('[ERROR] DISCORD_TOKEN is missing in .env');
    process.exit(1);
}
const client = new discord_js_1.Client({
    intents: [
        discord_js_1.GatewayIntentBits.Guilds,
        discord_js_1.GatewayIntentBits.GuildMessages
    ]
});
client.once('ready', async () => {
    console.log(`[AUTH] Connected as ${client.user?.tag}`);
    try {
        const channel = await client.channels.fetch(ANNOUNCEMENTS_CHANNEL_ID);
        if (!channel || !channel.isTextBased()) {
            console.error(`[ERROR] Channel ${ANNOUNCEMENTS_CHANNEL_ID} not found or not text-based!`);
            process.exit(1);
        }
        const announcementEmbed = new discord_js_1.EmbedBuilder()
            .setColor(0xFF0033) // Signature Crimson Red
            .setTitle('⚠️ IMPORTANT NOTICE — PRODUCT SAFETY & USAGE DISCLAIMER')
            .setDescription(`Please take a moment to carefully read this official notice regarding our panels, bypasses, and software usage.\n` +
            `━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━`)
            .addFields({
            name: '🛡️ Software Safety & Game Patches',
            value: `• At **MIAN XITERS**, we build and deliver top-tier, private panels engineered with advanced bypasses and maximum protection.\n` +
                `• However, by nature, these are third-party game modifications. **No cheat in the world can be guaranteed 100% undetected for a lifetime**.\n` +
                `• Game developers and anti-cheat systems continuously roll out silent updates, heuristic checks, and security patches.`
        }, {
            name: '⏱️ Patch Windows & Notification Delays',
            value: `• In the event of a sudden game update or patch, there may be brief windows before a status alert is posted or before you check your Discord notifications.\n` +
                `• If an unforeseen patch occurs or an alert is missed, **MIAN XITERS and Founder Mian will not be held responsible for any game bans, suspensions, or account losses**.`
        }, {
            name: '⚠️ Follow Instructions & Play Safe',
            value: `• A high percentage of bans occur because users **ignore setup instructions**, fail to follow recommended safe settings, or play excessively blatant (rage mode).\n` +
                `• Always read the setup guides carefully and adjust configurations as instructed by our team.`
        }, {
            name: '💡 Crucial Recommendation: Use Alt Accounts',
            value: `• **We strictly and personally advise against using cheats on your main / primary accounts.**\n` +
                `• Always operate on secondary or alternative accounts so your valuable personal accounts remain untouched.`
        }, {
            name: '🔔 Stay Alert',
            value: `• Keep notifications **ON** for <#1483580605986902167> and <#1483580851970244620> to receive real-time status updates, safe usage notices, and downtime alerts.`
        })
            .setFooter({
            text: 'MIAN XITERS • Transparency & Dominance • Dev by 1nOnlyMian',
            iconURL: client.user?.displayAvatarURL() || undefined
        })
            .setTimestamp();
        const sent = await channel.send({
            content: '@everyone',
            embeds: [announcementEmbed]
        });
        await sent.react('⚠️').catch(() => { });
        await sent.react('✅').catch(() => { });
        console.log(`[SUCCESS] Announcement posted successfully to #${channel.name} (Msg ID: ${sent.id})!`);
    }
    catch (err) {
        console.error('[ERROR] Failed to post announcement:', err);
    }
    finally {
        client.destroy();
        process.exit(0);
    }
});
client.login(TOKEN).catch(err => {
    console.error('[FATAL] Login failed:', err.message);
    process.exit(1);
});
