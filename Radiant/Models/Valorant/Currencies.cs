using Newtonsoft.Json;

namespace Radiant.Models.Valorant
{
    public class Currencies
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        
        [JsonProperty("data")]
        public Currency[] CurrencyData { get; set; }

        public class Currency
        {
            [JsonProperty("uuid")]
            public string Id { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("displayNameSingular")]
            public string DisplayNameSingular { get; set; }

            [JsonProperty("displayIcon")]
            public string DisplayIcon { get; set; }

            [JsonProperty("largeIcon")]
            public string LargeIcon { get; set; }

            [JsonProperty("assetPath")]
            public string AssetPath { get; set; }
        }

    }
}
