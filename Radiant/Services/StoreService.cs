using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Radiant.Extensions;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Models.Store;
using Radiant.Views.Dialogues;

namespace Radiant.Services
{
    public class StoreService : IStoreService
    {
        private readonly UserData _userData;
        private readonly AppConfig _appConfig;

        public StoreService(UserData userData, AppConfig appConfig)
        {
            _userData = userData;
            _appConfig = appConfig;
        }

        public async Task<PlayerStore> GetPlayerStore(CancellationToken cancellationToken)
        {
            if (_userData.RiotUserData?.Puuid == null || _userData.RiotUrl?.PdUrl == null)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Login to your account before trying to view your store."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/store/v2/storefront/{_userData.RiotUserData.Puuid}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to get player's store.", response.ReasonPhrase});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var playerStore = await response.Content.ReadAsJsonAsync<PlayerStore>(cancellationToken);
            return playerStore;
        }

        public async Task<StoreOffers> GetStoreOffers(CancellationToken cancellationToken)
        {
            if (_userData.RiotUrl?.PdUrl == null)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Login to your account before trying to view your store."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls11;

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/store/v1/offers/", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to get store offers.", response.ReasonPhrase});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var storeOffers = await response.Content.ReadAsJsonAsync<StoreOffers>(cancellationToken);
            return storeOffers;
        }
    }
}
