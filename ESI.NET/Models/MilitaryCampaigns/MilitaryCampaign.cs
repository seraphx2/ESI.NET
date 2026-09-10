using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.MilitaryCampaigns
{
    public class MilitaryCursor
    {
        [JsonProperty("after")]
        public string After { get; set; }

        [JsonProperty("before")]
        public string Before { get; set; }
    }

    public class MilitaryCampaignList
    {
        [JsonProperty("campaigns")]
        public List<MilitaryCampaign> Campaigns { get; set; } = new List<MilitaryCampaign>();
    }

    public class MilitaryCampaign
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Unspecified | Active | Completed | Expired</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("progress")]
        public long Progress { get; set; }

        [JsonProperty("started")]
        public DateTime? Started { get; set; }

        [JsonProperty("finished")]
        public DateTime? Finished { get; set; }
    }

    public class MilitaryObjectiveList
    {
        [JsonProperty("objectives")]
        public List<MilitaryObjective> Objectives { get; set; } = new List<MilitaryObjective>();

        [JsonProperty("cursor")]
        public MilitaryCursor Cursor { get; set; }
    }

    public class MilitaryObjective
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Unspecified | Active | Completed | Expired</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("progress")]
        public long Progress { get; set; }

        [JsonProperty("started")]
        public DateTime? Started { get; set; }

        [JsonProperty("finished")]
        public DateTime? Finished { get; set; }

        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        [JsonProperty("participants")]
        public MilitaryObjectiveParticipants Participants { get; set; }
    }

    public class MilitaryObjectiveParticipants
    {
        [JsonProperty("committed")]
        public long Committed { get; set; }

        [JsonProperty("contributors")]
        public long Contributors { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public class CharacterMilitaryObjectiveList
    {
        [JsonProperty("objectives")]
        public List<CharacterMilitaryObjective> Objectives { get; set; } = new List<CharacterMilitaryObjective>();

        [JsonProperty("cursor")]
        public MilitaryCursor Cursor { get; set; }
    }

    public class CharacterMilitaryObjective
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("campaign_id")]
        public string CampaignId { get; set; }

        [JsonProperty("contributed")]
        public long Contributed { get; set; }

        [JsonProperty("is_committed")]
        public bool IsCommitted { get; set; }

        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }
    }
}
