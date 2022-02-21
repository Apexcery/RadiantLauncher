using System.Threading.Tasks;
using ValorantLauncher.Models.Career;

namespace ValorantLauncher.Interfaces
{
    public interface ICareerService
    {
        Task<PlayerRankInfo> GetPlayerRankInfo();
        Task<PlayerRankUpdates> GetPlayerRankUpdates();
    }
}
