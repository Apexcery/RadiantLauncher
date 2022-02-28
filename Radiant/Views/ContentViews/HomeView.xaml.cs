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

        private void LoginAutomatically_OnChecked(object sender, RoutedEventArgs e)
        {
            var loginFormVisible = LoginForm.IsVisible;

            if (((sender as CheckBox)?.IsChecked ?? false) && loginFormVisible)
            {
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow == null)
                    return;

                var messageBoxResult = MessageBox.Show(mainWindow,
                    "Checking this box to log in automatically will save your username and password to this PC.\nDo not do this if this is a shared computer.",
                    "Are you sure?", MessageBoxButton.OKCancel);

                if (messageBoxResult != MessageBoxResult.OK)
                {
                    (sender as CheckBox).IsChecked = false;
                }
            }
        }
    }
}
