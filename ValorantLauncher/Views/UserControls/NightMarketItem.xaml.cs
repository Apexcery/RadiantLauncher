using System;
using System.Linq;
using System.Net.Cache;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ValorantLauncher.Interfaces;

namespace ValorantLauncher.Views.UserControls
{
    public partial class NightMarketItem : UserControl
    {
        private readonly IStoreService _storeService;
        private readonly string _itemId;

        public NightMarketItem(IStoreService storeService, string itemId)
        {
            _storeService = storeService;
            _itemId = itemId;

            InitializeComponent();

            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            TxtItemName.Width = Grid.ActualWidth / 2;
            TxtItemName.MaxWidth = Grid.ActualWidth / 2;

            var skinInfo = await _storeService.GetSkinInformation(_itemId);
            var skinPrice = await _storeService.GetSkinPrice(_itemId);

            var uri = skinInfo.DisplayIcon;
            if (skinInfo.Chromas.Any())
            {
                uri = skinInfo.Chromas.Last().DisplayIcon;
            }

            if (!string.IsNullOrEmpty(uri))
            {
                ItemImage.Source = new BitmapImage(new Uri(uri), new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable))
                {
                    CacheOption = BitmapCacheOption.OnLoad
                };
            }
            TxtItemName.Text = skinInfo.DisplayName;
            TxtItemCost.Text = $"{skinPrice:n0}";
        }
    }
}
