"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
exports.default = {
    name: discord_js_1.Events.ChannelCreate,
    async execute(channel) {
        if (!channel.guild)
            return;
        // Keep snapshot channel references fresh
    }
};
