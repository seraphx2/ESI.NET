using ESI.NET.Models.Fittings;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class FittingsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public FittingsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/fittings/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Fitting>>> List(EsiCallOptions options)
            => await Execute<List<Fitting>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/fittings/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/fittings/
        /// </summary>
        /// <param name="fitting"></param>
        /// <returns></returns>
        public async Task<EsiResponse<NewFitting>> Add(object fitting, EsiCallOptions options)
            => await Execute<NewFitting>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/fittings/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                body: fitting,
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/fittings/{fitting_id}/
        /// </summary>
        /// <param name="fitting_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> Delete(long fitting_id, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/characters/{character_id}/fittings/{fitting_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "fitting_id", fitting_id.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}