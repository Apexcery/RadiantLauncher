using System.Collections.Generic;
using Newtonsoft.Json;

namespace Radiant.Models.Store
{
    public class PlayerStore
    {
        public FeaturedBundle FeaturedBundle { get; set; }
        [JsonProperty("SkinsPanelLayout")]
        public RotatingStore RotatingStore { get; set; }
        [JsonProperty("BonusStore")]
        public NightMarket NightMarket { get; set; }
    }

    public class FeaturedBundle
    {
        public Bundle Bundle { get; set; }
        public List<Bundle> Bundles { get; set; }
        public int BundleRemainingDurationInSeconds { get; set; }
    }

    public class Bundle
    {
        public string ID { get; set; }
        public string DataAssetID { get; set; }
        public string CurrencyID { get; set; }
        [JsonProperty("Items")]
        public List<BundleItem> BundleItems { get; set; }
        public int DurationRemainingInSeconds { get; set; }
        public bool WholesaleOnly { get; set; }
    }

    public class BundleItem
    {
        [JsonProperty("Item")]
        public ItemOffer ItemData { get; set; }
        public int BasePrice { get; set; }
        public string CurrencyID { get; set; }
        public double DiscountPercent { get; set; }
        public int DiscountedPrice { get; set; }
        public bool IsPromoItem { get; set; }
    }

    public class ItemOffer
    {
        public string ItemTypeID { get; set; }
        public string ItemID { get; set; }
        public int Quantity { get; set; }
    }

    public class RotatingStore
    {
        public List<string> SingleItemOffers { get; set; }
        public int SingleItemOffersRemainingDurationInSeconds { get; set; }
    }

    public class NightMarket
    {
        [JsonProperty("BonusStoreOffers")]
        public List<NightMarketOffer> NightMarketOffers { get; set; }
        [JsonProperty("BonusStoreRemainingDurationInSeconds")]
        public int NightMarketRemainingDurationInSeconds { get; set; }

        public class NightMarketOffer
        {
            public string BonusOfferID { get; set; }
            public StoreOffer Offer { get; set; }
            public int DiscountPercent { get; set; }
            public Cost DiscountCosts { get; set; }
            public bool IsSeen { get; set; }
        }
    }

    public class StoreOffer
    {
        public string OfferID { get; set; }
        public bool IsDirectPurchase { get; set; }
        public string StartDate { get; set; }
        public Cost Cost { get; set; }
        public List<ItemOffer> Rewards { get; set; }
    }

    public class Cost
    {
        [JsonProperty("85ad13f7-3d1b-5128-9eb2-7cd8ee0b5741")]
        public int ValorantPointCost { get; set; }
    }
}
