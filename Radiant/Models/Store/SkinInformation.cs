using System.Collections.Generic;
using Newtonsoft.Json;

namespace Radiant.Models.Store
{
    public class SkinInformation
    {
        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("themeUuid")]
        public string ThemeUUID { get; set; }

        [JsonProperty("displayIcon")]
        public string DisplayIcon { get; set; }
        
        [JsonProperty("chromas")]
        public List<Chroma> Chromas { get; set; }
        
        [JsonProperty("levels")]
        public List<Level> Levels { get; set; }

        public class Level
        {
            [JsonProperty("LevelUuid")]
            public string LevelUUID { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("displayIcon")]
            public string DisplayIcon { get; set; }

            [JsonProperty("streamedVideoUrl")]
            public string StreamedVideoUrl { get; set; }
        }

        public class Chroma
        {
            [JsonProperty("ChromaUuid")]
            public string ChromaUUID { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("displayIcon")]
            public string DisplayIcon { get; set; }

            [JsonProperty("streamedVideoUrl")]
            public string StreamedVideoUrl { get; set; }

        }
    }
}
