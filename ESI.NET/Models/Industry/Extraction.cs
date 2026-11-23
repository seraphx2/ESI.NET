using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Industry
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.IndustryLogic.Extractions"/>.</summary>
    public class Extraction
    {
        /// <summary>ID of the refinery running the extraction.</summary>
        [JsonProperty("structure_id")]
        public long StructureId { get; set; }

        /// <summary>ID of the moon being drilled.</summary>
        [JsonProperty("moon_id")]
        public long MoonId { get; set; }

        /// <summary>The time at which the current extraction was initiated.</summary>
        [JsonProperty("extraction_start_time")]
        public DateTime ExtractionStartTime { get; set; }

        /// <summary>
        /// The time at which the chunk being extracted will arrive and can be fractured by the moon
        /// mining drill.
        /// </summary>
        [JsonProperty("chunk_arrival_time")]
        public DateTime ChunkArrivalTime { get; set; }

        /// <summary>
        /// The time at which the chunk being extracted will naturally fracture if it is not first
        /// fractured by the moon mining drill.
        /// </summary>
        [JsonProperty("natural_decay_time")]
        public DateTime NaturalDecayTime { get; set; }
    }
}
