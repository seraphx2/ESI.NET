using Newtonsoft.Json;

namespace ESI.NET.Models.Skills
{
    public class SkillQueueItem
    {
        [JsonProperty("skill_id")]
        public long SkillId { get; set; }

        [JsonProperty("finish_date")]
        public string FinishDate { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("finished_level")]
        public long FinishedLevel { get; set; }

        [JsonProperty("queue_position")]
        public long QueuePosition { get; set; }

        [JsonProperty("training_start_sp")]
        public long TrainingStartSp { get; set; }

        [JsonProperty("level_end_sp")]
        public long LevelEndSp { get; set; }

        [JsonProperty("level_start_sp")]
        public long LevelStartSp { get; set; }
    }
}
