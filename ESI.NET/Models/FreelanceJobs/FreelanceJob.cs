using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.FreelanceJobs
{
    /// <summary>A page of freelance jobs. Paginate with <see cref="Cursor"/> (null on the character list).</summary>
    public class FreelanceJobList
    {
        /// <summary>List of freelance jobs.</summary>
        [JsonProperty("freelance_jobs")]
        public List<FreelanceJobSummary> FreelanceJobs { get; set; } = new List<FreelanceJobSummary>();

        /// <summary>
        /// Cursors for paging: pass <c>Before</c> or <c>After</c> to the next call's <c>before</c>
        /// / <c>after</c> parameter.
        /// </summary>
        [JsonProperty("cursor")]
        public FreelanceCursor Cursor { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceJobList.Cursor"/> and
    /// <see cref="ESI.NET.Models.FreelanceJobs.FreelanceParticipants.Cursor"/>.
    /// </summary>
    public class FreelanceCursor
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

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceJobList.FreelanceJobs"/>.
    /// </summary>
    public class FreelanceJobSummary
    {
        /// <summary>Job's ID.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Job's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Job's last modified.</summary>
        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Job's state. One of <c>Unspecified</c>, <c>Active</c>, <c>Closed</c>, <c>Completed</c>,
        /// <c>Expired</c>, <c>Deleted</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>Job's progress.</summary>
        [JsonProperty("progress")]
        public FreelanceProgress Progress { get; set; }

        /// <summary>Job's reward.</summary>
        [JsonProperty("reward")]
        public FreelanceReward Reward { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceJobSummary.Progress"/>.
    /// </summary>
    public class FreelanceProgress
    {
        /// <summary>Current progress.</summary>
        [JsonProperty("current")]
        public long Current { get; set; }

        /// <summary>Desired progress.</summary>
        [JsonProperty("desired")]
        public long Desired { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceJobSummary.Reward"/>.
    /// </summary>
    public class FreelanceReward
    {
        /// <summary>Original amount of ISK that was reserved for this freelance job.</summary>
        [JsonProperty("initial")]
        public decimal Initial { get; set; }

        /// <summary>Remaining ISK to be awarded.</summary>
        [JsonProperty("remaining")]
        public decimal Remaining { get; set; }
    }

    /// <summary>Full detail for one freelance job.</summary>
    public class FreelanceJob : FreelanceJobSummary
    {
        /// <summary>Job's access and visibility.</summary>
        [JsonProperty("access_and_visibility")]
        public FreelanceAccess AccessAndVisibility { get; set; }

        /// <summary>Job's configuration.</summary>
        [JsonProperty("configuration")]
        public FreelanceConfiguration Configuration { get; set; }

        /// <summary>Job's contribution settings.</summary>
        [JsonProperty("contribution")]
        public FreelanceContributionConfig Contribution { get; set; }

        /// <summary>Job's details.</summary>
        [JsonProperty("details")]
        public FreelanceDetails Details { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceJob.AccessAndVisibility"/>.
    /// </summary>
    public class FreelanceAccess
    {
        /// <summary>Whether the job is protected by an ACL.</summary>
        [JsonProperty("acl_protected")]
        public bool AclProtected { get; set; }

        /// <summary>Solar systems where the job is broadcast.</summary>
        [JsonProperty("broadcast_locations")]
        public List<FreelanceNamedId> BroadcastLocations { get; set; } = new List<FreelanceNamedId>();

        /// <summary>Restrictions on who can participate.</summary>
        [JsonProperty("restrictions")]
        public FreelanceRestrictions Restrictions { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceAccess.Restrictions"/>.
    /// </summary>
    public class FreelanceRestrictions
    {
        /// <summary>Maximum age of the participant, in days.</summary>
        [JsonProperty("maximum_age")]
        public long? MaximumAge { get; set; }

        /// <summary>Minimum age of the participant, in days.</summary>
        [JsonProperty("minimum_age")]
        public long? MinimumAge { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceJob.Configuration"/>.
    /// </summary>
    public class FreelanceConfiguration
    {
        /// <summary>Contribution method (see SDE for valid values).</summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// Parameter values for the contribution method. Their shape depends on
        /// <see cref="Method"/>; the parameter definitions are in the SDE.
        /// </summary>
        [JsonProperty("parameters")]
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>Version of parameter definition used.</summary>
        [JsonProperty("version")]
        public long Version { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceJob.Contribution"/>.
    /// </summary>
    public class FreelanceContributionConfig
    {
        /// <summary>Limit on the contribution of the individual participant.</summary>
        [JsonProperty("contribution_per_participant_limit")]
        public long? ContributionPerParticipantLimit { get; set; }

        /// <summary>Maximum number of participants that can commit to this job.</summary>
        [JsonProperty("max_committed_participants")]
        public long MaxCommittedParticipants { get; set; }

        /// <summary>ISK reward per contribution.</summary>
        [JsonProperty("reward_per_contribution")]
        public decimal RewardPerContribution { get; set; }

        /// <summary>Limit on amount of contribution per submission.</summary>
        [JsonProperty("submission_limit")]
        public long? SubmissionLimit { get; set; }

        /// <summary>Multiplier towards progress per contribution.</summary>
        [JsonProperty("submission_multiplier")]
        public decimal SubmissionMultiplier { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceJob.Details"/>.
    /// </summary>
    public class FreelanceDetails
    {
        /// <summary>
        /// Assigned career path. One of <c>Unspecified</c>, <c>Explorer</c>, <c>Industrialist</c>,
        /// <c>Enforcer</c>, <c>Soldier of Fortune</c>.
        /// </summary>
        [JsonProperty("career")]
        public string Career { get; set; }

        /// <summary>Moment this freelance job was created.</summary>
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        /// <summary>Creator.</summary>
        [JsonProperty("creator")]
        public FreelanceCreator Creator { get; set; }

        /// <summary>Description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Moment this freelance job expires.</summary>
        [JsonProperty("expires")]
        public DateTime? Expires { get; set; }

        /// <summary>Moment this freelance job transitioned to a non-active state.</summary>
        [JsonProperty("finished")]
        public DateTime? Finished { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceDetails.Creator"/>.
    /// </summary>
    public class FreelanceCreator
    {
        /// <summary>Character that created the job.</summary>
        [JsonProperty("character")]
        public FreelanceNamedId Character { get; set; }

        /// <summary>Corporation that created the job.</summary>
        [JsonProperty("corporation")]
        public FreelanceNamedId Corporation { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceAccess.BroadcastLocations"/>,
    /// <see cref="ESI.NET.Models.FreelanceJobs.FreelanceCreator.Character"/>, and
    /// <see cref="ESI.NET.Models.FreelanceJobs.FreelanceCreator.Corporation"/>.
    /// </summary>
    public class FreelanceNamedId
    {
        /// <summary>Solar system ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Solar system name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>One character's participation in a freelance job.</summary>
    public class FreelanceParticipation
    {
        /// <summary>Your contribution.</summary>
        [JsonProperty("contributed")]
        public long Contributed { get; set; }

        /// <summary>Moment this information was last modified.</summary>
        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Your participation status. One of <c>Unspecified</c>, <c>Committed</c>, <c>Kicked</c>,
        /// <c>Resigned</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }
    }

    /// <summary>A page of participants in a freelance job.</summary>
    public class FreelanceParticipants
    {
        /// <summary>List of participants.</summary>
        [JsonProperty("participants")]
        public List<FreelanceParticipant> Participants { get; set; } = new List<FreelanceParticipant>();

        /// <summary>
        /// Cursors for paging: pass <c>Before</c> or <c>After</c> to the next call's <c>before</c>
        /// / <c>after</c> parameter.
        /// </summary>
        [JsonProperty("cursor")]
        public FreelanceCursor Cursor { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FreelanceJobs.FreelanceParticipants.Participants"/>.
    /// </summary>
    public class FreelanceParticipant
    {
        /// <summary>Participant's character ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Participant's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Participant's contributed progress.</summary>
        [JsonProperty("contributed")]
        public long Contributed { get; set; }

        /// <summary>
        /// Participant's state. One of <c>Unspecified</c>, <c>Committed</c>, <c>Kicked</c>,
        /// <c>Resigned</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
