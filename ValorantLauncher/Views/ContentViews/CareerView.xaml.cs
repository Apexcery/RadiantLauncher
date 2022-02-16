using System;
using System.Windows.Controls;
using ValorantLauncher.ViewModels;

namespace ValorantLauncher.Views.ContentViews
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
            await ((CareerViewModel)DataContext).GetRankData();
        }
    }
}
