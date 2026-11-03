import { Message, EmbedBuilder, TextChannel } from 'discord.js';
import { Logger } from '../utils/logger.js';

// Scam and phishing text patterns commonly used by hijacked/token-grabbed accounts
const TOKEN_GRAB_REGEX = /(mrbeast|crypto.*casino|cryptocurrency.*casino|vyro|followin|rakeback|promo.*code|bonus.*code|activate.*code|withdraw.*success|\$3500|\$3700|\$3000|3500\$|3700\$|3000\$|paycheck.*\$|\$[0-9]+.*paycheck|claim.*reward)/i;

// Dangerous / executable file extensions used in Discord token-grab attachments
const SUSPICIOUS_ATTACHMENT_REGEX = /\.(exe|scr|bat|cmd|vbs|ps1|hta|pif|com|cpl|jar|msi|reg)$/i;

const LOG_CHANNEL_ID = '1553720063977193474';

export class TokenGrabGuardService {
  /**
   * Ultra-lightweight zero-memory Token Grab & Scam Interceptor.
   * Analyzes message content, rich embeds, attachment metadata, and filenames.
   * Operates with 0MB additional RAM overhead (no heavy OCR or WebAssembly).
   */
  public async checkMessage(message: Message): Promise<{ isTokenGrab: boolean; reason: string }> {
    // 1. Instant Text Check
    if (TOKEN_GRAB_REGEX.test(message.content)) {
      return {
        isTokenGrab: true,
        reason: `Matched scam text pattern: "${message.content.slice(0, 60)}..."`
      };
    }

    // 2. Embeds Check (Titles, Descriptions, Fields, Footers, URLs)
    if (message.embeds && message.embeds.length > 0) {
      for (const emb of message.embeds) {
        const fieldsText = emb.fields?.map(f => `${f.name} ${f.value}`).join(' ') || '';
        const embText = `${emb.title || ''} ${emb.description || ''} ${emb.url || ''} ${fieldsText} ${emb.footer?.text || ''}`;
        if (TOKEN_GRAB_REGEX.test(embText)) {
          return {
            isTokenGrab: true,
            reason: `Matched scam embed content: "${embText.slice(0, 60)}..."`
          };
        }
      }
    }

    // 3. Attachments Inspection (Filenames, descriptions, and suspicious payloads)
    if (message.attachments && message.attachments.size > 0) {
      for (const [, att] of message.attachments) {
        const fileName = att.name || '';
        const fileDescription = att.description || '';

        // Check for malicious file extensions disguised as images/docs (e.g. proof.png.exe)
        if (SUSPICIOUS_ATTACHMENT_REGEX.test(fileName)) {
          return {
            isTokenGrab: true,
            reason: `Suspicious executable attachment detected: "${fileName}"`
          };
        }

        // Check attachment name and description against token-grab scam patterns
        if (TOKEN_GRAB_REGEX.test(fileName) || TOKEN_GRAB_REGEX.test(fileDescription)) {
          return {
            isTokenGrab: true,
            reason: `Scam keyword detected in attachment name/description: "${fileName}"`
          };
        }
      }
    }

    return { isTokenGrab: false, reason: '' };
  }

  /**
   * Logs a token grab interception to mian-xiters-logs without applying timeout/quarantine.
   */
  public async logTokenGrabIncident(message: Message, reason: string): Promise<void> {
    try {
      const logChannel = message.guild?.channels.cache.get(LOG_CHANNEL_ID) as TextChannel;
      if (logChannel) {
        const embed = new EmbedBuilder()
          .setColor(0xFF0033) // Crimson Red
          .setTitle('🚨 [TOKEN GRAB DETECTED] Compromised Account Spam Deleted')
          .setDescription(
            `**User Got Grabbed:** <@${message.author.id}> (\`${message.author.tag}\` / \`${message.author.id}\`)\n` +
            `**Action Taken:** Message Auto-Deleted instantly (No timeout/quarantine applied as per policy).\n` +
            `**Channel:** <#${message.channelId}>\n` +
            `**Trigger / Details:** ${reason}`
          )
          .setTimestamp();

        await logChannel.send({ embeds: [embed] }).catch(() => {});
      }
      Logger.threat(
        'TOKEN GRAB SCAM AUTO-DELETED',
        `User ${message.author.tag} (${message.author.id}) sent a compromised grab message. Auto-deleted, no timeout applied.`
      );
    } catch (err: unknown) {
      const msg = err instanceof Error ? err.message : String(err);
      Logger.error(`[TOKEN_GRAB_GUARD] Failed to dispatch log embed: ${msg}`);
    }
  }
}

export const tokenGrabGuardService = new TokenGrabGuardService();
