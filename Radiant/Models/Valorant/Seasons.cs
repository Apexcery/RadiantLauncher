using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace Radiant.Models.Valorant
{
    public class Seasons
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("data")]
        public Season[] SeasonData { get; set; }

        public class Season
        {
            [JsonProperty("uuid")]
            public string Id { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }
            
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("startTime")]
            public DateTime StartTime { get; set; }

            [JsonProperty("endTime")]
            public DateTime EndTime { get; set; }
            
            [JsonProperty("parentUuid")]
            public string ParentId { get; set; }
            
            [JsonProperty("assetPath")]
            public string AssetPath { get; set; }
        }
        
        public static string GetSeasonFullName(string seasonId, IEnumerable<Season> seasons)
        {
            var seasonArray = seasons as Season[] ?? seasons.ToArray();
            var season = seasonArray.FirstOrDefault(x => x.Id == seasonId);
            if (season == null)
                return null;
            
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            var seasonName = textInfo.ToTitleCase(season.DisplayName.ToLowerInvariant());
            
            if (string.IsNullOrEmpty(season.ParentId))
                return seasonName;

            var parentName = textInfo.ToTitleCase(seasonArray.FirstOrDefault(x => x.Id == season.ParentId)?.DisplayName.ToLowerInvariant() ?? "");
            return $"{parentName} {seasonName}";
        }
    }
}
