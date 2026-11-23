using Newtonsoft.Json;

namespace ESI.NET.Models.Character
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CharacterLogic.Affiliation"/>.</summary>
    public class Affiliation
    {
        /// <summary>The character's alliance ID, if their corporation is in an alliance.</summary>
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        /// <summary>The character's ID.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>ID of the character's corporation.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>The character's faction ID, if their corporation is in a faction.</summary>
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }
    }
}
