using System;
using System.Linq;
using System.Net.Cache;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Radiant.Interfaces;
using Radiant.Models.Store;
using Radiant.Utils;

namespace Radiant.Views.UserControls
{
    public partial class RotatingStoreItem : ObservableUserControl
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IStoreService _storeService;
        private readonly string _itemId;

        public RotatingStoreItem(CancellationTokenSource cancellationTokenSource, IStoreService storeService, string itemId)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _storeService = storeService;
            _itemId = itemId;

            InitializeComponent();
            
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            TxtItemName.Width = Grid.ActualWidth / 2;
            TxtItemName.MaxWidth = Grid.ActualWidth / 2;

            SkinInformation skinInfo = null;
            var skinPrice = -1;
            try
            {
                skinInfo = await _storeService.GetSkinInformation(_cancellationTokenSource.Token, _itemId);
                skinPrice = await _storeService.GetSkinPrice(_cancellationTokenSource.Token, _itemId);
            }
            catch (TaskCanceledException taskCanceledException) { }

            if (skinInfo is null || skinPrice == -1)
                return;

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