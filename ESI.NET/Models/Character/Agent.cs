using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Character
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CharacterLogic.AgentsResearch"/>.
    /// </summary>
    public class Agent
    {
        /// <summary>ID of the research agent (an NPC character).</summary>
        [JsonProperty("agent_id")]
        public long AgentId { get; set; }

        /// <summary>Research points the agent produces per day.</summary>
        [JsonProperty("points_per_day")]
        public float PointsPerDay { get; set; }

        /// <summary>
        /// Research points accrued before <see cref="StartedAt"/>. The current total is this plus
        /// <see cref="PointsPerDay"/> times the days since <see cref="StartedAt"/>.
        /// </summary>
        [JsonProperty("remainder_points")]
        public float RemainderPoints { get; set; }

        /// <summary>Type ID of the science skill the agent researches.</summary>
        [JsonProperty("skill_type_id")]
        public long SkillTypeId { get; set; }

        /// <summary>When research with this agent started.</summary>
        [JsonProperty("started_at")]
        public DateTime StartedAt { get; set; }
    }
}
