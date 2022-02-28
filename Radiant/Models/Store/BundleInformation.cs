using Newtonsoft.Json;

namespace ValorantLauncher.Models.Store
{
    public class BundleInformation
    {
        [JsonProperty("bundleDisplayName")]
        public string BundleDisplayName { get; set; }

        [JsonProperty("bundleDisplayIcon")]
        public string BundleDisplayIcon { get; set; }

        [JsonProperty("bundleVerticalImage")]
        public string BundleVerticalImage { get; set; }

        [JsonProperty("bundleDescription")]
        public string BundleDescription { get; set; }

        [JsonProperty("bundleExtraDescription")]
        public object BundleExtraDescription { get; set; }

        [JsonProperty("bundlePromoDescription")]
        public object BundlePromoDescription { get; set; }

        [JsonProperty("bundleDisplayIcon2")]
        public string BundleDisplayIcon2 { get; set; }

        [JsonProperty("bundleDataAssetID")]
        public string BundleDataAssetID { get; set; }
    }
}
