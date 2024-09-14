using ESI.NET.Logic;
using ESI.NET.Models.SSO;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using ESI.NET.Interfaces.Logic;

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
        /// <param name="_client">The <see cref="HttpClient"/> to use for HTTP requests.</param>
        public EsiClient(IOptions<EsiConfig> _config, HttpClient _client = null)
        {
            config = _config.Value;
            client = _client ?? new HttpClient(new HttpClientHandler
            {
// Switch to All which adds brotli encoding for .net core due to https://github.com/ccpgames/sso-issues/issues/81
#if NET
                AutomaticDecompression = DecompressionMethods.All
#else
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
#endif
            });

            // Enforce user agent value
            if (string.IsNullOrEmpty(config.UserAgent))
                throw new ArgumentException(
                    "For your protection, please provide an X-User-Agent value. This can be your character name and/or project name. CCP will be more likely to contact you rather than just cut off access to ESI if you provide something that can identify you within the New Eden galaxy.");
            client.DefaultRequestHeaders.Add("X-User-Agent", config.UserAgent);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));


            SSO = new SsoLogic(client, config);
            Alliance = new AllianceLogic(client, config);
            Assets = new AssetsLogic(client, config);
            Bookmarks = new BookmarksLogic(client, config);
            Calendar = new CalendarLogic(client, config);
            Character = new CharacterLogic(client, config);
            Clones = new ClonesLogic(client, config);
            Contacts = new ContactsLogic(client, config);
            Contracts = new ContractsLogic(client, config);
            Corporation = new CorporationLogic(client, config);
            Dogma = new DogmaLogic(client, config);
            FactionWarfare = new FactionWarfareLogic(client, config);
            Fittings = new FittingsLogic(client, config);
            Fleets = new FleetsLogic(client, config);
            Incursions = new IncursionsLogic(client, config);
            Industry = new IndustryLogic(client, config);
            Insurance = new InsuranceLogic(client, config);
            Killmails = new KillmailsLogic(client, config);
            Location = new LocationLogic(client, config);
            Loyalty = new LoyaltyLogic(client, config);
            Mail = new MailLogic(client, config);
            Market = new MarketLogic(client, config);
            Opportunities = new OpportunitiesLogic(client, config);
            PlanetaryInteraction = new PlanetaryInteractionLogic(client, config);
            Routes = new RoutesLogic(client, config);
            Search = new SearchLogic(client, config);
            Skills = new SkillsLogic(client, config);
            Sovereignty = new SovereigntyLogic(client, config);
            Status = new StatusLogic(client, config);
            Universe = new UniverseLogic(client, config);
            UserInterface = new UserInterfaceLogic(client, config);
            Wallet = new WalletLogic(client, config);
            Wars = new WarsLogic(client, config);
        }

        public ISsoLogic SSO { get; set; }
        public IAllianceLogic Alliance { get; set; }
        public IAssetsLogic Assets { get; set; }
        public IBookmarksLogic Bookmarks { get; set; }
        public ICalendarLogic Calendar { get; set; }
        public ICharacterLogic Character { get; set; }
        public IClonesLogic Clones { get; set; }
        public IContactsLogic Contacts { get; set; }
        public IContractsLogic Contracts { get; set; }
        public ICorporationLogic Corporation { get; set; }
        public IDogmaLogic Dogma { get; set; }
        public IFactionWarfareLogic FactionWarfare { get; set; }
        public IFleetsLogic Fleets { get; set; }
        public IFittingsLogic Fittings { get; set; }
        public IIncursionsLogic Incursions { get; set; }
        public IIndustryLogic Industry { get; set; }
        public IInsuranceLogic Insurance { get; set; }
        public IKillmailsLogic Killmails { get; set; }
        public ILocationLogic Location { get; set; }
        public ILoyaltyLogic Loyalty { get; set; }
        public IMailLogic Mail { get; set; }
        public IMarketLogic Market { get; set; }
        public IOpportunitiesLogic Opportunities { get; set; }
        public IPlanetaryInteractionLogic PlanetaryInteraction { get; set; }
        public IRoutesLogic Routes { get; set; }
        public ISearchLogic Search { get; set; }
        public ISkillsLogic Skills { get; set; }
        public IStatusLogic Status { get; set; }
        public ISovereigntyLogic Sovereignty { get; set; }
        public IUniverseLogic Universe { get; set; }
        public IUserInterfaceLogic UserInterface { get; set; }
        public IWalletLogic Wallet { get; set; }
        public IWarsLogic Wars { get; set; }


        public void SetCharacterData(AuthorizedCharacterData data)
        {
            Assets = new AssetsLogic(client, config, data);
            Bookmarks = new BookmarksLogic(client, config, data);
            Calendar = new CalendarLogic(client, config, data);
            Character = new CharacterLogic(client, config, data);
            Clones = new ClonesLogic(client, config, data);
            Contacts = new ContactsLogic(client, config, data);
            Contracts = new ContractsLogic(client, config, data);
            Corporation = new CorporationLogic(client, config, data);
            FactionWarfare = new FactionWarfareLogic(client, config, data);
            Fittings = new FittingsLogic(client, config, data);
            Fleets = new FleetsLogic(client, config, data);
            Industry = new IndustryLogic(client, config, data);
            Killmails = new KillmailsLogic(client, config, data);
            Location = new LocationLogic(client, config, data);
            Loyalty = new LoyaltyLogic(client, config, data);
            Mail = new MailLogic(client, config, data);
            Market = new MarketLogic(client, config, data);
            Opportunities = new OpportunitiesLogic(client, config, data);
            PlanetaryInteraction = new PlanetaryInteractionLogic(client, config, data);
            Search = new SearchLogic(client, config, data);
            Skills = new SkillsLogic(client, config, data);
            UserInterface = new UserInterfaceLogic(client, config, data);
            Wallet = new WalletLogic(client, config, data);
            Universe = new UniverseLogic(client, config, data);
        }

        [Obsolete]
        public void SetIfNoneMatchHeader(string eTag)
            => EsiRequest.ETag = eTag;
    }

    public interface IEsiClient
    {
        ISsoLogic SSO { get; set; }
        IAllianceLogic Alliance { get; set; }
        IAssetsLogic Assets { get; set; }
        IBookmarksLogic Bookmarks { get; set; }
        ICalendarLogic Calendar { get; set; }
        ICharacterLogic Character { get; set; }
        IClonesLogic Clones { get; set; }
        IContactsLogic Contacts { get; set; }
        IContractsLogic Contracts { get; set; }
        ICorporationLogic Corporation { get; set; }
        IDogmaLogic Dogma { get; set; }
        IFactionWarfareLogic FactionWarfare { get; set; }
        IFittingsLogic Fittings { get; set; }
        IFleetsLogic Fleets { get; set; }
        IIncursionsLogic Incursions { get; set; }
        IIndustryLogic Industry { get; set; }
        IInsuranceLogic Insurance { get; set; }
        IKillmailsLogic Killmails { get; set; }
        ILocationLogic Location { get; set; }
        ILoyaltyLogic Loyalty { get; set; }
        IMailLogic Mail { get; set; }
        IMarketLogic Market { get; set; }
        IOpportunitiesLogic Opportunities { get; set; }
        IPlanetaryInteractionLogic PlanetaryInteraction { get; set; }
        IRoutesLogic Routes { get; set; }
        ISearchLogic Search { get; set; }
        ISkillsLogic Skills { get; set; }
        ISovereigntyLogic Sovereignty { get; set; }
        IStatusLogic Status { get; set; }
        IUniverseLogic Universe { get; set; }
        IUserInterfaceLogic UserInterface { get; set; }
        IWalletLogic Wallet { get; set; }
        IWarsLogic Wars { get; set; }

        void SetCharacterData(AuthorizedCharacterData data);
        void SetIfNoneMatchHeader(string eTag);
    }
}