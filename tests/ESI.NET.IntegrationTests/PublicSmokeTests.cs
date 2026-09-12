using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESI.NET;
using Xunit;

namespace ESI.NET.IntegrationTests
{
    /// <summary>
    /// Unauthenticated GETs against live ESI. Proves the handler pipeline moves a
    /// request and that a real payload deserializes into <c>EsiResponse&lt;T&gt;</c>.
    /// A representative spread of tags rather than all 195 operations.
    /// </summary>
    [Collection("live")]
    public sealed class PublicSmokeTests
    {
        private readonly LiveFixture _f;
        private IEsiClient Esi => _f.Client;

        public PublicSmokeTests(LiveFixture fixture) => _f = fixture;

        [Fact]
        public async Task Status_reports_players_online()
        {
            var r = Ok.Response(await LiveFixture.Call(() => Esi.Status.Retrieve()));
            Assert.True(r.Data.Players > 0);
        }

        [Fact]
        public async Task Universe_reference_lists()
        {
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.Universe.Bloodlines()));
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.Universe.Races()));
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.Universe.Factions()));
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.Universe.Categories()));
        }

        [Fact]
        public async Task Universe_system_type_region_by_id()
        {
            var system = Ok.Response(await LiveFixture.Call(() => Esi.Universe.System(_f.JitaSystemId)));
            Assert.Equal("Jita", system.Data.Name);

            var type = Ok.Response(await LiveFixture.Call(() => Esi.Universe.Type(_f.TritaniumTypeId)));
            Assert.Equal("Tritanium", type.Data.Name);

            Ok.Response(await LiveFixture.Call(() => Esi.Universe.Region(_f.TheForgeRegionId)));
        }

        [Fact]
        public async Task Universe_names_round_trip()
        {
            var r = await LiveFixture.Call(() => Esi.Universe.Names(new List<long> { _f.TritaniumTypeId, _f.JitaSystemId }));
            Ok.Response(r);
            Assert.Contains(r.Data, x => x.Name == "Tritanium");
            Assert.Contains(r.Data, x => x.Name == "Jita");
        }

        [Fact]
        public async Task Dogma_attributes()
        {
            var ids = await LiveFixture.Call(() => Esi.Dogma.Attributes());
            Ok.NonEmpty(ids);

            var detail = Ok.Response(await LiveFixture.Call(() => Esi.Dogma.Attribute(ids.Data.First())));
            Assert.False(string.IsNullOrEmpty(detail.Data.Name));
        }

        [Fact]
        public async Task Market_prices_orders_history()
        {
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.Market.Prices()));
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.Market.RegionOrders(_f.TheForgeRegionId)));
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.Market.TypeHistoryInRegion(_f.TheForgeRegionId, _f.TritaniumTypeId)));
        }

        [Fact]
        public async Task Alliances_list_and_detail()
        {
            var all = await LiveFixture.Call(() => Esi.Alliance.All());
            Ok.NonEmpty(all);

            var info = Ok.Response(await LiveFixture.Call(() => Esi.Alliance.Information(_f.AllianceId)));
            Assert.False(string.IsNullOrEmpty(info.Data.Name));
        }

        [Fact]
        public async Task Incursions_ok_possibly_empty()
        {
            // No incursions is a valid state; just assert the call round-trips.
            Ok.Response(await LiveFixture.Call(() => Esi.Incursions.All()));
        }

        [Fact]
        public async Task Insurance_levels() =>
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.Insurance.Levels()));

        [Fact]
        public async Task Sovereignty_systems()
        {
            var r = Ok.Response(await LiveFixture.Call(() => Esi.Sovereignty.Systems()));
            Assert.NotEmpty(r.Data.SolarSystems);
        }

        [Fact]
        public async Task Industry_facilities_and_systems()
        {
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.Industry.Facilities()));
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.Industry.SolarSystemCostIndices()));
        }

        [Fact]
        public async Task Faction_warfare_systems_and_stats()
        {
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.FactionWarfare.Systems()));
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.FactionWarfare.Stats()));
        }

        [Fact]
        public async Task Route_between_two_hubs()
        {
            var r = Ok.Response(await LiveFixture.Call(() => Esi.Routes.Map(_f.JitaSystemId, _f.AmarrSystemId)));
            Assert.NotEmpty(r.Data.Route);
        }

        [Fact]
        public async Task Loyalty_store_offers() =>
            Ok.NonEmpty(await LiveFixture.Call(() => Esi.Loyalty.Offers(_f.LpCorporationId)));

        // ---- endpoints added in the 2026 snapshot -------------------------

        [Fact]
        public async Task Meta_compatibility_dates_lists_the_pinned_one()
        {
            var r = Ok.Response(await LiveFixture.Call(() => Esi.Meta.CompatibilityDates()));
            Assert.Contains(ESI.NET.EsiVersion.CompatibilityDate, r.Data.CompatibilityDates);
        }

        [Fact]
        public async Task Meta_status_reports_route_health()
        {
            var r = Ok.Response(await LiveFixture.Call(() => Esi.Meta.Status()));
            Assert.NotEmpty(r.Data.Routes);
        }

        [Fact]
        public async Task Military_campaigns_round_trip()
        {
            var r = Ok.Response(await LiveFixture.Call(() => Esi.MilitaryCampaigns.All()));
            Assert.NotNull(r.Data.Campaigns);
        }

        [Fact]
        public async Task Freelance_jobs_public_listing()
        {
            var r = Ok.Response(await LiveFixture.Call(() => Esi.FreelanceJobs.All()));
            Assert.NotNull(r.Data.FreelanceJobs);
        }

        [Fact]
        public async Task Raidable_skyhooks_round_trip()
        {
            var r = Ok.Response(await LiveFixture.Call(() => Esi.Structures.RaidableSkyhooks()));
            Assert.NotNull(r.Data.Skyhooks);
        }
    }
}
