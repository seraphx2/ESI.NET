using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Industry
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.IndustryLogic.SolarSystemCostIndices"/>.
    /// </summary>
    public class SolarSystem
    {
        /// <summary>ID of the solar system.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>The system's cost index for each industry activity.</summary>
        [JsonProperty("cost_indices")]
        public List<CostIndice> CostIndices { get; set; } = new List<CostIndice>();
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Industry.SolarSystem.CostIndices"/>.
    /// </summary>
    public class CostIndice
    {
        /// <summary>
        /// The industry activity: <c>manufacturing</c>, <c>researching_time_efficiency</c>,
        /// <c>researching_material_efficiency</c>, <c>copying</c>, <c>invention</c>, or
        /// <c>reaction</c>.
        /// </summary>
        [JsonProperty("activity")]
        public string Activity { get; set; }

        /// <summary>
        /// The system cost index for that activity, as a fraction (<c>0.05</c> is 5%).
        /// </summary>
        [JsonProperty("cost_index")]
        public decimal CostIndex { get; set; }
    }
}
