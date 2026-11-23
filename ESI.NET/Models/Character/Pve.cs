// Obsolete and undocumented on purpose: removed with the next compatibility-date release.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using Newtonsoft.Json;

namespace ESI.NET.Models.Character
{
    /// <summary>
    /// Part of ESI's removed character statistics endpoint (<c>/characters/{character_id}/stats/</c>).
    /// No ESI.NET method returns it.
    /// </summary>
    [Obsolete("No ESI endpoint returns this: CCP removed the character statistics endpoint. Will be removed with the next compatibility-date release.")]
    public class Pve
    {
        [JsonProperty("dungeons_completed_agent")]
        public long DungeonsCompletedAgent { get; set; }

        [JsonProperty("dungeons_completed_distribution")]
        public long DungeonsCompletedDistribution { get; set; }

        [JsonProperty("missions_succeeded")]
        public long MissionsSucceeded { get; set; }

        [JsonProperty("missions_succeeded_epic_arc")]
        public long MissionsSucceededEpicArc { get; set; }
    }
}
