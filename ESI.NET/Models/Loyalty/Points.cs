using Newtonsoft.Json;

namespace ESI.NET.Models.Loyalty
{
    public class Points
    {
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("loyalty_points")]
        public long LoyaltyPoints { get; set; }
    }
}
