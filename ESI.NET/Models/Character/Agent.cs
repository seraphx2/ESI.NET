using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Character
{
    public class Agent
    {
        [JsonProperty("agent_id")]
        public long AgentId { get; set; }

        [JsonProperty("points_per_day")]
        public decimal PointsPerDay { get; set; }

        [JsonProperty("remainder_points")]
        public decimal RemainderPoints { get; set; }

        [JsonProperty("skill_type_id")]
        public long SkillTypeId { get; set; }

        [JsonProperty("started_at")]
        public DateTime StartedAt { get; set; }
    }
}
