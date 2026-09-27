import { Events, Role, TextChannel } from 'discord.js';
import { whitelistManager } from '../config/whitelist.js';
import { Logger } from '../utils/logger.js';

export default {
  name: Events.GuildRoleDelete,
  async execute(role: Role) {
    const guild = role.guild;
    if (!guild) return;

    // Self-Healing Defense: Detect if attacker deleted the bot's Quarantine role
    const currentQuarantineRoleId = whitelistManager.getQuarantineRole(guild.id);
    if (currentQuarantineRoleId && role.id === currentQuarantineRoleId) {
      Logger.threat('CRITICAL DEFENSE TAMPERING', `Attacker deleted Quarantine isolation role (${role.name})! Self-healing activated.`);

      try {
        const recreated = await guild.roles.create({
          name: 'MIAN-XITERS-QUARANTINE',
          color: 0x555555,
          permissions: [],
          reason: '[MIAN XITERS] Self-Healing: Re-creating deleted Quarantine role'
        });

        // Re-apply channel isolation overwrites
        for (const [, ch] of guild.channels.cache) {
          if ('permissionOverwrites' in ch) {
            await (ch as TextChannel).permissionOverwrites.edit(recreated, {
              ViewChannel: false,
              SendMessages: false,
              Connect: false
            }).catch(() => {});
          }
        }

        whitelistManager.setQuarantineRole(guild.id, recreated.id);
        Logger.mitigation('QUARANTINE ROLE RECONSTRUCTED', recreated.name, 0);
      } catch (err: any) {
        Logger.error(`[SELF_HEAL_FAILED] Could not reconstruct quarantine role: ${err.message}`);
      }
    }
  }
};
