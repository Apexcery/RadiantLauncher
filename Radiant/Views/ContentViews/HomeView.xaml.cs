using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Radiant.Utils;
using Radiant.ViewModels;

namespace Radiant.Views.ContentViews
{
    public partial class HomeView : UserControl
    {
        public RelayCommand<object> RemoveAccountCommand { get; }

        public HomeView()
        {
            InitializeComponent();

            RemoveAccountCommand = new(async o => await RemoveAccount(o));

            this.Loaded += OnLoaded;
        }

        private async Task RemoveAccount(object obj)
        {
            var context = this.DataContext;
            if (context != null)
            {
                var ctx = (HomeViewModel)context;
                await ctx.RemoveAccount(obj);
            }
        }

        private async void OnLoaded(object sender, EventArgs e)
        {
            var vm = (HomeViewModel)DataContext;
            await vm.PopulateAccountList();
        }

        private async void AccountsComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isValid = this.IsLoaded && this.DataContext != null;
            if (isValid)
            {
                var vm = (HomeViewModel)DataContext;
                await vm.ChangeAccount(sender);
            }
            e.Handled = true;
        }
    }
}
