using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Sovereignty
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.SovereigntyLogic.Campaigns"/>.</summary>
    public class Campaign
    {
        /// <summary>Score for all attacking parties, only present in Defense Events.</summary>
        [JsonProperty("attackers_score")]
        public double AttackersScore { get; set; }

        /// <summary>Unique ID for this campaign.</summary>
        [JsonProperty("campaign_id")]
        public long CampaignId { get; set; }

        /// <summary>The constellation in which the campaign will take place.</summary>
        [JsonProperty("constellation_id")]
        public long ConstellationId { get; set; }

        /// <summary>Defending alliance, only present in Defense Events.</summary>
        [JsonProperty("defender_id")]
        public long DefenderId { get; set; }

        /// <summary>Score for the defending alliance, only present in Defense Events.</summary>
        [JsonProperty("defender_score")]
        public double DefenderScore { get; set; }

        /// <summary>
        /// Type of event this campaign is for. tcu_defense, ihub_defense and station_defense are
        /// referred to as "Defense Events", station_freeport as "Freeport Events". One of
        /// <c>tcu_defense</c>, <c>ihub_defense</c>, <c>station_defense</c>,
        /// <c>station_freeport</c>.
        /// </summary>
        [JsonProperty("event_type")]
        public string EventType { get; set; }

        /// <summary>
        /// Alliance participating and their respective scores, only present in Freeport Events.
        /// </summary>
        [JsonProperty("participants")]
        public List<Participants> Participants { get; set; } = new List<Participants>();

        /// <summary>The solar system the structure is located in.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>Time the event is scheduled to start.</summary>
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>The structure item ID that is related to this campaign.</summary>
        [JsonProperty("structure_id")]
        public long StructureId { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Sovereignty.Campaign.Participants"/>.
    /// </summary>
    public class Participants
    {
        /// <summary>ID of an alliance taking part in the campaign.</summary>
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        /// <summary>The alliance's score in the campaign.</summary>
        [JsonProperty("score")]
        public double Score { get; set; }
    }
}
