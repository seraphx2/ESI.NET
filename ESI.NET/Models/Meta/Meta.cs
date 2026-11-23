using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Meta
{
    /// <summary>GET /meta/changelog - keyed by the compatibility date the changes landed on.</summary>
    public class MetaChangelog
    {
        /// <summary>Per date, list changes for that date.</summary>
        [JsonProperty("changelog")]
        public Dictionary<string, List<MetaChangelogEntry>> Changelog { get; set; } = new Dictionary<string, List<MetaChangelogEntry>>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Meta.MetaChangelog.Changelog"/>.</summary>
    public class MetaChangelogEntry
    {
        /// <summary>
        /// HTTP method of the route. One of <c>GET</c>, <c>POST</c>, <c>PUT</c>, <c>DELETE</c>.
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>Path of the route.</summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>Compatibility date of the route.</summary>
        [JsonProperty("compatibility_date")]
        public string CompatibilityDate { get; set; }

        /// <summary>
        /// Type of the change. One of <c>breaking</c>, <c>changed</c>, <c>new</c>, <c>removed</c>.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>Description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }

    /// <summary>GET /meta/compatibility-dates - the published snapshot dates, newest first.</summary>
    public class MetaCompatibilityDates
    {
        /// <summary>List of compatibility dates.</summary>
        [JsonProperty("compatibility_dates")]
        public List<string> CompatibilityDates { get; set; } = new List<string>();
    }

    /// <summary>GET /meta/name - the current ESI product name and its history.</summary>
    public class MetaName
    {
        /// <summary>
        /// The name ESI is going by today. It was almost certainly something else last time you
        /// looked.
        /// </summary>
        [JsonProperty("current")]
        public string Current { get; set; }

        /// <summary>
        /// Every name ESI has ever gone by, newest first. It has never once been the same twice.
        /// </summary>
        [JsonProperty("history")]
        public List<MetaNameHistory> History { get; set; } = new List<MetaNameHistory>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Meta.MetaName.History"/>.</summary>
    public class MetaNameHistory
    {
        /// <summary>The date ESI started insisting on this name (UTC-11).</summary>
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>What those three letters allegedly stood for at the time.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>GET /meta/status - per-route health.</summary>
    public class MetaStatus
    {
        /// <summary>List of all API routes and their health status.</summary>
        [JsonProperty("routes")]
        public List<MetaRouteStatus> Routes { get; set; } = new List<MetaRouteStatus>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Meta.MetaStatus.Routes"/>.</summary>
    public class MetaRouteStatus
    {
        /// <summary>
        /// Route's HTTP method. One of <c>GET</c>, <c>POST</c>, <c>PUT</c>, <c>DELETE</c>.
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>Route's HTTP path.</summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// Route's health status. One of <c>Unknown</c>, <c>OK</c>, <c>Degraded</c>, <c>Down</c>,
        /// <c>Recovering</c>.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
