using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Mail;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class MailLogic : IMailLogic
    {
        private ESIConfig _config;
        private int character_id;

        public MailLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                character_id = _config.AuthorizedCharacter.CharacterID;
            }
        }

        /// <summary>
        /// /characters/{character_id}/mail/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Header>>> Headers(long[] labels = null, int last_mail_id = 0)
        {
            var parameters = new List<string>();

            if (labels != null)
                parameters.Add($"labels={string.Join(",", labels)}");

            if (last_mail_id > 0)
                parameters.Add($"last_mail_id={last_mail_id}");

            var endpoint = $"/characters/{character_id}/mail/";
            var response = await Execute<List<Header>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, parameters.ToArray());

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/mail/
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="approved_cost"></param>
        /// <returns></returns>
        public async Task<ApiResponse<int>> New(object[] recipients, string subject, string body, int approved_cost = 0)
        {
            var endpoint = $"/characters/{character_id}/mail/";
            var response = await Execute<int>(_config, RequestSecurity.Authenticated, RequestMethod.POST, endpoint, body: new
            {
                recipients = recipients,
                subject = subject,
                body = body,
                approved_cost = approved_cost
            });

            if (response.StatusCode == HttpStatusCode.Created)
                response.Message = Dictionaries.NoContentMessages["POST|/characters/{character_id}/mail/"];

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/mail/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<LabelCounts>> Labels()
        {
            var endpoint = $"/characters/{character_id}/mail/labels/";
            var response = await Execute<LabelCounts>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/mail/labels/
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public async Task<ApiResponse<long>> NewLabel(string name, string color)
        {
            var endpoint = $"/characters/{character_id}/mail/labels/";
            var response = await Execute<long>(_config, RequestSecurity.Authenticated, RequestMethod.POST, endpoint, body: new
            {
                name = name,
                color = color
            });

            if (response.StatusCode == HttpStatusCode.Created)
                response.Message = Dictionaries.NoContentMessages["POST|/characters/{character_id}/mail/labels/"];

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/mail/labels/{label_id}/
        /// </summary>
        /// <param name="label_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> DeleteLabel(long label_id)
        {
            var endpoint = $"/characters/{character_id}/mail/labels/{label_id}/";
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.DELETE, endpoint);

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["DELETE|/characters/{character_id}/mail/labels/{label_id}/"];

            return response;
        }


        public async Task<ApiResponse<List<MailingList>>> MailingLists()
        {
            var endpoint = $"/characters/{character_id}/mail/lists/";
            var response = await Execute<List<MailingList>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/mail/{mail_id}/
        /// </summary>
        /// <param name="mail_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Message>> Retrieve(int mail_id)
        {
            var endpoint = $"/characters/{character_id}/mail/{mail_id}/";
            var response = await Execute<Message>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/mail/{mail_id}/
        /// </summary>
        /// <param name="mail_id"></param>
        /// <param name="is_read"></param>
        /// <param name="labels"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Message>> Update(int mail_id, bool? is_read = null, int[] labels = null)
        {
            dynamic body = null;

            if (is_read != null && labels == null)
            {
                body = new
                {
                    is_read = is_read
                };
            }
            else if (is_read == null && labels != null)
            {
                body = new
                {
                    labels = labels
                };
            }
            else if (is_read != null && labels != null)
            {
                body = new
                {
                    is_read = is_read,
                    labels = labels
                };
            }

            var endpoint = $"/characters/{character_id}/mail/{mail_id}/";
            var response = await Execute<Message>(_config, RequestSecurity.Authenticated, RequestMethod.PUT, endpoint, body: body);

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["PUT|/characters/{character_id}/mail/{mail_id}/"];

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/mail/{mail_id}/
        /// </summary>
        /// <param name="mail_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Message>> Delete(int mail_id)
        {
            var endpoint = $"/characters/{character_id}/mail/{mail_id}/";
            var response = await Execute<Message>(_config, RequestSecurity.Authenticated, RequestMethod.DELETE, endpoint);

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["DELETE|/characters/{character_id}/mail/{mail_id}/"];

            return response;
        }
    }
}
