using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.PlanetaryInteraction
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.PlanetaryInteractionLogic.ColonyLayout"/>.
    /// </summary>
    public class ColonyLayout
    {
        /// <summary>The links between the colony's pins.</summary>
        [JsonProperty("links")]
        public List<Link> Links { get; set; } = new List<Link>();

        /// <summary>
        /// The colony's pins: its command center, extractors, factories, storage and launchpads.
        /// </summary>
        [JsonProperty("pins")]
        public List<Pin> Pins { get; set; } = new List<Pin>();

        /// <summary>The routes that move commodities between pins.</summary>
        [JsonProperty("routes")]
        public List<Route> Routes { get; set; } = new List<Route>();
    }

    #region Level 1 nested classes
    /// <summary>
    /// Part of <see cref="ESI.NET.Models.PlanetaryInteraction.ColonyLayout.Links"/>.
    /// </summary>
    public class Link
    {
        /// <summary>ID of the pin the link or route ends at.</summary>
        [JsonProperty("destination_pin_id")]
        public long DestinationPinId { get; set; }

        /// <summary>Upgrade level of the link, which sets how much it can carry.</summary>
        [JsonProperty("link_level")]
        public long LinkLevel { get; set; }

        /// <summary>ID of the pin the link or route starts at.</summary>
        [JsonProperty("source_pin_id")]
        public long SourcePinId { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.PlanetaryInteraction.ColonyLayout.Pins"/>.
    /// </summary>
    public class Pin
    {
        /// <summary>Commodities stored in the pin.</summary>
        [JsonProperty("contents")]
        public List<Content> Contents { get; set; } = new List<Content>();

        /// <summary>When an extractor's program ends.</summary>
        [JsonProperty("expiry_time")]
        public DateTime ExpirationTime { get; set; }

        /// <summary>Extractor settings, for an extractor pin.</summary>
        [JsonProperty("extractor_details")]
        public Extractor Extractor { get; set; }

        /// <summary>Factory settings, for a factory pin.</summary>
        [JsonProperty("factory_details")]
        public Factory Factory { get; set; }

        /// <summary>When the pin was installed or its program started.</summary>
        [JsonProperty("install_time")]
        public DateTime InstallTime { get; set; }

        /// <summary>When the pin's current cycle started.</summary>
        [JsonProperty("last_cycle_start")]
        public string LastCycleStart { get; set; }

        /// <summary>Latitude of the pin on the planet's surface, in radians.</summary>
        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        /// <summary>Longitude of the pin on the planet's surface, in radians.</summary>
        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }

        /// <summary>ID of the pin.</summary>
        [JsonProperty("pin_id")]
        public long PinId { get; set; }

        /// <summary>
        /// ID of the schematic (the recipe) the factory runs. Details are in the SDE.
        /// </summary>
        [JsonProperty("schematic_id")]
        public long SchematicId { get; set; }

        /// <summary>Type ID of the pin's structure.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.PlanetaryInteraction.ColonyLayout.Routes"/>.
    /// </summary>
    public class Route
    {
        /// <summary>Type ID of the commodity the route moves.</summary>
        [JsonProperty("content_type_id")]
        public long ContentTypeId { get; set; }

        /// <summary>ID of the pin the link or route ends at.</summary>
        [JsonProperty("destination_pin_id")]
        public long DestinationPinId { get; set; }

        /// <summary>Units moved per cycle.</summary>
        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        /// <summary>ID of the route.</summary>
        [JsonProperty("route_id")]
        public long RouteId { get; set; }

        /// <summary>ID of the pin the link or route starts at.</summary>
        [JsonProperty("source_pin_id")]
        public long SourcePinId { get; set; }

        /// <summary>List of pin ID waypoints.</summary>
        [JsonProperty("waypoints")]
        public long[] Waypoints { get; set; }
    }
    #endregion

    #region Level 2 nested classes
    /// <summary>
    /// Part of <see cref="ESI.NET.Models.PlanetaryInteraction.Pin.Contents"/>.
    /// </summary>
    public class Content
    {
        /// <summary>How many units of the commodity are stored.</summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>Type ID of the commodity.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.PlanetaryInteraction.Pin.Extractor"/>.
    /// </summary>
    public class Extractor
    {
        /// <summary>In seconds.</summary>
        [JsonProperty("cycle_time")]
        public long CycleTime { get; set; }

        /// <summary>Radius of the extractor heads' area.</summary>
        [JsonProperty("head_radius")]
        public decimal HeadRadius { get; set; }

        /// <summary>The extractor's heads.</summary>
        [JsonProperty("heads")]
        public List<Head> Heads { get; set; } = new List<Head>();

        /// <summary>Type ID of the raw material being extracted.</summary>
        [JsonProperty("product_type_id")]
        public long ProductTypeId { get; set; }

        /// <summary>Units extracted per cycle.</summary>
        [JsonProperty("qty_per_cycle")]
        public long QuantityPerCycle { get; set; }
    }
    #endregion

    #region Level 3 nested classes
    /// <summary>
    /// Part of <see cref="ESI.NET.Models.PlanetaryInteraction.Pin.Factory"/>.
    /// </summary>
    public class Factory
    {
        /// <summary>ID of the schematic the factory runs. Details are in the SDE.</summary>
        [JsonProperty("schematic_id")]
        public long SchematicId { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.PlanetaryInteraction.Extractor.Heads"/>.
    /// </summary>
    public class Head
    {
        /// <summary>ID of the extractor head.</summary>
        [JsonProperty("head_id")]
        public long Id { get; set; }

        /// <summary>Latitude of the pin on the planet's surface, in radians.</summary>
        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        /// <summary>Longitude of the pin on the planet's surface, in radians.</summary>
        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }
    }
    #endregion
}
