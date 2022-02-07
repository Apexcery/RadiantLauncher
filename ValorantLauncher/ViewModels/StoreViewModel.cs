using System.Threading.Tasks;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models;
using ValorantLauncher.Models.Store;

namespace ValorantLauncher.ViewModels
{
    public class StoreViewModel
    {
        private readonly UserData _userData;
        private readonly IStoreService _storeService;

        public StoreViewModel(UserData userData, IStoreService storeService)
        {
            _userData = userData;
            _storeService = storeService;
        }

        public async Task GetStoreData()
        {
            var playerStore = await _storeService.GetPlayerStore();
            var storeOffers = await _storeService.GetStoreOffers();

            PopulateStoreView(playerStore, storeOffers);
        }

        private void PopulateStoreView(PlayerStore playerStore, StoreOffers storeOffers)
        {
            for (var i = 0; i < playerStore.RotatingStore.SingleItemOffers.Count; i++)
            {
                //TODO: Create user control with item data.
            }
        }
    }
}
