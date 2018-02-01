# What is ESI.NET?

**ESI.NET** is a .NET wrapper for the [Eve Online ESI API](https://esi.tech.ccp.is/latest/). This wrapper simplifies the process of integrating ESI into your .NET application.

### Resources
* [Tweetfleet - #esi](https://tweetfleet.slack.com/messages/C30KX8UUX/) - This is the official slack channel to speak with CCP devs (and developers) concerning ESI.
* [ESI Swagger Definition](https://esi.tech.ccp.is/swagger.json)
* [Third-party Documentation](https://eveonline-third-party-documentation.readthedocs.io/en/latest/) - This is the best-known documentation concerning the SSO process.

It is extremely important to not solely rely on ESI.NET. You may need to refer to the official specifications to understand what data is expected to be provided. For example, in some instances, ESI.NET will ask for specific values in the endpoint method and construct the JSON object that needs to be sent in the POST request body because it is a simple object that requires a few values. Some of the more complex objects will need to be constructed with anonymous objects by the developer and this can be determined when the endpoint method requires an `object` instead of an `int` or a `string`. Refer to the official documentation and construct the anonymous object to reflect what is expected.

## ESI.NET on NuGet
https://www.nuget.org/packages/ESI.NET/0.9.0.1

`dotnet add package ESI.NET --version 0.9.0.1 `

## Public Endpoint Example
Accessing a public endpoint is extremely simple. Instantiate an instance of the client.
```
ESIClient client = new ESIClient(DataSource.tranquility, "<user_agent_value>");
ApiResponse response = client.Universe.Names(new List<long>()
{
    1590304510,
    99006319,
    20000006
}).Result;
```
For your protection, please provide a user_agent value. This can be your character name and/or project name. CCP will be more likely to contact you than just cut off access to ESI if you provide something that can identify you within the New Eden galaxy.

## SSO Example

Coming Soon!

## Authenticated Endpoint Example

Coming Soon!

