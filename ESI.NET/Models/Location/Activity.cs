using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Location
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.LocationLogic.Online"/>.</summary>
    public class Activity
    {
        /// <summary>Whether the character is online.</summary>
        [JsonProperty("online")]
        public bool Online { get; set; }

        /// <summary>When the character last logged in.</summary>
        [JsonProperty("last_login")]
        public DateTime LastLogin { get; set; }

        /// <summary>When the character last logged out.</summary>
        [JsonProperty("last_logout")]
        public DateTime LastLogout { get; set; }

        /// <summary>Total logins (all-time).</summary>
        [JsonProperty("logins")]
        public long Logins { get; set; }
    }
}
