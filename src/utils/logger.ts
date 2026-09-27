import { BOT_CONFIG } from '../config/config.js';

export class Logger {
  private static formatTime(): string {
    return new Date().toISOString().replace('T', ' ').substring(0, 19);
  }

  public static info(message: string, context?: any): void {
    console.log(`\x1b[36m[${this.formatTime()}]\x1b[0m \x1b[1m\x1b[37m[MIAN XITERS]\x1b[0m \x1b[32m[INFO]\x1b[0m ${message}`, context || '');
  }

  public static warn(message: string, context?: any): void {
    console.warn(`\x1b[33m[${this.formatTime()}]\x1b[0m \x1b[1m\x1b[37m[MIAN XITERS]\x1b[0m \x1b[33m[WARN]\x1b[0m ${message}`, context || '');
  }

  public static error(message: string, error?: any): void {
    console.error(`\x1b[31m[${this.formatTime()}]\x1b[0m \x1b[1m\x1b[37m[MIAN XITERS]\x1b[0m \x1b[41m\x1b[37m[ERROR]\x1b[0m ${message}`, error || '');
  }

  public static threat(threatName: string, details: string): void {
    console.log(`\x1b[41m\x1b[37m[THREAT DETECTED]\x1b[0m \x1b[1m\x1b[31m${threatName}\x1b[0m -> ${details}`);
  }

  public static mitigation(actionName: string, target: string, timeMs: number): void {
    console.log(`\x1b[42m\x1b[30m[NEUTRALIZED]\x1b[0m \x1b[1m\x1b[32m${actionName}\x1b[0m executed on \x1b[33m${target}\x1b[0m in \x1b[36m${timeMs}ms\x1b[0m`);
  }

  /**
   * Dispatches security notifications to an external webhook asynchronously.
   * Completely decoupled from the event loop so network slowdowns NEVER delay defense actions.
   */
  public static async sendWebhookLog(title: string, description: string, color: number = BOT_CONFIG.colors.primary, fields: { name: string; value: string; inline?: boolean }[] = []): Promise<void> {
    const webhookUrl = BOT_CONFIG.logWebhookUrl;
    if (!webhookUrl) return;

    try {
      fetch(webhookUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          username: BOT_CONFIG.name,
          avatar_url: 'https://cdn.discordapp.com/embed/avatars/0.png',
          embeds: [{
            title: `🛡️ ${title}`,
            description,
            color,
            fields,
            footer: {
              text: `${BOT_CONFIG.name} • ${BOT_CONFIG.slogan}`
            },
            timestamp: new Date().toISOString()
          }]
        })
      }).catch(err => {
        // Silent catch to prevent crashing if webhook URL is invalid or rate limited
      });
    } catch {
      // Ignore webhook fetch failure
    }
  }
}
