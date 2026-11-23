// Obsolete and undocumented on purpose: removed with the next compatibility-date release.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using Newtonsoft.Json;

namespace ESI.NET.Models.Character
{
    /// <summary>
    /// Part of ESI's removed character statistics endpoint (<c>/characters/{character_id}/stats/</c>).
    /// No ESI.NET method returns it.
    /// </summary>
    [Obsolete("No ESI endpoint returns this: CCP removed the character statistics endpoint. Will be removed with the next compatibility-date release.")]
    public class Orbital
    {
        [JsonProperty("strike_characters_killed")]
        public long StrikeCharactersKilled { get; set; }

        [JsonProperty("strike_damage_to_players_armor_amount")]
        public long StrikeDamageToPlayersArmorAmount { get; set; }

        [JsonProperty("strike_damage_to_players_shield_amount")]
        public long StrikeDamageToPlayersShieldAmount { get; set; }
    }
}
