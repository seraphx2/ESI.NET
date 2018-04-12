using Newtonsoft.Json;

namespace ESI.NET.Models.Contracts
{
    public class ContractItem
    {
        [JsonProperty("record_id")]
        public long RecordId { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("raw_quantity")]
        public int RawQuantity { get; set; }

        [JsonProperty("is_singleton")]
        public bool IsSingleton { get; set; }

        [JsonProperty("is_included")]
        public bool IsIncluded { get; set; }
    }
}
