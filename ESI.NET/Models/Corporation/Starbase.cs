using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CorporationLogic.Starbases"/>.</summary>
    public class Starbase
    {
        /// <summary>
        /// The moon this starbase (POS) is anchored on, unanchored POSes do not have this
        /// information.
        /// </summary>
        [JsonProperty("moon_id")]
        public long MoonId { get; set; }

        /// <summary>When the POS onlined, for starbases (POSes) in online state.</summary>
        [JsonProperty("onlined_since")]
        public DateTime OnlinedSince { get; set; }

        /// <summary>
        /// When the POS will be out of reinforcement, for starbases (POSes) in reinforced state.
        /// </summary>
        [JsonProperty("reinforced_until")]
        public DateTime ReinforcedUntil { get; set; }

        /// <summary>Unique ID for this starbase (POS).</summary>
        [JsonProperty("starbase_id")]
        public long StarbaseId { get; set; }

        /// <summary>
        /// The starbase's state: <c>offline</c>, <c>online</c>, <c>onlining</c>, <c>reinforced</c>,
        /// or <c>unanchoring</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// The solar system this starbase (POS) is in, unanchored POSes have this information.
        /// </summary>
        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        /// <summary>
        /// Type ID of the item: what kind of item it is. Names and details are in the SDE's types
        /// data.
        /// </summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>
        /// When the POS started unanchoring, for starbases (POSes) in unanchoring state.
        /// </summary>
        [JsonProperty("unanchor_at")]
        public DateTime UnanchorAt { get; set; }
    }
}
