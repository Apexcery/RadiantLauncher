using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using ValorantLauncher.Extensions;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models;
using ValorantLauncher.Models.Store;

namespace ValorantLauncher.Services
{
    public class StoreService : IStoreService
    {
        private readonly UserData _userData;

        public StoreService(UserData userData, IHttpClientFactory httpClientFactory)
        {
            _userData = userData;
        }

        public async Task<PlayerStore> GetPlayerStore()
        {
            if (_userData.RiotUserData?.Sub == null || _userData.RiotUrl?.PdUrl == null)
            {
                MessageBox.Show("Login to your account before trying to view your store.");
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/store/v2/storefront/{_userData.RiotUserData.Sub}");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to get player's store."); //TODO: Better error messages.
                return null;
            }

            var playerStore = await response.Content.ReadAsJsonAsync<PlayerStore>();
            return playerStore;
        }

        public async Task<StoreOffers> GetStoreOffers()
        {
            if (_userData.RiotUrl?.PdUrl == null)
            {
                MessageBox.Show("Login to your account before trying to view your store.");
                return null;
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls11;

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/store/v1/offers/");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to get store offers."); //TODO: Better error messages.
                return null;
            }

            var storeOffers = await response.Content.ReadAsJsonAsync<StoreOffers>();
            return storeOffers;
        }
    }
}
