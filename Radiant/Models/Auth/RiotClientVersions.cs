using System;
using Newtonsoft.Json;

namespace ValorantLauncher.Models.Auth
{
    public class RiotClientVersions
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("data")]
        public DataObject Data { get; set; }

        public class DataObject
        {
            [JsonProperty("manifestId")]
            public string ManifestId { get; set; }

            [JsonProperty("branch")]
            public string Branch { get; set; }

            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("buildVersion")]
            public string BuildVersion { get; set; }

            [JsonProperty("riotClientVersion")]
            public string RiotClientVersion { get; set; }

            [JsonProperty("buildDate")]
            public DateTime BuildDate { get; set; }
        }
    }
}
