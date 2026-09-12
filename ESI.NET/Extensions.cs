using ESI.NET.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace ESI.NET
{
    public static class Extensions
    {
        /// <summary>
        /// Registers <see cref="IEsiClient"/> as a typed <see cref="System.Net.Http.HttpClient"/>
        /// (via <see cref="System.Net.Http.IHttpClientFactory"/>) with the ESI header and
        /// error-limit handlers. Bind configuration from <paramref name="section"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="IHttpClientBuilder"/> so callers can chain, e.g.
        /// <c>.AddStandardResilienceHandler()</c> after referencing
        /// <c>Microsoft.Extensions.Http.Resilience</c>.
        /// </returns>
        public static IHttpClientBuilder AddEsi(this IServiceCollection services, IConfigurationSection section)
        {
            services.Configure<EsiConfig>(section);
            return services.AddEsiClient();
        }

        /// <summary>
        /// As <see cref="AddEsi(IServiceCollection, IConfigurationSection)"/>, configuring
        /// <see cref="EsiConfig"/> inline instead of from configuration.
        /// </summary>
        public static IHttpClientBuilder AddEsi(this IServiceCollection services, Action<EsiConfig> configure)
        {
            services.Configure(configure);
            return services.AddEsiClient();
        }

        private static IHttpClientBuilder AddEsiClient(this IServiceCollection services)
        {
            services.AddSingleton<EsiErrorLimitState>();
            services.AddTransient<EsiHeadersHandler>();
            services.AddTransient<EsiErrorLimitHandler>();
            services.AddTransient<EsiTokenRefreshHandler>();

            return services.AddHttpClient<IEsiClient, EsiClient>()
                .ConfigurePrimaryHttpMessageHandler(() => EsiClient.CreateDefaultHandler())
                .AddHttpMessageHandler<EsiTokenRefreshHandler>()
                .AddHttpMessageHandler<EsiHeadersHandler>()
                .AddHttpMessageHandler<EsiErrorLimitHandler>();
        }

        /// <summary>
        /// The ESI wire value for <paramref name="e"/> - its <see cref="EnumMemberAttribute.Value"/>,
        /// resolved via the same <c>StringEnumConverter</c> every ESI.NET enum is decorated with (so
        /// this can never drift from what actually serializing the enum would produce). For a
        /// <see cref="FlagsAttribute"/> enum, each set flag is resolved individually and joined with
        /// a comma - no space, matching ESI's comma-separated query parameters (Newtonsoft's own
        /// flags serialization inserts ", " between values, which is not what ESI expects).
        /// </summary>
        public static string ToEsiValue(this Enum e)
        {
            var type = e.GetType();

            if (Attribute.IsDefined(type, typeof(FlagsAttribute)))
                return string.Join(",", Enum.GetValues(type).Cast<Enum>()
                    .Where(e.HasFlag)
                    .Select(flag => JsonConvert.SerializeObject(flag).Trim('"')));

            return JsonConvert.SerializeObject(e).Trim('"');
        }
    }
}
