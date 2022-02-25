﻿using System.Net;
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

        public StoreService(UserData userData)
        {
            _userData = userData;
        }

        public async Task<PlayerStore> GetPlayerStore()
        {
            if (_userData.RiotUserData?.Puuid == null || _userData.RiotUrl?.PdUrl == null)
            {
                MessageBox.Show("Login to your account before trying to view your store."); //TODO: Better error handling.
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/store/v2/storefront/{_userData.RiotUserData.Puuid}");
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
                MessageBox.Show("Login to your account before trying to view your store."); //TODO: Better error handling.
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

        public async Task<SkinInformation> GetSkinInformation(string itemId)
        {
            var baseAddress = ApiURIs.URIs["SkinUri"].OriginalString;
            var response = await _userData.Client.GetAsync($"{baseAddress}/{itemId}");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Could not retrieve skin data for ID: {itemId}"); //TODO: Better error handling.
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
                MessageBox.Show($"Could not retrieve skin price for ID: {itemId}"); //TODO: Better error handling.
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
                MessageBox.Show($"Could not retrieve bundle data for ID: {bundleId}"); //TODO: Better error handling.
                return null;
            }

            var bundleInfo = await response.Content.ReadAsJsonAsync<BundleInformation>();
            return bundleInfo;
        }
    }
}