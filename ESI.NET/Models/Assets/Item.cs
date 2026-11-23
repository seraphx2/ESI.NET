using Newtonsoft.Json;

namespace ESI.NET.Models.Assets
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.AssetsLogic.ForCharacter"/> and
    /// <see cref="ESI.NET.Logic.AssetsLogic.ForCorporation"/>.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Whether the item is a blueprint copy (BPC) rather than an original. Absent for anything
        /// that is not a blueprint.
        /// </summary>
        [JsonProperty("is_blueprint_copy")]
        public bool IsBlueprintCopy { get; set; }

        /// <summary>
        /// Whether the item is assembled. Packaged items stack; assembled ones (ships, containers)
        /// are singletons and can hold other items.
        /// </summary>
        [JsonProperty("is_singleton")]
        public bool IsSingleton { get; set; }

        /// <summary>
        /// Unique ID of this item (or stack). Items inside it have this as their
        /// <see cref="LocationId"/>.
        /// </summary>
        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        /// <summary>
        /// Where within its location the item sits: a hangar, a fitting slot (<c>HiSlot0</c>),
        /// cargo, a drone bay, and so on.
        /// </summary>
        [JsonProperty("location_flag")]
        public string LocationFlag { get; set; }

        /// <summary>
        /// ID of what the item is in: a station, a structure, a solar system (in space), or another
        /// item such as a ship or container. <see cref="LocationType"/> says which.
        /// </summary>
        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        /// <summary>
        /// What kind of thing <see cref="LocationId"/> is: <c>station</c>, <c>solar_system</c>,
        /// <c>item</c>, or <c>other</c> (a structure is <c>item</c> or <c>other</c>).
        /// </summary>
        [JsonProperty("location_type")]
        public string LocationType { get; set; }

        /// <summary>
        /// Number of items in the stack. Always 1 for an assembled (singleton) item.
        /// </summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        /// <summary>
        /// Type ID of the item: what kind of item it is. Names and details are in the SDE's types
        /// data.
        /// </summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }
    }
}
