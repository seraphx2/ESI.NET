using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Skills
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.SkillsLogic.List"/>.</summary>
    public class SkillDetails
    {
        /// <summary>The trained skills.</summary>
        [JsonProperty("skills")]
        public List<Skill> Skills { get; set; } = new List<Skill>();

        /// <summary>The total Skill Points spent on skills.</summary>
        [JsonProperty("total_sp")]
        public long TotalSp { get; set; }

        /// <summary>The amount of unallocated Skill Points.</summary>
        [JsonProperty("unallocated_sp")]
        public long UnallocatedSp { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Skills.SkillDetails.Skills"/>.</summary>
    public class Skill
    {
        /// <summary>The Type ID of the skill.</summary>
        [JsonProperty("skill_id")]
        public long SkillId { get; set; }

        /// <summary>The amount of Skill Points in the skill.</summary>
        [JsonProperty("skillpoints_in_skill")]
        public long SkillpointsInSkill { get; set; }

        /// <summary>The trained skill level.</summary>
        [JsonProperty("trained_skill_level")]
        public long TrainedSkillLevel { get; set; }

        /// <summary>
        /// The active skill level (can differ from trained due to alpha status and/or active expert
        /// systems).
        /// </summary>
        [JsonProperty("active_skill_level")]
        public long ActiveSkillLevel { get; set; }
    }
}
