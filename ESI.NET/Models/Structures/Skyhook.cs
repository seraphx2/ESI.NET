using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Structures
{
    /// <summary>A start/end time window (reinforcement, theft, vulnerability). <see cref="Start"/> is unset where the spec only carries an end.</summary>
    public class StructureWindow
    {
        [JsonProperty("start")]
        public DateTime? Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }
    }

    public class SkyhookList
    {
        [JsonProperty("skyhooks")]
        public List<SkyhookSummary> Skyhooks { get; set; } = new List<SkyhookSummary>();
    }

    public class SkyhookSummary
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }
    }

    public class Skyhook
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("effective_workforce")]
        public long? EffectiveWorkforce { get; set; }

        /// <summary>Unspecified | ShieldVulnerable | ArmorReinforced | ArmorVulnerable | HullReinforced | HullVulnerable</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("reagents")]
        public List<SkyhookReagent> Reagents { get; set; } = new List<SkyhookReagent>();

        [JsonProperty("reinforcement_timer")]
        public StructureWindow ReinforcementTimer { get; set; }

        [JsonProperty("theft_vulnerability")]
        public StructureWindow TheftVulnerability { get; set; }
    }

    public class SkyhookReagent
    {
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        [JsonProperty("last_cycle")]
        public DateTime LastCycle { get; set; }

        [JsonProperty("secured_stock")]
        public long SecuredStock { get; set; }

        [JsonProperty("unsecured_stock")]
        public long UnsecuredStock { get; set; }
    }

    public class RaidableSkyhookList
    {
        [JsonProperty("skyhooks")]
        public List<RaidableSkyhook> Skyhooks { get; set; } = new List<RaidableSkyhook>();
    }

    public class RaidableSkyhook
    {
        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }

        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        [JsonProperty("theft_vulnerability")]
        public StructureWindow TheftVulnerability { get; set; }
    }

    public class SovereigntyHubList
    {
        [JsonProperty("sovereignty_hubs")]
        public List<SovereigntyHubSummary> SovereigntyHubs { get; set; } = new List<SovereigntyHubSummary>();
    }

    public class SovereigntyHubSummary
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }
    }

    public class SovereigntyHub
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        [JsonProperty("fuel_access_list_id")]
        public long? FuelAccessListId { get; set; }

        [JsonProperty("reagent_bay")]
        public SovereigntyHubReagentBay ReagentBay { get; set; }

        [JsonProperty("resources")]
        public SovereigntyHubResources Resources { get; set; }

        [JsonProperty("upgrades")]
        public List<SovereigntyHubUpgrade> Upgrades { get; set; } = new List<SovereigntyHubUpgrade>();

        [JsonProperty("vulnerability_window")]
        public StructureWindow VulnerabilityWindow { get; set; }

        /// <summary>Workforce import/export/transit config and state - each a discriminated union, kept loose.</summary>
        [JsonProperty("workforce_transport")]
        public Dictionary<string, object> WorkforceTransport { get; set; }
    }

    public class SovereigntyHubReagentBay
    {
        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("reagents")]
        public List<SovereigntyHubReagent> Reagents { get; set; } = new List<SovereigntyHubReagent>();
    }

    public class SovereigntyHubReagent
    {
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("burning_per_hour")]
        public long BurningPerHour { get; set; }
    }

    public class SovereigntyHubResources
    {
        [JsonProperty("power")]
        public SovereigntyHubAllocation Power { get; set; }

        [JsonProperty("workforce")]
        public SovereigntyHubAllocation Workforce { get; set; }
    }

    public class SovereigntyHubAllocation
    {
        [JsonProperty("allocated")]
        public long Allocated { get; set; }

        [JsonProperty("available")]
        public long Available { get; set; }
    }

    public class SovereigntyHubUpgrade
    {
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>Unspecified | Online | Offline | Low | Pending</summary>
        [JsonProperty("power_state")]
        public string PowerState { get; set; }
    }

    public class MercenaryDenList
    {
        [JsonProperty("mercenary_dens")]
        public List<MercenaryDenSummary> MercenaryDens { get; set; } = new List<MercenaryDenSummary>();
    }

    public class MercenaryDenSummary
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }
    }

    public class MercenaryDen
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>Unspecified | Running | Paused | Disabled</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("evolution")]
        public MercenaryDenEvolution Evolution { get; set; }

        [JsonProperty("infomorphs")]
        public MercenaryDenAmount Infomorphs { get; set; }

        [JsonProperty("reinforcement_timer")]
        public StructureWindow ReinforcementTimer { get; set; }

        [JsonProperty("skyhook")]
        public MercenaryDenSkyhook Skyhook { get; set; }
    }

    public class MercenaryDenEvolution
    {
        [JsonProperty("anarchy")]
        public MercenaryDenTrack Anarchy { get; set; }

        [JsonProperty("development")]
        public MercenaryDenTrack Development { get; set; }
    }

    public class MercenaryDenTrack
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>Unspecified | Level0 | Level1 | Level2 | Level3 | Level4</summary>
        [JsonProperty("level")]
        public string Level { get; set; }
    }

    public class MercenaryDenAmount
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }
    }

    public class MercenaryDenSkyhook
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }
    }
}
