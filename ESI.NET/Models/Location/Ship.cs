using Newtonsoft.Json;

namespace ESI.NET.Models.Location
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.LocationLogic.Ship"/>.</summary>
    public class Ship
    {
        /// <summary>Type of the ship currently piloted.</summary>
        [JsonProperty("ship_type_id")]
        public long ShipTypeId { get; set; }

        /// <summary>Unique identifier for this ship.</summary>
        [JsonProperty("ship_item_id")]
        public long ShipItemId { get; set; }

        /// <summary>Name of this ship.</summary>
        [JsonProperty("ship_name")]
        public string ShipName { get; set; }
    }
}
