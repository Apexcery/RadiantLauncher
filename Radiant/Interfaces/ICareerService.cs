using System.Threading.Tasks;
using ValorantLauncher.Models.Career;

namespace ValorantLauncher.Interfaces
{
    public interface ICareerService
    {
        Task<PlayerRankInfo> GetPlayerRankInfo();
        Task<PlayerRankUpdates> GetPlayerRankUpdates(int amount = 15, string queue = null);
        Task<PlayerMatchHistory> GetPlayerMatchHistory(int amount = 15, string queue = null);
        Task<MatchData> GetMatchData(string matchId);
    }
}
