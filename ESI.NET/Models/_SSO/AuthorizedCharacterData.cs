using System;

namespace ESI.NET.Models.SSO
{
    /// <summary>
    /// A logged-in character, from <see cref="ESI.NET.SsoLogic.Verify"/>: who they are and the token that
    /// authorizes calls for them. Pass it as <see cref="ESI.NET.EsiCallOptions.Character"/>. Store it
    /// (especially <see cref="RefreshToken"/>) to call ESI for the character later.
    /// </summary>
    public class AuthorizedCharacterData
    {
        /// <summary>
        /// ID of the logged-in character, from the access token's <c>sub</c> claim.
        /// </summary>
        public long CharacterID { get; set; }
        /// <summary>Name of the logged-in character.</summary>
        public string CharacterName { get; set; }
        /// <summary>
        /// When <see cref="Token"/> expires (UTC). ESI.NET refreshes it shortly before then when
        /// <see cref="ESI.NET.EsiCallOptions.OnTokenRefreshed"/> is set.
        /// </summary>
        public DateTime ExpiresOn { get; set; }
        /// <summary>
        /// The scopes the access token was granted, space-separated. ESI.NET checks it before each
        /// authenticated call (see <see cref="ESI.NET.EsiConfig.ValidateScopes"/>); when empty,
        /// nothing is checked.
        /// </summary>
        public string Scopes { get; set; }
        /// <summary>The token type, <c>Bearer</c>.</summary>
        public string TokenType { get; set; }
        /// <summary>
        /// Hash of the character and the account that owns it. It changes when the character is
        /// sold or transferred, so a stored token whose hash no longer matches belongs to someone
        /// else.
        /// </summary>
        public string CharacterOwnerHash { get; set; }

        /// <summary>
        /// The access token (a JWT) sent as the bearer token on authenticated calls.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// The refresh token, used to get a new <see cref="Token"/>. EVE SSO rotates it on every
        /// refresh: always store the newest one.
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// ID of the character's alliance, or 0. Filled by <see cref="ESI.NET.SsoLogic.Verify"/>.
        /// </summary>
        public long AllianceID { get; set; }
        /// <summary>
        /// ID of the character's corporation. Filled by <see cref="ESI.NET.SsoLogic.Verify"/>.
        /// </summary>
        public long CorporationID { get; set; }
        /// <summary>
        /// ID of the character's faction warfare faction, or 0. Filled by
        /// <see cref="ESI.NET.SsoLogic.Verify"/>.
        /// </summary>
        public long FactionID { get; set; }
    }
}
