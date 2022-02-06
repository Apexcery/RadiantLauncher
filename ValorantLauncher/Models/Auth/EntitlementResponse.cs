using Newtonsoft.Json;

namespace ValorantLauncher.Models.Auth
{
    public class EntitlementResponse
    {
        [JsonProperty("entitlements_token")]
        public string EntitlementsToken { get; set; }
    }
}
