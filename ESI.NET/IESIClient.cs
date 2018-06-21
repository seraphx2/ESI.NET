using ESI.NET.Logic;
using ESI.NET.Models.SSO;

namespace ESI.NET
{
    public interface IESIClient
    {
        SSOLogic SSO { get; set; }
        AllianceLogic Alliance { get; set; }
        AssetsLogic Assets { get; set; }
        BookmarksLogic Bookmarks { get; set; }
        CalendarLogic Calendar { get; set; }
        CharacterLogic Character { get; set; }
        ClonesLogic Clones { get; set; }
        ContactsLogic Contacts { get; set; }
        ContractsLogic Contracts { get; set; }
        CorporationLogic Corporation { get; set; }
        DogmaLogic Dogma { get; set; }
        FactionWarfareLogic FactionWarfare { get; set; }
        FittingsLogic Fittings { get; set; }
        FleetsLogic Fleets { get; set; }
        IncursionsLogic Incursions { get; set; }
        IndustryLogic Industry { get; set; }
        InsuranceLogic Insurance { get; set; }
        KillmailsLogic Killmails { get; set; }
        LocationLogic Location { get; set; }
        LoyaltyLogic Loyalty { get; set; }
        MailLogic Mail { get; set; }
        MarketLogic Market { get; set; }
        OpportunitiesLogic Opportunities { get; set; }
        PlanetaryInteractionLogic PlanetaryInteraction { get; set; }
        RoutesLogic Routes { get; set; }
        SearchLogic Search { get; set; }
        SkillsLogic Skills { get; set; }
        SovereigntyLogic Sovereignty { get; set; }
        StatusLogic Status { get; set; }
        UniverseLogic Universe { get; set; }
        UserInterfaceLogic UserInterface { get; set; }
        WalletLogic Wallet { get; set; }
        WarsLogic Wars { get; set; }

        void SetCharacterData(AuthorizedCharacterData characterData);
    }
}