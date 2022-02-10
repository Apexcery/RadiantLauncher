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

        public MainViewModel(HomeViewModel homeViewModel, StoreViewModel storeViewModel)
        {
            this.MinimizeCommand = new RelayCommand<IMinimizable>(this.MinimizeApplication);
            this.CloseCommand = new RelayCommand<ICloseable>(this.CloseApplication);

            CurrentView = homeViewModel;

            HomeViewCommand = ChangeView(homeViewModel);
            StoreViewCommand = ChangeView(storeViewModel);
        }

        private RelayCommand<object> ChangeView(object vm)
        {
            return new RelayCommand<object>(_ => CurrentView = vm);
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
