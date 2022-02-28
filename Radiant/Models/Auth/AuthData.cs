using Newtonsoft.Json;

namespace Radiant.Models.Auth
{
    public class AuthData
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("remember")]
        public string Remember { get; set; }
    }
}
