using System.Threading;
using System.Threading.Tasks;
using Radiant.Models.Career;

namespace Radiant.Interfaces
{
    public interface ICareerService
    {
        Task<PlayerRankInfo> GetPlayerRankInfo(CancellationToken cancellationToken);
        Task<PlayerRankUpdates> GetPlayerRankUpdates(CancellationToken cancellationToken, int amount = 15, string queue = null);
        Task<PlayerMatchHistory> GetPlayerMatchHistory(CancellationToken cancellationToken, int amount = 15, string queue = null);
        Task<MatchData> GetMatchData(CancellationToken cancellationToken, string matchId);
    }
}
