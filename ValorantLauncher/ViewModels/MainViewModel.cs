using System;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models;
using ValorantLauncher.Utils;

namespace ValorantLauncher.ViewModels
{
    public class MainViewModel : Observable
    {
        public RelayCommand<IMinimizable> MinimizeCommand { get; }
        public RelayCommand<ICloseable> CloseCommand { get; }

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        
        public RelayCommand<object> HomeViewCommand { get; }
        public RelayCommand<object> StoreViewCommand { get; }
        public RelayCommand<object> CareerViewCommand { get; }

        public RelayCommand<object> SettingsViewCommand { get; }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged();
            }
        }

        private Style _systemButtonsStyle;
        public Style SystemButtonsStyle
        {
            get => _systemButtonsStyle;
            set
            {
                _systemButtonsStyle = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(AppConfig appConfig, HomeViewModel homeViewModel, StoreViewModel storeViewModel, CareerViewModel careerViewModel, SettingsViewModel settingsViewModel)
        {
            this.MinimizeCommand = new(this.MinimizeApplication);
            this.CloseCommand = new(this.CloseApplication);

            CurrentView = homeViewModel;

            HomeViewCommand = ChangeView(homeViewModel);
            StoreViewCommand = ChangeView(storeViewModel);
            CareerViewCommand = ChangeView(careerViewModel);

            SettingsViewCommand = ChangeView(settingsViewModel);

            switch (appConfig.Settings.SystemButtonsType)
            {
                case SystemButtonsType.Colored:
                    SystemButtonsStyle = Application.Current.TryFindResource("ColoredSystemButton") as Style;
                    break;
                case SystemButtonsType.Simple:
                    SystemButtonsStyle = Application.Current.TryFindResource("SimpleSystemButton") as Style;
                    break;
            }

            var dict = new ResourceDictionary();
            switch (appConfig.Settings.ColorThemeType)
            {
                case ColorThemeType.Dark:
                    dict.Source = new("Resources/Values/Colors/DarkThemecolors.xaml", UriKind.Relative);
                    break;
                case ColorThemeType.Light:
                    dict.Source = new("Resources/Values/Colors/LightThemecolors.xaml", UriKind.Relative);
                    break;
            }
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private RelayCommand<object> ChangeView(object vm)
        {
            return new(_ => CurrentView = vm);
        }

        private void MinimizeApplication(IMinimizable window)
        {
            if (window != null)
                window.Minimize();
        }

        private void CloseApplication(ICloseable window)
        {
            if (window != null)
                window.Close();
        }
    }
}
