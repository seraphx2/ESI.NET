using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Corporation
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CorporationLogic.Divisions"/>.</summary>
    public class Divisions
    {
        /// <summary>The corporation's hangar divisions.</summary>
        [JsonProperty("hangar")]
        public List<Division> Hangar { get; set; } = new List<Division>();

        /// <summary>The corporation's wallet divisions.</summary>
        [JsonProperty("wallet")]
        public List<Division> Wallet { get; set; } = new List<Division>();
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Corporation.Divisions.Hangar"/> and
    /// <see cref="ESI.NET.Models.Corporation.Divisions.Wallet"/>.
    /// </summary>
    public class Division
    {
        /// <summary>The division's number, 1 to 7.</summary>
        [JsonProperty("division")]
        public long DivisionId { get; set; }

        /// <summary>The name the corporation gave the division.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
