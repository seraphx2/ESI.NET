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
            Assert.Null(r.Exception);
        }

        [Fact]
        public async Task Ok_with_json_array_populates_Data()
        {
            var r = await EsiResponse<int[]>.CreateAsync(
                Message(HttpStatusCode.OK, "[10, 20, 30]"), "GET|/x/");

            Assert.Equal(new[] { 10, 20, 30 }, r.Data);
        }

        [Fact]
        public async Task Ok_with_non_json_body_goes_to_Message()
        {
            var r = await EsiResponse<object>.CreateAsync(
                Message(HttpStatusCode.OK, "just text"), "GET|/x/");

            Assert.Equal("just text", r.Message);
            Assert.Null(r.Data);
            Assert.Null(r.Exception);
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
            Assert.Null(r.Exception);
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
            Assert.Null(r.Exception);
        }

        [Fact]
        public async Task NoContent_with_a_known_path_maps_to_its_message()
        {
            var r = await EsiResponse<object>.CreateAsync(
                Message(HttpStatusCode.NoContent),
                "DELETE|/characters/{character_id}/fittings/{fitting_id}/");

            Assert.Equal("Fitting deleted", r.Message);
        }

        [Fact]
        public async Task NoContent_with_an_unknown_path_falls_back_instead_of_throwing()
        {
            // Regression: the old code indexed the dictionary directly and threw
            // KeyNotFoundException into the catch, leaving Message null.
            var r = await EsiResponse<object>.CreateAsync(
                Message(HttpStatusCode.NoContent), "DELETE|/some/new/endpoint/");

            Assert.Equal("No Content", r.Message);
            Assert.Null(r.Exception);
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
        public async Task Deserialization_failure_is_captured_not_thrown()
        {
            // Body looks like JSON (starts { ends }) but does not fit the target type.
            var r = await EsiResponse<int[]>.CreateAsync(
                Message(HttpStatusCode.OK, @"{ ""not"": ""an array"" }"), "GET|/x/");

            Assert.NotNull(r.Exception);
            Assert.Equal(@"{ ""not"": ""an array"" }", r.Message);
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
