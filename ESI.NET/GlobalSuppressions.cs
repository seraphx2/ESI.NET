// This file centralizes every project-wide analyzer suppression so the source files
// themselves stay clean. Each block below states ONE shared reason once - nothing here
// is "we don't know / don't care", every suppression is a deliberate, explained exception
// to a rule that otherwise stays fully armed for everything else in the codebase.

using System.Diagnostics.CodeAnalysis;

// CA1062 - every one of these is the same parameter: an endpoint method's trailing
// `EsiCallOptions options = null`. It's deliberately optional - the vast majority of calls
// need no special options at all - and the null case is already substituted centrally in
// EsiRequest.Execute<T>. Throwing ArgumentNullException, CA1062's literal suggested fix,
// would turn the library's single most common call shape into a guaranteed crash.
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.AssetsLogic.ForCharacter(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.AssetsLogic.ForCorporation(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.AssetsLogic.LocationsForCharacter(System.Collections.Generic.IReadOnlyList{System.Int64},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.AssetsLogic.LocationsForCorporation(System.Collections.Generic.IReadOnlyList{System.Int64},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.AssetsLogic.NamesForCharacter(System.Collections.Generic.IReadOnlyList{System.Int64},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.AssetsLogic.NamesForCorporation(System.Collections.Generic.IReadOnlyList{System.Int64},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CalendarLogic.Event(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CalendarLogic.Events(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CalendarLogic.Respond(System.Int64,ESI.NET.Enumerations.EventResponse,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CalendarLogic.Responses(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.AccessList(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.AccessLists(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.AgentsResearch(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.Blueprints(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.CSPA(System.Object,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.ContactNotifications(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.Fatigue(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.Medals(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.MercenaryTacticalOperation(System.String,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.MercenaryTacticalOperations(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.Notifications(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.Roles(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.Standings(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CharacterLogic.Titles(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ClonesLogic.Implants(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ClonesLogic.List(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContactsLogic.Add(System.Int64[],System.Decimal,System.Int64[],System.Nullable{System.Boolean},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContactsLogic.Delete(System.Int64[],ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContactsLogic.LabelsForAlliance(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContactsLogic.LabelsForCharacter(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContactsLogic.LabelsForCorporation(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContactsLogic.ListForAlliance(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContactsLogic.ListForCharacter(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContactsLogic.ListForCorporation(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContactsLogic.Update(System.Int64[],System.Decimal,System.Int64[],System.Nullable{System.Boolean},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContractsLogic.CharacterContractBids(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContractsLogic.CharacterContractItems(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContractsLogic.CharacterContracts(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContractsLogic.CorporationContractBids(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContractsLogic.CorporationContractItems(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.ContractsLogic.CorporationContracts(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Blueprints(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.ContainerLogs(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Divisions(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Facilities(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Medals(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.MedalsIssued(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.MemberLimit(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.MemberTitles(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.MemberTracking(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Members(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Project(System.String,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.ProjectContribution(System.String,System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.ProjectContributors(System.String,System.String,System.String,System.Nullable{System.Int32},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Projects(System.String,System.String,System.String,System.Nullable{System.Int32},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Roles(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.RolesHistory(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Shareholders(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Standings(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Starbase(System.Int64,System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Starbases(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Structures(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CorporationLogic.Titles(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CosmeticsLogic.MyComponents(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CosmeticsLogic.MyLicenses(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.CosmeticsLogic.MyParagonListings(System.String,System.String,System.Nullable{System.Int32},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.FactionWarfareLogic.StatsForCharacter(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.FactionWarfareLogic.StatsForCorporation(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.FittingsLogic.Add(System.Object,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.FittingsLogic.Delete(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.FittingsLogic.List(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.FleetsLogic.FleetInfo(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.FleetsLogic.MoveCharacter(System.Int64,System.Int64,ESI.NET.Enumerations.FleetRole,System.Int64,System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.FreelanceJobsLogic.CharacterParticipation(System.String,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.FreelanceJobsLogic.CorporationParticipants(System.String,System.String,System.String,System.Nullable{System.Int32},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.FreelanceJobsLogic.ForCharacter(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.FreelanceJobsLogic.ForCorporation(System.String,System.String,System.Nullable{System.Int32},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.IndustryLogic.Extractions(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.IndustryLogic.JobsForCharacter(System.Boolean,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.IndustryLogic.JobsForCorporation(System.Boolean,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.IndustryLogic.MiningLedger(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.IndustryLogic.ObservedMining(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.IndustryLogic.Observers(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.KillmailsLogic.ForCharacter(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.KillmailsLogic.ForCorporation(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.LocationLogic.Location(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.LocationLogic.Online(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.LocationLogic.Ship(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.LoyaltyLogic.Points(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MailLogic.Delete(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MailLogic.DeleteLabel(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MailLogic.Headers(System.Int64[],System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MailLogic.Labels(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MailLogic.MailingLists(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MailLogic.New(System.Object[],System.String,System.String,System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MailLogic.NewLabel(System.String,System.String,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MailLogic.Retrieve(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MailLogic.Update(System.Int64,System.Nullable{System.Boolean},System.Int64[],ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MarketLogic.CharacterOrderHistory(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MarketLogic.CharacterOrders(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MarketLogic.CorporationOrderHistory(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MarketLogic.CorporationOrders(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MilitaryCampaignsLogic.CharacterObjective(System.String,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.MilitaryCampaignsLogic.CharacterObjectives(System.String,System.String,System.Nullable{System.Int32},ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.PlanetaryInteractionLogic.Colonies(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.PlanetaryInteractionLogic.ColonyLayout(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.PlanetaryInteractionLogic.CorporationCustomsOffices(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.SearchLogic.Query(System.String,ESI.NET.Enumerations.SearchCategory,ESI.NET.EsiCallOptions,System.Boolean,System.String)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.SkillsLogic.Attributes(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.SkillsLogic.List(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.SkillsLogic.Queue(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.StructuresLogic.MercenaryDen(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.StructuresLogic.MercenaryDens(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.StructuresLogic.Skyhook(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.StructuresLogic.Skyhooks(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.StructuresLogic.SovereigntyHub(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.StructuresLogic.SovereigntyHubs(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.WalletLogic.CharacterJournal(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.WalletLogic.CharacterTransactions(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.WalletLogic.CharacterWallet(ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.WalletLogic.CorporationJournal(System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.WalletLogic.CorporationTransactions(System.Int64,System.Int64,ESI.NET.EsiCallOptions)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Scope = "member", Target = "~M:ESI.NET.Logic.WalletLogic.CorporationWallets(ESI.NET.EsiCallOptions)")]

// CA1031 - EsiResponse<T>'s constructor is the boundary between untrusted ESI response bytes
// and a typed object. Any parsing failure - a header format ESI didn't document, a future
// spec deviation, an unanticipated JSON shape - has to land on Exception/Message instead of
// throwing out of Execute<T>, or every caller has to wrap every call. Narrowing to a specific
// exception list would defeat that: the next ESI quirk nobody's hit yet would throw straight
// through instead of being caught here.
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:ESI.NET.EsiResponse`1.#ctor(System.Net.Http.HttpResponseMessage,System.String,System.String)")]

// CA1031 - SsoLogic.Verify's affiliation-lookup catch is explicitly best-effort: the JWT has
// already been validated by the time this runs, so a failure here (ESI down, a transient
// network error, an unexpected response shape) must not invalidate an otherwise-good token.
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:ESI.NET.SsoLogic.Verify(ESI.NET.Models.SSO.SsoToken)~System.Threading.Tasks.Task{ESI.NET.Models.SSO.AuthorizedCharacterData}")]

// CA1724 - Extensions collides with the unrelated Microsoft.Extensions.* namespace family,
// not anything in this library. Renaming this public static class is a breaking change for a
// purely cosmetic rule; not worth it for what CA1724 catches here.
[assembly: SuppressMessage("Naming", "CA1724:Type names should not match namespaces", Scope = "type", Target = "~T:ESI.NET.Extensions")]

// CA1716 / CA1711 - these five public DTOs are named after ESI's own concepts (an event, a
// module, a structure, a dogma attribute). Renaming any of them to dodge a reserved-keyword or
// reserved-suffix collision in some other .NET language would break fidelity to the API they
// model for a purely cosmetic rule - the same call already made for CA1724 on Extensions above.
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Scope = "type", Target = "~T:ESI.NET.Models.Calendar.Event")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Scope = "type", Target = "~T:ESI.NET.Models.Character.Module")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Scope = "type", Target = "~T:ESI.NET.Models.Corporation.Structure")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Scope = "type", Target = "~T:ESI.NET.Models.Universe.Structure")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Scope = "type", Target = "~T:ESI.NET.Models.Dogma.Attribute")]

// CA1307 / CA2249 - the suggested StringComparison/Contains(char) overloads don't exist on
// netstandard2.0 (verified by compiling against it directly), so fixing these for real means an
// #if NET / #else split on four one-line, already-safe string operations (a query-parameter
// placeholder swap, an ETag quote-strip, a JSON-body first-character sniff) for no behavioral
// change - none of them are culture- or comparison-sensitive to begin with.
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison for clarity", Scope = "member", Target = "~M:ESI.NET.EsiRequest.Execute``1(System.Net.Http.HttpClient,ESI.NET.EsiConfig,ESI.NET.EsiRequest.RequestSecurity,System.Net.Http.HttpMethod,System.String,ESI.NET.EsiCallOptions,System.Collections.Generic.Dictionary{System.String,System.String},System.String[],System.Object)~System.Threading.Tasks.Task{ESI.NET.EsiResponse{``0}}")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison for clarity", Scope = "member", Target = "~M:ESI.NET.EsiResponse`1.#ctor(System.Net.Http.HttpResponseMessage,System.String,System.String)")]
[assembly: SuppressMessage("Performance", "CA2249:Consider using 'string.Contains' instead of 'string.IndexOf'", Scope = "member", Target = "~M:ESI.NET.EsiResponse`1.#ctor(System.Net.Http.HttpResponseMessage,System.String,System.String)")]

// CA1056 / CA1055 - EsiConfig.EsiUrl/CallbackUrl, Corporation.Url, and CreateAuthenticationUrl's
// return type are all plain strings by design rather than System.Uri. EsiUrl/CallbackUrl are
// bound from configuration and manipulated as strings (TrimEnd, EscapeDataString) throughout;
// Corporation.Url is a passthrough of whatever ESI sends back verbatim. A strict Uri parse can
// throw where a lenient string never would, and converting would break every consumer's config
// setup, the README examples, and MintToken for a modest type-safety gain.
[assembly: SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Scope = "member", Target = "~P:ESI.NET.EsiConfig.EsiUrl")]
[assembly: SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Scope = "member", Target = "~P:ESI.NET.EsiConfig.CallbackUrl")]
[assembly: SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Scope = "member", Target = "~P:ESI.NET.Models.Corporation.Corporation.Url")]
[assembly: SuppressMessage("Design", "CA1055:URI-like return values should not be strings", Scope = "member", Target = "~M:ESI.NET.SsoLogic.CreateAuthenticationUrl(System.Collections.Generic.IReadOnlyList{System.String},System.String,System.String)~System.String")]

// CA2000 - the EsiTokenRefreshHandler created here is handed directly to HttpClient's
// constructor, which takes ownership of it (disposeHandler defaults to true) and disposes it
// transitively when _client is disposed. _client's own disposal is handled correctly by
// EsiClient.Dispose() below, which only disposes it when this constructor created it (as it did
// on this path) - the analyzer just can't see that far across the two methods.
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "~M:ESI.NET.EsiClient.#ctor(Microsoft.Extensions.Options.IOptions{ESI.NET.EsiConfig},System.Net.Http.HttpClient)")]
