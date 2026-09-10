[![CI](https://github.com/seraphx2/ESI.NET/actions/workflows/ci.yml/badge.svg)](https://github.com/seraphx2/ESI.NET/actions/workflows/ci.yml) [![NuGet](https://img.shields.io/nuget/v/ESI.NET.svg)](https://www.nuget.org/packages/ESI.NET)

# ESI.NET

A .NET wrapper for the [EVE Online ESI API](https://esi.evetech.net/). Every ESI
endpoint is a typed method; SSO, transparent token refresh, conditional requests,
and the ESI error limit are handled for you.

> **Upgrading from `2023.12.12` or earlier?** Every consumer needs (mechanical)
> code changes — see **[MIGRATION.md](MIGRATION.md)**.

## Install

```
dotnet add package ESI.NET
```

## Setup

`ESI.NET` registers as a typed `HttpClient` through `IHttpClientFactory`.

Add an `EsiConfig` section to `appsettings.json`:

```json
"EsiConfig": {
  "EsiUrl": "https://esi.evetech.net/",
  "DataSource": "Tranquility",
  "UserAgent": "my-app / my-character-name",
  "ClientId": "",
  "SecretKey": "",
  "CallbackUrl": ""
}
```

- **`UserAgent` is required.** Use something that identifies you — a character
  and/or project name. CCP will contact you before cutting off access if they can
  tell who you are. The client throws on construction without it.
- `DataSource` is `Tranquility` or `Singularity` (sent as ESI's `X-Tenant`).
- `ClientId` / `SecretKey` / `CallbackUrl` are only needed for
  [authenticated requests](#authenticated-requests-sso).

Every request pins an ESI **compatibility date** (`X-Compatibility-Date`), so the
response shapes this package binds to stay fixed until you upgrade it. The
current target is in `ESI.NET.EsiVersion.CompatibilityDate`.

Register it and take `IEsiClient` in your constructor:

```csharp
// Program.cs
services.AddEsi(builder.Configuration.GetSection("EsiConfig"));

// or configure inline:
services.AddEsi(esi =>
{
    esi.EsiUrl = "https://esi.evetech.net/";
    esi.DataSource = DataSource.Tranquility;
    esi.UserAgent = "my-app / my-character-name";
});
```

```csharp
public class MarketService
{
    private readonly IEsiClient _esi;
    public MarketService(IEsiClient esi) => _esi = esi;
}
```

<details>
<summary>Without dependency injection (.NET Framework, console apps)</summary>

```csharp
using Microsoft.Extensions.Options;

var config = Options.Create(new EsiConfig
{
    EsiUrl = "https://esi.evetech.net/",
    DataSource = DataSource.Tranquility,
    UserAgent = "my-app / my-character-name",
});

var client = new EsiClient(config);
```

</details>

## Making a request

```csharp
EsiResponse<List<ResolvedInfo>> response = await _esi.Universe.Names(new List<int>
{
    1590304510, 99006319, 20000006
});

foreach (var item in response.Data)
    Console.WriteLine($"{item.Category}: {item.Name}");
```

Every call returns `EsiResponse<T>`:

| member | |
| --- | --- |
| `Data` | the deserialized payload (`T`) |
| `StatusCode` | the HTTP status |
| `Message` | ESI's error string on failure, or a status message |
| `Exception` | set if deserialization failed (`Data` is then `null`) |
| `Pages` | total pages on a paginated endpoint (`X-Pages`) |
| `ETag` | for the next conditional request |
| `Expires` / `LastModified` | cache headers |

### Per-call options

Every method takes an optional trailing `EsiCallOptions`:

```csharp
// a specific page, with cancellation
var groups = await _esi.Universe.Groups(new() { Page = 2, CancellationToken = ct });

// conditional request — 304 and no body if nothing changed
var prices = await _esi.Market.Prices(new() { IfNoneMatch = cached.ETag });
if (prices.StatusCode == HttpStatusCode.NotModified) { /* use your cache */ }
```

### POST bodies

Simple POST endpoints take the values as method arguments. Where a method asks
for `object`, build an anonymous object shaped like the JSON ESI expects (the
[ESI reference](https://docs.esi.evetech.net/) has the schema) — Json.NET
serializes it.

## Authenticated requests (SSO)

1. Register an application at
   [developers.eveonline.com](https://developers.eveonline.com/) and put its
   **Client ID**, **Secret Key**, and **Callback URL** into `EsiConfig`.

2. Send the user to EVE SSO with the scopes you need and a `state` value you
   check on the way back:

   ```csharp
   var url = _esi.SSO.CreateAuthenticationUrl(
       new List<string> { "esi-wallet.read_character_wallet.v1" },
       state: "a-value-you-verify-later");
   // redirect the browser to `url`
   ```

3. On the callback, exchange the `code` and validate it:

   ```csharp
   SsoToken token = await _esi.SSO.GetToken(GrantType.AuthorizationCode, code);
   AuthorizedCharacterData authChar = await _esi.SSO.Verify(token);
   // Verify throws InvalidOperationException if the token is bad.
   ```

4. Persist `authChar` — at least `RefreshToken` and `CharacterOwnerHash`. On
   every re-login, compare the fresh `CharacterOwnerHash` to your stored one; a
   mismatch means the character was transferred and the stored data must be
   discarded.

5. Pass the character on authenticated calls:

   ```csharp
   var wallet = await _esi.Wallet.CharacterWallet(new() { Character = authChar });
   ```

### Desktop, console, and GUI apps

No web server receives the callback, so register a **loopback** callback
(`http://localhost:<port>/callback`) and catch the redirect with a short-lived
`HttpListener`. A shipped app can't keep a client secret — register a
**native / PKCE** application, leave `EsiConfig.SecretKey` empty, and use
`SSO.GenerateChallengeCode()` with the `challengeCode` / `codeChallenge`
overloads of `CreateAuthenticationUrl` and `GetToken`. Everything from `GetToken`
on is identical to the web flow, and transparent refresh handles the no-secret
case.

[`tools/MintToken`](tools/MintToken) is a complete, runnable example of this flow.

### Token refresh

An authenticated call whose access token is within a minute of expiry is
refreshed automatically before the request goes out, and `authChar` is updated in
place. **EVE rotates the refresh token, so you must persist the new value.**

Handle that once, for every call, with an `IEsiTokenRefreshSink`:

```csharp
public class DbTokenSink : IEsiTokenRefreshSink
{
    private readonly MyDbContext _db;
    public DbTokenSink(MyDbContext db) => _db = db;

    public async Task OnRefreshedAsync(AuthorizedCharacterData c)
    {
        _db.Characters.Update(c);
        await _db.SaveChangesAsync();
    }
}

services.AddScoped<IEsiTokenRefreshSink, DbTokenSink>();
services.AddEsi(builder.Configuration.GetSection("EsiConfig"));
```

The handler resolves the sink in a fresh scope each time it fires, so a scoped
`DbContext` is safe. For one-offs or non-DI code, set `OnTokenRefreshed` on the
call's `EsiCallOptions` instead.

## Resilience

Add retries, timeouts, and a circuit breaker — reference
`Microsoft.Extensions.Http.Resilience` and chain off `AddEsi`:

```csharp
services.AddEsi(builder.Configuration.GetSection("EsiConfig"))
        .AddStandardResilienceHandler();
```

## Links

- [CHANGELOG.md](CHANGELOG.md) · [MIGRATION.md](MIGRATION.md)
- [Discord](https://discord.gg/SvdN39f) — questions, and where GitHub / build notifications post
- [ESI-Docs](https://docs.esi.evetech.net/) — the ESI and SSO reference

## Development

Maintainer tooling lives in `tools/` — a daily [spec-drift check](tools/SpecCheck)
against the live ESI OpenAPI document — and `tests/` — unit tests plus live
integration tests.
