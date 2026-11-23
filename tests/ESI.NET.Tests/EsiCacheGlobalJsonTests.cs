using System;
using System.Collections.Generic;
using ESI.NET.Http;
using Newtonsoft.Json;
using Xunit;

namespace ESI.NET.Tests
{
    [CollectionDefinition(nameof(GlobalJsonSettings), DisableParallelization = true)]
    public sealed class GlobalJsonSettings
    {
    }

    /// <summary>
    /// The stored format must not depend on <see cref="JsonConvert.DefaultSettings"/>, which an app
    /// can set process-wide. Not run in parallel: these tests change that global.
    /// </summary>
    [Collection(nameof(GlobalJsonSettings))]
    public class EsiCacheGlobalJsonTests
    {
        [Fact]
        public void An_apps_global_JSON_settings_do_not_change_the_stored_entry()
        {
            var expires = new DateTimeOffset(2026, 11, 21, 3, 4, 5, TimeSpan.Zero);
            var entry = new EsiCachedResponse
            {
                Status = 200,
                Body = new byte[] { 1, 2, 3 },
                Expires = expires,
                ETag = "\"v1\"",
                Headers = new Dictionary<string, string[]> { ["X-Pages"] = new[] { "3" } },
            };

            var previous = JsonConvert.DefaultSettings;
            try
            {
                // Deliberately hostile app-wide settings - the point of the test.
#pragma warning disable CA2326, CA2327
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd",           // would drop the time of day
                    TypeNameHandling = TypeNameHandling.All,   // would honour $type from the store
                };
#pragma warning restore CA2326, CA2327

                var back = EsiCachedResponse.FromBytes(entry.ToBytes());

                Assert.Equal(expires, back.Expires);
                Assert.Equal("\"v1\"", back.ETag);
                Assert.Equal(new byte[] { 1, 2, 3 }, back.Body);
            }
            finally
            {
                JsonConvert.DefaultSettings = previous;
            }
        }
    }
}
