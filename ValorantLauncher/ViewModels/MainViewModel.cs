using ValorantLauncher.Interfaces;
using ValorantLauncher.Utils;

namespace ValorantLauncher.ViewModels
{
    public class MainViewModel : Observable
    {
        public RelayCommand<IMinimizable> MinimizeCommand { get; }
        public RelayCommand<IMaximizable> MaximizeCommand { get; }
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

        private HomeViewModel HomeVM { get; } = new();
        public RelayCommand<object> HomeViewCommand { get; }

        private StoreViewModel StoreVM { get; } = new();
        public RelayCommand<object> StoreViewCommand { get; }

        public MainViewModel()
        {
            this.MinimizeCommand = new RelayCommand<IMinimizable>(this.MinimizeApplication);
            this.MaximizeCommand = new RelayCommand<IMaximizable>(this.MaximizeApplication);
            this.CloseCommand = new RelayCommand<ICloseable>(this.CloseApplication);

            CurrentView = HomeVM;

            HomeViewCommand = ChangeView(HomeVM);
            StoreViewCommand = ChangeView(StoreVM);
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

        private void MaximizeApplication(IMaximizable window)
        {
            if (window != null)
                window.Maximize();
        }

        private void CloseApplication(ICloseable window)
        {
            if (window != null)
                window.Close();
        }
    }
}
