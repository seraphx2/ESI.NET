using Newtonsoft.Json;

namespace ESI.NET.Models.Incursions
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.IncursionsLogic.All"/>.</summary>
    public class Incursion
    {
        /// <summary>The constellation id in which this incursion takes place.</summary>
        [JsonProperty("constellation_id")]
        public long ConstellationId { get; set; }

        /// <summary>ID of the faction attacking (the Sansha's Nation for incursions).</summary>
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        /// <summary>Whether the final encounter has boss or not.</summary>
        [JsonProperty("has_boss")]
        public bool HasBoss { get; set; }

        /// <summary>
        /// A list of infested solar system ids that are a part of this incursion.
        /// </summary>
        [JsonProperty("infested_solar_systems")]
        public long[] InfestedSystems { get; set; }

        /// <summary>Influence of this incursion as a float from 0 to 1.</summary>
        [JsonProperty("influence")]
        public double Influence { get; set; }

        /// <summary>Staging solar system for this incursion.</summary>
        [JsonProperty("staging_solar_system_id")]
        public long StagingSystemId { get; set; }

        /// <summary>
        /// The state of this incursion. One of <c>withdrawing</c>, <c>mobilizing</c>,
        /// <c>established</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>The type of this incursion.</summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
