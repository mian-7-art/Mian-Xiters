"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const discord_js_1 = require("discord.js");
exports.default = {
    name: discord_js_1.Events.GuildRoleCreate,
    async execute(role) {
        if (!role.guild)
            return;
    }
};
