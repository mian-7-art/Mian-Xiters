import { Client, GatewayIntentBits, EmbedBuilder, TextChannel } from 'discord.js';
import dotenv from 'dotenv';
import path from 'path';

dotenv.config();

const TOKEN = process.env.DISCORD_TOKEN;
const RULES_CHANNEL_ID = '1483579740802650193'; // 📜┃server-rules in MIAN XITERS

if (!TOKEN) {
  console.error('[ERROR] DISCORD_TOKEN is missing in .env');
  process.exit(1);
}

const client = new Client({
  intents: [
    GatewayIntentBits.Guilds,
    GatewayIntentBits.GuildMessages
  ]
});

client.once('ready', async () => {
  console.log(`[AUTH] Connected as ${client.user?.tag}`);

  try {
    const channel = await client.channels.fetch(RULES_CHANNEL_ID) as TextChannel;
    if (!channel || !channel.isTextBased()) {
      console.error(`[ERROR] Channel ${RULES_CHANNEL_ID} not found or not text-based!`);
      process.exit(1);
    }

    console.log(`[STATUS] Fetching old messages in #${channel.name} (${channel.id})...`);
    
    // Fetch and delete all existing messages
    let fetched;
    do {
      fetched = await channel.messages.fetch({ limit: 100 });
      if (fetched.size > 0) {
        console.log(`[DELETE] Deleting ${fetched.size} old messages...`);
        for (const msg of fetched.values()) {
          try {
            await msg.delete();
            await new Promise(r => setTimeout(r, 200)); // Rate limit safety
          } catch (err: any) {
            console.warn(`[WARN] Could not delete msg ${msg.id}: ${err.message}`);
          }
        }
      }
    } while (fetched.size >= 10);

    console.log('[CLEANUP] Old rules cleared. Generating new official ruleset...');

    // Embed 1: Header & Welcome Banner
    const bannerEmbed = new EmbedBuilder()
      .setColor(0xFF0033) // Crimson Red
      .setTitle('⚡ 𝗠𝗜𝗔𝗡 𝗫𝗜𝗧𝗘𝗥𝗦 ✦ OFFICIAL SERVER RULES & DIRECTIVES ⚡')
      .setDescription(
        `> **Welcome to MIAN XITERS** — the premier destination for private cheats, custom loaders, panels, emulators, and elite gaming dominance.\n\n` +
        `To ensure a secure, professional, and smooth experience for all buyers, community members, and staff, **everyone is required to strictly abide by the rules outlined below**.\n\n` +
        `*Entering and participating in this server constitutes full acceptance of these terms. Failure to comply will result in immediate disciplinary action up to permanent blacklisting and bans.*`
      )
      .setThumbnail(client.user?.displayAvatarURL() || null);

    // Embed 2: Core Directives & Rules
    const rulesEmbed = new EmbedBuilder()
      .setColor(0x0E1015) // Deep Dark Carbon
      .addFields(
        {
          name: '📌 § 1 • GENERAL CONDUCT & RESPECT',
          value: 
            `**1.1 Mutual Respect:** Treat all members and staff with dignity. Hate speech, extreme toxicity, racism, sexism, slurs, doxxing, harassment, or personal threats will result in an instant permanent ban.\n` +
            `**1.2 Clean Communication:** Keep conversations civil. Avoid unnecessary arguments, political debates, or toxic drama in public channels.\n` +
            `**1.3 No Spamming:** Do not flood text channels, spam emotes, chain message, or mic-spam/earrape in Voice Channels.\n` +
            `**1.4 No Ghost/Mass Pings:** Do not unnecessarily ping \`@everyone\`, \`@here\`, or spam-mention Staff members without legitimate urgency.`,
          inline: false
        },
        {
          name: '🛒 § 2 • CHEATS, PRODUCTS & TRANSACTION SECURITY',
          value:
            `**2.1 Official Purchases Only:** All purchases, license keys, and subscriptions must be handled strictly through <#1483581020346384544> (\`🛒┃create-ticket\`) or directly with Founder **\`1nOnlyMian\`**.\n` +
            `**2.2 Zero Tolerance for Middlemen/Scams:** Never buy or trade with third-party members in DMs. Unauthorized selling, middlemanning, or scamming will result in an immediate blacklist.\n` +
            `**2.3 Anti-Leak & Anti-Crack Protection:** Sharing, cracked distribution, reverse-engineering, leaking keys, or dumping loaders will lead to an instant **HWID Blacklist**, permanent key revocation, and a network ban.\n` +
            `**2.4 Usage Disclaimer:** Our software features state-of-the-art bypasses and security protections. However, always follow recommended safe settings and panel instructions. Staff are not liable for account suspensions caused by rage-cheating or ignoring safety guidelines.`,
          inline: false
        },
        {
          name: '🎫 § 3 • SUPPORT & TICKET PROTOCOL',
          value:
            `**3.1 Dedicated Channels:** For customer support, setup guidance, key activation, or bug reports, open a ticket in <#1483581020346384544>.\n` +
            `**3.2 Patience Required:** Support staff address tickets sequentially. Do not tag admins repeatedly inside your ticket — provide your order ID/proof and wait calmly.\n` +
            `**3.3 No False or Troll Tickets:** Opening empty, joke, or spam tickets will result in ticket cooldowns or temporary mutes.`,
          inline: false
        },
        {
          name: '🚫 § 4 • ADVERTISING, LINKS & MALICIOUS CONTENT',
          value:
            `**4.1 No Self-Promotion:** Unauthorized promotion of external Discord servers, YouTube channels, cheat vendors, or services in public channels or member DMs is strictly prohibited.\n` +
            `**4.2 Anti-Phishing & Token Grabbers:** Posting malicious files, grabbers, crypto-scam links, or trojans will trigger immediate automated ban via **MIAN XITERS Sentinel Shield**.\n` +
            `**4.3 No Account Selling:** Trading, buying, or selling hacked, cracked, or third-party accounts is strictly forbidden.`,
          inline: false
        },
        {
          name: '⚖️ § 5 • STAFF AUTHORITY & DISCORD COMPLIANCE',
          value:
            `**5.1 Discord ToS:** All members must adhere to Discord's official Terms of Service and Community Guidelines.\n` +
            `**5.2 Final Discretion:** Staff members reserve the right to mute, kick, or ban any member whose actions disrupt server integrity or client safety, even if not explicitly enumerated above.\n` +
            `**5.3 Moderation Appeals:** If you believe a moderation action was incorrect, contact a Senior Staff member via DM or ticket calmly with proof.`,
          inline: false
        }
      );

    // Embed 3: Footer & Verification
    const footerEmbed = new EmbedBuilder()
      .setColor(0xFF0033)
      .setTitle('🛡️ 𝗠𝗜𝗔𝗡 𝗫𝗜𝗧𝗘𝗥𝗦 ✦ COMPLIANCE & VERIFICATION')
      .setDescription(
        `> **Stay Safe. Dominate the Game. Respect the Brotherhood.**\n\n` +
        `• **Need Cheats / Panels?** Check out <#1564299581737664572> and open a ticket in <#1483581020346384544>.\n` +
        `• **Free Releases & Updates:** Keep notifications on for <#1564299583973232640> and <#1483580851970244620>.\n` +
        `• **Official Invite:** \`discord.gg/mianxiters\`\n\n` +
        `*Click the checkmark below or grab roles in <#1565027249319575582> to confirm you have read and agreed to the directives.*`
      )
      .setFooter({
        text: 'MIAN XITERS ULTIMATE DOMINANCE ! • Dev by 1nOnlyMian',
        iconURL: client.user?.displayAvatarURL() || undefined
      })
      .setTimestamp();

    const sentMessage = await channel.send({
      embeds: [bannerEmbed, rulesEmbed, footerEmbed]
    });

    await sentMessage.react('✅').catch(() => {});
    await sentMessage.react('⚔️').catch(() => {});

    console.log(`[SUCCESS] New official rules posted successfully to #${channel.name} (Msg ID: ${sentMessage.id})!`);
  } catch (err: any) {
    console.error('[ERROR] Failed to post rules:', err);
  } finally {
    client.destroy();
    process.exit(0);
  }
});

client.login(TOKEN).catch(err => {
  console.error('[FATAL] Login failed:', err.message);
  process.exit(1);
});
