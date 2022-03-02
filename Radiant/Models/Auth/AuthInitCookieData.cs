using Newtonsoft.Json;

namespace Radiant.Models.Auth
{
    public class AuthInitCookieData
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; } = "play-valorant-web-prod";

        [JsonProperty("nonce")]
        public int Nonce { get; set; } = 1;

        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; } = "https://playvalorant.com/opt_in";

        [JsonProperty("response_type")]
        public string ResponseType { get; set; } = "token id_token";

        [JsonProperty("scope")]
        public string Scope { get; set; } = "account openid";
    }
}
