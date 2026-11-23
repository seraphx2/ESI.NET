using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Insurance
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.InsuranceLogic.Levels"/>.</summary>
    public class Insurance
    {
        /// <summary>Type ID of the ship.</summary>
        [JsonProperty("type_id")]
        public long TypeID { get; set; }

        /// <summary>A list of a available insurance levels for this ship type.</summary>
        [JsonProperty("levels")]
        public List<Levels> Levels { get; set; } = new List<Levels>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Insurance.Insurance.Levels"/>.</summary>
    public class Levels
    {
        /// <summary>Localized insurance level.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>What this level of insurance costs, in ISK.</summary>
        [JsonProperty("cost")]
        public double Cost { get; set; }

        /// <summary>What this level pays out if the ship is destroyed, in ISK.</summary>
        [JsonProperty("payout")]
        public double Payout { get; set; }
    }
}
