using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Radiant.Interfaces;
using Radiant.ViewModels;

namespace Radiant
{
    public partial class MainWindow : Window, IMinimizable, ICloseable
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            this.DataContext = mainViewModel;

            InitializeComponent();
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
                if (DialogHost.IsDialogOpen("StoreDialogHost"))
                {
                    DialogHost.Close("StoreDialogHost");
                }
            }
            catch
            {
                // ignored, if dialog host does not exist yet, an exception is thrown.
            }
        }
    }
}
