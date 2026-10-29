import { Message, EmbedBuilder, TextChannel } from 'discord.js';
import { createWorker, Worker } from 'tesseract.js';
import { Logger } from '../utils/logger.js';

const TOKEN_GRAB_REGEX = /(mrbeast|crypto.*casino|cryptocurrency.*casino|vyro|followin|rakeback|promo.*code|bonus.*code|activate.*code|withdraw.*success|\$3500|\$3700|\$3000|3500\$|3700\$|3000\$|paycheck.*\$|\$[0-9]+.*paycheck|claim.*reward)/i;

const LOG_CHANNEL_ID = '1553720063977193474';

export class TokenGrabGuardService {
  private worker: Worker | null = null;
  private isInitializing = false;

  private async getWorker(): Promise<Worker> {
    if (!this.worker && !this.isInitializing) {
      this.isInitializing = true;
      try {
        this.worker = await createWorker('eng');
      } catch (err: unknown) {
        const msg = err instanceof Error ? err.message : String(err);
        Logger.error(`[TOKEN_GRAB_GUARD] Failed to initialize OCR worker: ${msg}`);
      } finally {
        this.isInitializing = false;
      }
    }
    return this.worker as Worker;
  }

  /**
   * Scans a message for token grab / hijacked account spam (MrBeast, crypto casino, fake checks).
   * Checks message text, embeds, and runs OCR on image attachments.
   */
  public async checkMessage(message: Message): Promise<{ isTokenGrab: boolean; reason: string }> {
    // 1. Instant Text Check
    if (TOKEN_GRAB_REGEX.test(message.content)) {
      return {
        isTokenGrab: true,
        reason: `Matched scam text pattern: "${message.content.slice(0, 60)}..."`
      };
    }

    // 2. Embeds Check
    if (message.embeds && message.embeds.length > 0) {
      for (const emb of message.embeds) {
        const embText = `${emb.title || ''} ${emb.description || ''} ${emb.url || ''}`;
        if (TOKEN_GRAB_REGEX.test(embText)) {
          return {
            isTokenGrab: true,
            reason: `Matched scam embed content: "${embText.slice(0, 60)}..."`
          };
        }
      }
    }

    // 3. Attachments OCR Check
    if (message.attachments && message.attachments.size > 0) {
      for (const [, att] of message.attachments) {
        const isImage = att.contentType?.startsWith('image/') || /\.(png|jpe?g|webp)$/i.test(att.name || '');
        if (isImage) {
          try {
            const worker = await this.getWorker();
            if (worker) {
              const res = await worker.recognize(att.url);
              const text = res.data.text || '';
              if (TOKEN_GRAB_REGEX.test(text)) {
                return {
                  isTokenGrab: true,
                  reason: `Detected token-grab scam text inside image: "${text.replace(/\s+/g, ' ').trim().slice(0, 60)}..."`
                };
              }
            }
          } catch (err: unknown) {
            const msg = err instanceof Error ? err.message : String(err);
            Logger.warn(`[TOKEN_GRAB_GUARD] OCR scan error on attachment: ${msg}`);
          }
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
