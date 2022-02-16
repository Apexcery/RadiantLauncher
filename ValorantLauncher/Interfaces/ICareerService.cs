using System.Threading.Tasks;
using ValorantLauncher.Models.Store.Career;

namespace ValorantLauncher.Interfaces
{
    public interface ICareerService
    {
        Task<PlayerRankInfo> GetPlayerRankInfo();
    }
}
