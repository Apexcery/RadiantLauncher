using System.Collections.Generic;
using Newtonsoft.Json;

namespace Radiant.Models.Auth
{
    public class RegionResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("affinities")]
        public Dictionary<string, string> Affinities { get; set; }
    }
}
