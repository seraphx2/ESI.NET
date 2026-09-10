using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Meta
{
    /// <summary>GET /meta/changelog - keyed by the compatibility date the changes landed on.</summary>
    public class MetaChangelog
    {
        [JsonProperty("changelog")]
        public Dictionary<string, List<MetaChangelogEntry>> Changelog { get; set; } = new Dictionary<string, List<MetaChangelogEntry>>();
    }

    public class MetaChangelogEntry
    {
        /// <summary>GET | POST | PUT | DELETE</summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("compatibility_date")]
        public string CompatibilityDate { get; set; }

        /// <summary>breaking | changed | new | removed</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    /// <summary>GET /meta/compatibility-dates - the published snapshot dates, newest first.</summary>
    public class MetaCompatibilityDates
    {
        [JsonProperty("compatibility_dates")]
        public List<string> CompatibilityDates { get; set; } = new List<string>();
    }

    /// <summary>GET /meta/name - the current ESI product name and its history.</summary>
    public class MetaName
    {
        [JsonProperty("current")]
        public string Current { get; set; }

        [JsonProperty("history")]
        public List<MetaNameHistory> History { get; set; } = new List<MetaNameHistory>();
    }

    public class MetaNameHistory
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>GET /meta/status - per-route health.</summary>
    public class MetaStatus
    {
        [JsonProperty("routes")]
        public List<MetaRouteStatus> Routes { get; set; } = new List<MetaRouteStatus>();
    }

    public class MetaRouteStatus
    {
        /// <summary>GET | POST | PUT | DELETE</summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>Unknown | OK | Degraded | Down | Recovering</summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
