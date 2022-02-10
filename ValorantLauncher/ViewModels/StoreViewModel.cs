using System;
using System.Collections.ObjectModel;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models;
using ValorantLauncher.Models.Store;
using ValorantLauncher.Utils;
using ValorantLauncher.Views.UserControls;

namespace ValorantLauncher.ViewModels
{
    public class StoreViewModel : Observable
    {
        private readonly UserData _userData;
        private readonly IStoreService _storeService;

        private ImageSource _bundleImageSource;
        public ImageSource BundleImageSource
        {
            get => _bundleImageSource;
            set
            {
                _bundleImageSource = value;
                OnPropertyChanged();
            }
        }

        private string _bundleName;
        public string BundleName
        {
            get => _bundleName;
            set
            {
                _bundleName = value;
                OnPropertyChanged();
            }
        }

        private string _bundleCost;
        public string BundleCost
        {
            get => _bundleCost;
            set
            {
                _bundleCost = value;
                OnPropertyChanged();
            }
        }

        private bool _isNightMarketAvailable;
        public bool IsNightMarketAvailable
        {
            get => _isNightMarketAvailable;
            set
            {
                _isNightMarketAvailable = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<RotatingStoreItem> _rotatingStoreItems = new();
        public ObservableCollection<RotatingStoreItem> RotatingStoreItems
        {
            get => _rotatingStoreItems;
            set
            {
                _rotatingStoreItems = value;
                OnPropertyChanged();
            }
        }

        public StoreViewModel(UserData userData, IStoreService storeService)
        {
            _userData = userData;
            _storeService = storeService;

            if (_userData.RiotUserData == null)
                ClearStoreData();
        }

        public void ClearStoreData()
        {
            RotatingStoreItems.Clear();
            BundleImageSource = null;
        }

        public async Task GetStoreData()
        {
            if (_userData.RiotUserData?.Sub == null || _userData.RiotUrl?.PdUrl == null)
            {
                MessageBox.Show("Log in to your account before trying to view the store.");
                return;
            }

            var playerStore = await _storeService.GetPlayerStore();

            await PopulateStoreView(playerStore);
        }

        private async Task PopulateStoreView(PlayerStore playerStore)
        {
            var rotatingStoreItems = playerStore.RotatingStore.SingleItemOffers;
            foreach (var item in rotatingStoreItems)
            {
                var skinControl = new RotatingStoreItem(_storeService, item);
                RotatingStoreItems.Add(skinControl);
            }

            var currentBundle = playerStore.FeaturedBundle.Bundle;
            var bundleId = currentBundle.DataAssetID;
            var bundleInfo = await _storeService.GetBundleInformation(bundleId);
            
            var bundlePrice = 0;
            foreach (var item in currentBundle.BundleItems)
            {
                bundlePrice += item.DiscountedPrice;
            }

            BundleImageSource = new BitmapImage(new Uri(bundleInfo.BundleDisplayIcon), new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable))
            {
                CacheOption = BitmapCacheOption.OnLoad
            };

            BundleName = bundleInfo.BundleDisplayName;
            BundleCost = $"{bundlePrice:n0}";

            IsNightMarketAvailable = playerStore.NightMarket?.NightMarketOffers?.Count > 0;
        }
    }
}
