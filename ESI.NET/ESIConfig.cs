using ESI.NET.Enumerations;

namespace ESI.NET
{
    public class ESIConfig
    {
        public string UserAgent { get; set; }
        public DataSource DataSource { get; set; }
        public string ClientId { get; set; }
        public string SecretKey { get; set; }
        public string CallbackUrl { get; set; }
    }
}
