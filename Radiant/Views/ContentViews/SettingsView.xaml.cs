using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Radiant.Views.ContentViews
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void RequestNavigateHandler(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = e.Uri.ToString(),
                UseShellExecute = true
            });
        }

        private void CreditsIconButton_OnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var tag = button.Tag.ToString();

            var psi = new ProcessStartInfo
            {
                UseShellExecute = true
            };

            switch (tag.ToLower())
            {
                case "twitter":
                    psi.FileName = "Https://twitter.com/Apexcery";
                    break;
                case "github":
                    psi.FileName = "Https://github.com/Apexcery";
                    break;
            }

            Process.Start(psi);
        }
    }
}