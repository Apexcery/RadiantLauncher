using System;
using System.Windows.Controls;
using ValorantLauncher.ViewModels;

namespace ValorantLauncher.Views.ContentViews
{
    public partial class StoreView : UserControl
    {
        public StoreView()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, EventArgs e)
        {
            var vm = (StoreViewModel)DataContext;
            if (vm.UserData.RiotUserData == null)
            {
                vm.ClearStoreData();
                return;
            }

            await vm.GetStoreData();
        }
    }
}
