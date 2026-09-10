using Newtonsoft.Json;

namespace ESI.NET.Models.Contracts
{
    public class ContractItem
    {
        [JsonProperty("record_id")]
        public long RecordId { get; set; }

        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("raw_quantity")]
        public long RawQuantity { get; set; }

        [JsonProperty("is_singleton")]
        public bool IsSingleton { get; set; }

        [JsonProperty("is_included")]
        public bool IsIncluded { get; set; }

        [JsonProperty("is_blueprint_copy")]
        public bool IsBlueprintCopy { get; set; }

        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        [JsonProperty("material_efficiency")]
        public long MaterialEfficiency { get; set; }

        [JsonProperty("runs")]
        public long Runs { get; set; }

        [JsonProperty("time_efficiency")]
        public long TimeEfficiency { get; set; }
    }
}
