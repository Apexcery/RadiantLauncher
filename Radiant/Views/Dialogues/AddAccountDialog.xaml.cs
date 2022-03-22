using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Utils;
using Radiant.ViewModels;

namespace Radiant.Views.Dialogues
{
    public partial class AddAccountDialog : ObservableUserControl
    {
        private readonly AppConfig _appConfig;
        private readonly HomeViewModel _homeViewModel;
        private readonly UserData _userData;

        public RelayCommand<ICloseable> CloseCommand { get; }
        public RelayCommand<object> AddAccountCommand { get; }

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

        private string _usernameText;
        public string UsernameText
        {
            get => _usernameText;
            set
            {
                _usernameText = value;
                OnPropertyChanged();
            }
        }

        public AddAccountDialog(AppConfig appConfig, HomeViewModel homeViewModel, UserData userData)
        {
            this.DataContext = this;

            _appConfig = appConfig;
            _homeViewModel = homeViewModel;
            _userData = userData;

            CloseCommand = new(_ => CloseDialog());

            AddAccountCommand = new(async o => await AddAccount(o));

            InitializeComponent();

            switch (_appConfig.Settings.SystemButtonsType)
            {
                case SystemButtonsType.Colored:
                    SystemButtonsStyle = Application.Current.TryFindResource("ColoredSystemButton") as Style;
                    break;
                case SystemButtonsType.Simple:
                    SystemButtonsStyle = Application.Current.TryFindResource("SimpleSystemButton") as Style;
                    break;
            }
        }

        private async Task AddAccount(object obj)
        {
            var passwordBox = (PasswordBox)obj;
            var username = UsernameText;
            var password = passwordBox.Password;

            if (_appConfig.Accounts.Any(x =>
                    x.Username.Equals(username, StringComparison.InvariantCulture) &&
                    x.Password.Equals(password, StringComparison.InvariantCulture)))
            {
                var dialog = new PopupDialog(_appConfig, "Account already exists.", new[] { "This account has already been added." });
                await DialogHost.Show(dialog, "AddAccountDialogHost");
                return;
            }
            
            var loginSuccess = await _homeViewModel.Login(username, password, true);

            if (loginSuccess)
            {
                _appConfig.Accounts.Add(new Account
                {
                    Username = username,
                    Password = password,
                    DisplayName = _userData.RiotUserData.AccountInfo.GameName,
                    Tag = _userData.RiotUserData.AccountInfo.TagLine
                });

                await _appConfig.SaveToFile();

                CloseDialog();
            }
        }

        public void CloseDialog()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow?.CloseDialogs();
        }
    }
}
