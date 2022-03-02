using Newtonsoft.Json;

namespace Radiant.Models.Auth
{
    public class MFAData
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "multifactor";

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("rememberDevice")]
        public bool RememberDevice { get; set; } = true;
    }
}
