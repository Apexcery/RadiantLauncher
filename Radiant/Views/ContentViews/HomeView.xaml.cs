using System;
using System.Windows;
using System.Windows.Controls;
using Radiant.ViewModels;

namespace Radiant.Views.ContentViews
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();

            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
                mainWindow.ContentRendered += OnLoaded;
        }

        private async void OnLoaded(object sender, EventArgs e)
        {
            await ((HomeViewModel)DataContext).LoginWithSavedData();
        }
    }
}
