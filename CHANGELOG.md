# Changelog

## Unreleased — next major

The first release since `2023.12.12`. It modernizes the target frameworks and
dependencies and reworks how per-call state (the authorized character, the ETag,
cancellation, pagination) is passed. **Every consumer needs code changes** — see
_Migration_ below.

### Breaking changes

**Target frameworks & dependencies**

- Targets are now `netstandard2.0;net8.0` (was `netcoreapp3.1;netstandard2.0;net462;net47;net471;net472;net48;net6.0;net7.0`).
  Consumers on a dropped runtime resolve the `netstandard2.0` assembly.
- `Microsoft.IdentityModel.Tokens` / `System.IdentityModel.Tokens.Jwt` `6.14.1` → `8.22.0`.
- `Microsoft.Extensions.*` `2.0.0` → `8.0.x`; added `Microsoft.Extensions.Http`.
- `Newtonsoft.Json` → `13.0.4`. Removed the explicit `System.Net.Http` package
  reference (in-box on both targets).

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

### Added

- `EsiCallOptions.CancellationToken` — honoured by every request.
- `EsiCallOptions.IfNoneMatch` + `EsiResponse<T>.ETag` — per-call conditional
  requests (`304 Not Modified`).
- `EsiCallOptions.OnTokenRefreshed` — when set, an authenticated call whose
  access token is within a minute of expiry is transparently refreshed with its
  refresh token first; the `AuthorizedCharacterData` is updated in place and the
  callback fires so you can persist the rotated refresh token.
- `EsiErrorLimitHandler` — reads `X-Esi-Error-Limit-*` and blocks further sends
  on the client until the window resets; throws `EsiErrorLimitException` on
  `420`. Wired by `AddEsi`.
- `AddEsi(Action<EsiConfig>)` overload.
- Resilience-ready: chain `.AddStandardResilienceHandler()` (Polly) off the
  `IHttpClientBuilder` that `AddEsi` returns, after referencing
  `Microsoft.Extensions.Http.Resilience`.

### Fixed

- Constructing `EsiClient` under Blazor WebAssembly no longer throws
  "Operation is not supported on this platform" (#77) — `AutomaticDecompression`
  is only set when the handler supports it.
- `EsiResponse<T>` no longer reads the response body synchronously with
  `.Result`, and a `204` from an endpoint not in its message table no longer
  throws `KeyNotFoundException` internally.
- `SsoLogic.Verify()` reuses the injected `HttpClient` instead of `new`-ing one
  per call, and no longer swallows every exception.

### Migration

| Before | After |
| --- | --- |
| `client.SetCharacterData(data);`<br>`await client.Clones.List();` | `await client.Clones.List(new() { Character = data });` |
| `client.SetIfNoneMatchHeader(etag);`<br>`await client.Universe.Names(ids);` | `await client.Universe.Names(ids, new() { IfNoneMatch = etag });` |
| `await client.Assets.ForCharacter(2);` | `await client.Assets.ForCharacter(new() { Character = data, Page = 2 });` |
| `services.AddEsi(cfg);` *(returns `IServiceCollection`)* | `services.AddEsi(cfg);` *(returns `IHttpClientBuilder`)* — optionally `.AddStandardResilienceHandler()` |
| `dogma.Attribute(id).Data.Name` | `dogma.Attribute(id).Data.Name` — the payload type is now `AttributeInfo` |
| `var d = dogma.DynamicItem(t, i).Data;` *(was `Effect`)* | `var d = dogma.DynamicItem(t, i).Data;` *(now `DynamicItem`)* |
| `var c = await sso.Verify(token);`<br>`if (c.CharacterID == 0) { /* failed */ }` | `try { var c = await sso.Verify(token); }`<br>`catch (InvalidOperationException) { /* failed */ }` |
