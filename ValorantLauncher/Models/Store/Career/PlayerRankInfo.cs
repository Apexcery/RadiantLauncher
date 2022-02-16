using System.Collections.Generic;
using Newtonsoft.Json;

namespace ValorantLauncher.Models.Store.Career
{
    public class PlayerRankInfo
    {
        public long Version { get; set; }
        
        public string Subject { get; set; }

        public bool NewPlayerExperienceFinished { get; set; }

        [JsonProperty("QueueSkills")]
        public QueueDataObject QueueData { get; set; }

        public CompetitiveUpdate LatestCompetitiveUpdate { get; set; }

        public bool IsLeaderboardAnonymized { get; set; }

        public bool IsActRankBadgeHidden { get; set; }

        public class QueueDataObject
        {
            [JsonProperty("competitive")]
            public QueueInfo Competitive { get; set; }

            [JsonProperty("custom")]
            public QueueInfo Custom { get; set; }

            [JsonProperty("deathmatch")]
            public QueueInfo Deathmatch { get; set; }

            [JsonProperty("ggteam")]
            public QueueInfo Escalation { get; set; }

            [JsonProperty("newmap")]
            public QueueInfo NewMap { get; set; }

            [JsonProperty("onefa")]
            public QueueInfo Replication { get; set; }

            [JsonProperty("seeding")]
            public QueueInfo Seeding { get; set; }

            [JsonProperty("snowball")]
            public QueueInfo SnowballFight { get; set; }

            [JsonProperty("spikerush")]
            public QueueInfo SpikeRush { get; set; }

            [JsonProperty("unrated")]
            public QueueInfo Unrated { get; set; }

            public class QueueInfo
            {
                public int TotalGamesNeededForRating { get; set; }
                public int TotalGamesNeededForLeaderboard { get; set; }
                public int CurrentSeasonGamesNeededForRating { get; set; }
                public SeasonalInfoBySeasonId SeasonalInfoBySeasonID { get; set; }

                public class SeasonalInfoBySeasonId
                {
                    [JsonProperty("0df5adb9-4dcb-6899-1306-3e9860661dd3")]
                    public ActInformation ClosedBeta { get; set; }


                    [JsonProperty("3f61c772-4560-cd3f-5d3f-a7ab5abda6b3")]
                    public ActInformation Episode1Act1 { get; set; }

                    [JsonProperty("0530b9c4-4980-f2ee-df5d-09864cd00542")]
                    public ActInformation Episode1Act2 { get; set; }

                    [JsonProperty("46ea6166-4573-1128-9cea-60a15640059b")]
                    public ActInformation Episode1Act3 { get; set; }


                    [JsonProperty("97b6e739-44cc-ffa7-49ad-398ba502ceb0")]
                    public ActInformation Episode2Act1 { get; set; }

                    [JsonProperty("ab57ef51-4e59-da91-cc8d-51a5a2b9b8ff")]
                    public ActInformation Episode2Act2 { get; set; }

                    [JsonProperty("52e9749a-429b-7060-99fe-4595426a0cf7")]
                    public ActInformation Episode2Act3 { get; set; }


                    [JsonProperty("2a27e5d2-4d30-c9e2-b15a-93b8909a442c")]
                    public ActInformation Episode3Act1 { get; set; }

                    [JsonProperty("4cb622e1-4244-6da3-7276-8daaf1c01be2")]
                    public ActInformation Episode3Act2 { get; set; }

                    [JsonProperty("a16955a5-4ad0-f761-5e9e-389df1c892fb")]
                    public ActInformation Episode3Act3 { get; set; }


                    [JsonProperty("573f53ac-41a5-3a7d-d9ce-d6a6298e5704")]
                    public ActInformation Episode4Act1 { get; set; }

                    [JsonProperty("d929bc38-4ab6-7da4-94f0-ee84f8ac141e")]
                    public ActInformation Episode4Act2 { get; set; }

                    [JsonProperty("3e47230a-463c-a301-eb7d-67bb60357d4f")]
                    public ActInformation Episode4Act3 { get; set; }
                    
                    public class ActInformation
                    {
                        public string SeasonID { get; set; }
                        
                        public int NumberOfWins { get; set; }

                        public int NumberOfWinsWithPlacements { get; set; }

                        public int NumberOfGames { get; set; }

                        public int Rank { get; set; }

                        public int CapstoneWins { get; set; }

                        public int LeaderboardRank { get; set; }

                        public int CompetitiveTier { get; set; }

                        public int RankedRating { get; set; }

                        public Dictionary<int, int> WinsByTier { get; set; }

                        public int GamesNeededForRating { get; set; }

                        public int TotalWinsNeededForRank { get; set; }
                    }
                }
            }
        }
    }

    public class CompetitiveUpdate
    {
        public string MatchID { get; set; }

        public string MapID { get; set; }

        public string SeasonID { get; set; }

        public long MatchStartTime { get; set; }

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
