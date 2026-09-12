using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.FreelanceJobs
{
    /// <summary>A page of freelance jobs. Paginate with <see cref="Cursor"/> (null on the character list).</summary>
    public class FreelanceJobList
    {
        [JsonProperty("freelance_jobs")]
        public List<FreelanceJobSummary> FreelanceJobs { get; set; } = new List<FreelanceJobSummary>();

        [JsonProperty("cursor")]
        public FreelanceCursor Cursor { get; set; }
    }

    public class FreelanceCursor
    {
        [JsonProperty("after")]
        public string After { get; set; }

        [JsonProperty("before")]
        public string Before { get; set; }
    }

    public class FreelanceJobSummary
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        /// <summary>Unspecified | Active | Closed | Completed | Expired | Deleted</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("progress")]
        public FreelanceProgress Progress { get; set; }

        [JsonProperty("reward")]
        public FreelanceReward Reward { get; set; }
    }

    public class FreelanceProgress
    {
        [JsonProperty("current")]
        public long Current { get; set; }

        [JsonProperty("desired")]
        public long Desired { get; set; }
    }

    public class FreelanceReward
    {
        [JsonProperty("initial")]
        public decimal Initial { get; set; }

        [JsonProperty("remaining")]
        public decimal Remaining { get; set; }
    }

    /// <summary>Full detail for one freelance job.</summary>
    public class FreelanceJob : FreelanceJobSummary
    {
        [JsonProperty("access_and_visibility")]
        public FreelanceAccess AccessAndVisibility { get; set; }

        [JsonProperty("configuration")]
        public FreelanceConfiguration Configuration { get; set; }

        [JsonProperty("contribution")]
        public FreelanceContributionConfig Contribution { get; set; }

        [JsonProperty("details")]
        public FreelanceDetails Details { get; set; }
    }

    public class FreelanceAccess
    {
        [JsonProperty("acl_protected")]
        public bool AclProtected { get; set; }

        [JsonProperty("broadcast_locations")]
        public List<FreelanceNamedId> BroadcastLocations { get; set; } = new List<FreelanceNamedId>();

        [JsonProperty("restrictions")]
        public FreelanceRestrictions Restrictions { get; set; }
    }

    public class FreelanceRestrictions
    {
        [JsonProperty("maximum_age")]
        public long? MaximumAge { get; set; }

        [JsonProperty("minimum_age")]
        public long? MinimumAge { get; set; }
    }

    public class FreelanceConfiguration
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>Method-specific parameters; shape depends on <see cref="Method"/>.</summary>
        [JsonProperty("parameters")]
        public Dictionary<string, object> Parameters { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }
    }

    public class FreelanceContributionConfig
    {
        [JsonProperty("contribution_per_participant_limit")]
        public long? ContributionPerParticipantLimit { get; set; }

        [JsonProperty("max_committed_participants")]
        public long MaxCommittedParticipants { get; set; }

        [JsonProperty("reward_per_contribution")]
        public decimal RewardPerContribution { get; set; }

        [JsonProperty("submission_limit")]
        public long? SubmissionLimit { get; set; }

        [JsonProperty("submission_multiplier")]
        public decimal SubmissionMultiplier { get; set; }
    }

    public class FreelanceDetails
    {
        /// <summary>Unspecified | Explorer | Industrialist | Enforcer | Soldier of Fortune</summary>
        [JsonProperty("career")]
        public string Career { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("creator")]
        public FreelanceCreator Creator { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("expires")]
        public DateTime? Expires { get; set; }

        [JsonProperty("finished")]
        public DateTime? Finished { get; set; }
    }

    public class FreelanceCreator
    {
        [JsonProperty("character")]
        public FreelanceNamedId Character { get; set; }

        [JsonProperty("corporation")]
        public FreelanceNamedId Corporation { get; set; }
    }

    public class FreelanceNamedId
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>One character's participation in a freelance job.</summary>
    public class FreelanceParticipation
    {
        [JsonProperty("contributed")]
        public long Contributed { get; set; }

        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        /// <summary>Unspecified | Committed | Kicked | Resigned</summary>
        [JsonProperty("state")]
        public string State { get; set; }
    }

    /// <summary>A page of participants in a freelance job.</summary>
    public class FreelanceParticipants
    {
        [JsonProperty("participants")]
        public List<FreelanceParticipant> Participants { get; set; } = new List<FreelanceParticipant>();

        [JsonProperty("cursor")]
        public FreelanceCursor Cursor { get; set; }
    }

    public class FreelanceParticipant
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contributed")]
        public long Contributed { get; set; }

        /// <summary>Unspecified | Committed | Kicked | Resigned</summary>
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
