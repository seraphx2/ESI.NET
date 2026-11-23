using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESI.NET.Models.Clones
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.ClonesLogic.List"/>.</summary>
    public class Clones
    {
        /// <summary>When the character last jumped to a jump clone.</summary>
        [JsonProperty("last_clone_jump_date")]
        public DateTime LastCloneJumpDate { get; set; }

        /// <summary>
        /// The character's home station or structure, where a new clone wakes up after a pod loss.
        /// </summary>
        [JsonProperty("home_location")]
        public HomeLocation HomeLocation { get; set; }

        /// <summary>When the character last changed home station.</summary>
        [JsonProperty("last_station_change_date")]
        public DateTime LastStationChangeDate { get; set; }

        /// <summary>The character's jump clones.</summary>
        [JsonProperty("jump_clones")]
        public List<JumpClone> JumpClones { get; set; } = new List<JumpClone>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Clones.Clones.HomeLocation"/>.</summary>
    public class HomeLocation
    {
        /// <summary>ID of the home station or structure.</summary>
        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        /// <summary>
        /// Whether <see cref="LocationId"/> is a <c>station</c> or a <c>structure</c>.
        /// </summary>
        [JsonProperty("location_type")]
        public string LocationType { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Clones.Clones.JumpClones"/>.</summary>
    public class JumpClone
    {
        /// <summary>ID of the jump clone.</summary>
        [JsonProperty("jump_clone_id")]
        public long JumpCloneId { get; set; }

        /// <summary>The name the player gave the clone.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>ID of the station or structure the clone is in.</summary>
        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        /// <summary>
        /// Whether <see cref="LocationId"/> is a <c>station</c> or a <c>structure</c>.
        /// </summary>
        [JsonProperty("location_type")]
        public string LocationType { get; set; }

        /// <summary>Type IDs of the implants plugged into this clone.</summary>
        [JsonProperty("implants")]
        public long[] Implants { get; set; }
    }
}
