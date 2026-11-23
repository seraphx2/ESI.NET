using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.SearchLogic.Query"/>.</summary>
    public class SearchResults
    {
        /// <summary>IDs of matching agents.</summary>
        [JsonProperty("agent")]
        public long[] Agents { get; set; }

        /// <summary>IDs of matching alliances.</summary>
        [JsonProperty("alliance")]
        public long[] Alliances { get; set; }

        /// <summary>IDs of matching characters.</summary>
        [JsonProperty("character")]
        public long[] Characters { get; set; }

        /// <summary>IDs of matching constellations.</summary>
        [JsonProperty("constellation")]
        public long[] Constellations { get; set; }

        /// <summary>IDs of matching corporations.</summary>
        [JsonProperty("corporation")]
        public long[] Corporations { get; set; }

        /// <summary>IDs of matching factions.</summary>
        [JsonProperty("faction")]
        public long[] Factions { get; set; }

        /// <summary>Type IDs of matching items.</summary>
        [JsonProperty("inventory_type")]
        public long[] InventoryTypes { get; set; }

        /// <summary>IDs of matching regions.</summary>
        [JsonProperty("region")]
        public long[] Regions { get; set; }

        /// <summary>IDs of matching solar systems.</summary>
        [JsonProperty("solar_system")]
        public long[] SolarSystems { get; set; }

        /// <summary>IDs of matching stations.</summary>
        [JsonProperty("station")]
        public long[] Stations { get; set; }

        /// <summary>IDs of matching structures the character can see.</summary>
        [JsonProperty("structure")]
        public long[] Structures { get; set; }
    }
}
