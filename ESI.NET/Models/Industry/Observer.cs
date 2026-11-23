using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Industry
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.IndustryLogic.Observers"/>.</summary>
    public class Observer
    {
        /// <summary>When the observer's mining ledger last changed.</summary>
        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        /// <summary>The entity that was observing the asteroid field when it was mined.</summary>
        [JsonProperty("observer_id")]
        public long ObserverId { get; set; }

        /// <summary>The category of the observing entity. One of <c>structure</c>.</summary>
        [JsonProperty("observer_type")]
        public string ObserverType { get; set; }
    }
}
