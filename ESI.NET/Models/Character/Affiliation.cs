using Newtonsoft.Json;

namespace ESI.NET.Models.Character
{
    public class Affiliation
    {
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("faction_id")]
        public long FactionId { get; set; }
    }
}
