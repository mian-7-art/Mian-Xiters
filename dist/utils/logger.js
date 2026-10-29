"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Logger = void 0;
const config_js_1 = require("../config/config.js");
class Logger {
    static formatTime() {
        return new Date().toISOString().replace('T', ' ').substring(0, 19);
    }
    static info(message, context) {
        console.log(`\x1b[36m[${this.formatTime()}]\x1b[0m \x1b[1m\x1b[37m[MIAN XITERS]\x1b[0m \x1b[32m[INFO]\x1b[0m ${message}`, context || '');
    }
    static warn(message, context) {
        console.warn(`\x1b[33m[${this.formatTime()}]\x1b[0m \x1b[1m\x1b[37m[MIAN XITERS]\x1b[0m \x1b[33m[WARN]\x1b[0m ${message}`, context || '');
    }
    static error(message, error) {
        console.error(`\x1b[31m[${this.formatTime()}]\x1b[0m \x1b[1m\x1b[37m[MIAN XITERS]\x1b[0m \x1b[41m\x1b[37m[ERROR]\x1b[0m ${message}`, error || '');
    }
    static threat(threatName, details) {
        console.log(`\x1b[41m\x1b[37m[THREAT DETECTED]\x1b[0m \x1b[1m\x1b[31m${threatName}\x1b[0m -> ${details}`);
    }
    static mitigation(actionName, target, timeMs) {
        console.log(`\x1b[42m\x1b[30m[NEUTRALIZED]\x1b[0m \x1b[1m\x1b[32m${actionName}\x1b[0m executed on \x1b[33m${target}\x1b[0m in \x1b[36m${timeMs}ms\x1b[0m`);
    }
    /**
     * Dispatches security notifications to an external webhook asynchronously.
     * Completely decoupled from the event loop so network slowdowns NEVER delay defense actions.
     */
    static async sendWebhookLog(title, description, color = config_js_1.BOT_CONFIG.colors.primary, fields = []) {
        const webhookUrl = config_js_1.BOT_CONFIG.logWebhookUrl;
        if (!webhookUrl)
            return;
        try {
            fetch(webhookUrl, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    username: config_js_1.BOT_CONFIG.name,
                    avatar_url: 'https://cdn.discordapp.com/embed/avatars/0.png',
                    embeds: [{
                            title: `🛡️ ${title}`,
                            description,
                            color,
                            fields,
                            footer: {
                                text: `${config_js_1.BOT_CONFIG.name} • ${config_js_1.BOT_CONFIG.slogan}`
                            },
                            timestamp: new Date().toISOString()
                        }]
                })
            }).catch(err => {
                // Silent catch to prevent crashing if webhook URL is invalid or rate limited
            });
        }
        catch {
            // Ignore webhook fetch failure
        }
    }
}
exports.Logger = Logger;
