using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Industry
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.IndustryLogic.JobsForCharacter"/> and
    /// <see cref="ESI.NET.Logic.IndustryLogic.JobsForCorporation"/>.
    /// </summary>
    public class Job
    {
        /// <summary>Unique job ID.</summary>
        [JsonProperty("job_id")]
        public long JobId { get; set; }

        /// <summary>ID of the character which installed this job.</summary>
        [JsonProperty("installer_id")]
        public long InstallerId { get; set; }

        /// <summary>ID of the facility where this job is running.</summary>
        [JsonProperty("facility_id")]
        public long FacilityId { get; set; }

        /// <summary>ID of the station where industry facility is located.</summary>
        [JsonProperty("station_id")]
        public long StationId { get; set; }

        /// <summary>
        /// ID of the station or structure the job runs in. The corporation endpoint fills this
        /// rather than a station ID, since structures have none.
        /// </summary>
        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        /// <summary>
        /// The job's activity: <c>1</c> manufacturing, <c>3</c> time-efficiency research, <c>4</c>
        /// material-efficiency research, <c>5</c> copying, <c>8</c> invention, <c>9</c> or
        /// <c>11</c> reactions.
        /// </summary>
        [JsonProperty("activity_id")]
        public long ActivityId { get; set; }

        /// <summary>Item ID of the blueprint used.</summary>
        [JsonProperty("blueprint_id")]
        public long BlueprintId { get; set; }

        /// <summary>Type ID of the blueprint used.</summary>
        [JsonProperty("blueprint_type_id")]
        public long BlueprintTypeId { get; set; }

        /// <summary>
        /// Location ID of the location from which the blueprint was installed. Normally a station
        /// ID, but can also be an asset (e.g. container) or corporation facility.
        /// </summary>
        [JsonProperty("blueprint_location_id")]
        public long BlueprintLocationId { get; set; }

        /// <summary>
        /// Location ID of the location to which the output of the job will be delivered. Normally a
        /// station ID, but can also be a corporation facility.
        /// </summary>
        [JsonProperty("output_location_id")]
        public long OutputLocationId { get; set; }

        /// <summary>
        /// Number of runs for a manufacturing job, or number of copies to make for a blueprint
        /// copy.
        /// </summary>
        [JsonProperty("runs")]
        public long Runs { get; set; }

        /// <summary>The sume of job installation fee and industry facility tax.</summary>
        [JsonProperty("cost")]
        public decimal Cost { get; set; }

        /// <summary>Number of runs blueprint is licensed for.</summary>
        [JsonProperty("licensed_runs")]
        public long LicensedRuns { get; set; }

        /// <summary>Chance of success for invention.</summary>
        [JsonProperty("probability")]
        public decimal Probability { get; set; }

        /// <summary>Type ID of product (manufactured, copied or invented).</summary>
        [JsonProperty("product_type_id")]
        public long ProductTypeId { get; set; }

        /// <summary>
        /// The job's status: <c>active</c>, <c>paused</c>, <c>ready</c>, <c>delivered</c>,
        /// <c>cancelled</c>, or <c>reverted</c>.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>Job duration in seconds.</summary>
        [JsonProperty("duration")]
        public long Duration { get; set; }

        /// <summary>Date and time when this job started.</summary>
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        /// <summary>Date and time when this job finished.</summary>
        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Date and time when this job was paused (i.e. time when the facility where this job was
        /// installed went offline).
        /// </summary>
        [JsonProperty("pause_date")]
        public DateTime PauseDate { get; set; }

        /// <summary>Date and time when this job was completed.</summary>
        [JsonProperty("completed_date")]
        public DateTime CompletedDate { get; set; }

        /// <summary>ID of the character which completed this job.</summary>
        [JsonProperty("completed_character_id")]
        public long CompletedCharacterId { get; set; }

        /// <summary>
        /// Number of successful runs for this job. Equal to runs unless this is an invention job.
        /// </summary>
        [JsonProperty("successful_runs")]
        public long SuccessfulRuns { get; set; }
    }
}
