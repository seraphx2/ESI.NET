# Changelog

## Unreleased — next major

The first release since `2023.12.12`. It modernizes the target frameworks and
dependencies and reworks how per-call state (the authorized character, the ETag,
cancellation, pagination) is passed. **Every consumer needs code changes** — see
**[MIGRATION.md](MIGRATION.md)**.

### Breaking changes

**Target frameworks & dependencies**

- Targets are now `netstandard2.0;net8.0` (was `netcoreapp3.1;netstandard2.0;net462;net47;net471;net472;net48;net6.0;net7.0`).
  Consumers on a dropped runtime resolve the `netstandard2.0` assembly.
- `Microsoft.IdentityModel.Tokens` / `System.IdentityModel.Tokens.Jwt` `6.14.1` → `8.22.0`.
- `Microsoft.Extensions.*` `2.0.0` → `8.0.x`; added `Microsoft.Extensions.Http`.
- `Newtonsoft.Json` → `13.0.4`. Removed the explicit `System.Net.Http` package
  reference (in-box on both targets).

**Parameter naming**

- Every `snake_case` method parameter (mirroring ESI's own field names -
  `alliance_id`, `character_id`, `max_war_id`, ...) is renamed to `camelCase`
  (`allianceId`, `characterId`, `maxWarId`). This only affects callers using
  named arguments (`Information(alliance_id: 123)` → `Information(allianceId: 123)`);
  positional calls are unaffected. `EsiClient`'s constructor parameters are
  `config` / `client` (were `_config` / `_client`).

**Per-call options**

- Every endpoint method takes a trailing `EsiCallOptions` parameter
  (`{ Character, CancellationToken, IfNoneMatch, Page }`). It is **required** on
  authenticated endpoints (a missing character is now a compile error) and
  optional (`= null`) on public ones.
- `EsiClient.SetCharacterData(AuthorizedCharacterData)` is **removed**. Pass the
  character per call: `client.Assets.ForCharacter(new() { Character = data })`.
- `EsiClient.SetIfNoneMatchHeader(string)` is **removed** (and the process-wide
  `static` ETag field it set — a concurrency bug — is gone). Use
  `new EsiCallOptions { IfNoneMatch = response.ETag }`.
- The `int page = 1` parameter is **removed** from the 13 paginated methods.
  Use `new EsiCallOptions { Page = 2 }`.

**Dependency injection**

- `AddEsi(...)` now returns `IHttpClientBuilder` (was `IServiceCollection`) and
  registers `IEsiClient` as a typed `HttpClient` via `IHttpClientFactory` — the
  lifetime is the typed-client default, no longer `AddScoped`.
- A `HttpClient` you pass to `new EsiClient(config, client)` is used as-is; the
  `X-User-Agent` / `Accept` headers are only added to a client the constructor
  creates itself. `new EsiClient(config)` (no client) still self-configures.
- The manual `Accept-Encoding: gzip, deflate` request headers are gone;
  decompression is handled by the primary handler's `AutomaticDecompression`.

**Dogma models**

- `ESI.NET.Models.Dogma.Attribute` / `Effect` are now the id-value pairs
  (`{ attribute_id, value }` / `{ effect_id, is_default }`) that appear on a type
  or a dynamic item.
- The full definitions from `/dogma/attributes/{id}/` and `/dogma/effects/{id}/`
  are new types `AttributeInfo` / `EffectInfo`. `DogmaLogic.Attribute()` /
  `Effect()` return those.
- `DogmaLogic.DynamicItem()` returns `EsiResponse<DynamicItem>` (was mistyped
  `EsiResponse<Effect>` and returned an all-empty object). `DynamicItem` is now
  `public`.
- `ESI.NET.Models.Universe.Attribute` / `Effect` are removed; `Universe.Type`
  binds to the `Dogma` types.

**Other**

- `SsoLogic.Verify()` now throws `InvalidOperationException` when access-token
  validation fails, instead of returning a blank `AuthorizedCharacterData`.
- `EsiResponse<T>`'s public constructor is removed; it is built by an internal
  async factory. Consumers never constructed it.
- `EsiResponse<T>.Message` on a `204 No Content` response is now always the
  literal `"No Content"`. It used to look up a hand-maintained dictionary of
  per-endpoint friendly strings (`"Fleet invitation sent"`, `"Mail deleted"`,
  ...) that covered a small, already-stale fraction of endpoints and fell
  further behind with every new one added; `StatusCode` + `Endpoint` already
  say everything a consumer needs.

**Removed endpoints** (CCP deleted them from ESI)

- `client.Bookmarks.*` and `client.Opportunities.*` — the whole logic classes,
  and `IEsiClient.Bookmarks` / `IEsiClient.Opportunities`.
- `client.Character.Names(ids)` — use `client.Universe.Names(ids)`.
- `client.Character.ChatChannels()`.
- `Search.Query` lost its `SearchType` argument (only character search remains)
  and now takes `EsiCallOptions` as a required argument.

**Response types corrected to match the spec**

- `Corporation.Standings(...)` returns `EsiResponse<List<Standing>>` (was a single
  `Standing`); `Universe.AsteroidBelt(id)` returns `EsiResponse<AsteroidBelt>`
  (was a `List<>`).
- `CustomsOffice` tax-rate fields (`CorporationTaxRate`, `ExcellentStandingTaxRate`,
  `GoodStandingTaxRate`) are `decimal`; `ColonyLayout.Route.Quantity` is `decimal`.
- New fields: `Information.Title`, `CustomsOffice.TypeId`, `Stat.Pilots`,
  `Order.IssuedBy`, `FleetInfo.FleetBossId`, `Job.LocationId`,
  `Stat.SystemsControlled`, `Universe.Structure.OwnerId`.
- `ItemLocation` (asset `.../locations`) exposes `Position` instead of flat
  `X` / `Y` / `Z` — the spec nests them and the flat fields never bound.
- Removed fields ESI no longer sends on any endpoint: `Information.AncestryId`,
  `Order.AccountId`, `Order.IsCorp` (`Order.IsCorporation` stays),
  `IDLookup.Structures`.
- `ResolvedInfoCategory.Structure` removed — `POST /universe/names` no longer
  resolves structure IDs, so `ResolvedInfo.Category` can never be `structure`.

**`int` → `long` throughout**

- ESI types every integer — response fields, path/query/body parameters — as
  int64, and EVE IDs are already at the int32 boundary. Every `int` is now `long`:
  response-model properties (`int[]` → `long[]`), `AuthorizedCharacterData` ids,
  the bare id-list/scalar return types (`Alliance.All`, `Corporation.Members`,
  `Dogma.Attributes`/`Effects`, `Mail.New`, `Routes.Map`, `Wars.All`, the
  `Universe.*` id lists, …), and every id-shaped method parameter.
- Passing an `int` to a `long` parameter still compiles. What needs a change is
  reading a value back (`long id = character.Data.CorporationId;`) or a
  `List<int>` / `int[]` built to pass in — the compiler flags each one.

**ESI compatibility-date versioning**

- Requests now send `X-Compatibility-Date` (this build pins `2026-08-18`, in
  `ESI.NET.EsiVersion.CompatibilityDate`) and `X-Tenant` instead of the `/latest`
  route prefix and `?datasource=`. ESI freezes each dated snapshot; consumers
  stay on this contract until they upgrade the package.
- `Routes.Map` is now a `POST` to `/route/{origin_system_id}/{destination_system_id}`
  with `avoid_systems` / `connections` in the body and an `EsiResponse<RouteResult>`
  return; `RoutesFlag` values are `Shorter` / `Safer` / `LessSecure`.
- `Sovereignty.Systems` → `/sovereignty/systems` (`EsiResponse<SovereigntySystems>`);
  `Sovereignty.Structures` removed.
- `Information`: `title` removed, `AchievementScore` / `CharacterTitleId` /
  `CorporationTitle` added. `Corporation`: `FactionId` → `EnlistedFactionId`,
  `TaxRate` → `TaxRates` object, `FriendlyFire` / `Palette` / `State` / `Type` added.

### Added

- **Every ESI endpoint added since 2020 is now wrapped** (coverage 233/233 at
  compatibility date `2026-08-18`). New accessors: `FreelanceJobs`,
  `MilitaryCampaigns`, `Structures` (skyhooks / sovereignty hubs / mercenary
  dens), `Cosmetics` (SKINR + Paragon Hub), `Meta` (changelog / compatibility
  dates / name / status). New methods on existing accessors:
  `Corporation.Projects*` (Corporation Projects), `Character.AccessLists*`,
  `Character.MercenaryTacticalOperations*`. See MIGRATION.md §13 for the scopes.

- `EsiCallOptions.CancellationToken` — honoured by every request.
- `EsiCallOptions.IfNoneMatch` + `EsiResponse<T>.ETag` — per-call conditional
  requests (`304 Not Modified`).
- **Transparent access-token refresh.** An authenticated call whose access token
  is within a minute of expiry is refreshed with its refresh token before the
  request goes out (done by `EsiTokenRefreshHandler` in the pipeline); the
  `AuthorizedCharacterData` is updated in place. The rotated refresh token is
  surfaced two ways, and both fire:
  - `IEsiTokenRefreshSink` — implement it, register one
    (`services.AddScoped<IEsiTokenRefreshSink, YourSink>()`), and it covers every
    authenticated call. This is the DI-friendly "persist once" hook.
  - `EsiCallOptions.OnTokenRefreshed` — a per-call `Func<AuthorizedCharacterData, Task>`
    for one-offs or non-DI use.
- `EsiErrorLimitHandler` — reads `X-Esi-Error-Limit-*` and blocks further sends
  on the client until the window resets; throws `EsiErrorLimitException` on
  `420`. Wired by `AddEsi`.
- `AddEsi(Action<EsiConfig>)` overload.
- Resilience-ready: chain `.AddStandardResilienceHandler()` (Polly) off the
  `IHttpClientBuilder` that `AddEsi` returns, after referencing
  `Microsoft.Extensions.Http.Resilience`.
- A symbol package (`.snupkg`) is now published alongside the `.nupkg`, with
  Source Link - step into ESI.NET's actual source while debugging.
- The package's license is now declared as the `MIT` SPDX expression (was a
  packed `LICENSE.txt`) - nuget.org links straight to the canonical license
  text. The repository's `LICENSE.txt` is unaffected.

### Tooling & tests

Before this release the library had no automated tests and no way to notice ESI
had changed short of a consumer filing a bug. That is now covered.

- **Test suite.** Unit tests (`tests/ESI.NET.Tests`) cover the request/response
  pipeline, the delegating handlers, and SSO token validation. Live integration
  tests (`tests/ESI.NET.IntegrationTests`) run a spread of public endpoints and
  an authenticated SSO probe — token exchange, JWKS validation, a bearer call,
  and transparent refresh — against the real API on a schedule.
- **Spec-drift check** (`tools/SpecCheck`, daily). Compares the wrapper against
  the live ESI OpenAPI document: endpoints it still exposes that ESI dropped,
  endpoints ESI added that it doesn't cover, and every `EsiResponse<T>` model
  against its endpoint's 200 schema (type, integer width, enum values, missing
  and extra properties, object-vs-array shape). API changes surface here.
- **CI and release automation.** GitHub Actions builds every PR and `dev`
  commit; a merge to `master` publishes a CalVer version to nuget.org via
  Trusted Publishing (OIDC, no stored key), mirrors it to GitHub Packages, tags
  the release, and posts to Discord. A manual dispatch publishes a `-beta`
  prerelease to nuget.org only.
- `tools/MintToken` — a one-shot local utility that runs the SSO flow and prints
  a refresh token for the integration probe.

### Fixed

- Constructing `EsiClient` under Blazor WebAssembly no longer throws
  "Operation is not supported on this platform" (#77) — `AutomaticDecompression`
  is only set when the handler supports it.
- `EsiResponse<T>` no longer reads the response body synchronously with
  `.Result`, and a `204` from an endpoint not in its message table no longer
  throws `KeyNotFoundException` internally.
- `SsoLogic.Verify()` reuses the injected `HttpClient` instead of `new`-ing one
  per call, and no longer swallows every exception.
- `EsiResponse<T>` trims the response body before deciding whether it is JSON.
  Endpoints whose body ends in a newline no longer come back as a `200` with
  `Data` null and the payload in `Message`; bare-scalar bodies (a wallet
  balance, a CSPA cost) now bind to `Data`.
- Enum values that never matched ESI: `ContractType.Loan` (`"loan "` → `"loan"`),
  `Contested.Vulnerable` (`"vulnerable "` → `"vulnerable"`),
  `StructureServiceState.Cleanup` (`"cleamup"` → `"cleanup"`), and a missing
  `EventResponse.NotResponded` (`"not_responded"`).
- `EsiResponse<T>.Expires` / `.LastModified` no longer parse the raw header
  string with a culture-sensitive `DateTime.Parse` (could misparse or throw
  under a non-default host locale); they now read `HttpContentHeaders`' own
  already-parsed `Expires` / `LastModified` properties. `Pages`,
  `ErrorLimitRemain`, and `ErrorLimitReset` parse with `CultureInfo.InvariantCulture`.
- `DataSource` and `GrantType` declared `[EnumMember]` wire values but no
  `StringEnumConverter` — a direct JSON serialization of either would have
  emitted the underlying int, not the ESI string. `Extensions.ToEsiValue()`
  (used to build query strings / headers / form bodies from an enum) is
  rewritten to resolve through that converter instead of hand-rolled
  reflection, so it can't silently drift from what real serialization
  produces; `SearchCategory` gets the same converter for consistency.
- 15 public methods that dereferenced a required reference-type parameter
  without checking it now throw a clear `ArgumentNullException` naming the
  parameter instead of an unrelated `NullReferenceException` (or, for
  `SsoLogic.Verify(null)` specifically, getting folded into the generic
  `InvalidOperationException` wrapper meant for a malformed *token*, not a
  caller bug): `EsiClient`/`EsiHeadersHandler`/`EsiTokenRefreshHandler`/
  `SsoLogic`'s `config` constructor parameters, `EsiHeadersHandler`'s and
  `EsiTokenRefreshHandler`'s `SendAsync(request, ...)`, `SsoLogic.Verify(token)`,
  `Extensions.ToEsiValue(e)`, `AssetsLogic`'s four `itemIds` parameters,
  `KillmailsLogic.Information(killmailHash, ...)`, and `UniverseLogic.Names(anyIds)`
  / `.IDs(names)`.
- The remaining 123 "validate this parameter" analyzer hits were all the same
  parameter - every endpoint method's trailing `EsiCallOptions options = null`
  - and are documented `[SuppressMessage]`s, not fixes: `options` is
  deliberately optional, `null` is the correct value for the overwhelming
  majority of calls, and the default is already substituted centrally in
  `EsiRequest.Execute<T>`. Throwing here would break the library's single
  most common call shape.

### Migration

See **[MIGRATION.md](MIGRATION.md)**.
