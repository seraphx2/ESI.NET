using Newtonsoft.Json;

namespace ESI.NET.Models.Assets
{
    public class ItemLocation
    {
        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }
    }
}
