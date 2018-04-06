using Newtonsoft.Json;

namespace ESI.NET.Models.FactionWarfare
{
    public class FactionWarfareSystem
    {
        [JsonProperty("solar_system_id")]
        public int SolarSystemId { get; set; }

        [JsonProperty("owner_faction_id")]
        public int OwnerFactionId { get; set; }

        [JsonProperty("occupier_faction_id")]
        public int OccupierFactionId { get; set; }

        [JsonProperty("victory_points")]
        public int VictoryPoints { get; set; }

        [JsonProperty("victory_points_threshold")]
        public int VictoryPointsThreshold { get; set; }

        [JsonProperty("contested")]
        public bool Contested { get; set; }
    }
}
