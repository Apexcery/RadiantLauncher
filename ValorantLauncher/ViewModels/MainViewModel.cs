using ValorantLauncher.Interfaces;
using ValorantLauncher.Utils;

namespace ValorantLauncher.ViewModels
{
    public class MainViewModel : Observable
    {
        public RelayCommand<IMinimizable> MinimizeCommand { get; }
        public RelayCommand<IMaximizable> MaximizeCommand { get; }
        public RelayCommand<ICloseable> CloseCommand { get; }

        public MainViewModel()
        {
            this.MinimizeCommand = new RelayCommand<IMinimizable>(this.MinimizeApplication);
            this.MaximizeCommand = new RelayCommand<IMaximizable>(this.MaximizeApplication);
            this.CloseCommand = new RelayCommand<ICloseable>(this.CloseApplication);
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
