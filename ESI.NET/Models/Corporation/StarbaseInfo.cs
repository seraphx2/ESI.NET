using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Corporation
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CorporationLogic.Starbase"/>.</summary>
    public class StarbaseInfo
    {
        /// <summary>Whether alliance members may use the starbase.</summary>
        [JsonProperty("allow_alliance_members")]
        public bool AllowAllianceMembers { get; set; }

        /// <summary>Whether corporation members may use the starbase.</summary>
        [JsonProperty("allow_corporation_members")]
        public bool AllowCorporationMembers { get; set; }

        /// <summary>
        /// Who can anchor starbase (POS) and its structures. One of <c>alliance_member</c>,
        /// <c>config_starbase_equipment_role</c>, <c>corporation_member</c>,
        /// <c>starbase_fuel_technician_role</c>.
        /// </summary>
        [JsonProperty("anchor")]
        public string Anchor { get; set; }

        /// <summary>Whether the starbase's defences fire on war targets.</summary>
        [JsonProperty("attack_if_at_war")]
        public bool AttackIfAtWar { get; set; }

        /// <summary>
        /// Whether the defences fire on anyone committing an act that would lower their security
        /// status.
        /// </summary>
        [JsonProperty("attack_if_other_security_status_dropping")]
        public bool AttackIfOtherSecurityStatusDropping { get; set; }

        /// <summary>
        /// Starbase (POS) will attack if target's security standing is lower than this value.
        /// </summary>
        [JsonProperty("attack_security_status_threshold")]
        public float AttackSecurityStatusThreshold { get; set; }

        /// <summary>
        /// Starbase (POS) will attack if target's standing is lower than this value.
        /// </summary>
        [JsonProperty("attack_standing_threshold")]
        public float AttackStandingThreshold { get; set; }

        /// <summary>
        /// Who can take fuel blocks out of the starbase (POS)'s fuel bay. One of
        /// <c>alliance_member</c>, <c>config_starbase_equipment_role</c>,
        /// <c>corporation_member</c>, <c>starbase_fuel_technician_role</c>.
        /// </summary>
        [JsonProperty("fuel_bay_take")]
        public string FuelBayTake { get; set; }

        /// <summary>
        /// Who can view the starbase (POS)'s fule bay. Characters either need to have required role
        /// or belong to the starbase (POS) owner's corporation or alliance, as described by the
        /// enum, all other access settings follows the same scheme. One of <c>alliance_member</c>,
        /// <c>config_starbase_equipment_role</c>, <c>corporation_member</c>,
        /// <c>starbase_fuel_technician_role</c>.
        /// </summary>
        [JsonProperty("fuel_bay_view")]
        public string FuelBayView { get; set; }

        /// <summary>
        /// Fuel blocks and other things that will be consumed when operating a starbase (POS).
        /// </summary>
        [JsonProperty("fuels")]
        public List<Fuel> Fuels { get; set; } = new List<Fuel>();

        /// <summary>
        /// Who can offline starbase (POS) and its structures. One of <c>alliance_member</c>,
        /// <c>config_starbase_equipment_role</c>, <c>corporation_member</c>,
        /// <c>starbase_fuel_technician_role</c>.
        /// </summary>
        [JsonProperty("offline")]
        public string Offline { get; set; }

        /// <summary>
        /// Who can online starbase (POS) and its structures. One of <c>alliance_member</c>,
        /// <c>config_starbase_equipment_role</c>, <c>corporation_member</c>,
        /// <c>starbase_fuel_technician_role</c>.
        /// </summary>
        [JsonProperty("online")]
        public string Online { get; set; }

        /// <summary>
        /// Who can unanchor starbase (POS) and its structures. One of <c>alliance_member</c>,
        /// <c>config_starbase_equipment_role</c>, <c>corporation_member</c>,
        /// <c>starbase_fuel_technician_role</c>.
        /// </summary>
        [JsonProperty("unanchor")]
        public string Unanchor { get; set; }

        /// <summary>
        /// True if the starbase (POS) is using alliance standings, otherwise using corporation's.
        /// </summary>
        [JsonProperty("use_alliance_standings")]
        public bool UseAllianceStandings { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Corporation.StarbaseInfo.Fuels"/>.</summary>
    public class Fuel
    {
        /// <summary>Type ID of the fuel.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>Units of this fuel in the starbase.</summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }
}
