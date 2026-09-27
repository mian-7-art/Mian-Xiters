import { Events, GuildChannel } from 'discord.js';
import { cacheService } from '../services/cacheService.js';

export default {
  name: Events.ChannelCreate,
  async execute(channel: GuildChannel) {
    if (!channel.guild) return;
    // Keep snapshot channel references fresh
  }
};
