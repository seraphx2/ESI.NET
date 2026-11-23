using Newtonsoft.Json;

namespace ESI.NET.Models.Assets
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.AssetsLogic.LocationsForCharacter"/> and
    /// <see cref="ESI.NET.Logic.AssetsLogic.LocationsForCorporation"/>.
    /// </summary>
    public class ItemLocation
    {
        /// <summary>ID of the asset item, as passed in the request.</summary>
        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        /// <summary>
        /// The item's coordinates in space, in metres from the solar system's centre.
        /// </summary>
        [JsonProperty("position")]
        public Position Position { get; set; }
    }
}
