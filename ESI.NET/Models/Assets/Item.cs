using Newtonsoft.Json;

namespace ESI.NET.Models.Assets
{
    public class Item
    {
        [JsonProperty("is_blueprint_copy")]
        public bool? IsBlueprintCopy { get; set; }

        [JsonProperty("is_singleton")]
        public bool IsSingleton { get; set; }

        [JsonProperty("item_id")]
        public string Id { get; set; }

        [JsonProperty("location_flag")]
        public string Location { get; set; }

        [JsonProperty("location_id")]
        public string LocationId { get; set; }

        [JsonProperty("location_type")]
        public string LocationType { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("type_id")]
        public string TypeId { get; set; }
    }
}
