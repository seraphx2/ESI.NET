using ESI.NET.Http;
using ESI.NET.Logic;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ESI.NET
{
    /// <summary>
    /// The entry point to ESI: one property per group of endpoints (<see cref="Character"/>,
    /// <see cref="Market"/>, ...), plus <see cref="SSO"/> for logging characters in. Register it
    /// with <c>services.AddEsi(...)</c> and inject <see cref="IEsiClient"/>, or construct it directly.
    /// </summary>
    public class EsiClient : IEsiClient, IDisposable
    {
        readonly HttpClient _client;
        readonly EsiConfig _config;

        // Only true when this instance created _client itself (the no-DI, no-supplied-client path).
        // A caller-supplied client (including one vended by AddEsi's IHttpClientFactory pipeline) is
        // not ours to dispose - the caller / factory owns its lifetime.
        readonly bool _ownsClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EsiClient"/> class.
        /// </summary>
        /// <param name="config">The configuration parameters of the <see cref="EsiClient"/>.</param>
        /// <param name="client">
        /// The <see cref="HttpClient"/> to use. When supplied (including via <c>AddEsi</c>'s
        /// <see cref="System.Net.Http.IHttpClientFactory"/> pipeline) it is used as-is — the caller
        /// / pipeline is responsible for the <c>X-User-Agent</c> and <c>Accept</c> headers and for
        /// content decompression. When omitted, a default client is created and configured here.
        /// </param>
        public EsiClient(IOptions<EsiConfig> config, HttpClient client = null)
        {
            Guard.NotNull(config, nameof(config));

            _config = config.Value;

            if (client != null)
                _client = client;
            else
            {
                if (string.IsNullOrWhiteSpace(_config.UserAgent))
                    throw new ArgumentException("EsiConfig.UserAgent is required. Set it to something that identifies your app (character and/or project name) so CCP can contact you rather than cut off ESI access.");

                // No DI pipeline here, so wire the token-refresh handler in manually. Without an
                // IServiceScopeFactory it honours EsiCallOptions.OnTokenRefreshed but not a sink.
                _client = new HttpClient(new EsiTokenRefreshHandler(config) { InnerHandler = CreateDefaultHandler() });
                _client.DefaultRequestHeaders.Add("X-User-Agent", _config.UserAgent);
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _ownsClient = true;
            }


            SSO = new SsoLogic(_client, _config);
            Alliance = new AllianceLogic(_client, _config);
            Assets = new AssetsLogic(_client, _config);
            Calendar = new CalendarLogic(_client, _config);
            Character = new CharacterLogic(_client, _config);
            Clones = new ClonesLogic(_client, _config);
            Contacts = new ContactsLogic(_client, _config);
            Cosmetics = new CosmeticsLogic(_client, _config);
            Contracts = new ContractsLogic(_client, _config);
            Corporation = new CorporationLogic(_client, _config);
            Dogma = new DogmaLogic(_client, _config);
            FactionWarfare = new FactionWarfareLogic(_client, _config);
            Fittings = new FittingsLogic(_client, _config);
            Fleets = new FleetsLogic(_client, _config);
            FreelanceJobs = new FreelanceJobsLogic(_client, _config);
            Incursions = new IncursionsLogic(_client, _config);
            Industry = new IndustryLogic(_client, _config);
            Insurance = new InsuranceLogic(_client, _config);
            Killmails = new KillmailsLogic(_client, _config);
            Location = new LocationLogic(_client, _config);
            Loyalty = new LoyaltyLogic(_client, _config);
            Mail = new MailLogic(_client, _config);
            Market = new MarketLogic(_client, _config);
            Meta = new MetaLogic(_client, _config);
            MilitaryCampaigns = new MilitaryCampaignsLogic(_client, _config);
            PlanetaryInteraction = new PlanetaryInteractionLogic(_client, _config);
            Routes = new RoutesLogic(_client, _config);
            Search = new SearchLogic(_client, _config);
            Skills = new SkillsLogic(_client, _config);
            Sovereignty = new SovereigntyLogic(_client, _config);
            Status = new StatusLogic(_client, _config);
            Structures = new StructuresLogic(_client, _config);
            Universe = new UniverseLogic(_client, _config);
            UserInterface = new UserInterfaceLogic(_client, _config);
            Wallet = new WalletLogic(_client, _config);
            Wars = new WarsLogic(_client, _config);
        }

        /// <summary>EVE SSO: building the login URL, exchanging the code for tokens, refreshing and verifying them.</summary>
        public SsoLogic SSO { get; set; }
        /// <summary>ESI's Alliance endpoints.</summary>
        public AllianceLogic Alliance { get; set; }
        /// <summary>ESI's Assets endpoints.</summary>
        public AssetsLogic Assets { get; set; }
        /// <summary>ESI's Calendar endpoints.</summary>
        public CalendarLogic Calendar { get; set; }
        /// <summary>ESI's Character endpoints.</summary>
        public CharacterLogic Character { get; set; }
        /// <summary>ESI's Clones endpoints.</summary>
        public ClonesLogic Clones { get; set; }
        /// <summary>ESI's Contacts endpoints.</summary>
        public ContactsLogic Contacts { get; set; }
        /// <summary>ESI's Paragon Hub endpoints.</summary>
        public CosmeticsLogic Cosmetics { get; set; }
        /// <summary>ESI's Contracts endpoints.</summary>
        public ContractsLogic Contracts { get; set; }
        /// <summary>ESI's Corporation endpoints.</summary>
        public CorporationLogic Corporation { get; set; }
        /// <summary>ESI's Dogma endpoints.</summary>
        public DogmaLogic Dogma { get; set; }
        /// <summary>ESI's Faction Warfare endpoints.</summary>
        public FactionWarfareLogic FactionWarfare { get; set; }
        /// <summary>ESI's Fleets endpoints.</summary>
        public FleetsLogic Fleets { get; set; }
        /// <summary>ESI's Fittings endpoints.</summary>
        public FittingsLogic Fittings { get; set; }
        /// <summary>ESI's Freelance Jobs endpoints.</summary>
        public FreelanceJobsLogic FreelanceJobs { get; set; }
        /// <summary>ESI's Incursions endpoints.</summary>
        public IncursionsLogic Incursions { get; set; }
        /// <summary>ESI's Industry endpoints.</summary>
        public IndustryLogic Industry { get; set; }
        /// <summary>ESI's Insurance endpoints.</summary>
        public InsuranceLogic Insurance { get; set; }
        /// <summary>ESI's Killmails endpoints.</summary>
        public KillmailsLogic Killmails { get; set; }
        /// <summary>ESI's Location endpoints.</summary>
        public LocationLogic Location { get; set; }
        /// <summary>ESI's Loyalty endpoints.</summary>
        public LoyaltyLogic Loyalty { get; set; }
        /// <summary>ESI's Mail endpoints.</summary>
        public MailLogic Mail { get; set; }
        /// <summary>ESI's Market endpoints.</summary>
        public MarketLogic Market { get; set; }
        /// <summary>ESI's Meta endpoints.</summary>
        public MetaLogic Meta { get; set; }
        /// <summary>ESI's Military Campaigns endpoints.</summary>
        public MilitaryCampaignsLogic MilitaryCampaigns { get; set; }
        /// <summary>ESI's Planetary Interaction endpoints.</summary>
        public PlanetaryInteractionLogic PlanetaryInteraction { get; set; }
        /// <summary>ESI's Routes endpoints.</summary>
        public RoutesLogic Routes { get; set; }
        /// <summary>ESI's Search endpoints.</summary>
        public SearchLogic Search { get; set; }
        /// <summary>ESI's Skills endpoints.</summary>
        public SkillsLogic Skills { get; set; }
        /// <summary>ESI's Status endpoints.</summary>
        public StatusLogic Status { get; set; }
        /// <summary>ESI's Structures endpoints.</summary>
        public StructuresLogic Structures { get; set; }
        /// <summary>ESI's Sovereignty endpoints.</summary>
        public SovereigntyLogic Sovereignty { get; set; }
        /// <summary>ESI's Universe endpoints.</summary>
        public UniverseLogic Universe { get; set; }
        /// <summary>ESI's User Interface endpoints.</summary>
        public UserInterfaceLogic UserInterface { get; set; }
        /// <summary>ESI's Wallet endpoints.</summary>
        public WalletLogic Wallet { get; set; }
        /// <summary>ESI's Wars endpoints.</summary>
        public WarsLogic Wars { get; set; }


        /// <summary>
        /// Creates the <see cref="HttpClientHandler"/> used when no <see cref="HttpClient"/> is supplied.
        /// </summary>
        /// <remarks>
        /// Automatic decompression is only configured when the handler reports support for it.
        /// On Blazor WebAssembly the underlying browser handler throws
        /// <see cref="PlatformNotSupportedException"/> from the setter, because the browser's fetch
        /// API performs content decoding itself. See https://github.com/seraphx2/ESI.NET/issues/77.
        /// </remarks>
        internal static HttpClientHandler CreateDefaultHandler()
        {
            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression)
            {
                // Switch to All which adds brotli encoding for .net core due to https://github.com/ccpgames/sso-issues/issues/81
#if NET
                handler.AutomaticDecompression = DecompressionMethods.All;
#else
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
#endif
            }

            return handler;
        }

        /// <summary>
        /// Disposes the underlying <see cref="HttpClient"/> - but only when this <see cref="EsiClient"/>
        /// created it itself (the no-DI, no-supplied-client constructor path). A caller-supplied client,
        /// including one vended through <c>AddEsi</c>'s <see cref="System.Net.Http.IHttpClientFactory"/>
        /// pipeline, is left alone; its lifetime belongs to whoever supplied it.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Disposes the <see cref="HttpClient"/> when <paramref name="disposing"/> and this instance created it.</summary>
        /// <param name="disposing">True when called from <see cref="Dispose()"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _ownsClient)
                _client.Dispose();
        }
    }

    /// <summary>The ESI client, as an interface for dependency injection and test doubles. See <see cref="EsiClient"/>.</summary>
    public interface IEsiClient
    {
        /// <summary>EVE SSO: building the login URL, exchanging the code for tokens, refreshing and verifying them.</summary>
        SsoLogic SSO { get; set; }
        /// <summary>ESI's Alliance endpoints.</summary>
        AllianceLogic Alliance { get; set; }
        /// <summary>ESI's Assets endpoints.</summary>
        AssetsLogic Assets { get; set; }
        /// <summary>ESI's Calendar endpoints.</summary>
        CalendarLogic Calendar { get; set; }
        /// <summary>ESI's Character endpoints.</summary>
        CharacterLogic Character { get; set; }
        /// <summary>ESI's Clones endpoints.</summary>
        ClonesLogic Clones { get; set; }
        /// <summary>ESI's Contacts endpoints.</summary>
        ContactsLogic Contacts { get; set; }
        /// <summary>ESI's Paragon Hub endpoints.</summary>
        CosmeticsLogic Cosmetics { get; set; }
        /// <summary>ESI's Contracts endpoints.</summary>
        ContractsLogic Contracts { get; set; }
        /// <summary>ESI's Corporation endpoints.</summary>
        CorporationLogic Corporation { get; set; }
        /// <summary>ESI's Dogma endpoints.</summary>
        DogmaLogic Dogma { get; set; }
        /// <summary>ESI's Faction Warfare endpoints.</summary>
        FactionWarfareLogic FactionWarfare { get; set; }
        /// <summary>ESI's Fittings endpoints.</summary>
        FittingsLogic Fittings { get; set; }
        /// <summary>ESI's Fleets endpoints.</summary>
        FleetsLogic Fleets { get; set; }
        /// <summary>ESI's Freelance Jobs endpoints.</summary>
        FreelanceJobsLogic FreelanceJobs { get; set; }
        /// <summary>ESI's Incursions endpoints.</summary>
        IncursionsLogic Incursions { get; set; }
        /// <summary>ESI's Industry endpoints.</summary>
        IndustryLogic Industry { get; set; }
        /// <summary>ESI's Insurance endpoints.</summary>
        InsuranceLogic Insurance { get; set; }
        /// <summary>ESI's Killmails endpoints.</summary>
        KillmailsLogic Killmails { get; set; }
        /// <summary>ESI's Location endpoints.</summary>
        LocationLogic Location { get; set; }
        /// <summary>ESI's Loyalty endpoints.</summary>
        LoyaltyLogic Loyalty { get; set; }
        /// <summary>ESI's Mail endpoints.</summary>
        MailLogic Mail { get; set; }
        /// <summary>ESI's Market endpoints.</summary>
        MarketLogic Market { get; set; }
        /// <summary>ESI's Meta endpoints.</summary>
        MetaLogic Meta { get; set; }
        /// <summary>ESI's Military Campaigns endpoints.</summary>
        MilitaryCampaignsLogic MilitaryCampaigns { get; set; }
        /// <summary>ESI's Planetary Interaction endpoints.</summary>
        PlanetaryInteractionLogic PlanetaryInteraction { get; set; }
        /// <summary>ESI's Routes endpoints.</summary>
        RoutesLogic Routes { get; set; }
        /// <summary>ESI's Search endpoints.</summary>
        SearchLogic Search { get; set; }
        /// <summary>ESI's Skills endpoints.</summary>
        SkillsLogic Skills { get; set; }
        /// <summary>ESI's Sovereignty endpoints.</summary>
        SovereigntyLogic Sovereignty { get; set; }
        /// <summary>ESI's Status endpoints.</summary>
        StatusLogic Status { get; set; }
        /// <summary>ESI's Structures endpoints.</summary>
        StructuresLogic Structures { get; set; }
        /// <summary>ESI's Universe endpoints.</summary>
        UniverseLogic Universe { get; set; }
        /// <summary>ESI's User Interface endpoints.</summary>
        UserInterfaceLogic UserInterface { get; set; }
        /// <summary>ESI's Wallet endpoints.</summary>
        WalletLogic Wallet { get; set; }
        /// <summary>ESI's Wars endpoints.</summary>
        WarsLogic Wars { get; set; }
    }
}
