using ESI.NET.Enumerations;

namespace ESI.NET
{
    /// <summary>
    /// ESI.NET's settings, usually bound from configuration: <c>services.AddEsi(configuration.GetSection("EsiConfig"))</c>.
    /// </summary>
    public class EsiConfig
    {
        /// <summary>Base URL of ESI, normally <c>https://esi.evetech.net/</c>.</summary>
        public string EsiUrl { get; set; }
        /// <summary>
        /// The EVE server to talk to, sent as <c>X-Tenant</c>. Defaults to
        /// <see cref="DataSource.Tranquility"/> (the live server); it used to default to
        /// <see cref="DataSource.Singularity"/> by accident (the enum's first value), which ESI
        /// rejects with <c>400 Invalid tenant</c>.
        /// </summary>
        public DataSource DataSource { get; set; } = DataSource.Tranquility;
        /// <summary>Your application's client ID from the EVE developers site. Needed for SSO and token refresh.</summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Your application's secret key from the EVE developers site. Leave it empty for a public
        /// client (a desktop or mobile app using PKCE), which must not hold a secret.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>The callback URL registered for your application; EVE SSO redirects here after login.</summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Identifies your application to CCP on every request (sent as <c>X-User-Agent</c>), so they can
        /// contact you about a problem instead of blocking you. Required: include a way to reach you,
        /// such as a character name or email.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Check, before each authenticated call, that the character's token carries the SSO scope
        /// the endpoint requires. If not, the call is not sent and returns a <c>403</c> response with
        /// <see cref="EsiResponse{T}.MissingScope"/> naming the scope. On by default: ESI would only
        /// answer <c>403</c> too, and every 403 it sends counts against its error limit.
        /// The check is skipped for a character whose
        /// <see cref="Models.SSO.AuthorizedCharacterData.Scopes"/> is empty.
        /// </summary>
        public bool ValidateScopes { get; set; } = true;
    }
}
