using ESI.NET.Models.SSO;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ESI.NET.Http
{
    /// <summary>
    /// Carries the per-request state that <see cref="EsiRequest"/>.Execute knows but a
    /// <see cref="DelegatingHandler"/> does not — the authorized character and any per-call
    /// <c>OnTokenRefreshed</c> callback — through <c>HttpRequestMessage</c> so
    /// <see cref="EsiTokenRefreshHandler"/> can act on it.
    /// </summary>
    internal static class EsiRequestState
    {
        private const string CharacterKey = "ESI.NET.Character";
        private const string CallbackKey = "ESI.NET.OnTokenRefreshed";

#if NET
        private static readonly HttpRequestOptionsKey<AuthorizedCharacterData> Character = new(CharacterKey);
        private static readonly HttpRequestOptionsKey<Func<AuthorizedCharacterData, Task>> Callback = new(CallbackKey);

        public static void SetCharacter(HttpRequestMessage request, AuthorizedCharacterData character) => request.Options.Set(Character, character);
        public static void SetCallback(HttpRequestMessage request, Func<AuthorizedCharacterData, Task> callback) => request.Options.Set(Callback, callback);
        public static AuthorizedCharacterData GetCharacter(HttpRequestMessage request) => request.Options.TryGetValue(Character, out var v) ? v : null;
        public static Func<AuthorizedCharacterData, Task> GetCallback(HttpRequestMessage request) => request.Options.TryGetValue(Callback, out var v) ? v : null;
#else
        public static void SetCharacter(HttpRequestMessage request, AuthorizedCharacterData character) => request.Properties[CharacterKey] = character;
        public static void SetCallback(HttpRequestMessage request, Func<AuthorizedCharacterData, Task> callback) => request.Properties[CallbackKey] = callback;
        public static AuthorizedCharacterData GetCharacter(HttpRequestMessage request) => request.Properties.TryGetValue(CharacterKey, out var v) ? v as AuthorizedCharacterData : null;
        public static Func<AuthorizedCharacterData, Task> GetCallback(HttpRequestMessage request) => request.Properties.TryGetValue(CallbackKey, out var v) ? v as Func<AuthorizedCharacterData, Task> : null;
#endif
    }
}
