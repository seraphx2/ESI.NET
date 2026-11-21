using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Enumerations;
using ESI.NET.Models.SSO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ESI.NET.Tests
{
    /// <summary>
    /// <c>AddEsiCaching()</c> end to end through the real DI pipeline, with a stub standing in
    /// for ESI. No network.
    /// </summary>
    public class EsiCacheTests
    {
        private const string StatusJson = @"{ ""players"": {0}, ""server_version"": ""1"", ""start_time"": ""2026-11-20T11:05:00Z"" }";

        private sealed class Esi : HttpMessageHandler
        {
            private int _calls;
            public int Calls => _calls;
            public HttpRequestMessage Last;
            public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> Respond;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Interlocked.Increment(ref _calls);
                Last = request;
                return Respond(request, cancellationToken);
            }
        }

        private static IEsiClient Client(Esi esi, Action<IServiceCollection> configure = null)
        {
            var services = new ServiceCollection();
            configure?.Invoke(services);
            services.AddEsi(c =>
                {
                    c.UserAgent = "cache-tests";
                    c.EsiUrl = "https://esi.evetech.net/";
                    c.DataSource = DataSource.Tranquility;
                })
                .AddEsiCaching()
                .ConfigurePrimaryHttpMessageHandler(() => esi);
            return services.BuildServiceProvider().GetRequiredService<IEsiClient>();
        }

        private static HttpResponseMessage Ok(string body, TimeSpan expiresIn, string etag = null)
        {
            var r = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json") };
            r.Headers.Date = DateTimeOffset.UtcNow;                         // ESI always sends Date
            r.Content.Headers.Expires = DateTimeOffset.UtcNow.Add(expiresIn);
            if (etag != null)
                r.Headers.TryAddWithoutValidation("ETag", $"\"{etag}\"");
            return r;
        }

        private static HttpResponseMessage Status(int players, TimeSpan expiresIn, string etag = null)
            => Ok(StatusJson.Replace("{0}", players.ToString(System.Globalization.CultureInfo.InvariantCulture), StringComparison.Ordinal), expiresIn, etag);

        private static AuthorizedCharacterData Character(long id)
            => new AuthorizedCharacterData { CharacterID = id, Token = "token-" + id, ExpiresOn = DateTime.UtcNow.AddHours(1) };

        [Fact]
        public async Task A_fresh_entry_is_served_without_asking_ESI()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromMinutes(5))) };
            var client = Client(esi);

            var first = await client.Status.Retrieve();
            var second = await client.Status.Retrieve();

            Assert.Equal(1, esi.Calls);
            Assert.Equal(EsiCacheStatus.Miss, first.Cache);
            Assert.Equal(EsiCacheStatus.Hit, second.Cache);
            Assert.Equal(HttpStatusCode.OK, second.StatusCode);
            Assert.Equal(100, second.Data.Players);
            Assert.NotNull(second.Expires);
        }

        [Fact]
        public async Task An_expired_entry_is_revalidated_with_its_ETag_and_a_304_keeps_the_body()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromSeconds(-1), "v1")) };
            var client = Client(esi);
            await client.Status.Retrieve();

            esi.Respond = (_, __) =>
            {
                var notModified = new HttpResponseMessage(HttpStatusCode.NotModified) { Content = new ByteArrayContent(Array.Empty<byte>()) };
                notModified.Content.Headers.Expires = DateTimeOffset.UtcNow.AddMinutes(5);
                notModified.Headers.TryAddWithoutValidation("ETag", "\"v1\"");
                notModified.Headers.TryAddWithoutValidation("X-Esi-Error-Limit-Remain", "77");
                return Task.FromResult(notModified);
            };
            var revalidated = await client.Status.Retrieve();

            Assert.Equal("\"v1\"", string.Join(",", esi.Last.Headers.GetValues("If-None-Match")));   // the validator exactly as ESI sent it
            Assert.Equal(EsiCacheStatus.Revalidated, revalidated.Cache);
            Assert.Equal(HttpStatusCode.OK, revalidated.StatusCode);
            Assert.Equal(100, revalidated.Data.Players);
            Assert.Equal(77, revalidated.ErrorLimitRemain);  // the live per-request header

            var hit = await client.Status.Retrieve();       // fresh again after the 304
            Assert.Equal(EsiCacheStatus.Hit, hit.Cache);
            Assert.Equal(2, esi.Calls);
            Assert.Null(hit.ErrorLimitRemain);               // per-request headers are not stored
        }

        [Fact]
        public async Task A_weak_ETag_is_sent_back_verbatim()
        {
            // ESI answers /status/ with a weak validator: W/"...".
            var esi = new Esi
            {
                Respond = (_, __) =>
                {
                    var r = Status(100, TimeSpan.FromSeconds(-1));
                    r.Headers.TryAddWithoutValidation("ETag", "W/\"ecde45\"");
                    return Task.FromResult(r);
                },
            };
            var client = Client(esi);
            await client.Status.Retrieve();
            await client.Status.Retrieve();

            Assert.Equal("W/\"ecde45\"", string.Join(",", esi.Last.Headers.GetValues("If-None-Match")));
        }

        [Fact]
        public async Task Characters_never_share_entries()
        {
            var esi = new Esi { Respond = (r, __) => Task.FromResult(Ok(r.Headers.Authorization.Parameter.EndsWith("-1", StringComparison.Ordinal) ? "1.5" : "2.5", TimeSpan.FromMinutes(5))) };
            var client = Client(esi);

            var one = await client.Wallet.CharacterWallet(new EsiCallOptions { Character = Character(1) });
            var two = await client.Wallet.CharacterWallet(new EsiCallOptions { Character = Character(2) });
            var oneAgain = await client.Wallet.CharacterWallet(new EsiCallOptions { Character = Character(1) });

            Assert.Equal(1.5m, one.Data);
            Assert.Equal(EsiCacheStatus.Miss, two.Cache);
            Assert.Equal(2.5m, two.Data);
            Assert.Equal(EsiCacheStatus.Hit, oneAgain.Cache);
            Assert.Equal(1.5m, oneAgain.Data);
            Assert.Equal(2, esi.Calls);
        }

        [Fact]
        public async Task A_call_with_its_own_IfNoneMatch_gets_ESIs_raw_answer()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromMinutes(5), "v1")) };
            var client = Client(esi);
            await client.Status.Retrieve();

            esi.Respond = (_, __) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotModified) { Content = new ByteArrayContent(Array.Empty<byte>()) });
            var r = await client.Status.Retrieve(new EsiCallOptions { IfNoneMatch = "v1" });

            Assert.Equal(HttpStatusCode.NotModified, r.StatusCode);
            Assert.Equal(EsiCacheStatus.None, r.Cache);
            Assert.Equal(2, esi.Calls);
        }

        [Fact]
        public async Task BypassCache_asks_ESI_and_refreshes_the_entry()
        {
            var players = 100;
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Status(players, TimeSpan.FromMinutes(5))) };
            var client = Client(esi);
            await client.Status.Retrieve();

            players = 200;
            var bypassed = await client.Status.Retrieve(new EsiCallOptions { BypassCache = true });
            var after = await client.Status.Retrieve();

            Assert.Equal(EsiCacheStatus.Miss, bypassed.Cache);
            Assert.Equal(200, bypassed.Data.Players);
            Assert.Equal(EsiCacheStatus.Hit, after.Cache);
            Assert.Equal(200, after.Data.Players);
            Assert.Equal(2, esi.Calls);
        }

        [Fact]
        public async Task Errors_are_not_stored()
        {
            var esi = new Esi
            {
                Respond = (_, __) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable) { Content = new StringContent(@"{ ""error"": ""down"" }") }),
            };
            var client = Client(esi);

            await client.Status.Retrieve();
            var second = await client.Status.Retrieve();

            Assert.Equal(2, esi.Calls);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, second.StatusCode);
            Assert.Equal(EsiCacheStatus.Miss, second.Cache);
        }

        [Fact]
        public async Task No_store_responses_are_not_stored()
        {
            var esi = new Esi
            {
                Respond = (_, __) =>
                {
                    var r = Status(100, TimeSpan.FromMinutes(5), "v1");
                    r.Headers.TryAddWithoutValidation("Cache-Control", "no-store");
                    return Task.FromResult(r);
                },
            };
            var client = Client(esi);

            await client.Status.Retrieve();
            await client.Status.Retrieve();

            Assert.Equal(2, esi.Calls);
            Assert.False(esi.Last.Headers.Contains("If-None-Match"));
        }

        [Fact]
        public async Task Posts_are_never_cached()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Ok("[]", TimeSpan.FromMinutes(5))) };
            var client = Client(esi);

            await client.Universe.Names(new List<long> { 1 });
            var second = await client.Universe.Names(new List<long> { 1 });

            Assert.Equal(2, esi.Calls);
            Assert.Equal(EsiCacheStatus.None, second.Cache);
        }

        [Fact]
        public async Task Different_pages_are_different_entries()
        {
            var esi = new Esi { Respond = (r, __) => Task.FromResult(Ok("[]", TimeSpan.FromMinutes(5))) };
            var client = Client(esi);

            await client.Market.RegionOrders(10000002, options: new EsiCallOptions { Page = 1 });
            var page2 = await client.Market.RegionOrders(10000002, options: new EsiCallOptions { Page = 2 });
            var page1Again = await client.Market.RegionOrders(10000002, options: new EsiCallOptions { Page = 1 });

            Assert.Equal(EsiCacheStatus.Miss, page2.Cache);
            Assert.Equal(EsiCacheStatus.Hit, page1Again.Cache);
            Assert.Equal(2, esi.Calls);
        }

        private sealed class BrokenStore : IDistributedCache
        {
            public byte[] Get(string key) => throw new InvalidOperationException("store down");
            public Task<byte[]> GetAsync(string key, CancellationToken token = default) => throw new InvalidOperationException("store down");
            public void Set(string key, byte[] value, DistributedCacheEntryOptions options) => throw new InvalidOperationException("store down");
            public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default) => throw new InvalidOperationException("store down");
            public void Refresh(string key) => throw new InvalidOperationException("store down");
            public Task RefreshAsync(string key, CancellationToken token = default) => throw new InvalidOperationException("store down");
            public void Remove(string key) => throw new InvalidOperationException("store down");
            public Task RemoveAsync(string key, CancellationToken token = default) => throw new InvalidOperationException("store down");
        }

        [Fact]
        public async Task A_failing_store_never_fails_the_call()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromMinutes(5))) };
            var client = Client(esi, s => s.AddSingleton<IDistributedCache>(new BrokenStore()));

            var r = await client.Status.Retrieve();

            Assert.Equal(HttpStatusCode.OK, r.StatusCode);
            Assert.Equal(100, r.Data.Players);
            Assert.Equal(EsiCacheStatus.Miss, r.Cache);
        }

        [Fact]
        public async Task Concurrent_misses_share_one_ESI_request()
        {
            var gate = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var esi = new Esi
            {
                Respond = async (_, __) =>
                {
                    await gate.Task.ConfigureAwait(false);
                    return Status(100, TimeSpan.FromMinutes(5));
                },
            };
            var client = Client(esi);

            var calls = Enumerable.Range(0, 5).Select(_ => client.Status.Retrieve()).ToArray();
            await Task.Delay(100);   // let every call reach the cache handler
            gate.SetResult(true);
            var results = await Task.WhenAll(calls);

            Assert.Equal(1, esi.Calls);
            Assert.All(results, r => Assert.Equal(100, r.Data.Players));
        }

        [Fact]
        public async Task A_cancelled_first_caller_does_not_cancel_the_others()
        {
            var esi = new Esi();
            esi.Respond = async (_, ct) =>
            {
                if (esi.Calls == 1)
                    await Task.Delay(Timeout.Infinite, ct).ConfigureAwait(false);  // hangs until cancelled
                return Status(100, TimeSpan.FromMinutes(5));
            };
            var client = Client(esi);

            using var cts = new CancellationTokenSource();
            var first = client.Status.Retrieve(new EsiCallOptions { CancellationToken = cts.Token });
            await Task.Delay(100);
            var second = client.Status.Retrieve();   // joins the first caller's in-flight request
            await Task.Delay(100);

            await cts.CancelAsync();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => first);

            var r = await second;                    // retried on its own instead of inheriting the cancel
            Assert.Equal(100, r.Data.Players);
            Assert.Equal(2, esi.Calls);
        }

        [Fact]
        public void The_cache_is_the_outermost_ESI_handler_and_is_installed_once()
        {
            var services = new ServiceCollection();
            services.AddEsi(c =>
                {
                    c.UserAgent = "cache-tests";
                    c.EsiUrl = "https://esi.evetech.net/";
                })
                .AddEsiCaching()
                .AddEsiCaching();   // a second call must not stack another cache
            var factory = services.BuildServiceProvider().GetRequiredService<IHttpMessageHandlerFactory>();

            var chain = new List<Type>();
            for (var h = factory.CreateHandler(nameof(IEsiClient)) as DelegatingHandler; h != null; h = h.InnerHandler as DelegatingHandler)
                if (h.GetType().Namespace == "ESI.NET.Http")
                    chain.Add(h.GetType());

            // Outermost: a hit must not trigger a token refresh or wait on the error-limit guard.
            Assert.Equal(
                new[] { typeof(ESI.NET.Http.EsiCacheHandler), typeof(ESI.NET.Http.EsiTokenRefreshHandler), typeof(ESI.NET.Http.EsiHeadersHandler), typeof(ESI.NET.Http.EsiErrorLimitHandler) },
                chain);
        }

        // ---- clock skew -----------------------------------------------------------------
        // ESI's Date says what time it is on ESI's clock; freshness must be judged on that clock.

        /// <summary>A status response as ESI would send it if ESI's clock read <paramref name="esiNow"/>.</summary>
        private static HttpResponseMessage StatusAt(DateTimeOffset esiNow, TimeSpan freshFor)
        {
            var r = Status(100, TimeSpan.Zero, "v1");
            r.Headers.Date = esiNow;
            r.Content.Headers.Expires = esiNow + freshFor;
            return r;
        }

        [Fact]
        public async Task A_local_clock_running_ahead_does_not_throw_away_fresh_entries()
        {
            // ESI is an hour behind us: its Expires is in our past, but 5 minutes in ESI's future.
            var esi = new Esi { Respond = (_, __) => Task.FromResult(StatusAt(DateTimeOffset.UtcNow.AddHours(-1), TimeSpan.FromMinutes(5))) };
            var client = Client(esi);

            await client.Status.Retrieve();
            var second = await client.Status.Retrieve();

            Assert.Equal(EsiCacheStatus.Hit, second.Cache);
            Assert.Equal(1, esi.Calls);
        }

        [Fact]
        public async Task A_local_clock_running_behind_does_not_serve_stale_entries()
        {
            // ESI is an hour ahead: its Expires is in our future, but already past on ESI's clock.
            var esi = new Esi { Respond = (_, __) => Task.FromResult(StatusAt(DateTimeOffset.UtcNow.AddHours(1), TimeSpan.FromSeconds(-1))) };
            var client = Client(esi);

            await client.Status.Retrieve();
            var second = await client.Status.Retrieve();

            Assert.NotEqual(EsiCacheStatus.Hit, second.Cache);
            Assert.Equal(2, esi.Calls);
            Assert.True(esi.Last.Headers.Contains("If-None-Match"));   // revalidated, not re-downloaded blind
        }

        [Fact]
        public async Task A_response_that_used_up_its_max_age_in_a_cache_on_the_way_is_stale()
        {
            var esi = new Esi
            {
                Respond = (_, __) =>
                {
                    var r = Status(100, TimeSpan.Zero, "v1");
                    r.Content.Headers.Expires = null;
                    r.Headers.Date = DateTimeOffset.UtcNow.AddSeconds(-60);   // generated a minute ago...
                    r.Headers.TryAddWithoutValidation("Cache-Control", "public, max-age=60");
                    r.Headers.TryAddWithoutValidation("Age", "60");          // ...and spent it in a cache
                    return Task.FromResult(r);
                },
            };
            var client = Client(esi);

            await client.Status.Retrieve();
            var second = await client.Status.Retrieve();

            Assert.NotEqual(EsiCacheStatus.Hit, second.Cache);
            Assert.Equal(2, esi.Calls);
        }

        // ---- stored format --------------------------------------------------------------

        private static ESI.NET.Http.EsiCachedResponse Entry(byte[] body) => new ESI.NET.Http.EsiCachedResponse
        {
            Status = 200,
            Body = body,
            ETag = "W/\"abc\"",
            Expires = DateTimeOffset.UtcNow.AddMinutes(5),
            Headers = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                ["ETag"] = new[] { "W/\"abc\"" },
                ["X-Pages"] = new[] { "413" },
            },
        };

        [Fact]
        public void A_stored_entry_round_trips_the_body_byte_for_byte()
        {
            var body = System.Text.Encoding.UTF8.GetBytes(@"[{""order_id"":1,""price"":5.01,""is_buy_order"":true,""name"":""Tritanium ☃""}]");

            var back = ESI.NET.Http.EsiCachedResponse.FromBytes(Entry(body).ToBytes());

            Assert.Equal(body, back.Body);
            Assert.Equal(200, back.Status);
            Assert.Equal("W/\"abc\"", back.ETag);
            Assert.Equal(new[] { "413" }, back.Headers["X-Pages"]);
        }

        [Fact]
        public void A_stored_entry_is_about_the_size_of_the_body()
        {
            // A 2 MB body - /industry/systems size - stored without base64 or escaping.
            var body = System.Text.Encoding.UTF8.GetBytes("[" + string.Join(",", Enumerable.Repeat(@"{""solar_system_id"":30000142,""cost_indices"":[]}", 45_000)) + "]");

            var stored = Entry(body).ToBytes();

            Assert.InRange(stored.Length - body.Length, 1, 1024);   // just the metadata
        }

        [Theory]
        [InlineData(new byte[0])]
        [InlineData(new byte[] { 1, 0, 0, 0, 0 })]                  // another format version
        [InlineData(new byte[] { 2, 0xFF, 0xFF, 0, 0, 0x7B })]      // metadata length past the end
        public void Bytes_this_build_did_not_write_read_as_a_miss(byte[] bytes)
            => Assert.Null(ESI.NET.Http.EsiCachedResponse.FromBytes(bytes));

        // ---- in-memory store bound ------------------------------------------------------

        [Fact]
        public void The_default_store_is_private_to_ESI_NET()
        {
            var services = new ServiceCollection();
            services.AddEsi(c => c.UserAgent = "cache-tests").AddEsiCaching();

            // Nothing app-wide: the app's own IDistributedCache slot stays empty...
            Assert.DoesNotContain(services, d => d.ServiceType == typeof(IDistributedCache));
            Assert.Null(services.BuildServiceProvider().GetService<IDistributedCache>());
        }

        [Fact]
        public void A_memory_store_the_app_adds_later_keeps_its_own_settings()
        {
            var services = new ServiceCollection();
            services.AddEsi(c => c.UserAgent = "cache-tests").AddEsiCaching(o => o.InMemorySizeLimit = 1000);
            services.AddDistributedMemoryCache();   // e.g. for sessions

            var memory = services.BuildServiceProvider()
                .GetRequiredService<Microsoft.Extensions.Options.IOptions<Microsoft.Extensions.Caching.Memory.MemoryDistributedCacheOptions>>().Value;

            // .NET's own default, not ESI.NET's cap of 1000.
            Assert.Equal(new Microsoft.Extensions.Caching.Memory.MemoryDistributedCacheOptions().SizeLimit, memory.SizeLimit);
        }

        private sealed class CountingStore : IDistributedCache
        {
            private readonly MemoryDistributedCache _inner = new MemoryDistributedCache(
                Microsoft.Extensions.Options.Options.Create(new Microsoft.Extensions.Caching.Memory.MemoryDistributedCacheOptions()));
            public int Sets;
            public byte[] Get(string key) => _inner.Get(key);
            public Task<byte[]> GetAsync(string key, CancellationToken token = default) => _inner.GetAsync(key, token);
            public void Set(string key, byte[] value, DistributedCacheEntryOptions options) { Sets++; _inner.Set(key, value, options); }
            public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default) { Interlocked.Increment(ref Sets); return _inner.SetAsync(key, value, options, token); }
            public void Refresh(string key) => _inner.Refresh(key);
            public Task RefreshAsync(string key, CancellationToken token = default) => _inner.RefreshAsync(key, token);
            public void Remove(string key) => _inner.Remove(key);
            public Task RemoveAsync(string key, CancellationToken token = default) => _inner.RemoveAsync(key, token);
        }

        [Fact]
        public async Task An_app_store_registered_after_AddEsiCaching_is_the_one_used()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromMinutes(5))) };
            var store = new CountingStore();
            var services = new ServiceCollection();
            services.AddEsi(c => { c.UserAgent = "cache-tests"; c.EsiUrl = "https://esi.evetech.net/"; })
                .AddEsiCaching()
                .ConfigurePrimaryHttpMessageHandler(() => esi);
            services.AddSingleton<IDistributedCache>(store);   // e.g. AddStackExchangeRedisCache, later

            await services.BuildServiceProvider().GetRequiredService<IEsiClient>().Status.Retrieve();

            Assert.Equal(1, store.Sets);
        }

        // ---- review fixes -----------------------------------------------------------------

        [Fact]
        public async Task Requests_that_are_not_ESI_calls_are_never_cached()
        {
            // e.g. the SSO JWKS fetch, which goes through the same HttpClient.
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Ok("{\"keys\":[]}", TimeSpan.FromMinutes(5), "k1")) };
            var services = new ServiceCollection();
            services.AddEsi(c => c.UserAgent = "cache-tests").AddEsiCaching().ConfigurePrimaryHttpMessageHandler(() => esi);
            var http = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>().CreateClient(nameof(IEsiClient));

            using var first = await http.GetAsync(new Uri("https://login.eveonline.com/oauth/jwks"));
            using var second = await http.GetAsync(new Uri("https://login.eveonline.com/oauth/jwks"));

            Assert.Equal(2, esi.Calls);
            Assert.False(second.Headers.Contains("X-ESI-NET-Cache"));
            Assert.False(esi.Last.Headers.Contains("If-None-Match"));
        }

        [Fact]
        public async Task A_process_that_has_not_seen_ESIs_clock_revalidates_shared_entries()
        {
            // Instance A fills a shared store; instance B starts later and has made no ESI call yet,
            // so it can't tell whether A's entry is fresh on ESI's clock.
            var shared = new CountingStore();
            var esiA = new Esi { Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromMinutes(5), "v1")) };
            var esiB = new Esi
            {
                Respond = (_, __) =>
                {
                    var notModified = new HttpResponseMessage(HttpStatusCode.NotModified) { Content = new ByteArrayContent(Array.Empty<byte>()) };
                    notModified.Headers.Date = DateTimeOffset.UtcNow;
                    notModified.Content.Headers.Expires = DateTimeOffset.UtcNow.AddMinutes(5);
                    return Task.FromResult(notModified);
                },
            };
            var a = Client(esiA, s => s.AddSingleton<IDistributedCache>(shared));
            var b = Client(esiB, s => s.AddSingleton<IDistributedCache>(shared));

            await a.Status.Retrieve();
            var bFirst = await b.Status.Retrieve();
            var bSecond = await b.Status.Retrieve();

            Assert.Equal(EsiCacheStatus.Revalidated, bFirst.Cache);   // a cheap 304, not a blind hit
            Assert.Equal(100, bFirst.Data.Players);
            Assert.Equal(EsiCacheStatus.Hit, bSecond.Cache);          // clock learned from that 304
            Assert.Equal(1, esiB.Calls);
        }

        [Fact]
        public async Task A_no_cache_response_is_revalidated_every_time()
        {
            var esi = new Esi
            {
                Respond = (_, __) =>
                {
                    var r = Status(100, TimeSpan.FromMinutes(5), "v1");
                    r.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
                    return Task.FromResult(r);
                },
            };
            var client = Client(esi);
            await client.Status.Retrieve();

            esi.Respond = (_, __) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotModified) { Content = new ByteArrayContent(Array.Empty<byte>()) });
            var second = await client.Status.Retrieve();

            Assert.Equal(EsiCacheStatus.Revalidated, second.Cache);
            Assert.Equal(2, esi.Calls);
            Assert.True(esi.Last.Headers.Contains("If-None-Match"));
        }

        [Fact]
        public async Task A_304_that_used_up_its_max_age_in_a_cache_leaves_the_entry_stale()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromSeconds(-1), "v1")) };
            var client = Client(esi);
            await client.Status.Retrieve();

            esi.Respond = (_, __) =>
            {
                var notModified = new HttpResponseMessage(HttpStatusCode.NotModified) { Content = new ByteArrayContent(Array.Empty<byte>()) };
                notModified.Headers.Date = DateTimeOffset.UtcNow.AddSeconds(-60);
                notModified.Headers.TryAddWithoutValidation("Cache-Control", "max-age=60");
                notModified.Headers.TryAddWithoutValidation("Age", "60");   // already used up
                return Task.FromResult(notModified);
            };
            await client.Status.Retrieve();
            var third = await client.Status.Retrieve();

            Assert.NotEqual(EsiCacheStatus.Hit, third.Cache);
            Assert.Equal(3, esi.Calls);
        }

        [Fact]
        public async Task A_304_saying_no_store_drops_the_entry()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromSeconds(-1), "v1")) };
            var client = Client(esi);
            await client.Status.Retrieve();

            esi.Respond = (_, __) =>
            {
                var notModified = new HttpResponseMessage(HttpStatusCode.NotModified) { Content = new ByteArrayContent(Array.Empty<byte>()) };
                notModified.Headers.TryAddWithoutValidation("Cache-Control", "no-store");
                return Task.FromResult(notModified);
            };
            await client.Status.Retrieve();

            esi.Respond = (_, __) => Task.FromResult(Status(200, TimeSpan.FromMinutes(5)));
            var third = await client.Status.Retrieve();

            Assert.False(esi.Last.Headers.Contains("If-None-Match"));   // nothing left to revalidate
            Assert.Equal(200, third.Data.Players);
        }

        [Fact]
        public async Task A_character_moved_to_another_account_gets_its_own_entries()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Ok("1.5", TimeSpan.FromMinutes(5))) };
            var client = Client(esi);
            var before = Character(1);
            before.CharacterOwnerHash = "owner-a";
            var after = Character(1);
            after.CharacterOwnerHash = "owner-b";

            await client.Wallet.CharacterWallet(new EsiCallOptions { Character = before });
            var r = await client.Wallet.CharacterWallet(new EsiCallOptions { Character = after });

            Assert.Equal(EsiCacheStatus.Miss, r.Cache);
            Assert.Equal(2, esi.Calls);
        }

        [Fact]
        public async Task A_revalidated_response_describes_the_cached_body_not_the_304()
        {
            const string body = "{ \"players\": 100, \"server_version\": \"1\", \"start_time\": \"2026-11-20T11:05:00Z\" }";
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Ok(body, TimeSpan.FromSeconds(-1), "v1")) };
            var services = new ServiceCollection();
            services.AddEsi(c => c.UserAgent = "cache-tests").AddEsiCaching().ConfigurePrimaryHttpMessageHandler(() => esi);
            var http = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>().CreateClient(nameof(IEsiClient));

            HttpRequestMessage Request()
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://esi.evetech.net/status/");
                ESI.NET.Http.EsiRequestState.MarkEsiRequest(request);
                return request;
            }

            using (var firstRequest = Request())
            using (await http.SendAsync(firstRequest)) { }

            esi.Respond = (_, __) =>
            {
                var notModified = new HttpResponseMessage(HttpStatusCode.NotModified) { Content = new ByteArrayContent(Array.Empty<byte>()) };
                notModified.Content.Headers.ContentLength = 0;
                return Task.FromResult(notModified);
            };
            using var secondRequest = Request();
            using var revalidated = await http.SendAsync(secondRequest);

            Assert.Equal(System.Text.Encoding.UTF8.GetByteCount(body), revalidated.Content.Headers.ContentLength);
        }

        [Fact]
        public void A_negative_RetainAfterExpiry_is_rejected()
        {
            var services = new ServiceCollection();
            services.AddEsi(c => c.UserAgent = "cache-tests").AddEsiCaching(o => o.RetainAfterExpiry = TimeSpan.FromHours(-1));

            Assert.Throws<Microsoft.Extensions.Options.OptionsValidationException>(() =>
                services.BuildServiceProvider().GetRequiredService<Microsoft.Extensions.Options.IOptions<EsiCacheOptions>>().Value);
        }

        [Fact]
        public async Task Zero_RetainAfterExpiry_skips_storing_stale_entries_quietly()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromSeconds(-1), "v1")) };
            var store = new CountingStore();
            var services = new ServiceCollection();
            services.AddSingleton<IDistributedCache>(store);
            services.AddEsi(c => { c.UserAgent = "cache-tests"; c.EsiUrl = "https://esi.evetech.net/"; })
                .AddEsiCaching(o => o.RetainAfterExpiry = TimeSpan.Zero)
                .ConfigurePrimaryHttpMessageHandler(() => esi);

            var r = await services.BuildServiceProvider().GetRequiredService<IEsiClient>().Status.Retrieve();

            Assert.Equal(HttpStatusCode.OK, r.StatusCode);
            Assert.Equal(0, store.Sets);   // already stale, nothing kept: no write attempted (none failing)
        }

        // ---- second review ----------------------------------------------------------------

        [Fact]
        public async Task A_response_that_waited_in_a_CDN_is_fresh_for_what_is_left_of_its_max_age()
        {
            // Generated 200s ago, served by a CDN with Age 200, max-age 300: 100s left.
            var esi = new Esi
            {
                Respond = (_, __) =>
                {
                    var r = Status(100, TimeSpan.Zero, "v1");
                    r.Content.Headers.Expires = null;
                    r.Headers.Date = DateTimeOffset.UtcNow.AddSeconds(-200);
                    r.Headers.TryAddWithoutValidation("Age", "200");
                    r.Headers.TryAddWithoutValidation("Cache-Control", "max-age=300");
                    return Task.FromResult(r);
                },
            };
            var client = Client(esi);

            await client.Status.Retrieve();
            var second = await client.Status.Retrieve();

            Assert.Equal(EsiCacheStatus.Hit, second.Cache);   // Age is not subtracted twice
            Assert.Equal(1, esi.Calls);
        }

        [Fact]
        public async Task An_entry_about_to_expire_is_not_served()
        {
            // Date has whole seconds and the response took time to arrive, so ESI's clock may be up
            // to a second-plus ahead of what Date says: an entry within that margin is not a hit.
            var esi = new Esi
            {
                Respond = (_, __) =>
                {
                    var now = DateTimeOffset.UtcNow;
                    var r = Status(100, TimeSpan.Zero, "v1");
                    r.Headers.Date = now;
                    r.Content.Headers.Expires = now.AddMilliseconds(500);
                    return Task.FromResult(r);
                },
            };
            var client = Client(esi);

            await client.Status.Retrieve();
            var second = await client.Status.Retrieve();

            Assert.NotEqual(EsiCacheStatus.Hit, second.Cache);
        }

        [Fact]
        public async Task A_bypassed_call_that_gets_403_drops_the_old_200()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Ok("1.5", TimeSpan.FromMinutes(5))) };
            var client = Client(esi);
            await client.Wallet.CharacterWallet(new EsiCallOptions { Character = Character(1) });

            // The role is gone: ESI now says 403.
            esi.Respond = (_, __) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = new StringContent(@"{ ""error"": ""forbidden"" }") });
            var bypassed = await client.Wallet.CharacterWallet(new EsiCallOptions { Character = Character(1), BypassCache = true });
            var after = await client.Wallet.CharacterWallet(new EsiCallOptions { Character = Character(1) });

            Assert.Equal(HttpStatusCode.Forbidden, bypassed.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, after.StatusCode);   // not the cached 200
            Assert.Equal(EsiCacheStatus.Miss, after.Cache);
            Assert.Equal(3, esi.Calls);
        }

        [Fact]
        public async Task An_ESI_outage_does_not_drop_a_good_entry()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromMinutes(5))) };
            var client = Client(esi);
            await client.Status.Retrieve();

            esi.Respond = (_, __) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable) { Content = new StringContent("{}") });
            await client.Status.Retrieve(new EsiCallOptions { BypassCache = true });
            var after = await client.Status.Retrieve();

            Assert.Equal(EsiCacheStatus.Hit, after.Cache);   // a 5xx says nothing about the resource
            Assert.Equal(100, after.Data.Players);
        }

        [Fact]
        public async Task A_non_JSON_200_is_not_stored()
        {
            var esi = new Esi
            {
                Respond = (_, __) =>
                {
                    var r = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("<html>gateway</html>", System.Text.Encoding.UTF8, "text/html") };
                    r.Headers.Date = DateTimeOffset.UtcNow;
                    r.Headers.TryAddWithoutValidation("Cache-Control", "max-age=60");
                    return Task.FromResult(r);
                },
            };
            var client = Client(esi);

            await Assert.ThrowsAsync<EsiDeserializationException>(() => client.Status.Retrieve());
            esi.Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromMinutes(5)));
            var retry = await client.Status.Retrieve();

            Assert.Equal(100, retry.Data.Players);   // the retry reached ESI instead of re-reading the HTML
            Assert.Equal(2, esi.Calls);
        }

        [Fact]
        public async Task Concurrent_calls_with_different_character_objects_are_not_merged()
        {
            // Each object's token is refreshed (and its OnTokenRefreshed run) only by its own request.
            var gate = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var esi = new Esi
            {
                Respond = async (_, __) =>
                {
                    await gate.Task.ConfigureAwait(false);
                    return Ok("1.5", TimeSpan.FromMinutes(5));
                },
            };
            var client = Client(esi);
            var same = Character(1);

            var calls = new[]
            {
                client.Wallet.CharacterWallet(new EsiCallOptions { Character = same }),
                client.Wallet.CharacterWallet(new EsiCallOptions { Character = same }),
                client.Wallet.CharacterWallet(new EsiCallOptions { Character = Character(1) }),   // another object
            };
            await Task.Delay(100);
            gate.SetResult(true);
            await Task.WhenAll(calls);

            Assert.Equal(2, esi.Calls);   // the two with `same` merged; the other went on its own
        }

        [Fact]
        public async Task A_full_store_just_stops_caching()
        {
            var esi = new Esi { Respond = (_, __) => Task.FromResult(Status(100, TimeSpan.FromMinutes(5))) };
            var services = new ServiceCollection();
            services.AddEsi(c =>
                {
                    c.UserAgent = "cache-tests";
                    c.EsiUrl = "https://esi.evetech.net/";
                })
                .AddEsiCaching(o => o.InMemorySizeLimit = 16)   // smaller than any entry
                .ConfigurePrimaryHttpMessageHandler(() => esi);
            var client = services.BuildServiceProvider().GetRequiredService<IEsiClient>();

            var first = await client.Status.Retrieve();
            var second = await client.Status.Retrieve();

            Assert.Equal(100, second.Data.Players);
            Assert.Equal(EsiCacheStatus.Miss, second.Cache);
            Assert.Equal(2, esi.Calls);
        }
    }
}
