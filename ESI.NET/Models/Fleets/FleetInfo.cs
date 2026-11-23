using Newtonsoft.Json;

namespace ESI.NET.Models.Fleets
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.FleetsLogic.FleetInfo"/>.</summary>
    public class FleetInfo
    {
        /// <summary>The character's current fleet ID.</summary>
        [JsonProperty("fleet_id")]
        public long FleetId { get; set; }

        /// <summary>Character ID of the current fleet boss.</summary>
        [JsonProperty("fleet_boss_id")]
        public long FleetBossId { get; set; }

        /// <summary>
        /// ID of the wing the member is in. If not applicable, will be set to -1.
        /// </summary>
        [JsonProperty("wing_id")]
        public long WingId { get; set; }

        /// <summary>
        /// ID of the squad the member is in. If not applicable, will be set to -1.
        /// </summary>
        [JsonProperty("squad_id")]
        public long SquadId { get; set; }

        /// <summary>
        /// Member’s role in fleet. One of <c>fleet_commander</c>, <c>squad_commander</c>,
        /// <c>squad_member</c>, <c>wing_commander</c>.
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
