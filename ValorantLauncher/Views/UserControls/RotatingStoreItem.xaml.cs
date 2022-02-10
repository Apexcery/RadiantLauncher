using System;
using System.Net.Cache;
using System.Windows;
using System.Windows.Media.Imaging;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Utils;

namespace ValorantLauncher.Views.UserControls
{
    public partial class RotatingStoreItem : ObservableUserControl
    {
        private readonly IStoreService _storeService;
        private readonly string _itemId;

        public RotatingStoreItem(IStoreService storeService, string itemId)
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

            ItemImage.Source = new BitmapImage(new Uri(skinInfo.DisplayIcon), new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable))
            {
                CacheOption = BitmapCacheOption.OnLoad
            };
            TxtItemName.Text = skinInfo.DisplayName;
            TxtItemCost.Text = $"{skinPrice:n0}";
        }
    }
}
