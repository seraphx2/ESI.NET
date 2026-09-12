using ESI.NET.Models.Mail;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class MailLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public MailLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/mail/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Header>>> Headers(long[] labels = null, long lastMailId = 0, EsiCallOptions options = null)
        {
            var parameters = new List<string>();

            if (labels != null)
                parameters.Add($"labels={string.Join(",", labels)}");

            if (lastMailId > 0)
                parameters.Add($"last_mail_id={lastMailId}");

            var response = await Execute<List<Header>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mail/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: parameters.ToArray(),
                options: options).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/mail/
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="approvedCost"></param>
        /// <returns></returns>
        public async Task<EsiResponse<long>> New(object[] recipients, string subject, string body, long approvedCost = 0, EsiCallOptions options = null)
            => await Execute<long>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/mail/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                body: new
                {
                    recipients,
                    subject,
                    body,
                    approvedCost
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/mail/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<LabelCounts>> Labels(EsiCallOptions options)
            => await Execute<LabelCounts>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mail/labels/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/mail/labels/
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public async Task<EsiResponse<long>> NewLabel(string name, string color, EsiCallOptions options)
            => await Execute<long>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/mail/labels/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                body: new
                {
                    name,
                    color
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/mail/labels/{label_id}/
        /// </summary>
        /// <param name="labelId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> DeleteLabel(long labelId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/characters/{character_id}/mail/labels/{label_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "label_id", labelId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/mail/lists/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<MailingList>>> MailingLists(EsiCallOptions options)
            => await Execute<List<MailingList>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mail/lists/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/mail/{mail_id}/
        /// </summary>
        /// <param name="mailId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Message>> Retrieve(long mailId, EsiCallOptions options)
            => await Execute<Message>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mail/{mail_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "mail_id", mailId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/mail/{mail_id}/
        /// </summary>
        /// <param name="mailId"></param>
        /// <param name="isRead"></param>
        /// <param name="labels"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Message>> Update(long mailId, bool? isRead = null, long[] labels = null, EsiCallOptions options = null)
            => await Execute<Message>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/characters/{character_id}/mail/{mail_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "mail_id", mailId.ToString(CultureInfo.InvariantCulture) }
                },
                body: BuildUpdateObject(isRead, labels),
                options: options).ConfigureAwait(false);
        
        /// <summary>
        /// /characters/{character_id}/mail/{mail_id}/
        /// </summary>
        /// <param name="mailId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Message>> Delete(long mailId, EsiCallOptions options)
            => await Execute<Message>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/characters/{character_id}/mail/{mail_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "mail_id", mailId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="is_read"></param>
        /// <param name="labels"></param>
        /// <returns></returns>
        private static dynamic BuildUpdateObject(bool? is_read, long[] labels = null)
        {
            dynamic body = null;

            if (is_read != null && labels == null)
                body = new { is_read };
            else if (is_read == null && labels != null)
                body = new { labels };
            else
                body = new { is_read, labels };
            return body;
        }
    }
}