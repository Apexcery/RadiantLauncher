using ValorantLauncher.Interfaces;
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

        public MainViewModel(HomeViewModel homeViewModel, StoreViewModel storeViewModel, CareerViewModel careerViewModel, SettingsViewModel settingsViewModel)
        {
            this.MinimizeCommand = new(this.MinimizeApplication);
            this.CloseCommand = new(this.CloseApplication);

            CurrentView = homeViewModel;

            HomeViewCommand = ChangeView(homeViewModel);
            StoreViewCommand = ChangeView(storeViewModel);
            CareerViewCommand = ChangeView(careerViewModel);

            SettingsViewCommand = ChangeView(settingsViewModel);
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
