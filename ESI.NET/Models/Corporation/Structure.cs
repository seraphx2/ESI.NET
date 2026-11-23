using ESI.NET.Enumerations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Corporation
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CorporationLogic.Structures"/>.</summary>
    public class Structure
    {
        /// <summary>ID of the corporation that owns the structure.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>Date on which the structure will run out of fuel.</summary>
        [JsonProperty("fuel_expires")]
        public DateTime FuelExpires { get; set; }

        /// <summary>The structure name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The date and time when the structure's newly requested reinforcement times (e.g.
        /// next_reinforce_hour and next_reinforce_day) will take effect.
        /// </summary>
        [JsonProperty("next_reinforce_apply")]
        public DateTime NextReinforceApply { get; set; }

        /// <summary>
        /// The requested change to reinforce_hour that will take effect at the time shown by
        /// next_reinforce_apply.
        /// </summary>
        [JsonProperty("next_reinforce_hour")]
        public long NextReinforceHour { get; set; }

        /// <summary>The id of the ACL profile for this citadel.</summary>
        [JsonProperty("profile_id")]
        public long ProfileId { get; set; }

        /// <summary>
        /// The hour of day that determines the four hour window when the structure will randomly
        /// exit its reinforcement periods and become vulnerable to attack against its armor and/or
        /// hull. The structure will become vulnerable at a random time that is +/- 2 hours centered
        /// on the value of this property.
        /// </summary>
        [JsonProperty("reinforce_hour")]
        public long ReinforceHour { get; set; }

        /// <summary>Contains a list of service upgrades, and their state.</summary>
        [JsonProperty("services")]
        public List<Service> Services { get; set; } = new List<Service>();

        /// <summary>
        /// Where the structure is in its reinforcement cycle, e.g. <c>shield_vulnerable</c>,
        /// <c>armor_reinforce</c>, <c>hull_vulnerable</c>, or <c>anchoring</c>.
        /// </summary>
        [JsonProperty("state")]
        public StructureState State { get; set; }

        /// <summary>Date at which the structure will move to it's next state.</summary>
        [JsonProperty("state_timer_end")]
        public DateTime StateTimerEnd { get; set; }

        /// <summary>Date at which the structure entered it's current state.</summary>
        [JsonProperty("state_timer_start")]
        public DateTime StateTimerStart { get; set; }

        /// <summary>The Item ID of the structure.</summary>
        [JsonProperty("structure_id")]
        public long StructureId { get; set; }

        /// <summary>The solar system the structure is in.</summary>
        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        /// <summary>The type id of the structure.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>Date at which the structure will unanchor.</summary>
        [JsonProperty("unanchors_at")]
        public DateTime UnanchorsAt { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Corporation.Structure.Services"/>.</summary>
    public class Service
    {
        /// <summary>The service module's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Whether the service is <c>online</c>, <c>offline</c>, or in <c>cleanup</c>.
        /// </summary>
        [JsonProperty("state")]
        public StructureServiceState State { get; set; }
    }
}
