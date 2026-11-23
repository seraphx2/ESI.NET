using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Character
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CharacterLogic.Fatigue"/>.</summary>
    public class Fatigue
    {
        /// <summary>Character's jump fatigue expiry.</summary>
        [JsonProperty("jump_fatigue_expire_date")]
        public DateTime JumpFatigueExpireDate { get; set; }

        /// <summary>Character's last jump activation.</summary>
        [JsonProperty("last_jump_date")]
        public DateTime LastJumpDate { get; set; }

        /// <summary>Character's last jump update.</summary>
        [JsonProperty("last_update_date")]
        public DateTime LastUpdateDate { get; set; }
    }
}
