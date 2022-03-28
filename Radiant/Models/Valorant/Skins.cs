using Newtonsoft.Json;

namespace Radiant.Models.Valorant
{
    public class Skins
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        
        [JsonProperty("data")]
        public Skin[] SkinData { get; set; }

        public class Skin
        {
            [JsonProperty("uuid")]
            public string Id { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("themeUuid")]
            public string ThemeId { get; set; }

            [JsonProperty("contentTierUuid")]
            public string ContentTierUuid { get; set; }

            [JsonProperty("displayIcon")]
            public string DisplayIcon { get; set; }

            [JsonProperty("wallpaper")]
            public string Wallpaper { get; set; }

            [JsonProperty("assetPath")]
            public string AssetPath { get; set; }

            [JsonProperty("chromas")]
            public Chroma[] Chromas { get; set; }

            [JsonProperty("levels")]
            public Level[] Levels { get; set; }
        }

        public class Chroma
        {
            [JsonProperty("uuid")]
            public string Id { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("displayIcon")]
            public string DisplayIcon { get; set; }

            [JsonProperty("fullRender")]
            public string FullRender { get; set; }

            [JsonProperty("swatch")]
            public string Swatch { get; set; }

            [JsonProperty("streamedVideo")]
            public string StreamedVideo { get; set; }

            [JsonProperty("assetPath")]
            public string AssetPath { get; set; }
        }

        public class Level
        {
            [JsonProperty("uuid")]
            public string Id { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("levelItem")]
            public string LevelItem { get; set; }

            [JsonProperty("displayIcon")]
            public string DisplayIcon { get; set; }

            [JsonProperty("streamedVideo")]
            public string StreamedVideo { get; set; }

            [JsonProperty("assetPath")]
            public string AssetPath { get; set; }
        }

    }
}
