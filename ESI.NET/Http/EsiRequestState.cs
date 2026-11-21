using ESI.NET.Models.SSO;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ESI.NET.Http
{
    /// <summary>
    /// Carries the per-request state that <see cref="EsiRequest"/>.Execute knows but a
    /// <see cref="DelegatingHandler"/> does not — the authorized character, any per-call
    /// <c>OnTokenRefreshed</c> callback, the cache bypass flag, and the mark that a request is an
    /// ESI API call built by <see cref="EsiRequest"/> (so the cache never touches SSO or other traffic) — through
    /// <c>HttpRequestMessage</c> so the pipeline handlers can act on it.
    /// </summary>
    internal static class EsiRequestState
    {
        private const string CharacterKey = "ESI.NET.Character";
        private const string CallbackKey = "ESI.NET.OnTokenRefreshed";
        private const string BypassCacheKey = "ESI.NET.BypassCache";
        private const string EsiRequestKey = "ESI.NET.EsiRequest";

#if NET
        private static readonly HttpRequestOptionsKey<AuthorizedCharacterData> Character = new(CharacterKey);
        private static readonly HttpRequestOptionsKey<Func<AuthorizedCharacterData, Task>> Callback = new(CallbackKey);
        private static readonly HttpRequestOptionsKey<bool> BypassCache = new(BypassCacheKey);
        private static readonly HttpRequestOptionsKey<bool> EsiApiCall = new(EsiRequestKey);

        public static void SetCharacter(HttpRequestMessage request, AuthorizedCharacterData character) => request.Options.Set(Character, character);
        public static void SetCallback(HttpRequestMessage request, Func<AuthorizedCharacterData, Task> callback) => request.Options.Set(Callback, callback);
        public static void SetBypassCache(HttpRequestMessage request) => request.Options.Set(BypassCache, true);
        public static void MarkEsiRequest(HttpRequestMessage request) => request.Options.Set(EsiApiCall, true);
        public static bool IsEsiRequest(HttpRequestMessage request) => request.Options.TryGetValue(EsiApiCall, out var v) && v;
        public static AuthorizedCharacterData GetCharacter(HttpRequestMessage request) => request.Options.TryGetValue(Character, out var v) ? v : null;
        public static Func<AuthorizedCharacterData, Task> GetCallback(HttpRequestMessage request) => request.Options.TryGetValue(Callback, out var v) ? v : null;
        public static bool GetBypassCache(HttpRequestMessage request) => request.Options.TryGetValue(BypassCache, out var v) && v;
#else
        public static void SetCharacter(HttpRequestMessage request, AuthorizedCharacterData character) => request.Properties[CharacterKey] = character;
        public static void SetCallback(HttpRequestMessage request, Func<AuthorizedCharacterData, Task> callback) => request.Properties[CallbackKey] = callback;
        public static void SetBypassCache(HttpRequestMessage request) => request.Properties[BypassCacheKey] = true;
        public static void MarkEsiRequest(HttpRequestMessage request) => request.Properties[EsiRequestKey] = true;
        public static bool IsEsiRequest(HttpRequestMessage request) => request.Properties.TryGetValue(EsiRequestKey, out var v) && v is bool b && b;
        public static AuthorizedCharacterData GetCharacter(HttpRequestMessage request) => request.Properties.TryGetValue(CharacterKey, out var v) ? v as AuthorizedCharacterData : null;
        public static Func<AuthorizedCharacterData, Task> GetCallback(HttpRequestMessage request) => request.Properties.TryGetValue(CallbackKey, out var v) ? v as Func<AuthorizedCharacterData, Task> : null;
        public static bool GetBypassCache(HttpRequestMessage request) => request.Properties.TryGetValue(BypassCacheKey, out var v) && v is bool b && b;
#endif
    }
}
