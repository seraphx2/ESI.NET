using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.FactionWarfare
{
    /// <summary>Faction warfare leaderboards: the top entries by kills and by victory points.</summary>
    /// <typeparam name="T">The entry: <see cref="FactionTotal"/>, <see cref="CorporationTotal"/> or <see cref="CharacterTotal"/>.</typeparam>
    public class Leaderboards<T>
    {
        /// <summary>The top entries by ships destroyed.</summary>
        [JsonProperty("kills")]
        public Summary<T> Kills { get; set; }

        /// <summary>The top entries by victory points earned.</summary>
        [JsonProperty("victory_points")]
        public Summary<T> VictoryPoints { get; set; }
    }

    /// <summary>One leaderboard, over three periods.</summary>
    /// <typeparam name="T">The entry type.</typeparam>
    public class Summary<T>
    {
        /// <summary>The top entries for yesterday.</summary>
        [JsonProperty("yesterday")]
        public List<T> Yesterday { get; set; } = new List<T>();

        /// <summary>The top entries for the last 7 days.</summary>
        [JsonProperty("last_week")]
        public List<T> LastWeek { get; set; } = new List<T>();

        /// <summary>
        /// The top entries over all time (for characters and corporations: while enlisted in
        /// faction warfare).
        /// </summary>
        [JsonProperty("active_total")]
        public List<T> ActiveTotal { get; set; } = new List<T>();
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.FactionWarfareLogic.Leaderboads"/>.
    /// </summary>
    public class FactionTotal
    {
        /// <summary>ID of the faction.</summary>
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        /// <summary>Amount of kills.</summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.FactionWarfareLogic.LeaderboardsForCorporations"/>.
    /// </summary>
    public class CorporationTotal
    {
        /// <summary>ID of the corporation.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>Amount of kills.</summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.FactionWarfareLogic.LeaderboardsForCharacters"/>.
    /// </summary>
    public class CharacterTotal
    {
        /// <summary>ID of the character.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>Amount of kills.</summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }
    }
}
