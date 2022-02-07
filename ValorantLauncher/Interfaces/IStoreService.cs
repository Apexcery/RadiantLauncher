using System.Threading.Tasks;
using ValorantLauncher.Models.Store;

namespace ValorantLauncher.Interfaces
{
    public interface IStoreService
    {
        Task<PlayerStore> GetPlayerStore();
        Task<StoreOffers> GetStoreOffers();
    }
}
