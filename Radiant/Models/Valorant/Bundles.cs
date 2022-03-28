using Newtonsoft.Json;

namespace Radiant.Models.Valorant
{
    public class Bundles
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        
        [JsonProperty("data")]
        public Bundle[] BundleData { get; set; }

        public class Bundle
        {
            [JsonProperty("uuid")]
            public string Id { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("displayNameSubText")]
            public string DisplayNameSubText { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("extraDescription")]
            public string ExtraDescription { get; set; }

            [JsonProperty("promoDescription")]
            public string PromoDescription { get; set; }

            [JsonProperty("useAdditionalContext")]
            public bool UseAdditionalContext { get; set; }

            [JsonProperty("displayIcon")]
            public string DisplayIcon { get; set; }

            [JsonProperty("displayIcon2")]
            public string DisplayIcon2 { get; set; }

            [JsonProperty("verticalPromoImage")]
            public string VerticalPromoImage { get; set; }

            [JsonProperty("assetPath")]
            public string AssetPath { get; set; }
        }

    }
}
