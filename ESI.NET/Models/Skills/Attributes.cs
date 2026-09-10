using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Skills
{
    public class Attributes
    {
        [JsonProperty("charisma")]
        public long Charisma { get; set; }

        [JsonProperty("intelligence")]
        public long Intelligence { get; set; }

        [JsonProperty("memory")]
        public long Memory { get; set; }

        [JsonProperty("perception")]
        public long Perception { get; set; }

        [JsonProperty("willpower")]
        public long Willpower { get; set; }

        [JsonProperty("bonus_remaps")]
        public long BonusRemaps { get; set; }

        [JsonProperty("last_remap_date")]
        public DateTime LastRemapDate { get; set; }

        [JsonProperty("accrued_remap_cooldown_date")]
        public string AccruedRemapCooldownDate { get; set; }
    }
}
