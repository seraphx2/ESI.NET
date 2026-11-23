using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CorporationLogic.Shareholders"/>.
    /// </summary>
    public class Shareholder
    {
        /// <summary>Number of shares held.</summary>
        [JsonProperty("share_count")]
        public long ShareCount { get; set; }

        /// <summary>
        /// ID of the shareholder; <see cref="ShareholderType"/> says what kind of entity it is.
        /// </summary>
        [JsonProperty("shareholder_id")]
        public long ShareholderId { get; set; }

        /// <summary>
        /// Whether the shareholder is a <c>character</c> or a <c>corporation</c>.
        /// </summary>
        [JsonProperty("shareholder_type")]
        public string ShareholderType { get; set; }
    }
}
