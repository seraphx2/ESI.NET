using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Enumerations;
using ESI.NET.Models.SSO;

namespace ESI.NET.Interfaces.Logic
{
    public interface ISsoLogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="state"></param>
        /// <param name="code_challenge">All hashing/encryption will be done automatically. Just provide the code.</param>
        /// <param name=""></param>
        /// <returns></returns>
        string CreateAuthenticationUrl(List<string> scope = null, string state = null,
            string challengeCode = null);

        string GenerateChallengeCode();

        /// <summary>
        /// SSO Token helper
        /// </summary>
        /// <param name="grantType"></param>
        /// <param name="code">The authorization_code or the refresh_token</param>
        /// <param name="codeChallenge">Provide the same value that was provided for codeChallenge in CreateAuthenticationUrl(). All hashing/encryption will be done automatically. Just provide the code.</param>
        /// <returns></returns>
        Task<SsoToken> GetToken(GrantType grantType, string code, string codeChallenge = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// SSO Token revokation helper
        /// ESI will invalidate the provided refreshToken
        /// </summary>
        /// <param name="code">refresh_token to revoke</param>
        /// <returns></returns>
        Task RevokeToken(string code, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifies the Character information for the provided Token information.
        /// While this method represents the oauth/verify request, in addition to the verified data that ESI returns, this object also stores the Token and Refresh token
        /// and this method also uses ESI retrieves other information pertinent to making calls in the ESI.NET API. (alliance_id, corporation_id, faction_id)
        /// You will need a record in your database that stores at least this information. Serialize and store this object for quick retrieval and token refreshing.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<AuthorizedCharacterData> Verify(SsoToken token, CancellationToken cancellationToken = default);
    }
}