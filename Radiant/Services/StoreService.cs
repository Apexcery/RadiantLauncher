using System.Net;
using System.Threading.Tasks;
using System.Windows;
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

        public async Task<PlayerStore> GetPlayerStore()
        {
            if (_userData.RiotUserData?.Puuid == null || _userData.RiotUrl?.PdUrl == null)
            {
                var dialog = new PopupDialog(_appConfig, "Error", "Login to your account before trying to view your store.");
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/store/v2/storefront/{_userData.RiotUserData.Puuid}");
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", "Failed to get player's store.", response.ReasonPhrase);
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var playerStore = await response.Content.ReadAsJsonAsync<PlayerStore>();
            return playerStore;
        }

        public async Task<StoreOffers> GetStoreOffers()
        {
            if (_userData.RiotUrl?.PdUrl == null)
            {
                var dialog = new PopupDialog(_appConfig, "Error", "Login to your account before trying to view your store.");
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls11;

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/store/v1/offers/");
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", "Failed to get store offers.", response.ReasonPhrase);
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var storeOffers = await response.Content.ReadAsJsonAsync<StoreOffers>();
            return storeOffers;
        }

        public async Task<SkinInformation> GetSkinInformation(string itemId)
        {
            var baseAddress = ApiURIs.URIs["SkinUri"].OriginalString;
            var response = await _userData.Client.GetAsync($"{baseAddress}/{itemId}");
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", "Could not retrieve skin data for ID:", itemId, response.ReasonPhrase);
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var skinInfo = await response.Content.ReadAsJsonAsync<SkinInformation>();
            return skinInfo;
        }

        public async Task<int> GetSkinPrice(string itemId)
        {
            var baseAddress = ApiURIs.URIs["OfferUri"].OriginalString;
            var response = await _userData.Client.GetAsync($"{baseAddress}/{itemId}");
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", "Could not retrieve skin price for ID:", itemId, response.ReasonPhrase);
                await DialogHost.Show(dialog, "MainDialogHost");
                return 99999;
            }

            var price = await response.Content.ReadAsJsonAsync<SkinCost>();
            return price.Cost.ValorantPointCost;
        }

        public async Task<BundleInformation> GetBundleInformation(string bundleId)
        {
            var baseAddress = ApiURIs.URIs["BundleUri"].OriginalString;
            var response = await _userData.Client.GetAsync($"{baseAddress}/{bundleId}");
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", "Could not retrieve bundle data for ID:", bundleId, response.ReasonPhrase);
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var bundleInfo = await response.Content.ReadAsJsonAsync<BundleInformation>();
            return bundleInfo;
        }
    }
}
