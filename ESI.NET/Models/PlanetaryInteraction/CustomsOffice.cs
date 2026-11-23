using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.PlanetaryInteraction
{
    /// <summary>
    /// Returned by
    /// <see cref="ESI.NET.Logic.PlanetaryInteractionLogic.CorporationCustomsOffices"/>.
    /// </summary>
    public class CustomsOffice
    {
        /// <summary>Only present if alliance access is allowed.</summary>
        [JsonProperty("alliance_tax_rate")]
        public decimal AllianceTaxRate { get; set; }

        /// <summary>
        /// Standing_level and any standing related tax rate only present when this is true.
        /// </summary>
        [JsonProperty("allow_access_with_standings")]
        public bool AllowAccessWithStandings { get; set; }

        /// <summary>Whether alliance members may use the customs office.</summary>
        [JsonProperty("allow_alliance_access")]
        public bool AllowAllianceAccess { get; set; }

        /// <summary>
        /// Tax rate for pilots with bad standing, as a fraction. Absent when they may not use it.
        /// </summary>
        [JsonProperty("bad_standing_tax_rate")]
        public decimal BadStandingTaxRate { get; set; }

        /// <summary>Tax rate for the owner's corporation members, as a fraction.</summary>
        [JsonProperty("corporation_tax_rate")]
        public decimal CorporationTaxRate { get; set; }

        /// <summary>
        /// Tax rate for entities with excellent level of standing, only present if this level is
        /// allowed, same for all other standing related tax rates.
        /// </summary>
        [JsonProperty("excellent_standing_tax_rate")]
        public decimal ExcellentStandingTaxRate { get; set; }

        /// <summary>
        /// Tax rate for pilots with good standing, as a fraction. Absent when they may not use it.
        /// </summary>
        [JsonProperty("good_standing_tax_rate")]
        public decimal GoodStandingTaxRate { get; set; }

        /// <summary>
        /// Tax rate for pilots with neutral standing, as a fraction. Absent when they may not use
        /// it.
        /// </summary>
        [JsonProperty("neutral_standing_tax_rate")]
        public decimal NeutralStandingTaxRate { get; set; }

        /// <summary>Unique ID of this customs office.</summary>
        [JsonProperty("office_id")]
        public long Id { get; set; }

        /// <summary>Hour of the day (0-23, EVE time) the reinforcement exit window ends.</summary>
        [JsonProperty("reinforce_exit_end")]
        public long ReinforceExitEnd { get; set; }

        /// <summary>
        /// Together with reinforce_exit_end, marks a 2-hour or 6-hour (depending on the office
        /// type) period where this customs office could exit reinforcement mode during the day
        /// after initial attack.
        /// </summary>
        [JsonProperty("reinforce_exit_start")]
        public long ReinforceExitStart { get; set; }

        /// <summary>
        /// Access is allowed only for entities with this level of standing or better. One of
        /// <c>bad</c>, <c>excellent</c>, <c>good</c>, <c>neutral</c>, <c>terrible</c>.
        /// </summary>
        [JsonProperty("standing_level")]
        public string StandingLevel { get; set; }

        /// <summary>ID of the solar system this customs office is located in.</summary>
        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        /// <summary>
        /// Tax rate for pilots with terrible standing, as a fraction. Absent when they may not use
        /// it.
        /// </summary>
        [JsonProperty("terrible_standing_tax_rate")]
        public decimal TerribleDtandingRate { get; set; }

        /// <summary>ID of the type of this customs office.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }
    }
}
