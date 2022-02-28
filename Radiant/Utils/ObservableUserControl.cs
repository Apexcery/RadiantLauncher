using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace ValorantLauncher.Utils
{
    public class ObservableUserControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
