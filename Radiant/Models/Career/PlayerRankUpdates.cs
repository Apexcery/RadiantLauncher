using System.Collections.Generic;

namespace Radiant.Models.Career
{
    public class PlayerRankUpdates
    {
        public int Version { get; set; }

        public string Subject { get; set; }

        public List<CompetitiveUpdate> Matches { get; set; }
    }
}
