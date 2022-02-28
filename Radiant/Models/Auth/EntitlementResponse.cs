using Newtonsoft.Json;

namespace Radiant.Models.Auth
{
    public class EntitlementResponse
    {
        [JsonProperty("entitlements_token")]
        public string EntitlementsToken { get; set; }
    }
}
