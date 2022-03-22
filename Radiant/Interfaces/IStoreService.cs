using System.Threading;
using System.Threading.Tasks;
using Radiant.Models.Store;

namespace Radiant.Interfaces
{
    public interface IStoreService
    {
        Task<PlayerStore> GetPlayerStore(CancellationToken cancellationToken);
        Task<StoreOffers> GetStoreOffers(CancellationToken cancellationToken);

        Task<SkinInformation> GetSkinInformation(CancellationToken cancellationToken, string itemId);
        Task<int> GetSkinPrice(CancellationToken cancellationToken, string itemId);

        Task<BundleInformation> GetBundleInformation(CancellationToken cancellationToken, string bundleId);
    }
}
