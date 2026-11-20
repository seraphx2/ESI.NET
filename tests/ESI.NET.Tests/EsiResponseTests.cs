using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ESI.NET;
using Xunit;

namespace ESI.NET.Tests
{
    /// <summary>
    /// Covers <see cref="EsiResponse{T}"/>.CreateAsync — the header/body projection that used to run
    /// synchronously (<c>.Result</c>) in the constructor. All in-memory, no network.
    /// </summary>
    public class EsiResponseTests
    {
        private static HttpResponseMessage Message(
            HttpStatusCode status,
            string body = null,
            Action<HttpResponseMessage> configure = null)
        {
            var msg = new HttpResponseMessage(status);
            if (body != null)
                msg.Content = new StringContent(body);
            else
                msg.Content = new StringContent(string.Empty);
            configure?.Invoke(msg);
            return msg;
        }

        [Fact]
        public async Task Ok_with_json_object_populates_Data()
        {
            var r = await EsiResponse<Dictionary<string, int>>.CreateAsync(
                Message(HttpStatusCode.OK, @"{ ""a"": 1, ""b"": 2 }"),
                "GET|/x/");

            Assert.Equal(HttpStatusCode.OK, r.StatusCode);
            Assert.Equal("/x/", r.Endpoint);
            Assert.Equal(2, r.Data["b"]);
            Assert.Null(r.Message);
        }

        [Fact]
        public async Task Ok_with_json_array_populates_Data()
        {
            var r = await EsiResponse<int[]>.CreateAsync(
                Message(HttpStatusCode.OK, "[10, 20, 30]"), "GET|/x/");

            Assert.Equal(new[] { 10, 20, 30 }, r.Data);
        }

        [Fact]
        public async Task Ok_with_non_json_body_throws_for_a_typed_model()
        {
            var ex = await Assert.ThrowsAsync<EsiDeserializationException>(() =>
                EsiResponse<int[]>.CreateAsync(Message(HttpStatusCode.OK, "<html>gateway</html>"), "GET|/x/"));

            Assert.Equal("<html>gateway</html>", ex.Body);
        }

        [Fact]
        public async Task Ok_with_plain_text_body_is_kept_for_a_string_model()
        {
            var r = await EsiResponse<string>.CreateAsync(
                Message(HttpStatusCode.OK, "just text"), "POST|/x/");

            Assert.Equal("just text", r.Data);
        }

        [Fact]
        public async Task Ok_with_json_string_body_reads_the_string()
        {
            var r = await EsiResponse<string>.CreateAsync(
                Message(HttpStatusCode.OK, "\"quoted\""), "POST|/x/");

            Assert.Equal("quoted", r.Data);
        }

        [Fact]
        public async Task Ok_with_empty_body_leaves_Data_default()
        {
            var r = await EsiResponse<int[]>.CreateAsync(Message(HttpStatusCode.OK, ""), "GET|/x/");

            Assert.Null(r.Data);
        }

        [Fact]
        public async Task Ok_with_a_trailing_newline_still_populates_Data()
        {
            // Regression: ESI ends some bodies with "\n", which defeated the
            // body.EndsWith("}") check and silently left Data null.
            var r = await EsiResponse<Dictionary<string, int>>.CreateAsync(
                Message(HttpStatusCode.OK, "{ \"a\": 1 }\n"), "GET|/x/");

            Assert.Equal(1, r.Data["a"]);
            Assert.Null(r.Message);
        }

        [Fact]
        public async Task Ok_with_surrounding_whitespace_still_populates_Data()
        {
            var r = await EsiResponse<int[]>.CreateAsync(
                Message(HttpStatusCode.OK, "  [1, 2, 3]\r\n"), "GET|/x/");

            Assert.Equal(new[] { 1, 2, 3 }, r.Data);
        }

        [Fact]
        public async Task Ok_with_a_bare_scalar_body_populates_Data()
        {
            // e.g. GET /characters/{id}/wallet/ returns just a number.
            var r = await EsiResponse<decimal>.CreateAsync(
                Message(HttpStatusCode.OK, "123456.78\n"), "GET|/x/");

            Assert.Equal(123456.78m, r.Data);
            Assert.Null(r.Message);
        }

        [Fact]
        public async Task NoContent_sets_a_message()
        {
            // Every 204 gets the same flat message regardless of path - there used
            // to be a hand-maintained per-endpoint dictionary here, which only ever
            // covered a handful of paths and had already gone stale (it never
            // picked up any endpoint added since). StatusCode + Endpoint already
            // say everything a consumer needs; this needs no maintenance.
            var r = await EsiResponse<object>.CreateAsync(
                Message(HttpStatusCode.NoContent), "DELETE|/some/endpoint/");

            Assert.Equal("No Content", r.Message);
        }

        [Fact]
        public async Task NotModified_sets_a_message()
        {
            var r = await EsiResponse<object>.CreateAsync(
                Message(HttpStatusCode.NotModified), "GET|/x/");

            Assert.Equal("Not Modified", r.Message);
        }

        [Fact]
        public async Task Error_status_surfaces_the_esi_error_string()
        {
            var r = await EsiResponse<object>.CreateAsync(
                Message(HttpStatusCode.NotFound, @"{ ""error"": ""character not found"" }"),
                "GET|/x/");

            Assert.Equal("character not found", r.Message);
            Assert.Null(r.Data);
        }

        [Fact]
        public async Task Headers_are_parsed_onto_the_response()
        {
            var r = await EsiResponse<int[]>.CreateAsync(
                Message(HttpStatusCode.OK, "[1]", m =>
                {
                    m.Headers.TryAddWithoutValidation("ETag", "\"abc123\"");
                    m.Headers.TryAddWithoutValidation("X-Pages", "7");
                    m.Headers.TryAddWithoutValidation("X-Esi-Error-Limit-Remain", "95");
                    m.Headers.TryAddWithoutValidation("X-Esi-Error-Limit-Reset", "42");
                    m.Headers.TryAddWithoutValidation("X-ESI-Request-ID", "6f1b2c3d-0000-4000-8000-000000000000");
                    m.Content.Headers.TryAddWithoutValidation("Expires", "Wed, 10 Sep 2036 12:00:00 GMT");
                    m.Content.Headers.TryAddWithoutValidation("Last-Modified", "Tue, 09 Sep 2036 12:00:00 GMT");
                }),
                "GET|/x/");

            Assert.Equal("abc123", r.ETag);           // quotes stripped
            Assert.Equal(7, r.Pages);
            Assert.Equal(95, r.ErrorLimitRemain);
            Assert.Equal(42, r.ErrorLimitReset);
            Assert.NotEqual(Guid.Empty, r.RequestId);
            Assert.True(r.Expires > DateTime.UtcNow);
            Assert.NotNull(r.LastModified);
        }

        [Fact]
        public async Task Deserialization_failure_throws_with_the_details()
        {
            // Valid JSON that does not fit the model - what spec drift looks like.
            // It used to come back as 200 OK with Data == null.
            var ex = await Assert.ThrowsAsync<EsiDeserializationException>(() =>
                EsiResponse<int[]>.CreateAsync(
                    Message(HttpStatusCode.OK, @"{ ""not"": ""an array"" }", m =>
                        m.Headers.TryAddWithoutValidation("X-ESI-Request-ID", "6f1b2c3d-0000-4000-8000-000000000000")),
                    "GET|/x/{id}/"));

            Assert.Equal("/x/{id}/", ex.Endpoint);
            Assert.Equal(HttpStatusCode.OK, ex.StatusCode);
            Assert.Equal(Guid.Parse("6f1b2c3d-0000-4000-8000-000000000000"), ex.RequestId);
            Assert.Equal(typeof(int[]), ex.TargetType);
            Assert.Equal(@"{ ""not"": ""an array"" }", ex.Body);
            Assert.IsAssignableFrom<Newtonsoft.Json.JsonException>(ex.InnerException);
        }

        [Fact]
        public async Task Deserialization_failure_caps_the_body_it_keeps()
        {
            var huge = "{ \"x\": \"" + new string('a', 10_000) + "\" }";
            var ex = await Assert.ThrowsAsync<EsiDeserializationException>(() =>
                EsiResponse<int[]>.CreateAsync(Message(HttpStatusCode.OK, huge), "GET|/x/"));

            Assert.Equal(EsiDeserializationException.MaxBodyLength, ex.Body.Length);
        }

        [Fact]
        public async Task Non_json_error_body_is_passed_through_on_Message()
        {
            // e.g. an HTML 502 from a gateway in front of ESI.
            var r = await EsiResponse<int[]>.CreateAsync(
                Message(HttpStatusCode.BadGateway, "<html>502 Bad Gateway</html>"), "GET|/x/");

            Assert.Equal(HttpStatusCode.BadGateway, r.StatusCode);
            Assert.Equal("<html>502 Bad Gateway</html>", r.Message);
            Assert.Null(r.Data);
        }

        [Fact]
        public async Task Malformed_headers_are_dropped_and_the_body_still_reads()
        {
            // A bad header used to abort the whole projection and leave Data null.
            var r = await EsiResponse<int[]>.CreateAsync(
                Message(HttpStatusCode.OK, "[1, 2]", m =>
                {
                    m.Headers.TryAddWithoutValidation("X-ESI-Request-ID", "not-a-guid");
                    m.Headers.TryAddWithoutValidation("X-Pages", "many");
                    m.Headers.TryAddWithoutValidation("X-Esi-Error-Limit-Remain", "");
                }),
                "GET|/x/");

            Assert.Equal(new[] { 1, 2 }, r.Data);
            Assert.Equal(Guid.Empty, r.RequestId);
            Assert.Null(r.Pages);
            Assert.Null(r.ErrorLimitRemain);
        }

        [Fact]
        public async Task The_response_is_disposed_by_the_factory()
        {
            var msg = Message(HttpStatusCode.OK, "[1]");
            await EsiResponse<int[]>.CreateAsync(msg, "GET|/x/");

            // Reading the content of a disposed HttpResponseMessage throws.
            await Assert.ThrowsAnyAsync<Exception>(() => msg.Content.ReadAsStringAsync());
        }
    }
}
