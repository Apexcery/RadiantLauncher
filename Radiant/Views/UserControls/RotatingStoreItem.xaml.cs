using System;
using System.Linq;
using System.Net.Cache;
using System.Windows;
using System.Windows.Media.Imaging;
using Radiant.Interfaces;
using Radiant.Utils;

namespace Radiant.Views.UserControls
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

            var uri = skinInfo.DisplayIcon;
            if (skinInfo.Levels.Any(x => !string.IsNullOrEmpty(x.DisplayIcon)))
                uri = skinInfo.Levels.Last(x => !string.IsNullOrEmpty(x.DisplayIcon)).DisplayIcon;
            if (string.IsNullOrEmpty(uri) && skinInfo.Chromas.Any(x => !string.IsNullOrEmpty(x.DisplayIcon)))
                uri = skinInfo.Chromas.Last(x => !string.IsNullOrEmpty(x.DisplayIcon)).DisplayIcon;
            if (string.IsNullOrEmpty(uri))
                uri = skinInfo.DisplayIcon;

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