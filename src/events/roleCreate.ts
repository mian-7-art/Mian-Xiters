import { Events, Role } from 'discord.js';

export default {
  name: Events.GuildRoleCreate,
  async execute(role: Role) {
    if (!role.guild) return;
  }
};
