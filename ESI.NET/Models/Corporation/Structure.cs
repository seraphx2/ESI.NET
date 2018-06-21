using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESI.NET.Models.Corporation
{
    public class Structure
    {
        [JsonProperty("structure_id")]
        public long StructureId { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("corporation_id")]
        public int CorporationId { get; set; }

        [JsonProperty("system_id")]
        public int SystemId { get; set; }

        [JsonProperty("profile_id")]
        public int ProfileId { get; set; }

        [JsonProperty("current_vul")]
        public List<Vulnerability> CurrentVul { get; set; } = new List<Vulnerability>();

        [JsonProperty("next_vul")]
        public List<Vulnerability> NextVul { get; set; } = new List<Vulnerability>();

        [JsonProperty("fuel_expires")]
        public string FuelExpires { get; set; }

        [JsonProperty("services")]
        public List<Service> Services { get; set; } = new List<Service>();

        [JsonProperty("state_timer_start")]
        public DateTime StateTimerStart { get; set; }

        [JsonProperty("state_timer_end")]
        public DateTime StateTimerEnd { get; set; }

        [JsonProperty("unanchors_at")]
        public DateTime UnanchorsAt { get; set; }
    }

    public class Vulnerability
    {
        [JsonProperty("day")]
        public int Day { get; set; }

        [JsonProperty("hour")]
        public int Hour { get; set; }
    }

    public class Service
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}
