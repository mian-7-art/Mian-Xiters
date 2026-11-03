"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
const dotenv_1 = __importDefault(require("dotenv"));
dotenv_1.default.config();
const TOKEN = process.env.DISCORD_TOKEN;
const RULES_CHANNEL_ID = '1483579740802650193'; // 📜┃server-rules
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
        const channel = await client.channels.fetch(RULES_CHANNEL_ID);
        if (!channel || !channel.isTextBased()) {
            console.error(`[ERROR] Channel not found or not text-based!`);
            process.exit(1);
        }
        // Fetch and delete all existing messages to keep it 100% clean
        const existing = await channel.messages.fetch({ limit: 50 });
        for (const msg of existing.values()) {
            try {
                await msg.delete();
                await new Promise(r => setTimeout(r, 250));
            }
            catch (err) {
                console.warn(`[WARN] Could not delete msg ${msg.id}: ${err.message}`);
            }
        }
        console.log('[CLEANUP] Channel cleared. Sending single unified rules embed...');
        // Single sleek, crisp, ultra-professional embed
        const rulesEmbed = new discord_js_1.EmbedBuilder()
            .setColor(0xFF0033) // Crimson Red signature
            .setTitle('📜 𝗠𝗜𝗔𝗡 𝗫𝗜𝗧𝗘𝗥𝗦 — SERVER RULES & GUIDELINES')
            .setDescription(`Welcome to **MIAN XITERS**. Please read and respect the server directives below.\n` +
            `By being a member of this community, you agree to comply with all rules.\n` +
            `━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━`)
            .addFields({
            name: '⚔️ 1. Respect & Conduct',
            value: `• Treat all members and staff with mutual respect.\n` +
                `• Zero tolerance for extreme toxicity, racism, harassment, or doxxing.\n` +
                `• Keep chat civil and avoid unnecessary drama in public channels.`
        }, {
            name: '🛒 2. Products & Purchases',
            value: `• All purchases are handled **ONLY** via <#1483581020346384544> or Founder **\`1nOnlyMian\`**.\n` +
                `• Never buy or trade with members in DMs. Middleman deals outside tickets are strictly forbidden.\n` +
                `• We do not provide compensation for unauthorized third-party trades.`
        }, {
            name: '🛡️ 3. Cheats & Anti-Leak Policy',
            value: `• Sharing, cracked distribution, dumping, or leaking software/keys = **Instant HWID Blacklist & Ban**.\n` +
                `• Follow the recommended safe configs and instructions provided with your product.\n` +
                `• Use cheats responsibly; staff are not liable for blatant rage-play bans.`
        }, {
            name: '🚫 4. Advertising & Malicious Content',
            value: `• Strictly no self-promotion, server invite links, or DM advertising.\n` +
                `• Posting grabbers, phishing links, or trojans triggers an automated permanent ban.`
        }, {
            name: '🎫 5. Support & Ticket Protocol',
            value: `• For product inquiries, keys, setups, or technical support, open a ticket in <#1483581020346384544>.\n` +
                `• Please be patient; tickets are answered in order. Avoid spam-pinging staff.`
        })
            .setImage('https://cdn.discordapp.com/attachments/1483579740802650193/1234567890/banner.png') // fallback graceful
            .setFooter({
            text: 'MIAN XITERS ULTIMATE DOMINANCE ! • Dev by 1nOnlyMian',
            iconURL: client.user?.displayAvatarURL() || undefined
        })
            .setTimestamp();
        // Remove empty/dummy image if not needed to avoid empty embed gap
        rulesEmbed.setImage(null);
        const sent = await channel.send({
            embeds: [rulesEmbed]
        });
        await sent.react('✅').catch(() => { });
        console.log(`[SUCCESS] Unified rules embed posted successfully! (Msg ID: ${sent.id})`);
    }
    catch (err) {
        console.error('[ERROR] Failed to post rules:', err);
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
