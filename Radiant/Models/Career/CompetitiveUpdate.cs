using System;
using Newtonsoft.Json;
using ValorantLauncher.Utils;

namespace ValorantLauncher.Models.Career
{
    public class CompetitiveUpdate
    {
        public string MatchID { get; set; }

        public string MapID { get; set; }

        public string SeasonID { get; set; }

        [JsonConverter(typeof(MillisecondEpochConverter))]
        public DateTime MatchStartTime { get; set; }

        public int TierAfterUpdate { get; set; }

        public int TierBeforeUpdate { get; set; }

        public int RankedRatingAfterUpdate { get; set; }

        public int RankedRatingBeforeUpdate { get; set; }

        public int RankedRatingEarned { get; set; }

        public int RankedRatingPerformanceBonus { get; set; }

        public string CompetitiveMovement { get; set; }

        public int AFKPenalty { get; set; }
    }
}
