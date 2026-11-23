using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESI.NET.Models.Industry
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.IndustryLogic.MiningLedger"/>.</summary>
    public class Entry
    {
        /// <summary>The day the ore was mined.</summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>ID of the solar system it was mined in.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>Type ID of the ore mined.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>Units mined that day.</summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }
}
