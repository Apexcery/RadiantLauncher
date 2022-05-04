using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Cache;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Radiant.Constants;
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

        private ImageSource _bundleCurrencyIcon;
        public ImageSource BundleCurrencyIcon
        {
            get => _bundleCurrencyIcon;
            set
            {
                _bundleCurrencyIcon = value;
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

        public CancellationTokenSource CancellationTokenSource = new();
        
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
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Log in to your account before trying to view the store."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return;
            }

            if (CancellationTokenSource.IsCancellationRequested)
            {
                CancellationTokenSource = new();
            }

            try
            {
                _playerStore = await _storeService.GetPlayerStore(CancellationTokenSource.Token);
                _storeOffers = await _storeService.GetStoreOffers(CancellationTokenSource.Token);
            }
            catch (TaskCanceledException) { }

            await PopulateStoreView();
        }

        private async Task PopulateStoreView()
        {
            var rotatingStoreItems = _playerStore.RotatingStore.SingleItemOffers;
            foreach (var item in rotatingStoreItems)
            {
                var offer = _storeOffers.Offers.FirstOrDefault(x => x.Rewards.Any(y => y.ItemID == item));
                var skinControl = new RotatingStoreItem(item, offer);
                RotatingStoreItems.Add(skinControl);
            }

            var currentBundle = _playerStore.FeaturedBundle.Bundle;
            var bundleCurrencyId = currentBundle.CurrencyID;
            var currency = ValorantConstants.CurrencyById[bundleCurrencyId];
            BundleCurrencyIcon = new BitmapImage(new Uri(currency.DisplayIcon), new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable))
            {
                CacheOption = BitmapCacheOption.OnDemand
            };
            var bundleId = currentBundle.DataAssetID;

            var bundleInfo = ValorantConstants.BundleById[bundleId];

            if (bundleInfo is null)
                return;

            var bundlePrice = 0;
            foreach (var item in currentBundle.BundleItems)
            {
                bundlePrice += item.DiscountedPrice;
            }

            BundleImageSource = new BitmapImage(new Uri(bundleInfo.DisplayIcon), new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable))
            {
                CacheOption = BitmapCacheOption.OnDemand
            };

            BundleName = bundleInfo.DisplayName;
            BundleCost = $"{bundlePrice:n0}";

            var nightMarket = _playerStore.NightMarket;

            IsNightMarketAvailable = nightMarket?.NightMarketOffers?.Count > 0;

            if (!IsNightMarketAvailable)
                return;

            var offers = _storeOffers.Offers;

            foreach (var offer in nightMarket?.NightMarketOffers ?? new List<NightMarket.NightMarketOffer>())
            {
                var rewards = offers.FirstOrDefault(x => x.OfferID == offer.Offer.OfferID)?.Rewards;
                if (rewards == null)
                    continue;

                foreach (var item in rewards)
                {
                    var tile = new NightMarketItem(item.ItemID, offer)
                    {
                        MaxWidth = 200
                    };
                    NightMarketItems.Add(tile);
                }
            }
        }
    }
}
