using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.UniverseLogic.Jumps"/>.</summary>
    public class Jumps
    {
        /// <summary>ID of the solar system.</summary>
        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        /// <summary>Ship jumps into the system in the last hour.</summary>
        [JsonProperty("ship_jumps")]
        public long ShipJumps { get; set; }
    }
}
