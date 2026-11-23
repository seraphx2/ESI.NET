using Newtonsoft.Json;

namespace ESI.NET.Models.Loyalty
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.LoyaltyLogic.Points"/>.</summary>
    public class Points
    {
        /// <summary>ID of the NPC corporation the points are with.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>Loyalty points the character has with that corporation.</summary>
        [JsonProperty("loyalty_points")]
        public long LoyaltyPoints { get; set; }
    }
}
