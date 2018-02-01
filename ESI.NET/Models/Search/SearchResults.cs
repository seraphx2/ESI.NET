using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models
{
    public class SearchResults
    {
        [JsonProperty("agent")]
        public long[] Agent { get; set; }

        [JsonProperty("alliance")]
        public long[] AllianceId { get; set; }

        [JsonProperty("character")]
        public long[] CharacterId { get; set; }

        [JsonProperty("constellation")]
        public long[] ConstellationId { get; set; }

        [JsonProperty("corporation")]
        public long[] CorporationId { get; set; }

        [JsonProperty("faction")]
        public long[] FactionId { get; set; }

        [JsonProperty("inventorytype")]
        public long[] InventoryTypeId { get; set; }

        [JsonProperty("region")]
        public long[] RegionId { get; set; }

        [JsonProperty("solarsystem")]
        public long[] SolarSystemId { get; set; }

        [JsonProperty("station")]
        public long[] StationId { get; set; }

        [JsonProperty("wormhole")]
        public long[] WormholeId { get; set; }
    }
}
