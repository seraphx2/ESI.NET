using ESI.NET.Enumerations;
using ESI.NET.Logic;
using System;

namespace ESI.NET
{
    public class ESIClient
    {
        private ESIConfig _config;

        /// <summary>
        /// If you are accessing an endpoint that requires Authorization, you must provide both an SSOToken and AuthorizedCharacter object
        /// </summary>
        /// <param name="source"></param>
        /// <param name="user_agent"></param>
        /// <param name="token"></param>
        /// <param name="auth_char"></param>
        public ESIClient(DataSource source, string user_agent, string token = null, AuthorizedCharacter auth_char = null)
            : this(new ESIConfig {
                DataSource = source,
                UserAgent = user_agent,
                Token = token,
                AuthorizedCharacter = auth_char
            }) { }
        public ESIClient() : this(new ESIConfig()) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public ESIClient(ESIConfig config)
        {
            _config = config;

            if (config.Token != null && config.AuthorizedCharacter != null)
                _config.AuthorizedCharacter = config.AuthorizedCharacter;
            else if (config.Token != null && config.AuthorizedCharacter == null)
                throw new Exception("If providing a token, you must provide authorized character data.");

            Alliance = new AllianceLogic(_config);
            Assets = new AssetsLogic(_config);
            Bookmarks = new BookmarksLogic(_config);
            Character = new CharacterLogic(_config);
            Corporation = new CorporationLogic(_config);
            Dogma = new DogmaLogic(_config);
            FactionWarfare = new FactionWarfareLogic(_config);
            Fleets = new FleetsLogic(_config);
            Incursions = new IncursionsLogic(_config);
            Industry = new IndustryLogic(_config);
            Insurance = new InsuranceLogic(_config);
            Killmails = new KillmailsLogic(_config);
            Mail = new MailLogic(_config);
            Market = new MarketLogic(_config);
            Opportunities = new OpportunitiesLogic(_config);
            PlanetaryInteraction = new PlanetaryInteractionLogic(_config);
            Routes = new RoutesLogic(_config);
            Search = new SearchLogic(_config);
            Skills = new SkillsLogic(_config);
            Sovereignty = new SovereigntyLogic(_config);
            Status = new StatusLogic(_config);
            Universe = new UniverseLogic(_config);
            UserInterface = new UserInterfaceLogic(_config);
            Wars = new WarsLogic(_config);
        }

        public AllianceLogic Alliance { get; set; }
        public AssetsLogic Assets { get; set; }
        public BookmarksLogic Bookmarks { get; set; }
        public CharacterLogic Character { get; set; }
        public CorporationLogic Corporation { get; set; }
        public DogmaLogic Dogma { get; set; }
        public FactionWarfareLogic FactionWarfare { get; set; }
        public FleetsLogic Fleets { get; set; }
        public IncursionsLogic Incursions { get; set; }
        public IndustryLogic Industry { get; set; }
        public InsuranceLogic Insurance { get; set; }
        public KillmailsLogic Killmails { get; set; }
        public MailLogic Mail { get; set; }
        public MarketLogic Market { get; set; }
        public OpportunitiesLogic Opportunities { get; set; }
        public PlanetaryInteractionLogic PlanetaryInteraction { get; set; }
        public RoutesLogic Routes { get; set; }
        public SearchLogic Search { get; set; }
        public SkillsLogic Skills { get; set; }
        public StatusLogic Status { get; set; }
        public SovereigntyLogic Sovereignty { get; set; }
        public UniverseLogic Universe { get; set; }
        public UserInterfaceLogic UserInterface { get; set; }
        public WarsLogic Wars { get; set; }


        public async void SetNewCharacter(string token)
        {
            _config.Token = token;
            _config.AuthorizedCharacter = await SSO.Verify(token, true);
        }
    }
}
