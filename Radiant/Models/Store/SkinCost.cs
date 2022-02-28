using System.Collections.Generic;
using Newtonsoft.Json;

namespace Radiant.Models.Store
{
    public class SkinCost
    {
        [JsonProperty("offerID")]
        public string OfferID { get; set; }

        [JsonProperty("isDirectPurchase")]
        public bool IsDirectPurchase { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("cost")]
        public CostObject Cost { get; set; }

        [JsonProperty("rewards")]
        public List<RewardObject> Rewards { get; set; }

        public class CostObject
        {
            [JsonProperty("valorantPointCost")]
            public int ValorantPointCost { get; set; }
        }

        public class RewardObject
        {
            [JsonProperty("itemTypeID")]
            public string ItemTypeID { get; set; }

            [JsonProperty("itemID")]
            public string ItemID { get; set; }

            [JsonProperty("quantity")]
            public int Quantity { get; set; }
        }
    }
}
