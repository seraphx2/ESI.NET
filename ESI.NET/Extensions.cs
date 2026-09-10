using ESI.NET.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static string ToEsiValue(this Enum e)
        {
            var enums = e.ToString();
            if (enums.Contains(", "))
            {
                var values = enums.Replace(" ", "").Split(',');
                var newValues = new List<string>();
                foreach (var item in values)
                    newValues.Add(Enum.Parse(e.GetType(), item).GetType().GetTypeInfo().DeclaredMembers.SingleOrDefault(x => x.Name == item.ToString())
                        ?.GetCustomAttribute<EnumMemberAttribute>(true)?.Value);

                return string.Join(",", newValues);
            }
            else
                return e.GetType().GetTypeInfo().DeclaredMembers.SingleOrDefault(x => x.Name == e.ToString())
                    ?.GetCustomAttribute<EnumMemberAttribute>(false)?.Value;
        }
    }
}
