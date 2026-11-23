using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.SSO;
using Xunit;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Tests
{
    /// <summary>
    /// The pre-flight scope check (a <c>403</c> with <see cref="EsiResponse{T}.MissingScope"/>, unsent), the generated route table
    /// behind it, and the corporation-role hint on a 403.
    /// </summary>
    public class ScopeCheckTests
    {
        private const string WalletRoute = "/characters/{character_id}/wallet/";
        private const string WalletScope = "esi-wallet.read_character_wallet.v1";

        private sealed class CountingHandler : HttpMessageHandler
        {
            private readonly HttpStatusCode _status;
            private readonly string _body;

            public CountingHandler(HttpStatusCode status = HttpStatusCode.OK, string body = "1")
            {
                _status = status;
                _body = body;
            }

            public int Calls;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Interlocked.Increment(ref Calls);
                return Task.FromResult(new HttpResponseMessage(_status) { Content = new StringContent(_body) });
            }
        }

        private static EsiConfig Config(bool validate = true) => new EsiConfig
        {
            EsiUrl = "https://esi.evetech.net/",
            ValidateScopes = validate,
        };

        private static EsiCallOptions As(string scopes) => new EsiCallOptions
        {
            Character = new AuthorizedCharacterData { Token = "t", CharacterID = 42, Scopes = scopes },
        };

        private static Task<EsiResponse<decimal>> Wallet(HttpMessageHandler handler, EsiConfig config, EsiCallOptions options)
            => Execute<decimal>(new HttpClient(handler), config, RequestSecurity.Authenticated, HttpMethod.Get, WalletRoute,
                options, replacements: new Dictionary<string, string> { { "character_id", "42" } });

        [Fact]
        public async Task A_missing_scope_is_a_403_that_is_never_sent()
        {
            var handler = new CountingHandler();

            var r = await Wallet(handler, Config(), As("esi-skills.read_skills.v1 esi-location.read_location.v1"));

            Assert.Equal(0, handler.Calls); // ESI never saw it, so no error-limit cost
            Assert.Equal(HttpStatusCode.Forbidden, r.StatusCode);
            Assert.Equal(WalletScope, r.MissingScope);
            Assert.Equal(WalletRoute, r.Endpoint);
            Assert.Equal(0m, r.Data);
            Assert.Contains(WalletScope, r.Message, StringComparison.Ordinal);
            Assert.Contains("esi-skills.read_skills.v1 esi-location.read_location.v1", r.Message, StringComparison.Ordinal);
            Assert.Contains("character 42", r.Message, StringComparison.Ordinal);
        }

        [Fact]
        public async Task A_granted_scope_goes_through()
        {
            var handler = new CountingHandler();

            var r = await Wallet(handler, Config(), As("esi-skills.read_skills.v1 " + WalletScope));

            Assert.Equal(1, handler.Calls);
            Assert.Equal(1m, r.Data);
            Assert.Null(r.MissingScope);
        }

        [Fact]
        public async Task Scope_matching_is_exact_not_a_prefix()
        {
            // "esi-wallet.read_character_wallet.v1" must not be satisfied by a longer or shorter name.
            var r = await Wallet(new CountingHandler(), Config(), As("esi-wallet.read_character_wallet.v12 esi-wallet.read_character_wallet"));

            Assert.Equal(WalletScope, r.MissingScope);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task A_character_without_recorded_scopes_is_not_checked(string scopes)
        {
            // Nothing to check against (a character built by hand, or stored without its scopes).
            var handler = new CountingHandler();

            await Wallet(handler, Config(), As(scopes));

            Assert.Equal(1, handler.Calls);
        }

        [Fact]
        public async Task The_check_can_be_turned_off()
        {
            var handler = new CountingHandler();

            await Wallet(handler, Config(validate: false), As("esi-skills.read_skills.v1"));

            Assert.Equal(1, handler.Calls);
        }

        [Fact]
        public async Task Public_calls_are_never_checked()
        {
            var handler = new CountingHandler(body: "{}");

            await Execute<object>(new HttpClient(handler), Config(), RequestSecurity.Public, HttpMethod.Get, "/status/", As("nothing"));

            Assert.Equal(1, handler.Calls);
        }

        [Theory]
        [InlineData("GET", "/characters/{character_id}/wallet/")]
        [InlineData("GET", "/characters/{character_id}/wallet")]
        [InlineData("get", "characters/{character_id}/wallet/")]
        public void The_table_finds_a_route_however_it_is_spelled(string method, string route)
        {
            var requirement = EsiEndpointRequirements.Find(method, route);

            Assert.NotNull(requirement);
            Assert.Equal(WalletScope, requirement.Scope);
            Assert.Empty(requirement.Roles);
        }

        [Fact]
        public void The_table_carries_corporation_roles()
        {
            var requirement = EsiEndpointRequirements.Find("GET", "/corporation/{corporation_id}/mining/extractions/");

            Assert.NotNull(requirement);
            Assert.Equal("esi-industry.read_corporation_mining.v1", requirement.Scope);
            Assert.Contains("Station_Manager", requirement.Roles);
        }

        [Fact]
        public void Public_routes_are_not_in_the_table()
        {
            Assert.Null(EsiEndpointRequirements.Find("GET", "/status/"));
            Assert.Null(EsiEndpointRequirements.Find("GET", "/markets/prices/"));
        }

        [Fact]
        public async Task A_403_on_a_role_route_names_the_roles()
        {
            var r = await EsiResponse<object>.CreateAsync(
                new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = new StringContent("{\"error\":\"Character does not have required role(s)\"}") },
                "GET|/corporation/{corporation_id}/mining/extractions/");

            Assert.StartsWith("Character does not have required role(s)", r.Message, StringComparison.Ordinal);
            Assert.Contains("Station_Manager", r.Message, StringComparison.Ordinal);
        }

        [Fact]
        public async Task A_null_content_does_not_throw()
        {
            // .NET Framework can hand back a response with no Content (a 204 from a DELETE).
            var r = await EsiResponse<object>.CreateAsync(
                new HttpResponseMessage(HttpStatusCode.NoContent) { Content = null }, "DELETE|/characters/{character_id}/contacts/");

            Assert.Equal(HttpStatusCode.NoContent, r.StatusCode);
            Assert.Null(r.Expires);
        }

        [Fact]
        public async Task A_403_elsewhere_is_left_as_ESI_wrote_it()
        {
            var r = await EsiResponse<object>.CreateAsync(
                new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = new StringContent("{\"error\":\"token is expired\"}") },
                "GET|" + WalletRoute);

            Assert.Equal("token is expired", r.Message);
        }
    }
}
