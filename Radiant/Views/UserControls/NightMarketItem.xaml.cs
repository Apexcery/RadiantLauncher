using System;
using System.Linq;
using System.Net.Cache;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Radiant.Constants;
using Radiant.Models.Store;
using Radiant.Utils;

namespace Radiant.Views.UserControls
{
    public partial class NightMarketItem : ObservableUserControl
    {
        private readonly string _itemId;
        private readonly NightMarket.NightMarketOffer _offer;

        private ImageSource _itemCurrencyIcon;
        public ImageSource ItemCurrencyIcon
        {
            get => _itemCurrencyIcon;
            set
            {
                _itemCurrencyIcon = value;
                OnPropertyChanged();
            }
        }

        public NightMarketItem(string itemId, NightMarket.NightMarketOffer offer)
        {
            _itemId = itemId;
            _offer = offer;

            InitializeComponent();

            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            TxtItemName.Width = Grid.ActualWidth / 2;
            TxtItemName.MaxWidth = Grid.ActualWidth / 2;

            var skinInfo = ValorantConstants.Skins.FirstOrDefault(x => x.Levels.Any(y => y.Id == _itemId));
            var skinPrice = _offer.DiscountCosts.ValorantPointCost;

            if (skinInfo is null || skinPrice == -1)
                return;

            var uri = skinInfo.DisplayIcon;
            if (string.IsNullOrEmpty(uri))
                uri = skinInfo.Levels.First().DisplayIcon;

            if (!string.IsNullOrEmpty(uri))
            {
                ItemImage.Source = new BitmapImage(new Uri(uri), new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable))
                {
                    CacheOption = BitmapCacheOption.OnDemand
                };
            }
            TxtItemName.Text = skinInfo.DisplayName;
            TxtItemCost.Text = $"{skinPrice:n0}";
            ItemCurrencyIcon = new BitmapImage(new Uri(ValorantConstants.CurrencyByName["VP"].DisplayIcon), new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable))
            {
                CacheOption = BitmapCacheOption.OnDemand
            };
        }
    }
}
