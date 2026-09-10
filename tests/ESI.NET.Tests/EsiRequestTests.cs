using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Models.SSO;
using Xunit;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Tests
{
    /// <summary>
    /// Covers how <see cref="EsiRequest"/>.Execute shapes the outgoing request from an
    /// <see cref="EsiCallOptions"/> — the state that used to live on the client
    /// (SetCharacterData) and, for the ETag, in a process-wide static field.
    /// </summary>
    public class EsiRequestTests
    {
        private sealed class CapturingHandler : HttpMessageHandler
        {
            public HttpRequestMessage Last;
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Last = request;
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{}"),
                });
            }
        }

        private static readonly EsiConfig Config = new EsiConfig
        {
            EsiUrl = "https://esi.evetech.net/",
            DataSource = DataSource.Tranquility,
        };

        private static (HttpClient client, CapturingHandler handler) NewClient()
        {
            var handler = new CapturingHandler();
            return (new HttpClient(handler), handler);
        }

        private static EsiCallOptions Auth(string token = "access-token") =>
            new EsiCallOptions { Character = new AuthorizedCharacterData { Token = token, CharacterID = 42 } };

        [Fact]
        public async Task Public_call_builds_a_bare_url_and_sends_the_version_headers()
        {
            var (client, h) = NewClient();
            await Execute<object>(client, Config, RequestSecurity.Public, HttpMethod.Get, "/status/", options: new EsiCallOptions());

            Assert.Equal("https://esi.evetech.net/status/", h.Last.RequestUri.ToString());
            Assert.Equal(EsiVersion.CompatibilityDate, string.Join("", h.Last.Headers.GetValues("X-Compatibility-Date")));
            Assert.Equal("tranquility", string.Join("", h.Last.Headers.GetValues("X-Tenant")));
        }

        [Fact]
        public async Task Replacements_substitute_into_the_path()
        {
            var (client, h) = NewClient();
            await Execute<object>(client, Config, RequestSecurity.Public, HttpMethod.Get, "/x/{id}/",
                replacements: new Dictionary<string, string> { { "id", "999" } }, options: new EsiCallOptions());

            Assert.Equal("https://esi.evetech.net/x/999/", h.Last.RequestUri.ToString());
        }

        [Fact]
        public async Task Authenticated_without_a_Character_throws()
        {
            var (client, _) = NewClient();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                Execute<object>(client, Config, RequestSecurity.Authenticated, HttpMethod.Get, "/x/", options: new EsiCallOptions()));
        }

        [Fact]
        public async Task Authenticated_with_a_Character_sends_the_bearer_token()
        {
            var (client, h) = NewClient();
            await Execute<object>(client, Config, RequestSecurity.Authenticated, HttpMethod.Get, "/x/", options: Auth("tok-123"));

            Assert.Equal("Bearer", h.Last.Headers.Authorization.Scheme);
            Assert.Equal("tok-123", h.Last.Headers.Authorization.Parameter);
        }

        [Fact]
        public async Task Page_option_is_appended_as_a_query_parameter()
        {
            var (client, h) = NewClient();
            var options = new EsiCallOptions { Page = 4 };
            await Execute<object>(client, Config, RequestSecurity.Public, HttpMethod.Get, "/x/", options: options);

            Assert.Contains("page=4", h.Last.RequestUri.ToString());
        }

        [Fact]
        public async Task IfNoneMatch_is_sent_quoted_and_tolerates_existing_quotes()
        {
            var (client, h) = NewClient();
            await Execute<object>(client, Config, RequestSecurity.Public, HttpMethod.Get, "/x/",
                options: new EsiCallOptions { IfNoneMatch = "\"already-quoted\"" });

            Assert.Equal("\"already-quoted\"", h.Last.Headers.IfNoneMatch.ToString());
        }

        [Fact]
        public async Task CancellationToken_flows_through_to_the_send()
        {
            var (client, _) = NewClient();
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                Execute<object>(client, Config, RequestSecurity.Public, HttpMethod.Get, "/x/",
                    options: new EsiCallOptions { CancellationToken = cts.Token }));
        }

        [Fact]
        public async Task Null_options_is_tolerated_for_public_calls()
        {
            var (client, h) = NewClient();
            await Execute<object>(client, Config, RequestSecurity.Public, HttpMethod.Get, "/x/", options: null);

            Assert.NotNull(h.Last);
        }
    }
}
