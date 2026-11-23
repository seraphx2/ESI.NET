using Newtonsoft.Json;

namespace ESI.NET.Models.Skills
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.SkillsLogic.Queue"/>.</summary>
    public class SkillQueueItem
    {
        /// <summary>The TypeID of the skill.</summary>
        [JsonProperty("skill_id")]
        public long SkillId { get; set; }

        /// <summary>The date the skill training will finish.</summary>
        [JsonProperty("finish_date")]
        public string FinishDate { get; set; }

        /// <summary>The date the skill training will start/continue.</summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        /// <summary>The level the skill is training for.</summary>
        [JsonProperty("finished_level")]
        public long FinishedLevel { get; set; }

        /// <summary>The position of the skill in the queue.</summary>
        [JsonProperty("queue_position")]
        public long QueuePosition { get; set; }

        /// <summary>The Skill Points at the start of training.</summary>
        [JsonProperty("training_start_sp")]
        public long TrainingStartSp { get; set; }

        /// <summary>The Skill Points at the end of the level.</summary>
        [JsonProperty("level_end_sp")]
        public long LevelEndSp { get; set; }

        /// <summary>The Skill Points at the start of the level.</summary>
        [JsonProperty("level_start_sp")]
        public long LevelStartSp { get; set; }
    }
}
