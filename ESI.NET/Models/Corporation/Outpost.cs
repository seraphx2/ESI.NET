// Obsolete and undocumented on purpose: removed with the next compatibility-date release.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESI.NET.Models.Corporation
{
    /// <summary>
    /// A player-owned outpost. CCP removed outposts from EVE in 2018 and no ESI.NET method returns
    /// this.
    /// </summary>
    [Obsolete("No ESI endpoint returns this: CCP removed outposts in 2018. Will be removed with the next compatibility-date release.")]
    public class Outpost
    {
        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        [JsonProperty("docking_cost_per_ship_volume")]
        public decimal DockingCostPerShipVolume { get; set; }

        [JsonProperty("office_rental_cost")]
        public long OfficeRentalCost { get; set; }

        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        [JsonProperty("reprocessing_efficiency")]
        public decimal ReprocessingEfficiency { get; set; }

        [JsonProperty("reprocessing_station_take")]
        public decimal ReprocessingStationTake { get; set; }

        [JsonProperty("standing_owner_id")]
        public long StandingOwnerId { get; set; }

        [JsonProperty("coordinates")]
        public Position Coordinates { get; set; }

        [JsonProperty("services")]
        public List<OutpostService> Services { get; set; } = new List<OutpostService>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Corporation.Outpost.Services"/>.</summary>
    /// <summary>A service of an <see cref="Outpost"/>. CCP removed outposts from EVE in 2018.</summary>
    [Obsolete("No ESI endpoint returns this: CCP removed outposts in 2018. Will be removed with the next compatibility-date release.")]
    public class OutpostService
    {
        [JsonProperty("service_name")]
        public string ServiceName { get; set; }

        [JsonProperty("minimum_standing")]
        public decimal MinimumStanding { get; set; }

        [JsonProperty("surcharge_per_bad_standing")]
        public decimal SurchargePerBadStanding { get; set; }

        [JsonProperty("discount_per_good_standing")]
        public decimal DiscountPerGoodStanding { get; set; }
    }
}
