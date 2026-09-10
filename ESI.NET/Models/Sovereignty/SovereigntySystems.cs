using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Sovereignty
{
    /// <summary>
    /// GET /sovereignty/systems - the 2026 rework that unified the old
    /// /sovereignty/map and /sovereignty/structures endpoints.
    /// </summary>
    public class SovereigntySystems
    {
        [JsonProperty("solar_systems")]
        public List<SovereigntySystem> SolarSystems { get; set; } = new List<SovereigntySystem>();
    }

    public class SovereigntySystem
    {
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        [JsonProperty("claim")]
        public SovereigntyClaim Claim { get; set; }
    }

    /// <summary>Exactly one of <see cref="Faction"/> / <see cref="Alliance"/> is set, or <see cref="Unclaimed"/> is true.</summary>
    public class SovereigntyClaim
    {
        [JsonProperty("faction")]
        public SovereigntyFactionClaim Faction { get; set; }

        [JsonProperty("alliance")]
        public SovereigntyAllianceClaim Alliance { get; set; }

        [JsonProperty("unclaimed")]
        public bool Unclaimed { get; set; }
    }

    public class SovereigntyFactionClaim
    {
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }
    }

    public class SovereigntyAllianceClaim
    {
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        [JsonProperty("claimed_since")]
        public DateTime ClaimedSince { get; set; }

        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("is_capital_system")]
        public bool IsCapitalSystem { get; set; }
    }
}
