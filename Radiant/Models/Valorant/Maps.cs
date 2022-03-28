using Newtonsoft.Json;

namespace Radiant.Models.Valorant
{
    public class Maps
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        
        [JsonProperty("data")]
        public Map[] MapData { get; set; }

        public class Map
        {
            [JsonProperty("uuid")]
            public string Id { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("coordinates")]
            public string Coordinates { get; set; }

            [JsonProperty("displayIcon")]
            public string DisplayIcon { get; set; }

            [JsonProperty("listViewIcon")]
            public string ListViewIcon { get; set; }

            [JsonProperty("splash")]
            public string Splash { get; set; }

            [JsonProperty("assetPath")]
            public string AssetPath { get; set; }

            [JsonProperty("mapUrl")]
            public string MapUrl { get; set; }

            [JsonProperty("xMultiplier")]
            public float XMultiplier { get; set; }

            [JsonProperty("yMultiplier")]
            public float YMultiplier { get; set; }

            [JsonProperty("xScalarToAdd")]
            public float XScalarToAdd { get; set; }

            [JsonProperty("yScalarToAdd")]
            public float YScalarToAdd { get; set; }

            [JsonProperty("callouts")]
            public Callout[] Callouts { get; set; }
        }

        public class Callout
        {
            [JsonProperty("regionName")]
            public string RegionName { get; set; }

            [JsonProperty("superRegionName")]
            public string SuperRegionName { get; set; }

            [JsonProperty("location")]
            public Location Location { get; set; }
        }

        public class Location
        {
            [JsonProperty("x")]
            public float X { get; set; }

            [JsonProperty("y")]
            public float Y { get; set; }
        }

    }
}
