using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Structures
{
    /// <summary>A start/end time window (reinforcement, theft, vulnerability). <see cref="Start"/> is unset where the spec only carries an end.</summary>
    public class StructureWindow
    {
        /// <summary>
        /// Start time of the vulnerability window; if in the past, the Sovereignty Hub is
        /// vulnerable right now.
        /// </summary>
        [JsonProperty("start")]
        public DateTime? Start { get; set; }

        /// <summary>
        /// End time of the vulnerability window; if in the past, the Sovereignty Hub is in
        /// overtime.
        /// </summary>
        [JsonProperty("end")]
        public DateTime End { get; set; }
    }

    /// <summary>Returned by <see cref="ESI.NET.Logic.StructuresLogic.Skyhooks"/>.</summary>
    public class SkyhookList
    {
        /// <summary>List of Skyhooks.</summary>
        [JsonProperty("skyhooks")]
        public List<SkyhookSummary> Skyhooks { get; set; } = new List<SkyhookSummary>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Structures.SkyhookList.Skyhooks"/>.</summary>
    public class SkyhookSummary
    {
        /// <summary>Skyhook's ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>ID of the planet the Skyhook is anchored on.</summary>
        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }
    }

    /// <summary>Returned by <see cref="ESI.NET.Logic.StructuresLogic.Skyhook"/>.</summary>
    public class Skyhook
    {
        /// <summary>Skyhook's ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>ID of the planet the Skyhook is anchored on.</summary>
        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }

        /// <summary>Whether the Skyhook is active and producing workforce/power/reagents.</summary>
        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Skyhook's effective workforce; this can differ from the Skyhook's normal workforce due
        /// to the influence of an attached Mercenary Den.
        /// </summary>
        [JsonProperty("effective_workforce")]
        public long? EffectiveWorkforce { get; set; }

        /// <summary>
        /// Skyhook's state. One of <c>Unspecified</c>, <c>ShieldVulnerable</c>,
        /// <c>ArmorReinforced</c>, <c>ArmorVulnerable</c>, <c>HullReinforced</c>,
        /// <c>HullVulnerable</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>Skyhook's reagents.</summary>
        [JsonProperty("reagents")]
        public List<SkyhookReagent> Reagents { get; set; } = new List<SkyhookReagent>();

        /// <summary>Skyhook's reinforcement timer (if the structure is reinforced).</summary>
        [JsonProperty("reinforcement_timer")]
        public StructureWindow ReinforcementTimer { get; set; }

        /// <summary>Skyhook's theft vulnerability.</summary>
        [JsonProperty("theft_vulnerability")]
        public StructureWindow TheftVulnerability { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Structures.Skyhook.Reagents"/>.</summary>
    public class SkyhookReagent
    {
        /// <summary>
        /// Type ID of the item: what kind of item it is. Names and details are in the SDE's types
        /// data.
        /// </summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>
        /// Moment the 'SecureStock'/'UnsecuredStock' value had its last cycle; use SDE to calculate
        /// the current values.
        /// </summary>
        [JsonProperty("last_cycle")]
        public DateTime LastCycle { get; set; }

        /// <summary>Secured stock of the reagent at the time of 'last_cycle'.</summary>
        [JsonProperty("secured_stock")]
        public long SecuredStock { get; set; }

        /// <summary>Unsecured stock of the reagent at the time of 'last_cycle'.</summary>
        [JsonProperty("unsecured_stock")]
        public long UnsecuredStock { get; set; }
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.StructuresLogic.RaidableSkyhooks"/>.
    /// </summary>
    public class RaidableSkyhookList
    {
        /// <summary>List of (upcoming) raidable Skyhooks.</summary>
        [JsonProperty("skyhooks")]
        public List<RaidableSkyhook> Skyhooks { get; set; } = new List<RaidableSkyhook>();
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Structures.RaidableSkyhookList.Skyhooks"/>.
    /// </summary>
    public class RaidableSkyhook
    {
        /// <summary>ID of the planet the Skyhook is anchored on.</summary>
        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }

        /// <summary>ID of the solar system the Skyhook is anchored on.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>Skyhook's theft vulnerability.</summary>
        [JsonProperty("theft_vulnerability")]
        public StructureWindow TheftVulnerability { get; set; }
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.StructuresLogic.SovereigntyHubs"/>.
    /// </summary>
    public class SovereigntyHubList
    {
        /// <summary>List of Sovereignty Hubs.</summary>
        [JsonProperty("sovereignty_hubs")]
        public List<SovereigntyHubSummary> SovereigntyHubs { get; set; } = new List<SovereigntyHubSummary>();
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Structures.SovereigntyHubList.SovereigntyHubs"/>.
    /// </summary>
    public class SovereigntyHubSummary
    {
        /// <summary>Sovereignty Hub's ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Sovereignty Hub's location.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.StructuresLogic.SovereigntyHub"/>.
    /// </summary>
    public class SovereigntyHub
    {
        /// <summary>Sovereignty Hub's ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Sovereignty Hub's solar system ID.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>Access List with who can manage Sovereignty Hub's fuel.</summary>
        [JsonProperty("fuel_access_list_id")]
        public long? FuelAccessListId { get; set; }

        /// <summary>Sovereignty Hub's reagent bay.</summary>
        [JsonProperty("reagent_bay")]
        public SovereigntyHubReagentBay ReagentBay { get; set; }

        /// <summary>Sovereignty Hub's resources.</summary>
        [JsonProperty("resources")]
        public SovereigntyHubResources Resources { get; set; }

        /// <summary>Sovereignty Hub's installed upgrades.</summary>
        [JsonProperty("upgrades")]
        public List<SovereigntyHubUpgrade> Upgrades { get; set; } = new List<SovereigntyHubUpgrade>();

        /// <summary>
        /// Sovereignty Hub's vulnerability window; if omitted, this Sovereignty Hub is part of an
        /// active campaign.
        /// </summary>
        [JsonProperty("vulnerability_window")]
        public StructureWindow VulnerabilityWindow { get; set; }

        /// <summary>Sovereignty Hub's workforce transport settings.</summary>
        [JsonProperty("workforce_transport")]
        public Dictionary<string, object> WorkforceTransport { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Structures.SovereigntyHub.ReagentBay"/>.
    /// </summary>
    public class SovereigntyHubReagentBay
    {
        /// <summary>
        /// Moment the 'amount' value was last updated; use 'burning_per_hour' to calculate the
        /// current value.
        /// </summary>
        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        /// <summary>Sovereignty Hub's reagents.</summary>
        [JsonProperty("reagents")]
        public List<SovereigntyHubReagent> Reagents { get; set; } = new List<SovereigntyHubReagent>();
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Structures.SovereigntyHubReagentBay.Reagents"/>.
    /// </summary>
    public class SovereigntyHubReagent
    {
        /// <summary>
        /// Type ID of the item: what kind of item it is. Names and details are in the SDE's types
        /// data.
        /// </summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>Amount of reagent in the bay at the time of 'last_updated'.</summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>Amount of reagent burning per hour.</summary>
        [JsonProperty("burning_per_hour")]
        public long BurningPerHour { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Structures.SovereigntyHub.Resources"/>.
    /// </summary>
    public class SovereigntyHubResources
    {
        /// <summary>Sovereignty Hub's power.</summary>
        [JsonProperty("power")]
        public SovereigntyHubAllocation Power { get; set; }

        /// <summary>Sovereignty Hub's workforce.</summary>
        [JsonProperty("workforce")]
        public SovereigntyHubAllocation Workforce { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Structures.SovereigntyHubResources.Power"/> and
    /// <see cref="ESI.NET.Models.Structures.SovereigntyHubResources.Workforce"/>.
    /// </summary>
    public class SovereigntyHubAllocation
    {
        /// <summary>Allocated power.</summary>
        [JsonProperty("allocated")]
        public long Allocated { get; set; }

        /// <summary>Available power.</summary>
        [JsonProperty("available")]
        public long Available { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Structures.SovereigntyHub.Upgrades"/>.
    /// </summary>
    public class SovereigntyHubUpgrade
    {
        /// <summary>
        /// Type ID of the item: what kind of item it is. Names and details are in the SDE's types
        /// data.
        /// </summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>
        /// Upgrade's power state. One of <c>Unspecified</c>, <c>Online</c>, <c>Offline</c>,
        /// <c>Low</c>, <c>Pending</c>.
        /// </summary>
        [JsonProperty("power_state")]
        public string PowerState { get; set; }
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.StructuresLogic.MercenaryDens"/>.
    /// </summary>
    public class MercenaryDenList
    {
        /// <summary>List of Mercenary Dens.</summary>
        [JsonProperty("mercenary_dens")]
        public List<MercenaryDenSummary> MercenaryDens { get; set; } = new List<MercenaryDenSummary>();
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Structures.MercenaryDenList.MercenaryDens"/>.
    /// </summary>
    public class MercenaryDenSummary
    {
        /// <summary>Mercenary Den's ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// ID of the planet the Skyhook (to which the Mercenary Den is attached) is anchored on.
        /// </summary>
        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }
    }

    /// <summary>Returned by <see cref="ESI.NET.Logic.StructuresLogic.MercenaryDen"/>.</summary>
    public class MercenaryDen
    {
        /// <summary>Mercenary Den's ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Type ID of the item: what kind of item it is. Names and details are in the SDE's types
        /// data.
        /// </summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>
        /// Mercenary Den's state. One of <c>Unspecified</c>, <c>Running</c>, <c>Paused</c>,
        /// <c>Disabled</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>Mercenary Den's evolution.</summary>
        [JsonProperty("evolution")]
        public MercenaryDenEvolution Evolution { get; set; }

        /// <summary>Mercenary Den's infomorphs.</summary>
        [JsonProperty("infomorphs")]
        public MercenaryDenAmount Infomorphs { get; set; }

        /// <summary>Mercenary Den's reinforcement timer (if the structure is reinforced).</summary>
        [JsonProperty("reinforcement_timer")]
        public StructureWindow ReinforcementTimer { get; set; }

        /// <summary>Skyhook the Mercenary Den is attached to.</summary>
        [JsonProperty("skyhook")]
        public MercenaryDenSkyhook Skyhook { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Structures.MercenaryDen.Evolution"/>.
    /// </summary>
    public class MercenaryDenEvolution
    {
        /// <summary>Mercenary Den's anarchy.</summary>
        [JsonProperty("anarchy")]
        public MercenaryDenTrack Anarchy { get; set; }

        /// <summary>Mercenary Den's development.</summary>
        [JsonProperty("development")]
        public MercenaryDenTrack Development { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Structures.MercenaryDenEvolution.Anarchy"/> and
    /// <see cref="ESI.NET.Models.Structures.MercenaryDenEvolution.Development"/>.
    /// </summary>
    public class MercenaryDenTrack
    {
        /// <summary>Anarchy's cumulative amount (0-100).</summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Anarchy's level. One of <c>Unspecified</c>, <c>Level0</c>, <c>Level1</c>, <c>Level2</c>,
        /// <c>Level3</c>, <c>Level4</c>.
        /// </summary>
        [JsonProperty("level")]
        public string Level { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Structures.MercenaryDen.Infomorphs"/>.
    /// </summary>
    public class MercenaryDenAmount
    {
        /// <summary>Amount of infomorphs.</summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Structures.MercenaryDen.Skyhook"/>.</summary>
    public class MercenaryDenSkyhook
    {
        /// <summary>Skyhook's ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Corporation that owns the Skyhook.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>ID of the planet the Skyhook is anchored on.</summary>
        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }
    }
}
