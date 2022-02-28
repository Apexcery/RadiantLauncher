using System.Collections.Generic;
using Newtonsoft.Json;

namespace ValorantLauncher.Models.Store
{
    public class StoreOffers
    {
        [JsonProperty("Offers")]
        public List<Offer> Offers { get; set; }

        [JsonProperty("UpgradeCurrencyOffers")]
        public List<UpgradeCurrencyOffer> UpgradeCurrencyOffers { get; set; }

        public class Cost
        {
            [JsonProperty("85ad13f7-3d1b-5128-9eb2-7cd8ee0b5741")]
            public int ValorantPointCost { get; set; }
        }

        public class Reward
        {
            [JsonProperty("ItemTypeID")]
            public string ItemTypeID { get; set; }

            [JsonProperty("ItemID")]
            public string ItemID { get; set; }

            [JsonProperty("Quantity")]
            public int Quantity { get; set; }
        }

        public class Offer
        {
            [JsonProperty("OfferID")]
            public string OfferID { get; set; }

            [JsonProperty("IsDirectPurchase")]
            public bool IsDirectPurchase { get; set; }

            [JsonProperty("StartDate")]
            public string StartDate { get; set; }

            [JsonProperty("Cost")]
            public Cost Cost { get; set; }

            [JsonProperty("Rewards")]
            public List<Reward> Rewards { get; set; }
        }

        public class UpgradeCurrencyOffer
        {
            [JsonProperty("OfferID")]
            public string OfferID { get; set; }

            [JsonProperty("StorefrontItemID")]
            public string StorefrontItemID { get; set; }
        }
    }
}
