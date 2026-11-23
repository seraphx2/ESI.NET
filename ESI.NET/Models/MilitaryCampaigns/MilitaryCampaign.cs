using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.MilitaryCampaigns
{
    /// <summary>
    /// Part of
    /// <see cref="ESI.NET.Models.MilitaryCampaigns.CharacterMilitaryObjectiveList.Cursor"/> and
    /// <see cref="ESI.NET.Models.MilitaryCampaigns.MilitaryObjectiveList.Cursor"/>.
    /// </summary>
    public class MilitaryCursor
    {
        /// <summary>
        /// Cursor to use as 'after' in your next request, to continue walking forwards in time.
        /// </summary>
        [JsonProperty("after")]
        public string After { get; set; }

        /// <summary>
        /// Cursor to use as 'before' in your next request, to continue walking backwards in time.
        /// </summary>
        [JsonProperty("before")]
        public string Before { get; set; }
    }

    /// <summary>Returned by <see cref="ESI.NET.Logic.MilitaryCampaignsLogic.All"/>.</summary>
    public class MilitaryCampaignList
    {
        /// <summary>List of military campaigns.</summary>
        [JsonProperty("campaigns")]
        public List<MilitaryCampaign> Campaigns { get; set; } = new List<MilitaryCampaign>();
    }

    /// <summary>Returned by <see cref="ESI.NET.Logic.MilitaryCampaignsLogic.Get"/>.</summary>
    public class MilitaryCampaign
    {
        /// <summary>Campaign's ID.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Campaign's state. One of <c>Unspecified</c>, <c>Active</c>, <c>Completed</c>,
        /// <c>Expired</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>Campaign's progress.</summary>
        [JsonProperty("progress")]
        public long Progress { get; set; }

        /// <summary>Moment the campaign started.</summary>
        [JsonProperty("started")]
        public DateTime? Started { get; set; }

        /// <summary>Moment the campaign transitioned to a non-active state.</summary>
        [JsonProperty("finished")]
        public DateTime? Finished { get; set; }
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.MilitaryCampaignsLogic.Objectives"/>.
    /// </summary>
    public class MilitaryObjectiveList
    {
        /// <summary>List of military campaign objectives.</summary>
        [JsonProperty("objectives")]
        public List<MilitaryObjective> Objectives { get; set; } = new List<MilitaryObjective>();

        /// <summary>
        /// Cursors for paging: pass <c>Before</c> or <c>After</c> to the next call's <c>before</c>
        /// / <c>after</c> parameter.
        /// </summary>
        [JsonProperty("cursor")]
        public MilitaryCursor Cursor { get; set; }
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.MilitaryCampaignsLogic.Objective"/>.
    /// </summary>
    public class MilitaryObjective
    {
        /// <summary>Objective's ID.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Objective's state. One of <c>Unspecified</c>, <c>Active</c>, <c>Completed</c>,
        /// <c>Expired</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>Objective's progress.</summary>
        [JsonProperty("progress")]
        public long Progress { get; set; }

        /// <summary>Moment the objective started.</summary>
        [JsonProperty("started")]
        public DateTime? Started { get; set; }

        /// <summary>Moment the objective transitioned to a non-active state.</summary>
        [JsonProperty("finished")]
        public DateTime? Finished { get; set; }

        /// <summary>Objective's last modified.</summary>
        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        /// <summary>Objective's participants.</summary>
        [JsonProperty("participants")]
        public MilitaryObjectiveParticipants Participants { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.MilitaryCampaigns.MilitaryObjective.Participants"/>.
    /// </summary>
    public class MilitaryObjectiveParticipants
    {
        /// <summary>Number of currently committed participants.</summary>
        [JsonProperty("committed")]
        public long Committed { get; set; }

        /// <summary>Number of participants with at least one contribution.</summary>
        [JsonProperty("contributors")]
        public long Contributors { get; set; }

        /// <summary>Number of participants (including those that resigned).</summary>
        [JsonProperty("total")]
        public long Total { get; set; }
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.MilitaryCampaignsLogic.CharacterObjectives"/>.
    /// </summary>
    public class CharacterMilitaryObjectiveList
    {
        /// <summary>List of military campaign objectives.</summary>
        [JsonProperty("objectives")]
        public List<CharacterMilitaryObjective> Objectives { get; set; } = new List<CharacterMilitaryObjective>();

        /// <summary>
        /// Cursors for paging: pass <c>Before</c> or <c>After</c> to the next call's <c>before</c>
        /// / <c>after</c> parameter.
        /// </summary>
        [JsonProperty("cursor")]
        public MilitaryCursor Cursor { get; set; }
    }

    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.MilitaryCampaignsLogic.CharacterObjective"/>.
    /// </summary>
    public class CharacterMilitaryObjective
    {
        /// <summary>Objective's ID.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Campaign's ID.</summary>
        [JsonProperty("campaign_id")]
        public string CampaignId { get; set; }

        /// <summary>The character's cumulative contribution.</summary>
        [JsonProperty("contributed")]
        public long Contributed { get; set; }

        /// <summary>Whether the character is currently committed to the objective.</summary>
        [JsonProperty("is_committed")]
        public bool IsCommitted { get; set; }

        /// <summary>Moment this information was last modified.</summary>
        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }
    }
}
