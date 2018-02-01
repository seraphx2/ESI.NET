using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Corporation
{
    public class Divisions
    {
        [JsonProperty("hangar")]
        public List<Division> Hangar { get; set; }

        [JsonProperty("wallet")]
        public List<Division> Wallet { get; set; }
    }

    public class Division
    {
        [JsonProperty("division")]
        public int DivisionId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
