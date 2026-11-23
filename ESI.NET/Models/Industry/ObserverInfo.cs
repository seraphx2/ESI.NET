using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Industry
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.IndustryLogic.ObservedMining"/>.</summary>
    public class ObserverInfo
    {
        /// <summary>The day this mining was recorded.</summary>
        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        /// <summary>The character that did the mining.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>The corporation id of the character at the time data was recorded.</summary>
        [JsonProperty("recorded_corporation_id")]
        public long RecordedCorporationId { get; set; }

        /// <summary>Type ID of the ore mined.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>Units mined.</summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }
}
