# What is ESI.NET?

**ESI.NET** is a .NET wrapper for the [Eve Online ESI API](https://esi.tech.ccp.is/latest/). This wrapper simplifies the process of integrating ESI into your .NET application.

### Resources
* [Tweetfleet - #esi](https://tweetfleet.slack.com/messages/C30KX8UUX/) - This is the official slack channel to speak with CCP devs (and developers) concerning ESI (and where I can be found as Psianna Archeia).
* [ESI Application Keys](https://developers.eveonline.com/)
* [ESI Swagger Definition](https://esi.tech.ccp.is/swagger.json)
* [Third-party Documentation](https://eveonline-third-party-documentation.readthedocs.io/en/latest/) - This is the best-known documentation concerning the SSO process.

It is extremely important to not solely rely on ESI.NET. You may need to refer to the official specifications to understand what data is expected to be provided. For example, in some instances, ESI.NET will ask for specific values in the endpoint method and construct the JSON object that needs to be sent in the POST request body because it is a simple object that requires a few values. Some of the more complex objects will need to be constructed with anonymous objects by the developer and this can be determined when the endpoint method requires an `object` instead of an `int` or a `string`. Refer to the official documentation and construct the anonymous object to reflect what is expected as Json.NET will be able to convert that anonymous object into the appropriate JSON data.

## ESI.NET on NuGet
https://www.nuget.org/packages/ESI.NET

`dotnet add package ESI.NET `

## Public Endpoint Example
ESI.NET is now Dependency Injection compatible. There are a few parts required to set this up propery in a .NET Standard/Core application:

### .NET Standard
In your appsettings.json, add the following object and fill it in appropriately:
```json
"ESIConfig": {
    "EsiUrl": "https://esi.tech.ccp.is/",
    "DataSource": "Tranquility",
    "ClientId": "**********",
    "SecretKey": "**********",
    "CallbackUrl": "",
    "UserAgent": ""
  }
```
Inject the ESIConfig object into your configuration in Startup.cs in the ConfigureServices method:
```cs
services.Configure<ESIConfig>(Configuration.GetSection("ESIConfig"));
```

Lastly, access the configuration in your class constructor:
```cs
IESIClient _client;
public ApiTestController(IESIClient client) { _client = client; }

```

### .NET Framework
If you are using a .NET Standard-compatible .NET Framework application, you can instatiate the client in this manner:

```cs
IOptions<ESIConfig> config = Options.Create(new ESIConfig()
{
    EsiUrl = "https://esi.tech.ccp.is/",
    DataSource = DataSource.Tranquility,
    ClientId = "**********",
    SecretKey = "**********",
    CallbackUrl = "",
    UserAgent = ""
});

ESIClient client = new ESIClient(config);
```

NOTE: You will need to import `Microsoft.Extensions.Options` to accomplish the above.

### Endpoint Example
Accessing a public endpoint is extremely simple. Instantiate an instance of the client.
```cs
ApiResponse response = _client.Universe.Names(new List<long>()
{
    1590304510,
    99006319,
    20000006
}).Result;
```
For your protection, please provide a user_agent value. This can be your character name and/or project name. CCP will be more likely to contact you than just cut off access to ESI if you provide something that can identify you within the New Eden galaxy.

## SSO Example

### SSO Login URL generator
ESI.NET has a helper method to generat the URL required to authenticate a character or authorize roles (by providing a List<string> of scopes) in the Eve Online SSO.
```cs
var url = _client.SSO.CreateAuthenticationUrl();
```

### Initial SSO Token Request
```cs
SSOToken token = await _client.SSO.GetToken(GrantType.AuthorizationCode, code);
AuthorizedCharacter auth_char = await SSO.Verify(token.Value);
```
### Refresh Token Request
```cs
SSOToken token = await _client.SSO.GetToken(GrantType.RefreshToken, auth_char.RefreshToken);
```