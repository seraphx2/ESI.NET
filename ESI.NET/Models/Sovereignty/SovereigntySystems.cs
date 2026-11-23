using ESI.NET.Models.Structures;
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
        /// <summary>List of solar systems and their sovereignty owners.</summary>
        [JsonProperty("solar_systems")]
        public List<SovereigntySystem> SolarSystems { get; set; } = new List<SovereigntySystem>();
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Sovereignty.SovereigntySystems.SolarSystems"/>.
    /// </summary>
    public class SovereigntySystem
    {
        /// <summary>ID of the solar system.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>Claim on this solar system.</summary>
        [JsonProperty("claim")]
        public SovereigntyClaim Claim { get; set; }
    }

    /// <summary>Exactly one of <see cref="Faction"/> / <see cref="Alliance"/> is set, or <see cref="Unclaimed"/> is true.</summary>
    public class SovereigntyClaim
    {
        /// <summary>Solar system is claimed by a faction.</summary>
        [JsonProperty("faction")]
        public SovereigntyFactionClaim Faction { get; set; }

        /// <summary>Solar system is claimed by an alliance.</summary>
        [JsonProperty("alliance")]
        public SovereigntyAllianceClaim Alliance { get; set; }

        /// <summary>Solar system is unclaimed.</summary>
        [JsonProperty("unclaimed")]
        public bool Unclaimed { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Sovereignty.SovereigntyClaim.Faction"/>.
    /// </summary>
    public class SovereigntyFactionClaim
    {
        /// <summary>Faction that claimed this solar system.</summary>
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Sovereignty.SovereigntyClaim.Alliance"/>.
    /// </summary>
    public class SovereigntyAllianceClaim
    {
        /// <summary>Alliance that claimed this solar system.</summary>
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        /// <summary>Time the claim was made.</summary>
        [JsonProperty("claimed_since")]
        public DateTime ClaimedSince { get; set; }

        /// <summary>Corporation that claimed this solar system.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>Whether the system is the capital system of the alliance.</summary>
        [JsonProperty("is_capital_system")]
        public bool IsCapitalSystem { get; set; }

        /// <summary>Sovereignty Hub holding the claim.</summary>
        [JsonProperty("sovereignty_hub")]
        public SovereigntyClaimHub SovereigntyHub { get; set; }

        /// <summary>Solar system's development.</summary>
        [JsonProperty("development")]
        public SovereigntyDevelopment Development { get; set; }
    }

    /// <summary>The Sovereignty Hub holding an alliance claim.</summary>
    public class SovereigntyClaimHub
    {
        /// <summary>ID of the Sovereignty Hub.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Sovereignty Hub's vulnerability window; if omitted, this Sovereignty Hub is part of an
        /// active campaign.
        /// </summary>
        [JsonProperty("vulnerability_window")]
        public StructureWindow VulnerabilityWindow { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Sovereignty.SovereigntyAllianceClaim.Development"/>.
    /// </summary>
    public class SovereigntyDevelopment
    {
        /// <summary>Current Activity Defense Multiplier.</summary>
        [JsonProperty("activity_defense_multiplier")]
        public double ActivityDefenseMultiplier { get; set; }

        /// <summary>Military level (0-5) of this solar system.</summary>
        [JsonProperty("military_level")]
        public long MilitaryLevel { get; set; }

        /// <summary>Industrial level (0-5) of this solar system.</summary>
        [JsonProperty("industrial_level")]
        public long IndustrialLevel { get; set; }

        /// <summary>Strategic level (0-5) of this solar system.</summary>
        [JsonProperty("strategic_level")]
        public long StrategicLevel { get; set; }
    }
}
