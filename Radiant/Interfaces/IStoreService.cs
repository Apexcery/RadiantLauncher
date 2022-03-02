using System.Threading.Tasks;
using Radiant.Models.Store;

namespace Radiant.Interfaces
{
    public interface IStoreService
    {
        Task<PlayerStore> GetPlayerStore();
        Task<StoreOffers> GetStoreOffers();

        Task<SkinInformation> GetSkinInformation(string itemId);
        Task<int> GetSkinPrice(string itemId);

        Task<BundleInformation> GetBundleInformation(string bundleId);
    }
}
