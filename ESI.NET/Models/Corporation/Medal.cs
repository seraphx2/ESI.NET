using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CorporationLogic.Medals"/>.</summary>
    public class Medal
    {
        /// <summary>When the medal was created.</summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>ID of the character who created this medal.</summary>
        [JsonProperty("creator_id")]
        public long CreatorId { get; set; }

        /// <summary>The medal's description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>ID of the medal.</summary>
        [JsonProperty("medal_id")]
        public long MedalId { get; set; }

        /// <summary>The medal's name.</summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
