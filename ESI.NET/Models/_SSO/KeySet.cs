using ESI.NET.Enumerations;

namespace ESI.NET.Models._SSO
{
    public class KeySet
    {
        public JwtKeyType alg { get; set; }
        public string use { get; set; }
        public string kid { get; set; }
        public string kty { get; set; }

        public string crv { get; set; }

        public string e { get; set; }

        public string n { get; set; }
        public string x { get; set; }
        public string y { get; set; }
    }
}
