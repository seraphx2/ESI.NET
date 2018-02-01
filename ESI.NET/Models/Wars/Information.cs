using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Wars
{
    public class Information
    {
        [JsonProperty("aggressor")]
        public Combatant Agressor { get; set; }

        [JsonProperty("declared")]
        public DateTime DateDeclared { get; set; }

        [JsonProperty("defender")]
        public Combatant Defender { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("mutual")]
        public bool IsMutual { get; set; }

        [JsonProperty("open_for_allies")]
        public bool IsOpenForAllies { get; set; }

        [JsonProperty("allies")]
        public List<Ally> Allies { get; set; }

        [JsonProperty("finished")]
        public string Fin { get; set; }

        [JsonProperty("retracted")]
        public DateTime Retracted { get; set; }

        [JsonProperty("started")]
        public DateTime Started { get; set; }
    }

    public class Combatant
    {
        [JsonProperty("corporation_id")]
        public int CorporationId { get; set; }

        [JsonProperty("isk_destroyed")]
        public double IskDestroyed { get; set; }

        [JsonProperty("ships_killed")]
        public int ShipsKilled { get; set; }
    }

    public class Ally
    {
        [JsonProperty("alliance_id")]
        public int AllianceId { get; set; }

        [JsonProperty("corporation_id")]
        public int CorporationId { get; set; }
    }
}
