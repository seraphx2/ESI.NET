using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Alliance
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.AllianceLogic.Information"/>.</summary>
    public class Alliance
    {
        /// <summary>ID of the corporation that founded the alliance.</summary>
        [JsonProperty("creator_corporation_id")]
        public long CreatorCorporationId { get; set; }

        /// <summary>ID of the character who founded the alliance.</summary>
        [JsonProperty("creator_id")]
        public long CreatorId { get; set; }

        /// <summary>Alliance's founding date.</summary>
        [JsonProperty("date_founded")]
        public DateTime DateFounded { get; set; }

        /// <summary>
        /// ID of the executor corporation, which runs the alliance. Absent when the alliance has
        /// none (it is closed or disbanding).
        /// </summary>
        [JsonProperty("executor_corporation_id")]
        public long ExecutorCorporationId { get; set; }

        /// <summary>
        /// ID of the faction the alliance is enlisted with in faction warfare, if any.
        /// </summary>
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        /// <summary>Alliance's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Alliance's ticker.</summary>
        [JsonProperty("ticker")]
        public string Ticker { get; set; }

    }
}
