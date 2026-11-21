using ESI.NET.Enumerations;

namespace ESI.NET
{
    public class EsiConfig
    {
        public string EsiUrl { get; set; }
        /// <summary>
        /// The EVE server to talk to, sent as <c>X-Tenant</c>. Defaults to
        /// <see cref="DataSource.Tranquility"/> (the live server); it used to default to
        /// <see cref="DataSource.Singularity"/> by accident (the enum's first value), which ESI
        /// rejects with <c>400 Invalid tenant</c>.
        /// </summary>
        public DataSource DataSource { get; set; } = DataSource.Tranquility;
        public string ClientId { get; set; }
        public string SecretKey { get; set; }
        public string CallbackUrl { get; set; }
        public string UserAgent { get; set; }
    }
}
