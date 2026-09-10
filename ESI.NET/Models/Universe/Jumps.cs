using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    public class Jumps
    {
        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        [JsonProperty("ship_jumps")]
        public long ShipJumps { get; set; }
    }
}
