using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Corporation
{
    /// <summary>A page of Corporation Projects. Paginate with <see cref="Cursor"/>.</summary>
    public class ProjectList
    {
        /// <summary>List of projects.</summary>
        [JsonProperty("projects")]
        public List<ProjectSummary> Projects { get; set; } = new List<ProjectSummary>();

        /// <summary>
        /// Cursors for paging: pass <c>Before</c> or <c>After</c> to the next call's <c>before</c>
        /// / <c>after</c> parameter.
        /// </summary>
        [JsonProperty("cursor")]
        public ProjectCursor Cursor { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Corporation.ProjectContributors.Cursor"/> and
    /// <see cref="ESI.NET.Models.Corporation.ProjectList.Cursor"/>.
    /// </summary>
    public class ProjectCursor
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
    /// Part of <see cref="ESI.NET.Models.Corporation.ProjectList.Projects"/>.
    /// </summary>
    public class ProjectSummary
    {
        /// <summary>Project's ID.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Project's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Moment this project was last modified. Project contributions also count as a
        /// modification.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Project's current state. One of <c>Unspecified</c>, <c>Active</c>, <c>Closed</c>,
        /// <c>Completed</c>, <c>Expired</c>, <c>Deleted</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>Project's progress.</summary>
        [JsonProperty("progress")]
        public ProjectProgress Progress { get; set; }

        /// <summary>Project's reward.</summary>
        [JsonProperty("reward")]
        public ProjectReward Reward { get; set; }
    }

    /// <summary>Full detail for one project.</summary>
    public class Project : ProjectSummary
    {
        /// <summary>Project's creator.</summary>
        [JsonProperty("creator")]
        public ProjectActor Creator { get; set; }

        /// <summary>Project's details.</summary>
        [JsonProperty("details")]
        public ProjectDetails Details { get; set; }

        /// <summary>Project's contribution settings.</summary>
        [JsonProperty("contribution")]
        public ProjectContributionConfig Contribution { get; set; }

        /// <summary>
        /// The project's objective configuration. Its shape depends on the project type, so it is
        /// kept as a loose map: inspect the single populated key (e.g. <c>deliver_item</c>,
        /// <c>mine_material</c>).
        /// </summary>
        [JsonProperty("configuration")]
        public Dictionary<string, object> Configuration { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Corporation.ProjectSummary.Progress"/>.
    /// </summary>
    public class ProjectProgress
    {
        /// <summary>Current progress.</summary>
        [JsonProperty("current")]
        public long Current { get; set; }

        /// <summary>Desired progress.</summary>
        [JsonProperty("desired")]
        public long Desired { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Corporation.ProjectSummary.Reward"/>.
    /// </summary>
    public class ProjectReward
    {
        /// <summary>Original amount of ISK that was reserved for this project.</summary>
        [JsonProperty("initial")]
        public decimal Initial { get; set; }

        /// <summary>Remaining ISK to be awarded.</summary>
        [JsonProperty("remaining")]
        public decimal Remaining { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Corporation.Project.Creator"/>.</summary>
    public class ProjectActor
    {
        /// <summary>Creator's character ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Creator's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Corporation.Project.Details"/>.</summary>
    public class ProjectDetails
    {
        /// <summary>
        /// Assigned career path. One of <c>Unspecified</c>, <c>Explorer</c>, <c>Industrialist</c>,
        /// <c>Enforcer</c>, <c>Soldier of Fortune</c>.
        /// </summary>
        [JsonProperty("career")]
        public string Career { get; set; }

        /// <summary>Moment this project was created.</summary>
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        /// <summary>Description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Moment this project expires.</summary>
        [JsonProperty("expires")]
        public DateTime? Expires { get; set; }

        /// <summary>Moment this project transitioned to a non-active state.</summary>
        [JsonProperty("finished")]
        public DateTime? Finished { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Corporation.Project.Contribution"/>.
    /// </summary>
    public class ProjectContributionConfig
    {
        /// <summary>Limit on the contribution of the individual participant.</summary>
        [JsonProperty("participation_limit")]
        public long ParticipationLimit { get; set; }

        /// <summary>ISK reward per contribution.</summary>
        [JsonProperty("reward_per_contribution")]
        public decimal RewardPerContribution { get; set; }

        /// <summary>Limit on amount of contribution per submission.</summary>
        [JsonProperty("submission_limit")]
        public long SubmissionLimit { get; set; }

        /// <summary>Multiplier towards progress per contribution.</summary>
        [JsonProperty("submission_multiplier")]
        public decimal SubmissionMultiplier { get; set; }
    }

    /// <summary>A page of contributors to a project.</summary>
    public class ProjectContributors
    {
        /// <summary>List of contributors.</summary>
        [JsonProperty("contributors")]
        public List<ProjectContributor> Contributors { get; set; } = new List<ProjectContributor>();

        /// <summary>
        /// Cursors for paging: pass <c>Before</c> or <c>After</c> to the next call's <c>before</c>
        /// / <c>after</c> parameter.
        /// </summary>
        [JsonProperty("cursor")]
        public ProjectCursor Cursor { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Corporation.ProjectContributors.Contributors"/>.
    /// </summary>
    public class ProjectContributor
    {
        /// <summary>Contributor's character ID.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Contributor's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Contributor's contributed progress.</summary>
        [JsonProperty("contributed")]
        public long Contributed { get; set; }
    }

    /// <summary>One character's contribution to a project.</summary>
    public class ProjectContribution
    {
        /// <summary>Your contribution.</summary>
        [JsonProperty("contributed")]
        public long Contributed { get; set; }

        /// <summary>Moment this information was last modified.</summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }
    }
}
