using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Fleets
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.FleetsLogic.Wings"/>.</summary>
    public class Wing
    {
        /// <summary>The wing's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>ID of the wing.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The wing's squads.</summary>
        [JsonProperty("squads")]
        public List<Squad> Squads { get; set; } = new List<Squad>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Fleets.Wing.Squads"/>.</summary>
    public class Squad
    {
        /// <summary>The squad's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>ID of the squad.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }
    }
}
