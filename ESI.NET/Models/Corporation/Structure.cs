using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESI.NET.Models.Corporation
{
    public class Structure
    {
        [JsonProperty("corporation_id")]
        public int CorporationId { get; set; }

        [JsonProperty("fuel_expires")]
        public string FuelExpires { get; set; }

        [JsonProperty("next_reinforce_apply")]
        public string NextReinforceApply { get; set; }

        [JsonProperty("next_reinforce_hour")]
        public int NextReinforceHour { get; set; }

        [JsonProperty("next_reinforce_weekday")]
        public int NextReinforceWeekday { get; set; }

        [JsonProperty("profile_id")]
        public int ProfileId { get; set; }

        [JsonProperty("reinforce_hour")]
        public int ReinforceHour { get; set; }

        [JsonProperty("reinforce_weekday")]
        public int ReinforceWeekday { get; set; }

        [JsonProperty("services")]
        public List<Service> Services { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("state_timer_end")]
        public string StateTimerEnd { get; set; }

        [JsonProperty("state_timer_start")]
        public string StateTimerStart { get; set; }

        [JsonProperty("structure_id")]
        public long StructureId { get; set; }

        [JsonProperty("system_id")]
        public int SystemId { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("unanchors_at")]
        public string UnanchorsAt { get; set; }
    }

    public class Service
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}
