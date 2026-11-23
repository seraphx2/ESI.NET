using ESI.NET.Enumerations;
using Newtonsoft.Json;

namespace ESI.NET.Models.FactionWarfare
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.FactionWarfareLogic.Systems"/>.</summary>
    public class FactionWarfareSystem
    {
        /// <summary>
        /// How contested the system is: <c>captured</c>, <c>contested</c>, <c>uncontested</c>, or
        /// <c>vulnerable</c>.
        /// </summary>
        [JsonProperty("contested")]
        public Contested Contested { get; set; }

        /// <summary>ID of the faction occupying the system.</summary>
        [JsonProperty("occupier_faction_id")]
        public long OccupierFactionId { get; set; }

        /// <summary>ID of the faction that owns the system.</summary>
        [JsonProperty("owner_faction_id")]
        public long OwnerFactionId { get; set; }

        /// <summary>ID of the solar system.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>Victory points the attackers have built up in the system.</summary>
        [JsonProperty("victory_points")]
        public long VictoryPoints { get; set; }

        /// <summary>Victory points at which the system becomes vulnerable.</summary>
        [JsonProperty("victory_points_threshold")]
        public long VictoryPointsThreshold { get; set; }
    }
}
