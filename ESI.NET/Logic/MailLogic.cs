using ESI.NET.Models.Mail;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Mail endpoints. Reach them through <see cref="EsiClient.Mail"/>.
    /// </summary>
    public class MailLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Mail endpoints. <see cref="EsiClient"/> does this; there is no need to call
        /// it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public MailLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Return mail headers. Return the 50 most recent mail headers belonging to the character
        /// that match the query criteria. Queries can be filtered by label, and last_mail_id can be
        /// used to paginate backwards.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/mail</c>.</para>
        /// <para>Requires the <c>esi-mail.read_mail.v1</c> scope.</para>
        /// </remarks>
        /// <param name="labels">Fetch only mails that match one or more of the given
        /// labels.</param>
        /// <param name="lastMailId">List only mail with an ID lower than the given ID, if
        /// present.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Header"/>.</returns>
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

        /// <summary>Create and send a new mail.</summary>
        /// <remarks>
        /// <para>Calls <c>POST /characters/{character_id}/mail</c>.</para>
        /// <para>Requires the <c>esi-mail.send_mail.v1</c> scope.</para>
        /// </remarks>
        /// <param name="recipients">Who to send the mail to (at most 50), each with its ID and
        /// type: characters, plus at most one corporation, alliance or mailing list.</param>
        /// <param name="subject">The mail's subject (at most 1000 characters).</param>
        /// <param name="body">The mail's body (at most 10000 characters). EVE client markup is
        /// allowed.</param>
        /// <param name="approvedCost">The CSPA charge (ISK) you agree to pay to mail recipients who
        /// charge for unsolicited mail; get it from
        /// <see cref="ESI.NET.Logic.CharacterLogic.CSPA"/>. 0 if none.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <c>long</c> value.</returns>
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
        /// Get mail labels and unread counts. Return a list of the users mail labels, unread counts
        /// for each label and a total unread count.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/mail/labels</c>.</para>
        /// <para>Requires the <c>esi-mail.read_mail.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="LabelCounts"/>.</returns>
        public async Task<EsiResponse<LabelCounts>> Labels(EsiCallOptions options)
            => await Execute<LabelCounts>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mail/labels/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Create a mail label.</summary>
        /// <remarks>
        /// <para>Calls <c>POST /characters/{character_id}/mail/labels</c>.</para>
        /// <para>Requires the <c>esi-mail.organize_mail.v1</c> scope.</para>
        /// </remarks>
        /// <param name="name">The new label's name (at most 40 characters).</param>
        /// <param name="color">Hexadecimal string representing label color, in RGB format.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <c>long</c> value.</returns>
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

        /// <summary>Delete a mail label.</summary>
        /// <remarks>
        /// <para>Calls <c>DELETE /characters/{character_id}/mail/labels/{label_id}</c>.</para>
        /// <para>Requires the <c>esi-mail.organize_mail.v1</c> scope.</para>
        /// </remarks>
        /// <param name="labelId">An EVE label id.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> DeleteLabel(long labelId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/characters/{character_id}/mail/labels/{label_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "label_id", labelId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Return mailing list subscriptions. Return all mailing lists that the character is
        /// subscribed to.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/mail/lists</c>.</para>
        /// <para>Requires the <c>esi-mail.read_mail.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="MailingList"/>.</returns>
        public async Task<EsiResponse<List<MailingList>>> MailingLists(EsiCallOptions options)
            => await Execute<List<MailingList>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mail/lists/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return the contents of an EVE mail.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/mail/{mail_id}</c>.</para>
        /// <para>Requires the <c>esi-mail.read_mail.v1</c> scope.</para>
        /// </remarks>
        /// <param name="mailId">An EVE mail ID.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Message"/>.</returns>
        public async Task<EsiResponse<Message>> Retrieve(long mailId, EsiCallOptions options)
            => await Execute<Message>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/mail/{mail_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "mail_id", mailId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Update metadata about a mail.</summary>
        /// <remarks>
        /// <para>Calls <c>PUT /characters/{character_id}/mail/{mail_id}</c>.</para>
        /// <para>Requires the <c>esi-mail.organize_mail.v1</c> scope.</para>
        /// </remarks>
        /// <param name="mailId">An EVE mail ID.</param>
        /// <param name="isRead">Whether to mark the mail read (true) or unread (false). Leave null
        /// to keep it as it is.</param>
        /// <param name="labels">Labels to assign to the mail. Pre-existing labels are
        /// unassigned.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<Message>> Update(long mailId, bool? isRead = null, long[] labels = null, EsiCallOptions options = null)
            => await Execute<Message>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/characters/{character_id}/mail/{mail_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "mail_id", mailId.ToString(CultureInfo.InvariantCulture) }
                },
                body: BuildUpdateObject(isRead, labels),
                options: options).ConfigureAwait(false);
        
        /// <summary>Delete a mail.</summary>
        /// <remarks>
        /// <para>Calls <c>DELETE /characters/{character_id}/mail/{mail_id}</c>.</para>
        /// <para>Requires the <c>esi-mail.organize_mail.v1</c> scope.</para>
        /// </remarks>
        /// <param name="mailId">An EVE mail ID.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
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