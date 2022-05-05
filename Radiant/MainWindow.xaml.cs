using System;
using System.Windows;
using System.Windows.Input;
using AutoUpdaterDotNET;
using MaterialDesignThemes.Wpf;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.ViewModels;

namespace Radiant
{
    public partial class MainWindow : Window, IMinimizable, ICloseable
    {
        private readonly AppConfig _appConfig;

        public MainWindow(MainViewModel mainViewModel, AppConfig appConfig)
        {
            _appConfig = appConfig;
            this.DataContext = mainViewModel;

            AutoUpdater.Start("https://raw.githubusercontent.com/Apexcery/RadiantLauncher/master/Radiant/Updates.xml");

            InitializeComponent();
            
            this.SourceInitialized += OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            InitLocation();
        }

        private void OnLocationChanged(object? sender, EventArgs e)
        {
            if (((Window)sender)?.ActualHeight == 0 && ((Window)sender)?.ActualWidth == 0)
                return;
            UpdateLocation();
        }

        private void InitLocation()
        {
            if (_appConfig.Location is null)
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _appConfig.Location = new Location
                {
                    X = this.Left,
                    Y = this.Top
                };
                _appConfig.SaveToFile();
            }
            else
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = _appConfig.Location.X;
                this.Top = _appConfig.Location.Y;
            }

            this.LocationChanged += OnLocationChanged;
        }

        private void UpdateLocation()
        {
            _appConfig.Location = new Location
            {
                X = this.Left,
                Y = this.Top
            };
            _appConfig.SaveToFile();
        }

        public void Minimize()
        {
            this.WindowState = WindowState.Minimized;
        }

        public new void Close()
        {
            Application.Current.Shutdown();
        }

        private void SystemBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseDialogs();
            DragMove();
        }

        public void CloseDialogs()
        {
            try
            {
                if (DialogHost.IsDialogOpen("MainDialogHost"))
                {
                    DialogHost.Close("MainDialogHost");
                }
            }
            catch
            {
                // ignored, if dialog host does not exist yet, an exception is thrown.
            }
        }
    }
}
