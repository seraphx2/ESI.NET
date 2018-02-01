using Newtonsoft.Json;

namespace ESI.NET.Models.Character
{
    public class Affiliation
    {
        [JsonProperty("character_id")]
        public int CharacterId { get; set; }

        [JsonProperty("alliance_id")]
        public int AllianceId { get; set; }

        [JsonProperty("corporation_id")]
        public int CorporationId { get; set; }

        [JsonProperty("faction_id")]
        public int FactionId { get; set; }
    }
}
