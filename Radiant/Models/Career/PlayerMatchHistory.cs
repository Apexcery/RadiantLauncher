using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Radiant.Utils;

namespace Radiant.Models.Career
{
	public class PlayerMatchHistory
    {
        public string Subject { get; set; }

        public int BeginIndex { get; set; }

        public int EndIndex { get; set; }

        public int Total { get; set; }

        [JsonProperty("History")]
        public List<Match> Matches { get; set; }

        public class Match
        {
            public string MatchID { get; set; }
            
            [JsonConverter(typeof(MillisecondEpochConverter))]
            public DateTime GameStartTime { get; set; }

            public string QueueID { get; set; }
        }
    }
}
