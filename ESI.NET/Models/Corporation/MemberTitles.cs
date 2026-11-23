using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CorporationLogic.MemberTitles"/>.
    /// </summary>
    public class MemberTitles
    {
        /// <summary>ID of the member.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>A list of title_id.</summary>
        [JsonProperty("titles")]
        public long[] Titles { get; set; }

    }
}
