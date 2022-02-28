using Newtonsoft.Json;
using Radiant.Utils;

namespace Radiant.Models.Client
{
    public class RiotClientInstalls
    {
        [JsonProperty("patchlines")]
        public PatchlinesObject Patchlines { get; set; }

        [JsonProperty("rc_default")]
        public string RcDefault { get; set; }

        [JsonProperty("rc_live")]
        public string RcLive { get; set; }

        public class PatchlinesObject
        {
            [JsonProperty("KeystoneFoundationLiveWin")]
            public string KeystoneFoundationLiveWin { get; set; }
        }
    }
}
