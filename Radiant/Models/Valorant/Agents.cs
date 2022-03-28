using Newtonsoft.Json;

namespace Radiant.Models.Valorant
{
    public class Agents
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        
        [JsonProperty("data")]
        public Agent[] AgentData { get; set; }

        public class Agent
        {
            [JsonProperty("uuid")]
            public string Id { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("developerName")]
            public string DeveloperName { get; set; }

            [JsonProperty("characterTags")]
            public string[] CharacterTags { get; set; }

            [JsonProperty("displayIcon")]
            public string DisplayIcon { get; set; }

            [JsonProperty("displayIconSmall")]
            public string DisplayIconSmall { get; set; }

            [JsonProperty("bustPortrait")]
            public string BustPortrait { get; set; }

            [JsonProperty("fullPortrait")]
            public string FullPortrait { get; set; }

            [JsonProperty("fullPortraitV2")]
            public string FullPortraitV2 { get; set; }

            [JsonProperty("killfeedPortrait")]
            public string KillFeedPortrait { get; set; }

            [JsonProperty("background")]
            public string Background { get; set; }

            [JsonProperty("assetPath")]
            public string AssetPath { get; set; }

            [JsonProperty("isFullPortraitRightFacing")]
            public bool IsFullPortraitRightFacing { get; set; }

            [JsonProperty("isPlayableCharacter")]
            public bool IsPlayableCharacter { get; set; }

            [JsonProperty("isAvailableForTest")]
            public bool IsAvailableForTest { get; set; }

            [JsonProperty("isBaseContent")]
            public bool IsBaseContent { get; set; }

            [JsonProperty("role")]
            public Role Role { get; set; }

            [JsonProperty("abilities")]
            public Ability[] Abilities { get; set; }

            [JsonProperty("voiceLine")]
            public Voiceline VoiceLine { get; set; }
        }

        public class Role
        {
            [JsonProperty("uuid")]
            public string Id { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("displayIcon")]
            public string DisplayIcon { get; set; }

            [JsonProperty("assetPath")]
            public string AssetPath { get; set; }
        }

        public class Voiceline
        {
            [JsonProperty("minDuration")]
            public float MinDuration { get; set; }

            [JsonProperty("maxDuration")]
            public float MaxDuration { get; set; }

            [JsonProperty("mediaList")]
            public Medialist[] MediaList { get; set; }
        }

        public class Medialist
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("wwise")]
            public string Wwise { get; set; }

            [JsonProperty("wave")]
            public string Wave { get; set; }
        }

        public class Ability
        {
            [JsonProperty("slot")]
            public string Slot { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("displayIcon")]
            public string DisplayIcon { get; set; }
        }
    }
}
