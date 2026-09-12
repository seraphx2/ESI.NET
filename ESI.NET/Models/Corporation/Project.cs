using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Corporation
{
    /// <summary>A page of Corporation Projects. Paginate with <see cref="Cursor"/>.</summary>
    public class ProjectList
    {
        [JsonProperty("projects")]
        public List<ProjectSummary> Projects { get; set; } = new List<ProjectSummary>();

        [JsonProperty("cursor")]
        public ProjectCursor Cursor { get; set; }
    }

    public class ProjectCursor
    {
        [JsonProperty("after")]
        public string After { get; set; }

        [JsonProperty("before")]
        public string Before { get; set; }
    }

    public class ProjectSummary
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
        public ProjectProgress Progress { get; set; }

        [JsonProperty("reward")]
        public ProjectReward Reward { get; set; }
    }

    /// <summary>Full detail for one project.</summary>
    public class Project : ProjectSummary
    {
        [JsonProperty("creator")]
        public ProjectActor Creator { get; set; }

        [JsonProperty("details")]
        public ProjectDetails Details { get; set; }

        [JsonProperty("contribution")]
        public ProjectContributionConfig Contribution { get; set; }

        /// <summary>
        /// The objective config - a per-project-type discriminated union. Left as a
        /// loose map; inspect the single populated key (e.g. <c>deliver_item</c>, <c>mine_material</c>).
        /// </summary>
        [JsonProperty("configuration")]
        public Dictionary<string, object> Configuration { get; set; }
    }

    public class ProjectProgress
    {
        [JsonProperty("current")]
        public long Current { get; set; }

        [JsonProperty("desired")]
        public long Desired { get; set; }
    }

    public class ProjectReward
    {
        [JsonProperty("initial")]
        public decimal Initial { get; set; }

        [JsonProperty("remaining")]
        public decimal Remaining { get; set; }
    }

    public class ProjectActor
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ProjectDetails
    {
        /// <summary>Unspecified | Explorer | Industrialist | Enforcer | Soldier of Fortune</summary>
        [JsonProperty("career")]
        public string Career { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("expires")]
        public DateTime? Expires { get; set; }

        [JsonProperty("finished")]
        public DateTime? Finished { get; set; }
    }

    public class ProjectContributionConfig
    {
        [JsonProperty("participation_limit")]
        public long ParticipationLimit { get; set; }

        [JsonProperty("reward_per_contribution")]
        public decimal RewardPerContribution { get; set; }

        [JsonProperty("submission_limit")]
        public long SubmissionLimit { get; set; }

        [JsonProperty("submission_multiplier")]
        public decimal SubmissionMultiplier { get; set; }
    }

    /// <summary>A page of contributors to a project.</summary>
    public class ProjectContributors
    {
        [JsonProperty("contributors")]
        public List<ProjectContributor> Contributors { get; set; } = new List<ProjectContributor>();

        [JsonProperty("cursor")]
        public ProjectCursor Cursor { get; set; }
    }

    public class ProjectContributor
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contributed")]
        public long Contributed { get; set; }
    }

    /// <summary>One character's contribution to a project.</summary>
    public class ProjectContribution
    {
        [JsonProperty("contributed")]
        public long Contributed { get; set; }

        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
