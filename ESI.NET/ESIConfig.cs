using ESI.NET.Enumerations;

namespace ESI.NET
{
    public class ESIConfig
    {
        public string BaseUrl { get { return "https://esi.tech.ccp.is/"; } }
        public string UserAgent { get; set; }
        public DataSource DataSource { get; set; }
        public string Token { get; set; }

        public AuthorizedCharacter AuthorizedCharacter { get; set; }
    }
}
