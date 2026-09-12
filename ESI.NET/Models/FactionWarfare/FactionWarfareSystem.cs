using ESI.NET.Enumerations;
using Newtonsoft.Json;

namespace ESI.NET.Models.FactionWarfare
{
    public class FactionWarfareSystem
    {
        [JsonProperty("contested")]
        public Contested Contested { get; set; }

        [JsonProperty("occupier_faction_id")]
        public long OccupierFactionId { get; set; }

        [JsonProperty("owner_faction_id")]
        public long OwnerFactionId { get; set; }

        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        [JsonProperty("victory_points")]
        public long VictoryPoints { get; set; }

        [JsonProperty("victory_points_threshold")]
        public long VictoryPointsThreshold { get; set; }
    }
}
