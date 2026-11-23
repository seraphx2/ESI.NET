using Newtonsoft.Json;

namespace ESI.NET.Models.Assets
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.AssetsLogic.NamesForCharacter"/> and
    /// <see cref="ESI.NET.Logic.AssetsLogic.NamesForCorporation"/>.
    /// </summary>
    public class ItemName
    {
        /// <summary>ID of the asset item, as passed in the request.</summary>
        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        /// <summary>The name the player gave the item (a ship's or container's name).</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
