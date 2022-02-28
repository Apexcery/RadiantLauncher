using Newtonsoft.Json;

namespace ValorantLauncher.Models.Client
{
    public class RiotClientInstalls
    {
        [JsonProperty("associated_client")]
        public AssociatedClientObject AssociatedClient { get; set; }

        [JsonProperty("patchlines")]
        public PatchlinesObject Patchlines { get; set; }

        [JsonProperty("rc_default")]
        public string RcDefault { get; set; }

        [JsonProperty("rc_live")]
        public string RcLive { get; set; }

        public class AssociatedClientObject
        {
            [JsonProperty("F:/Riot Games/League of Legends/")]
            public string FRiotGamesLeagueOfLegends { get; set; }

            [JsonProperty("F:/Riot Games/VALORANT/live/")]
            public string FRiotGamesValorantLive { get; set; }
        }

        public class PatchlinesObject
        {
            [JsonProperty("KeystoneFoundationLiveWin")]
            public string KeystoneFoundationLiveWin { get; set; }
        }
    }
}
