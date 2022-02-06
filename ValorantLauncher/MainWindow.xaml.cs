using System.Windows;
using ValorantLauncher.Interfaces;

namespace ValorantLauncher
{
    public partial class MainWindow : Window, IMinimizable, IMaximizable, ICloseable
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Minimize()
        {
            this.WindowState = WindowState.Minimized;
        }

        public void Maximize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.MaxHeight = 1080;
                this.MaxWidth = 1920;
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                this.WindowState = WindowState.Maximized;
            }
        }

        public new void Close()
        {
            Application.Current.Shutdown();
        }
    }
}
