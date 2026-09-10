using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Models.Universe;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ESI.NET.IntegrationTests
{
    /// <summary>Environment-sourced configuration for the live run.</summary>
    internal static class LiveConfig
    {
        public static string UserAgent =>
            Env("ESI_USER_AGENT") ?? "ESI.NET integration tests (github.com/seraphx2/ESI.NET)";
        public static string ClientId => Env("ESI_CLIENT_ID");
        public static string SecretKey => Env("ESI_SECRET_KEY");
        public static string RefreshToken => Env("ESI_REFRESH_TOKEN");

        public static DataSource DataSource =>
            Enum.TryParse<DataSource>(Env("ESI_DATASOURCE"), ignoreCase: true, out var ds) ? ds : DataSource.Tranquility;

        /// <summary>The auth probe needs client credentials to exchange the token, and the token.</summary>
        public static bool HasAuthConfig =>
            !string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(SecretKey) && !string.IsNullOrEmpty(RefreshToken);

        private static string Env(string key) => Environment.GetEnvironmentVariable(key)?.Trim();
    }

    /// <summary>
    /// Builds a real <see cref="IEsiClient"/> (the full handler pipeline) and pins a
    /// handful of entity ids <em>by name</em> so tests never hard-code ids that could drift.
    /// </summary>
    public sealed class LiveFixture : IAsyncLifetime
    {
        public IEsiClient Client { get; private set; }

        public long JitaSystemId { get; private set; }
        public long AmarrSystemId { get; private set; }
        public long TritaniumTypeId { get; private set; }
        public long TheForgeRegionId { get; private set; }
        public long LpCorporationId { get; private set; } // Federal Navy Academy - has an LP store
        public long AllianceId { get; private set; }

        public async Task InitializeAsync()
        {
            var services = new ServiceCollection();
            services.AddEsi(c =>
            {
                c.EsiUrl = "https://esi.evetech.net/";
                c.DataSource = LiveConfig.DataSource;
                c.UserAgent = LiveConfig.UserAgent;
                c.ClientId = LiveConfig.ClientId;
                c.SecretKey = LiveConfig.SecretKey;
            });
            Client = services.BuildServiceProvider().GetRequiredService<IEsiClient>();

            var status = await Call(() => Client.Status.Retrieve());
            Assert.Equal(HttpStatusCode.OK, status.StatusCode); // ESI is up - fail the whole run fast if not

            var ids = await Call(() => Client.Universe.IDs(new List<string>
            {
                "Jita", "Amarr", "Tritanium", "The Forge", "Federal Navy Academy", "Pandemic Horde"
            }));
            Assert.Equal(HttpStatusCode.OK, ids.StatusCode);

            JitaSystemId = Resolve(ids.Data.Systems, "Jita");
            AmarrSystemId = Resolve(ids.Data.Systems, "Amarr");
            TritaniumTypeId = Resolve(ids.Data.InventoryTypes, "Tritanium");
            TheForgeRegionId = Resolve(ids.Data.Regions, "The Forge");
            LpCorporationId = Resolve(ids.Data.Corporations, "Federal Navy Academy");
            AllianceId = Resolve(ids.Data.Alliances, "Pandemic Horde");
        }

        public Task DisposeAsync() => Task.CompletedTask;

        private static long Resolve(List<ResolvedInfo> list, string name)
        {
            var hit = list?.Find(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            Assert.True(hit != null, $"/universe/ids did not resolve \"{name}\"");
            return hit.Id;
        }

        /// <summary>
        /// Retries an ESI call up to three times on a transport exception or a 5xx, so a
        /// single blip does not fail the run.
        /// </summary>
        public static async Task<EsiResponse<T>> Call<T>(Func<Task<EsiResponse<T>>> call)
        {
            EsiResponse<T> response = null;
            for (var attempt = 1; attempt <= 3; attempt++)
            {
                response = await call();
                if (response.Exception is null && (int)response.StatusCode < 500)
                    return response;
                await Task.Delay(attempt * 750);
            }
            return response;
        }
    }

    /// <summary>Asserts an <see cref="EsiResponse{T}"/> came back <c>200 OK</c> with a body and no exception.</summary>
    internal static class Ok
    {
        public static EsiResponse<T> Response<T>(EsiResponse<T> r)
        {
            Assert.Null(r.Exception);
            Assert.Equal(HttpStatusCode.OK, r.StatusCode);
            Assert.NotNull(r.Data);
            return r;
        }

        public static void NonEmpty<T>(EsiResponse<List<T>> r)
        {
            Response(r);
            Assert.NotEmpty(r.Data);
        }

        public static void NonEmpty<T>(EsiResponse<T[]> r)
        {
            Response(r);
            Assert.NotEmpty(r.Data);
        }
    }

    [CollectionDefinition("live")]
    public sealed class LiveCollection : ICollectionFixture<LiveFixture> { }
}
