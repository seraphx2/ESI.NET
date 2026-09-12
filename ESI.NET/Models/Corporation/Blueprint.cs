using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    public class Blueprint
    {
        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        [JsonProperty("location_flag")]
        public string LocationFlag { get; set; }

        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        [JsonProperty("material_efficiency")]
        public long MaterialEfficiency { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("runs")]
        public long Runs { get; set; }

        [JsonProperty("time_efficiency")]
        public long TimeEfficiency { get; set; }

        [JsonProperty("type_id")]
        public long TypeId { get; set; }
    }
}
