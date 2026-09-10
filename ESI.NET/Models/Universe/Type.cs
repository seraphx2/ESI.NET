using Dogma = ESI.NET.Models.Dogma;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Universe
{
    public class Type
    {
        [JsonProperty("capacity")]
        public float Capacity { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("dogma_attributes")]
        public List<Dogma.Attribute> DogmaAttributes { get; set; } = new List<Dogma.Attribute>();

        [JsonProperty("dogma_effects")]
        public List<Dogma.Effect> DogmaEffects { get; set; } = new List<Dogma.Effect>();

        [JsonProperty("graphic_id")]
        public long GraphicId { get; set; }

        [JsonProperty("group_id")]
        public long GroupId { get; set; }

        [JsonProperty("icon_id")]
        public long IconId { get; set; }

        [JsonProperty("market_group_id")]
        public long MarketGroupId { get; set; }

        [JsonProperty("mass")]
        public float Mass { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("packaged_volume")]
        public float PackagedVolume { get; set; }

        [JsonProperty("portion_size")]
        public long PortionSize { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }

        [JsonProperty("radius")]
        public float Radius { get; set; }

        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        [JsonProperty("volume")]
        public float Volume { get; set; }
    }
}
