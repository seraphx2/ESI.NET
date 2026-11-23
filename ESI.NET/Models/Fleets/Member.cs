using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Fleets
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.FleetsLogic.Members"/>.</summary>
    public class Member
    {
        /// <summary>ID of the fleet member.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>Type ID of the ship the member is flying.</summary>
        [JsonProperty("ship_type_id")]
        public long ShipTypeId { get; set; }

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
        /// Member’s role in fleet. One of <c>fleet_commander</c>, <c>wing_commander</c>,
        /// <c>squad_commander</c>, <c>squad_member</c>.
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>Localized role names.</summary>
        [JsonProperty("role_name")]
        public string RoleName { get; set; }

        /// <summary>When the member joined the fleet.</summary>
        [JsonProperty("join_time")]
        public DateTime JoinTime { get; set; }

        /// <summary>Whether the member take fleet warps.</summary>
        [JsonProperty("takes_fleet_warp")]
        public bool TakesFleetWarp { get; set; }

        /// <summary>Solar system the member is located in.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>Station in which the member is docked in, if applicable.</summary>
        [JsonProperty("station_id")]
        public long StationId { get; set; }
    }
}
