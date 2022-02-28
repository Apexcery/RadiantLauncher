using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Models.Store;
using Radiant.Utils;
using Radiant.Views.Dialogues;
using Radiant.Views.UserControls;

namespace Radiant.ViewModels
{
    public class StoreViewModel : Observable
    {
        public readonly UserData UserData;
        private readonly AppConfig _appConfig;
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

        public RelayCommand<object> ShowNightMarketCommand { get; }

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

        private ObservableCollection<NightMarketItem> _nightMarketItems = new();
        public ObservableCollection<NightMarketItem> NightMarketItems
        {
            get => _nightMarketItems;
            set
            {
                _nightMarketItems = value;
                OnPropertyChanged();
            }
        }

        private PlayerStore _playerStore;
        private StoreOffers _storeOffers;

        private NightMarket _nightMarket;

        public StoreViewModel(UserData userData, AppConfig appConfig, IStoreService storeService)
        {
            UserData = userData;
            _appConfig = appConfig;
            _storeService = storeService;

            ShowNightMarketCommand = ShowNightMarket();
        }

        private RelayCommand<object> ShowNightMarket()
        {
            return new RelayCommand<object>(async _ =>
            {
                await DialogHost.Show(new NightMarketView(), "StoreDialogHost");
            });
        }

        public void ClearStoreData()
        {
            RotatingStoreItems.Clear();
            NightMarketItems.Clear();
            IsNightMarketAvailable = false;
            BundleImageSource = null;
            BundleName = null;
            BundleCost = null;
        }

        public async Task GetStoreData()
        {
            ClearStoreData();

            if (UserData.RiotUserData?.Puuid == null || UserData.RiotUrl?.PdUrl == null)
            {
                var dialog = new PopupDialog(_appConfig, "Error", "Log in to your account before trying to view the store.");
                await DialogHost.Show(dialog, "MainDialogHost");
                return;
            }

            _playerStore = await _storeService.GetPlayerStore();
            _storeOffers = await _storeService.GetStoreOffers();

            await PopulateStoreView();
        }

        private async Task PopulateStoreView()
        {
            var rotatingStoreItems = _playerStore.RotatingStore.SingleItemOffers;
            foreach (var item in rotatingStoreItems)
            {
                var skinControl = new RotatingStoreItem(_storeService, item);
                RotatingStoreItems.Add(skinControl);
            }

            var currentBundle = _playerStore.FeaturedBundle.Bundle;
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

            _nightMarket = _playerStore.NightMarket;

            IsNightMarketAvailable = _nightMarket?.NightMarketOffers?.Count > 0;

            if (!IsNightMarketAvailable)
                return;

            var offers = _storeOffers.Offers;

            foreach (var offer in _nightMarket.NightMarketOffers)
            {
                var rewards = offers.FirstOrDefault(x => x.OfferID == offer.Offer.OfferID)?.Rewards;
                if (rewards == null)
                    continue;

                foreach (var item in rewards)
                {
                    var tile = new NightMarketItem(_storeService, item.ItemID)
                    {
                        MaxWidth = 200
                    };
                    NightMarketItems.Add(tile);
                }
            }
        }
    }
}
