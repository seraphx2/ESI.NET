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
        readonly HttpClient client;
        readonly EsiConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="EsiClient"/> class.
        /// </summary>
        /// <param name="_config">The configuration parameters of the <see cref="EsiClient"/>.</param>
        /// <param name="_client">
        /// The <see cref="HttpClient"/> to use. When supplied (including via <c>AddEsi</c>'s
        /// <see cref="System.Net.Http.IHttpClientFactory"/> pipeline) it is used as-is — the caller
        /// / pipeline is responsible for the <c>X-User-Agent</c> and <c>Accept</c> headers and for
        /// content decompression. When omitted, a default client is created and configured here.
        /// </param>
        public EsiClient(IOptions<EsiConfig> _config, HttpClient _client = null)
        {
            config = _config.Value;

            if (_client != null)
                client = _client;
            else
            {
                if (string.IsNullOrWhiteSpace(config.UserAgent))
                    throw new ArgumentException("EsiConfig.UserAgent is required. Set it to something that identifies your app (character and/or project name) so CCP can contact you rather than cut off ESI access.");

                // No DI pipeline here, so wire the token-refresh handler in manually. Without an
                // IServiceScopeFactory it honours EsiCallOptions.OnTokenRefreshed but not a sink.
                var handler = new EsiTokenRefreshHandler(_config) { InnerHandler = CreateDefaultHandler() };
                client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("X-User-Agent", config.UserAgent);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }


            SSO = new SsoLogic(client, config);
            Alliance = new AllianceLogic(client, config);
            Assets = new AssetsLogic(client, config);
            Calendar = new CalendarLogic(client, config);
            Character = new CharacterLogic(client, config);
            Clones = new ClonesLogic(client, config);
            Contacts = new ContactsLogic(client, config);
            Cosmetics = new CosmeticsLogic(client, config);
            Contracts = new ContractsLogic(client, config);
            Corporation = new CorporationLogic(client, config);
            Dogma = new DogmaLogic(client, config);
            FactionWarfare = new FactionWarfareLogic(client, config);
            Fittings = new FittingsLogic(client, config);
            Fleets = new FleetsLogic(client, config);
            FreelanceJobs = new FreelanceJobsLogic(client, config);
            Incursions = new IncursionsLogic(client, config);
            Industry = new IndustryLogic(client, config);
            Insurance = new InsuranceLogic(client, config);
            Killmails = new KillmailsLogic(client, config);
            Location = new LocationLogic(client, config);
            Loyalty = new LoyaltyLogic(client, config);
            Mail = new MailLogic(client, config);
            Market = new MarketLogic(client, config);
            Meta = new MetaLogic(client, config);
            MilitaryCampaigns = new MilitaryCampaignsLogic(client, config);
            PlanetaryInteraction = new PlanetaryInteractionLogic(client, config);
            Routes = new RoutesLogic(client, config);
            Search = new SearchLogic(client, config);
            Skills = new SkillsLogic(client, config);
            Sovereignty = new SovereigntyLogic(client, config);
            Status = new StatusLogic(client, config);
            Structures = new StructuresLogic(client, config);
            Universe = new UniverseLogic(client, config);
            UserInterface = new UserInterfaceLogic(client, config);
            Wallet = new WalletLogic(client, config);
            Wars = new WarsLogic(client, config);
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
