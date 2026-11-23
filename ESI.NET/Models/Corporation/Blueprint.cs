using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CorporationLogic.Blueprints"/>.</summary>
    public class Blueprint
    {
        /// <summary>Unique ID for this item.</summary>
        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        /// <summary>Type of the location_id.</summary>
        [JsonProperty("location_flag")]
        public string LocationFlag { get; set; }

        /// <summary>
        /// References a station, a ship or an item_id if this blueprint is located within a
        /// container.
        /// </summary>
        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        /// <summary>Material Efficiency Level of the blueprint.</summary>
        [JsonProperty("material_efficiency")]
        public long MaterialEfficiency { get; set; }

        /// <summary>
        /// A range of numbers with a minimum of -2 and no maximum value where -1 is an original and
        /// -2 is a copy. It can be a positive integer if it is a stack of blueprint originals fresh
        /// from the market (e.g. no activities performed on them yet).
        /// </summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        /// <summary>
        /// Number of runs remaining if the blueprint is a copy, -1 if it is an original.
        /// </summary>
        [JsonProperty("runs")]
        public long Runs { get; set; }

        /// <summary>Time Efficiency Level of the blueprint.</summary>
        [JsonProperty("time_efficiency")]
        public long TimeEfficiency { get; set; }

        /// <summary>Type ID of the blueprint.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }
    }
}
