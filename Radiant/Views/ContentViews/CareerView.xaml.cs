using System;
using System.Windows.Controls;
using Radiant.ViewModels;

namespace Radiant.Views.ContentViews
{
    public partial class CareerView : UserControl
    {
        public CareerView()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, EventArgs e)
        {
            var vm = (CareerViewModel)DataContext;
            if (vm.UserData.RiotUserData == null)
            {
                vm.ClearCareerData();
                return;
            }
            await vm.GetRankData();
        }
    }
}
