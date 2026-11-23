using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Status
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.StatusLogic.Retrieve"/>.</summary>
    public class Status
    {
        /// <summary>Number of characters currently logged in.</summary>
        [JsonProperty("players")]
        public long Players { get; set; }

        /// <summary>Build number of the cluster.</summary>
        [JsonProperty("server_version")]
        public string ServerVersion { get; set; }

        /// <summary>Moment the cluster started accepting connections.</summary>
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>Whether the cluster only accepts VIP logins.</summary>
        [JsonProperty("vip")]
        public bool VIP { get; set; }
    }
}
