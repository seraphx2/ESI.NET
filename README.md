[![CI](https://github.com/seraphx2/ESI.NET/actions/workflows/ci.yml/badge.svg)](https://github.com/seraphx2/ESI.NET/actions/workflows/ci.yml) [![NuGet](https://img.shields.io/nuget/v/ESI.NET.svg)](https://www.nuget.org/packages/ESI.NET)

# What is ESI.NET?

**ESI.NET** is a .NET wrapper for the [Eve Online ESI API](https://esi.evetech.net/). This wrapper simplifies the process of integrating ESI into your .NET application.

### Resources
* [Discord - E.N](https://discord.gg/SvdN39f) - This channel is where you can contact me (Psianna Archeia) for questions and where automated webhook notifications will be pushed for github and when builds are completed. (If you have Discord, this is the preferred way to contact me concerning ESI.NET. I **DO NOT** monitor Slack anymore for ESI.NET issues.)
* [Tweetfleet - #esi](https://tweetfleet.slack.com/messages/C30KX8UUX/) - This is the official slack channel to speak with CCP devs (and developers) concerning ESI.
* [ESI Application Keys](https://developers.eveonline.com/)
* [ESI OpenAPI Definition](https://esi.evetech.net/meta/openapi.json)
* [ESI-Docs](https://docs.esi.evetech.net/) ([source](https://github.com/esi/esi-docs)) - This is the best documentation concerning ESI and the SSO process.

It is extremely important to not solely rely on ESI.NET. You may need to refer to the official specifications to understand what data is expected to be provided. For example, in some instances, ESI.NET will ask for specific values in the endpoint method and construct the JSON object that needs to be sent in the POST request body because it is a simple object that requires a few values. Some of the more complex objects will need to be constructed with anonymous objects by the developer and this can be determined when the endpoint method requires an `object` instead of an `int` or a `string`. Refer to the official documentation and construct the anonymous object to reflect what is expected as Json.NET will be able to convert that anonymous object into the appropriate JSON data.

## ESI.NET on NuGet
https://www.nuget.org/packages/ESI.NET

`dotnet add package ESI.NET `

## Client Instantiation
ESI.NET is Dependency Injection compatible. There are a few parts required to set this up properly in a .NET Standard/Core application:

### .NET Standard (Dependency Injection)
In your appsettings.json, add the following object and fill it in appropriately:
```json
"EsiConfig": {
    "EsiUrl": "https://esi.evetech.net/",
    "DataSource": "Tranquility",
    "ClientId": "**********",
    "SecretKey": "**********",
    "CallbackUrl": "",
    "UserAgent": ""
  }
```
*For your protection (and mine), you are required to supply a user_agent value. This can be your character name and/or project name. CCP will be more likely to contact you than just cut off access to ESI if you provide something that can identify you within the New Eden galaxy. Without this property populated, the wrapper will not work.*

Register the client. `AddEsi` binds the config, registers `IEsiClient` as a typed
`HttpClient` (via `IHttpClientFactory`), and returns an `IHttpClientBuilder`:
```cs
services.AddEsi(Configuration.GetSection("EsiConfig"));

// or configure inline:
services.AddEsi(esi => { esi.EsiUrl = "https://esi.evetech.net/"; esi.DataSource = DataSource.Tranquility; esi.UserAgent = "my-app / me"; });
```

Opt in to Polly resilience by adding the `Microsoft.Extensions.Http.Resilience`
package and chaining off the returned builder:
```cs
services.AddEsi(Configuration.GetSection("EsiConfig"))
        .AddStandardResilienceHandler();
```

Then take `IEsiClient` in your constructor:
```cs
private readonly IEsiClient _client;
public ApiTestController(IEsiClient client) { _client = client; }
```

### .NET Framework
If you are using a .NET Standard-compatible .NET Framework application, you can instantiate the client in this manner:

```cs
IOptions<EsiConfig> config = Options.Create(new EsiConfig()
{
    EsiUrl = "https://esi.evetech.net/",
    DataSource = DataSource.Tranquility,
    ClientId = "**********",
    SecretKey = "**********",
    CallbackUrl = "",
    UserAgent = ""
});

EsiClient client = new EsiClient(config);
```
*For your protection (and mine), you are required to supply a user_agent value. This can be your character name and/or project name. CCP will be more likely to contact you than just cut off access to ESI if you provide something that can identify you within the New Eden galaxy. Without this property populated, the wrapper will not work.*

NOTE: You will need to import `Microsoft.Extensions.Options` to accomplish the above.

### Public endpoint
```cs
EsiResponse<List<ResolvedInfo>> response = await _client.Universe.Names(new List<long>
{
    1590304510, 99006319, 20000006
});
```

### Per-call options
Every endpoint method takes a trailing `EsiCallOptions`. It is **optional** on
public endpoints and **required** on authenticated ones:
```cs
public sealed class EsiCallOptions
{
    public AuthorizedCharacterData Character { get; set; } // required for authenticated endpoints
    public CancellationToken CancellationToken { get; set; }
    public string IfNoneMatch { get; set; }               // conditional request; a match -> 304, no body
    public int? Page { get; set; }                        // paginated endpoints
}
```
```cs
var page2 = await _client.Universe.Groups(new() { Page = 2, CancellationToken = ct });

var fresh = await _client.Market.RegionOrders(region_id, new() { IfNoneMatch = previous.ETag });
if (fresh.StatusCode == HttpStatusCode.NotModified) { /* use your cache */ }
```

## SSO Example

### SSO Login URL generator
ESI.NET has a helper method to generate the URL required to authenticate a character or authorize roles (by providing a `List<string>` of scopes) for the Eve Online SSO. You should also provide a value for "state" that you verify when it is returned (it will be included in the callback).
```cs
var url = _client.SSO.CreateAuthenticationUrl();
```

### Initial SSO Token Request
`Verify` throws `InvalidOperationException` if the token fails validation.
```cs
SsoToken token = await _client.SSO.GetToken(GrantType.AuthorizationCode, code);
AuthorizedCharacterData authChar = await _client.SSO.Verify(token);
// persist authChar (at least RefreshToken + CharacterOwnerHash) in your database.
// On every re-login, compare the fresh CharacterOwnerHash to your stored one — a
// mismatch means the character was transferred and the old data must be discarded.
```
### Refresh Token Request
```cs
SsoToken token = await _client.SSO.GetToken(GrantType.RefreshToken, authChar.RefreshToken);
```
### Authenticated request
Pass the stored character on the call:
```cs
var wallet = await _client.Wallet.CharacterWallet(new() { Character = authChar });
```

### Transparent token refresh
An authenticated call whose access token is within a minute of expiry is
refreshed with its refresh token before the request goes out; `authChar` is
updated in place. EVE rotates the refresh token, so you must persist the updated
value.

**Once, via DI (recommended).** Implement `IEsiTokenRefreshSink` — normal
constructor injection works — and register it; it then covers every
authenticated call:
```cs
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
services.AddEsi(Configuration.GetSection("EsiConfig"));
```
The handler resolves the sink in a fresh scope each time it fires, so a scoped
`DbContext` is safe.

**Per call**, for one-offs or non-DI use:
```cs
var wallet = await _client.Wallet.CharacterWallet(new()
{
    Character = authChar,
    OnTokenRefreshed = async c => { /* persist c */ },
});
```

---

See [CHANGELOG.md](CHANGELOG.md) for the migration guide from `2023.12.12`
(`SetCharacterData` / `SetIfNoneMatchHeader` removal, the Dogma model split, TFM
and dependency changes).
