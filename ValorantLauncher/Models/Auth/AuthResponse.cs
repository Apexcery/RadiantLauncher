using System.Collections.Generic;

namespace ValorantLauncher.Models.Auth
{
    public class AuthResponse
    {
        public string Type { get; set; }
        public string? Error { get; set; }
        public ResponseObject? Response { get; set; }
        public string Country { get; set; }
        public MultifactorObject? Multifactor { get; set; }
        public string SecurityProfile { get; set; }

        public class ParametersObject
        {
            public string Uri { get; set; }
        }

        public class ResponseObject
        {
            public string Mode { get; set; }
            public ParametersObject Parameters { get; set; }
        }

        public class MultifactorObject
        {
            public string Email { get; set; }
            public string Method { get; set; }
            public List<string> Methods { get; set; }
            public int MultiFactorCodeLength { get; set; }
            public string MfaVersion { get; set; }
        }
    }
}
