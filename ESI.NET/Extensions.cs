using ESI.NET.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        /// Turns on the response cache for the ESI client registered by <c>AddEsi</c>: <c>GET</c>
        /// responses are reused until ESI's <c>Expires</c>, then revalidated with their <c>ETag</c>
        /// (a cheap <c>304</c> when unchanged). See <see cref="EsiCacheHandler"/> for the rules.
        /// <para>
        /// Entries are kept in the app's <see cref="Microsoft.Extensions.Caching.Distributed.IDistributedCache"/>
        /// if it registers one (before or after this call). If it doesn't, ESI.NET uses a private
        /// in-memory store, capped by <see cref="EsiCacheOptions.InMemorySizeLimit"/>, that is not
        /// registered in the container - so it never takes the place of, or changes the settings
        /// of, a store the app adds for anything else. To share the cache between app
        /// instances, register a distributed one as usual - e.g.
        /// <c>services.AddStackExchangeRedisCache(...)</c> from
        /// <c>Microsoft.Extensions.Caching.StackExchangeRedis</c>. Authenticated responses are
        /// stored there too (keyed per character), so secure the store accordingly.
        /// </para>
        /// </summary>
        /// <example>
        /// <code>
        /// services.AddEsi(builder.Configuration.GetSection("EsiConfig"))
        ///         .AddEsiCaching();
        /// </code>
        /// </example>
        public static IHttpClientBuilder AddEsiCaching(this IHttpClientBuilder builder, Action<EsiCacheOptions> configure = null)
        {
            Guard.NotNull(builder, nameof(builder));

            var services = builder.Services;
            var options = services.AddOptions<EsiCacheOptions>();
            if (configure != null)
                options.Configure(configure);
            options.Validate(o => o.RetainAfterExpiry >= TimeSpan.Zero,
                    $"{nameof(EsiCacheOptions)}.{nameof(EsiCacheOptions.RetainAfterExpiry)} can't be negative.")
                .Validate(o => o.InMemorySizeLimit == null || o.InMemorySizeLimit > 0,
                    $"{nameof(EsiCacheOptions)}.{nameof(EsiCacheOptions.InMemorySizeLimit)} must be positive, or null for no limit.")
                .ValidateOnStart();   // a misconfiguration fails at startup, not on the first ESI call

            // The store is chosen when first needed, so an app store registered after this call
            // still counts; with none, a private in-memory one (never registered app-wide).
            services.TryAddSingleton(provider => new EsiCacheStore(
                provider.GetService<IDistributedCache>()
                ?? new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions
                {
                    SizeLimit = provider.GetRequiredService<IOptions<EsiCacheOptions>>().Value.InMemorySizeLimit,
                }))));
            services.TryAddSingleton<EsiCacheState>();
            services.TryAddTransient(provider => new EsiCacheHandler(
                provider.GetRequiredService<EsiCacheStore>().Cache,
                provider.GetRequiredService<EsiCacheState>(),
                provider.GetRequiredService<IOptions<EsiCacheOptions>>(),
                provider.GetService<Microsoft.Extensions.Logging.ILogger<EsiCacheHandler>>()));

            // Outermost, ahead of the handlers AddEsi added, so a hit skips the token refresh and
            // the error-limit wait. Guarded so calling this twice doesn't stack two caches.
            return builder.ConfigureAdditionalHttpMessageHandlers((handlers, provider) =>
            {
                if (!handlers.Any(h => h is EsiCacheHandler))
                    handlers.Insert(0, provider.GetRequiredService<EsiCacheHandler>());
            });
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
            Guard.NotNull(e, nameof(e));

            var type = e.GetType();

            if (Attribute.IsDefined(type, typeof(FlagsAttribute)))
                return string.Join(",", Enum.GetValues(type).Cast<Enum>()
                    .Where(e.HasFlag)
                    .Select(flag => JsonConvert.SerializeObject(flag).Trim('"')));

            return JsonConvert.SerializeObject(e).Trim('"');
        }
    }
}
