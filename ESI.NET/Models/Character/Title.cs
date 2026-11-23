using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESI.NET.Models.Character
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CharacterLogic.Titles"/>.</summary>
    public class Title
    {
        /// <summary>ID of the corporation title.</summary>
        [JsonProperty("title_id")]
        public long Id { get; set; }

        /// <summary>The title's name, which may contain EVE client markup.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
