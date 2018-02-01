using Newtonsoft.Json;

namespace ESI.NET.Models.Character
{
    public class Stats
    {
        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("character")]
        public CharacterInfo Character { get; set; }

        [JsonProperty("combat")]
        public Combat Combat { get; set; }

        [JsonProperty("industry")]
        public Industry Industry { get; set; }

        [JsonProperty("inventory")]
        public Inventory Inventory { get; set; }

        [JsonProperty("isk")]
        public Isk Isk { get; set; }

        [JsonProperty("market")]
        public Market Market { get; set; }

        [JsonProperty("mining")]
        public Mining Mining { get; set; }

        [JsonProperty("module")]
        public Module Module { get; set; }

        [JsonProperty("orbital")]
        public Orbital Orbital { get; set; }

        [JsonProperty("pve")]
        public Pve Pve { get; set; }

        [JsonProperty("social")]
        public Social Social { get; set; }

        [JsonProperty("travel")]
        public Travel Travel { get; set; }
    }
}
