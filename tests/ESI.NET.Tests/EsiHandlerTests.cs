using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace ESI.NET.Tests
{
    public class EsiHandlerTests
    {
        private sealed class StubHandler : HttpMessageHandler
        {
            public HttpRequestMessage Last;
            public Func<HttpResponseMessage> Respond = () => new HttpResponseMessage(HttpStatusCode.OK);
            public int Calls;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Calls++;
                Last = request;
                return Task.FromResult(Respond());
            }
        }

        private static HttpResponseMessage WithHeaders(HttpStatusCode status, int remain, int reset)
        {
            var r = new HttpResponseMessage(status);
            r.Headers.TryAddWithoutValidation("X-Esi-Error-Limit-Remain", remain.ToString());
            r.Headers.TryAddWithoutValidation("X-Esi-Error-Limit-Reset", reset.ToString());
            return r;
        }

        // ---- EsiHeadersHandler -------------------------------------------------

        [Fact]
        public async Task HeadersHandler_adds_user_agent_and_accept()
        {
            var stub = new StubHandler();
            var handler = new EsiHeadersHandler(Options.Create(new EsiConfig { UserAgent = "my-app / me" })) { InnerHandler = stub };
            using var invoker = new HttpMessageInvoker(handler);

            await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://esi/"), default);

            Assert.Equal("my-app / me", string.Join("", stub.Last.Headers.GetValues("X-User-Agent")));
            Assert.Contains(stub.Last.Headers.Accept, a => a.MediaType == "application/json");
        }

        [Fact]
        public async Task HeadersHandler_does_not_duplicate_existing_headers()
        {
            var stub = new StubHandler();
            var handler = new EsiHeadersHandler(Options.Create(new EsiConfig { UserAgent = "ua" })) { InnerHandler = stub };
            using var invoker = new HttpMessageInvoker(handler);

            var req = new HttpRequestMessage(HttpMethod.Get, "https://esi/");
            req.Headers.Add("X-User-Agent", "caller-set");
            await invoker.SendAsync(req, default);

            Assert.Equal(new[] { "caller-set" }, stub.Last.Headers.GetValues("X-User-Agent"));
        }

        [Fact]
        public void HeadersHandler_throws_when_user_agent_missing()
        {
            Assert.Throws<ArgumentException>(() => new EsiHeadersHandler(Options.Create(new EsiConfig { UserAgent = " " })));
        }

        // ---- EsiErrorLimitHandler -------------------------------------------

        [Fact]
        public async Task ErrorLimitHandler_passes_through_a_healthy_response()
        {
            var stub = new StubHandler { Respond = () => WithHeaders(HttpStatusCode.OK, remain: 95, reset: 50) };
            var handler = new EsiErrorLimitHandler(new EsiErrorLimitState()) { InnerHandler = stub };
            using var invoker = new HttpMessageInvoker(handler);

            var resp = await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://esi/"), default);

            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        }

        [Fact]
        public async Task ErrorLimitHandler_throws_EsiErrorLimitException_on_420()
        {
            var stub = new StubHandler { Respond = () => WithHeaders((HttpStatusCode)420, remain: 0, reset: 12) };
            var handler = new EsiErrorLimitHandler(new EsiErrorLimitState()) { InnerHandler = stub };
            using var invoker = new HttpMessageInvoker(handler);

            var ex = await Assert.ThrowsAsync<EsiErrorLimitException>(() =>
                invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://esi/"), default));

            Assert.Equal(12, ex.RetryAfter.TotalSeconds);
        }

        [Fact]
        public async Task ErrorLimitHandler_blocks_the_next_send_until_the_window_resets()
        {
            var state = new EsiErrorLimitState();
            var stub = new StubHandler { Respond = () => WithHeaders(HttpStatusCode.OK, remain: 0, reset: 1) };
            var handler = new EsiErrorLimitHandler(state) { InnerHandler = stub };
            using var invoker = new HttpMessageInvoker(handler);

            // first call exhausts the budget (remain: 0) -> state is now blocked for ~1s
            await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://esi/"), default);

            var sw = Stopwatch.StartNew();
            await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://esi/"), default);
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds >= 500, $"expected a delay, waited {sw.ElapsedMilliseconds}ms");
            Assert.Equal(2, stub.Calls);
        }

        // ---- AddEsi wiring -------------------------------------------------

        [Fact]
        public async Task AddEsi_wires_the_pipeline_and_resolves_a_working_client()
        {
            var stub = new StubHandler { Respond = () => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("[1,2]") } };

            var services = new ServiceCollection();
            services.AddEsi(c =>
            {
                c.UserAgent = "wiring-test";
                c.EsiUrl = "https://esi.evetech.net/";
                c.DataSource = DataSource.Tranquility;
            }).ConfigurePrimaryHttpMessageHandler(() => stub);

            var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<IEsiClient>();

            var response = await client.Status.Retrieve();   // public endpoint

            Assert.Equal("wiring-test", string.Join("", stub.Last.Headers.GetValues("X-User-Agent")));
            Assert.Contains("/latest/status/", stub.Last.RequestUri.ToString());
        }
    }
}
