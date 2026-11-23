using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Skills
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.SkillsLogic.Attributes"/>.</summary>
    public class Attributes
    {
        /// <summary>The character's Charisma attribute, implants included.</summary>
        [JsonProperty("charisma")]
        public long Charisma { get; set; }

        /// <summary>The character's Intelligence attribute, implants included.</summary>
        [JsonProperty("intelligence")]
        public long Intelligence { get; set; }

        /// <summary>The character's Memory attribute, implants included.</summary>
        [JsonProperty("memory")]
        public long Memory { get; set; }

        /// <summary>The character's Perception attribute, implants included.</summary>
        [JsonProperty("perception")]
        public long Perception { get; set; }

        /// <summary>The character's Willpower attribute, implants included.</summary>
        [JsonProperty("willpower")]
        public long Willpower { get; set; }

        /// <summary>Number of available bonus character neural remaps.</summary>
        [JsonProperty("bonus_remaps")]
        public long BonusRemaps { get; set; }

        /// <summary>Datetime of last neural remap, including usage of bonus remaps.</summary>
        [JsonProperty("last_remap_date")]
        public DateTime LastRemapDate { get; set; }

        /// <summary>
        /// Neural remapping cooldown after a character uses remap accrued over time.
        /// </summary>
        [JsonProperty("accrued_remap_cooldown_date")]
        public string AccruedRemapCooldownDate { get; set; }
    }
}
