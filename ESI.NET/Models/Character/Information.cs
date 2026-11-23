using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Character
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CharacterLogic.Information"/>.</summary>
    public class Information
    {
        /// <summary>Character's achievement score.</summary>
        [JsonProperty("achievement_score")]
        public long AchievementScore { get; set; }

        /// <summary>Character's alliance ID.</summary>
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        /// <summary>Character's creation date.</summary>
        [JsonProperty("birthday")]
        public DateTime Birthday { get; set; }

        /// <summary>ID of the character's bloodline. Bloodlines are in the SDE.</summary>
        [JsonProperty("bloodline_id")]
        public long BloodlineId { get; set; }

        /// <summary>Character's equipped cosmetic title ID.</summary>
        [JsonProperty("character_title_id")]
        public string CharacterTitleId { get; set; }

        /// <summary>Character's corporation ID.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>The character's title in their corporation, if any.</summary>
        [JsonProperty("corporation_title")]
        public string CorporationTitle { get; set; }

        /// <summary>Character's description (biography).</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// ID of the faction the character is enlisted with in faction warfare, if any.
        /// </summary>
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        /// <summary>The character's gender: <c>male</c> or <c>female</c>.</summary>
        [JsonProperty("gender")]
        public string Gender { get; set; }

        /// <summary>Character's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>ID of the character's race. Races are in the SDE.</summary>
        [JsonProperty("race_id")]
        public long RaceId { get; set; }

        /// <summary>
        /// The character's security status, from -10 to 10. Below -5 they are an outlaw in high-sec
        /// space.
        /// </summary>
        [JsonProperty("security_status")]
        public decimal SecurityStatus { get; set; }
    }
}
