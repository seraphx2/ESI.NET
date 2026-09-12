using ESI.NET.Http;
using ESI.NET.Logic;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ESI.NET
{
    public class EsiClient : IEsiClient
    {
        readonly HttpClient _client;
        readonly EsiConfig _config;

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
            if (config == null) throw new ArgumentNullException(nameof(config));

            _config = config.Value;

            if (client != null)
                _client = client;
            else
            {
                if (string.IsNullOrWhiteSpace(_config.UserAgent))
                    throw new ArgumentException("EsiConfig.UserAgent is required. Set it to something that identifies your app (character and/or project name) so CCP can contact you rather than cut off ESI access.");

                // No DI pipeline here, so wire the token-refresh handler in manually. Without an
                // IServiceScopeFactory it honours EsiCallOptions.OnTokenRefreshed but not a sink.
                var handler = new EsiTokenRefreshHandler(config) { InnerHandler = CreateDefaultHandler() };
                _client = new HttpClient(handler);
                _client.DefaultRequestHeaders.Add("X-User-Agent", _config.UserAgent);
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

        public SsoLogic SSO { get; set; }
        public AllianceLogic Alliance { get; set; }
        public AssetsLogic Assets { get; set; }
        public CalendarLogic Calendar { get; set; }
        public CharacterLogic Character { get; set; }
        public ClonesLogic Clones { get; set; }
        public ContactsLogic Contacts { get; set; }
        public CosmeticsLogic Cosmetics { get; set; }
        public ContractsLogic Contracts { get; set; }
        public CorporationLogic Corporation { get; set; }
        public DogmaLogic Dogma { get; set; }
        public FactionWarfareLogic FactionWarfare { get; set; }
        public FleetsLogic Fleets { get; set; }
        public FittingsLogic Fittings { get; set; }
        public FreelanceJobsLogic FreelanceJobs { get; set; }
        public IncursionsLogic Incursions { get; set; }
        public IndustryLogic Industry { get; set; }
        public InsuranceLogic Insurance { get; set; }
        public KillmailsLogic Killmails { get; set; }
        public LocationLogic Location { get; set; }
        public LoyaltyLogic Loyalty { get; set; }
        public MailLogic Mail { get; set; }
        public MarketLogic Market { get; set; }
        public MetaLogic Meta { get; set; }
        public MilitaryCampaignsLogic MilitaryCampaigns { get; set; }
        public PlanetaryInteractionLogic PlanetaryInteraction { get; set; }
        public RoutesLogic Routes { get; set; }
        public SearchLogic Search { get; set; }
        public SkillsLogic Skills { get; set; }
        public StatusLogic Status { get; set; }
        public StructuresLogic Structures { get; set; }
        public SovereigntyLogic Sovereignty { get; set; }
        public UniverseLogic Universe { get; set; }
        public UserInterfaceLogic UserInterface { get; set; }
        public WalletLogic Wallet { get; set; }
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
    }

    public interface IEsiClient
    {
        SsoLogic SSO { get; set; }
        AllianceLogic Alliance { get; set; }
        AssetsLogic Assets { get; set; }
        CalendarLogic Calendar { get; set; }
        CharacterLogic Character { get; set; }
        ClonesLogic Clones { get; set; }
        ContactsLogic Contacts { get; set; }
        CosmeticsLogic Cosmetics { get; set; }
        ContractsLogic Contracts { get; set; }
        CorporationLogic Corporation { get; set; }
        DogmaLogic Dogma { get; set; }
        FactionWarfareLogic FactionWarfare { get; set; }
        FittingsLogic Fittings { get; set; }
        FleetsLogic Fleets { get; set; }
        FreelanceJobsLogic FreelanceJobs { get; set; }
        IncursionsLogic Incursions { get; set; }
        IndustryLogic Industry { get; set; }
        InsuranceLogic Insurance { get; set; }
        KillmailsLogic Killmails { get; set; }
        LocationLogic Location { get; set; }
        LoyaltyLogic Loyalty { get; set; }
        MailLogic Mail { get; set; }
        MarketLogic Market { get; set; }
        MetaLogic Meta { get; set; }
        MilitaryCampaignsLogic MilitaryCampaigns { get; set; }
        PlanetaryInteractionLogic PlanetaryInteraction { get; set; }
        RoutesLogic Routes { get; set; }
        SearchLogic Search { get; set; }
        SkillsLogic Skills { get; set; }
        SovereigntyLogic Sovereignty { get; set; }
        StatusLogic Status { get; set; }
        StructuresLogic Structures { get; set; }
        UniverseLogic Universe { get; set; }
        UserInterfaceLogic UserInterface { get; set; }
        WalletLogic Wallet { get; set; }
        WarsLogic Wars { get; set; }
    }
}
