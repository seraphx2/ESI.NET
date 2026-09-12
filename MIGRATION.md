# Migrating to the 2026 release

The first release since `2023.12.12`. **Every consumer needs code changes.** They
are mechanical — mostly adding a trailing `new() { ... }` to endpoint calls — and
the compiler finds nearly all of them.

New to ESI.NET? Skip this file and start from the [README](README.md).

---

## 1. Endpoint calls take a trailing `EsiCallOptions`

This is the bulk of the work. Every endpoint method gained a final
`EsiCallOptions` parameter:

```csharp
public sealed class EsiCallOptions
{
    public AuthorizedCharacterData Character { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public string IfNoneMatch { get; set; }
    public int? Page { get; set; }
}
```

- **Public endpoints** — optional. `await client.Universe.Names(ids);` still compiles.
- **Authenticated endpoints** — the character travels in the options now, and a
  missing one is a **compile error**:

```csharp
// before
client.SetCharacterData(authChar);
await client.Wallet.CharacterWallet();

// after
await client.Wallet.CharacterWallet(new() { Character = authChar });
```

## 2. `SetCharacterData` and `SetIfNoneMatchHeader` are removed

Both set process-wide state on the shared client (the ETag field was a
concurrency bug). Pass the value on the call:

| before | after |
| --- | --- |
| `client.SetCharacterData(data);`<br>`await client.Clones.List();` | `await client.Clones.List(new() { Character = data });` |
| `client.SetIfNoneMatchHeader(etag);`<br>`await client.Universe.Names(ids);` | `await client.Universe.Names(ids, new() { IfNoneMatch = etag });` |

## 3. The `page` parameter is removed

The 13 paginated methods dropped `int page = 1`:

```csharp
// before
await client.Assets.ForCharacter(2);

// after
await client.Assets.ForCharacter(new() { Character = data, Page = 2 });
```

## 4. `SsoLogic.Verify()` throws on failure

It used to return a blank `AuthorizedCharacterData` (`CharacterID == 0`) when
token validation failed. It now throws `InvalidOperationException`:

```csharp
// before
var c = await sso.Verify(token);
if (c.CharacterID == 0) { /* failed */ }

// after
try { var c = await sso.Verify(token); }
catch (InvalidOperationException) { /* failed */ }
```

## 5. Dogma models

- `ESI.NET.Models.Dogma.Attribute` / `Effect` are now the small id-value pairs
  that appear on a type or dynamic item (`{ attribute_id, value }` /
  `{ effect_id, is_default }`).
- The full records from `/dogma/attributes/{id}/` and `/dogma/effects/{id}/` are
  new types **`AttributeInfo`** / **`EffectInfo`**, and that is what
  `DogmaLogic.Attribute()` / `Effect()` return. Member names are unchanged —
  `dogma.Attribute(id).Data.Name` still works, only the type name differs.
- `DogmaLogic.DynamicItem()` now returns `EsiResponse<DynamicItem>` (it was
  mistyped and returned an all-empty object).
- `ESI.NET.Models.Universe.Attribute` / `Effect` are gone; `Universe.Type` binds
  to the `Dogma` types.

## 6. Dependency injection

`AddEsi(...)` returns `IHttpClientBuilder` instead of `IServiceCollection` and
registers `IEsiClient` as a typed `HttpClient`. If you only wrote
`services.AddEsi(cfg);`, nothing changes. If you chained off the result, it is
now an `IHttpClientBuilder` — which is what lets you add
`.AddStandardResilienceHandler()`.

A `HttpClient` passed to `new EsiClient(config, client)` is used verbatim now;
the constructor only adds `X-User-Agent` / `Accept` to a client it creates
itself. The manual `Accept-Encoding` request headers are gone — the primary
handler decompresses.

## 7. Target frameworks and dependencies

- Targets are now `netstandard2.0;net8.0`. The old explicit `net462`–`net48`,
  `net6.0`, `net7.0`, and `netcoreapp3.1` targets are gone; consumers on those
  runtimes resolve the `netstandard2.0` assembly. Usually nothing to do.
- `Microsoft.IdentityModel.Tokens` / `System.IdentityModel.Tokens.Jwt`
  `6.14.1` → `8.22.0`; `Microsoft.Extensions.*` `2.0.0` → `8.0.x`;
  `Newtonsoft.Json` → `13.0.4`. Check for version conflicts if your app also
  references these directly.

## 8. `EsiResponse<T>` has no public constructor

It is built by an internal async factory now. Consumers never constructed it.

## 9. Removed endpoints

CCP deleted these from ESI, so the wrapper methods are gone:

| Removed | Replacement / note |
| --- | --- |
| `client.Bookmarks.*` (all of `BookmarksLogic`) | none — bookmark read access was removed in the 2019 ACL rework |
| `client.Opportunities.*` (all of `OpportunitiesLogic`) | none — the feature was retired |
| `client.Character.Names(ids)` | `client.Universe.Names(ids)` (`POST /universe/names`) |
| `client.Character.ChatChannels()` | none |
| `client.Search.Query(SearchType.Public, ...)` | character search only (see below) |

`IEsiClient.Bookmarks` and `IEsiClient.Opportunities` are gone from the interface.

`Search.Query` lost its `SearchType` argument — only `/characters/{character_id}/search/`
still exists, so it is always an authenticated character search and now takes
`EsiCallOptions` as a required third argument:

```csharp
// before
await client.Search.Query(SearchType.Character, "Jita", SearchCategory.SolarSystem,
                          options: new() { Character = c });

// after
await client.Search.Query("Jita", SearchCategory.SolarSystem, new() { Character = c });
```

## 10. A few response types changed shape

The wrapper was returning the wrong container or CLR type on these:

| Method | before | after |
| --- | --- | --- |
| `Corporation.Standings(...)` | `EsiResponse<Standing>` | `EsiResponse<List<Standing>>` |
| `Universe.AsteroidBelt(id)` | `EsiResponse<List<AsteroidBelt>>` | `EsiResponse<AsteroidBelt>` |
| `CustomsOffice.CorporationTaxRate` | `string` | `decimal` |
| `CustomsOffice.ExcellentStandingTaxRate` / `GoodStandingTaxRate` | `long` / `int` | `decimal` |
| `ColonyLayout.Route.Quantity` | `long` | `decimal` |
| `ItemLocation` (asset `/locations`) | flat `X` / `Y` / `Z` | nested `Position` (`.Position.X` …) |

New fields to match the spec: `Information.Title`, `CustomsOffice.TypeId`,
`Stat.Pilots`, `Order.IssuedBy`, `FleetInfo.FleetBossId`, `Job.LocationId`
(corporation industry jobs), `Stat.SystemsControlled` (`GET /fw/stats`),
`Universe.Structure.OwnerId`.

Removed fields ESI no longer returns anywhere: `Information.AncestryId`,
`Order.AccountId`, `Order.IsCorp` (use the still-present `Order.IsCorporation`),
`IDLookup.Structures`. They only ever deserialized to their default.

`ResolvedInfoCategory.Structure` is removed — `POST /universe/names` (the only
endpoint that populates `ResolvedInfo.Category`) stopped resolving structure IDs,
so that value can no longer come back.

## 11. Every integer is now `long`

ESI's schema types **every** integer — response fields, path/query/body
parameters, the lot — as a 64-bit integer, and EVE's own IDs are already at the
32-bit boundary. So `int` is now `long` throughout:

- Every `int` property on a response model (`int[]` → `long[]`), including
  `AuthorizedCharacterData.CharacterID` / `AllianceID` / `CorporationID` /
  `FactionID`.
- Methods returning a bare list or scalar of IDs — `Alliance.All`,
  `Alliance.Corporations`, `Clones.Implants`, `Contacts.Add`, `Corporation.NpcCorps`,
  `Corporation.Members`, `Corporation.MemberLimit`, `Dogma.Attributes`,
  `Dogma.Effects`, `Mail.New`, `Market.Groups`, `Market.Types`, `Routes.Map`,
  `Wars.All`, every `Universe.*` id-list method — now return `EsiResponse<long[]>`
  / `EsiResponse<long>`.
- Every id-shaped method parameter — `Universe.Type(long type_id)`,
  `Character.Information(long character_id)`, `Contracts.ContractItems(long contract_id)`,
  `Wallet.CorporationJournal(long division)`, `Character.Affiliation(long[] character_ids)`,
  `Universe.Names(List<long> any_ids)`, and so on.

Passing an `int` to a `long` parameter needs no change — it widens on its own.
What the compiler flags is the other direction: a value you *read back* is now
`long`, so `int id = character.Data.CorporationId;` becomes `long id = …`, and a
`List<int>` / `int[]` you built to pass in becomes `List<long>` / `long[]`.

## 12. ESI compatibility-date versioning

ESI is moving off the `/latest` route prefix and the `?datasource=` query
parameter and onto a per-request **`X-Compatibility-Date`** header that pins a
frozen dated snapshot of the API. This build targets **`2026-08-18`**
(`ESI.NET.EsiVersion.CompatibilityDate`). It is not a config knob — you stay on
this contract until you upgrade the package, and each bump ships with its own
migration note. Nothing to do; every request now sends the header, and the
`DataSource` travels as `X-Tenant` instead of `?datasource=`.

Consequences of moving from the old (effectively `2020-01-01`) view to
`2026-08-18`:

- `GET /route/{origin}/{destination}` is now
  `POST /route/{origin_system_id}/{destination_system_id}`. `Routes.Map` keeps
  its name but takes `origin_system_id` / `destination_system_id`, moves
  `avoid_systems` / `connections` into the request body (`connections` is now
  `[from, to]` pairs), adds `security_penalty`, and returns
  `EsiResponse<RouteResult>` (`RouteResult.Route` is the system-id array).
  `RoutesFlag` values are `Shorter` / `Safer` / `LessSecure`.
- `Sovereignty.Systems` moved from `/sovereignty/map` to `/sovereignty/systems`
  (the rework that folded in `/sovereignty/structures`) and returns
  `EsiResponse<SovereigntySystems>`. `Sovereignty.Structures` is removed.
- `Character` (`Information`): `title` is gone; `AchievementScore`,
  `CharacterTitleId`, `CorporationTitle` are new.
- `Corporation`: `FactionId` → `EnlistedFactionId`; `TaxRate` (a `decimal`) →
  `TaxRates` (`{ Isk, LoyaltyPoint }`); `FriendlyFire`, `Palette`, `State`,
  `Type` are new.

## 13. New endpoint groups

Everything ESI added since 2020 is now wrapped. New accessors on `IEsiClient`:

| Accessor | Endpoints | Scope(s) |
| --- | --- | --- |
| `FreelanceJobs` | public listing, character & corporation views | `esi-characters.read_freelance_jobs.v1`, `esi-corporations.read_freelance_jobs.v1` |
| `MilitaryCampaigns` | campaigns, objectives, the character's objective progress | `esi.activity.char:read` |
| `Structures` | skyhooks, sovereignty hubs, mercenary dens, raidable skyhooks | `esi-structures.read_corporation.v1`, `esi-structures.read_character.v1` |
| `Cosmetics` | SKINR designs, licenses, components; Paragon Hub listings | `esi.cosmetic.char:read` |
| `Meta` | `/meta/changelog`, `/meta/compatibility-dates`, `/meta/name`, `/meta/status` | — |

Plus new methods on existing accessors: `Corporation.Projects` /
`Corporation.Project` / `Corporation.ProjectContributors` /
`Corporation.ProjectContribution` (`esi-corporations.read_projects.v1`);
`Character.AccessLists` / `Character.AccessList` (`esi-access.read_lists.v1`);
`Character.MercenaryTacticalOperations` / `Character.MercenaryTacticalOperation`
(`esi-activities.read_character.v1`).

## 14. Parameter names are `camelCase`

Every method parameter that mirrored ESI's own `snake_case` field name
(`alliance_id`, `character_id`, `max_war_id`, ...) is renamed to `camelCase`
(`allianceId`, `characterId`, `maxWarId`). Only affects callers using named
arguments:

```csharp
// before
client.Alliance.Information(alliance_id: 99005338);
// after
client.Alliance.Information(allianceId: 99005338);
```

Positional calls (`client.Alliance.Information(99005338)`, by far the common
case) need no change. `EsiClient`'s own constructor parameters are `config` /
`client` (were `_config` / `_client`).

## 15. `RoutesFlag` is renamed `RoutePreference`

`ESI.NET.Enumerations.RoutesFlag` (the `flag` parameter on `RoutesLogic.Map`)
is renamed `RoutePreference` — it isn't actually a `[Flags]` enum, and the name
didn't mirror anything in ESI's own API, so there was no reason to keep a name
that collides with the reserved `Flag` type-name suffix:

```csharp
// before
await client.Routes.Map(from, to, RoutesFlag.Safer);
// after
await client.Routes.Map(from, to, RoutePreference.Safer);
```

---

## What did not change

- Method names, namespaces, and the `_client.Category.Method(...)` shape.
- Model member names (the Dogma type *names* changed; their members did not).
- The SSO flow — `CreateAuthenticationUrl` → `GetToken` → `Verify` — apart from
  `Verify` now throwing.
