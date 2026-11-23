using Newtonsoft.Json;

namespace ESI.NET.Models.Killmails
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.KillmailsLogic.ForCharacter"/>,
    /// <see cref="ESI.NET.Logic.KillmailsLogic.ForCorporation"/>, and
    /// <see cref="ESI.NET.Logic.WarsLogic.Kills"/>.
    /// </summary>
    public class Killmail
    {
        /// <summary>A hash of this killmail.</summary>
        [JsonProperty("killmail_hash")]
        public string Hash { get; set; }

        /// <summary>ID of this killmail.</summary>
        [JsonProperty("killmail_id")]
        public long Id { get; set; }
    }
}
