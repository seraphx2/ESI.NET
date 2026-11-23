using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Fittings
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.FittingsLogic.List"/>.</summary>
    public class Fitting
    {
        /// <summary>ID of the saved fitting.</summary>
        [JsonProperty("fitting_id")]
        public long FittingId { get; set; }

        /// <summary>The fitting's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The fitting's description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Type ID of the ship the fitting is for.</summary>
        [JsonProperty("ship_type_id")]
        public long ShipTypeId { get; set; }

        /// <summary>The modules, charges, drones and cargo in the fitting.</summary>
        [JsonProperty("items")]
        public List<Item> Items { get; set; } = new List<Item>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Fittings.Fitting.Items"/>.</summary>
    public class Item
    {
        /// <summary>
        /// Type ID of the item: what kind of item it is. Names and details are in the SDE's types
        /// data.
        /// </summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>
        /// The slot or bay the item goes in, e.g. <c>HiSlot0</c>, <c>MedSlot2</c>, <c>DroneBay</c>,
        /// or <c>Cargo</c>.
        /// </summary>
        [JsonProperty("flag")]
        public string Flag { get; set; }

        /// <summary>How many of the item.</summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }
}
