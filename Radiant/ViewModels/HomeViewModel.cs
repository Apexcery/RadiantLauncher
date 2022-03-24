using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using MaterialDesignThemes.Wpf;

using Newtonsoft.Json;

using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Models.AppConfigs;
using Radiant.Models.Client;
using Radiant.Utils;
using Radiant.Views.Dialogues;

namespace Radiant.ViewModels
{
    public class HomeViewModel : Observable
    {
        private readonly IAuthService _authService;
        private UserData _userData;
        private readonly AppConfig _appConfig;
        
        public RelayCommand<object> PlayCommand { get; }
        public RelayCommand<object> AddAccountCommand { get; }

        private bool _logInFormVisible = true;
        public bool LogInFormVisible
        {
            get => _logInFormVisible;
            set
            {
                _logInFormVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoggedIn = false;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged();
            }
        }

        private bool _loadingVisible = false;
        public bool LoadingVisible
        {
            get => _loadingVisible;
            set
            {
                _loadingVisible = value;
                OnPropertyChanged();
            }
        }

        private string _username = "";
        public string UsernameText
        {
            get => _username;
            set
            {
                if (!string.Equals(_username, value))
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _gameName = "";
        public string GameNameText
        {
            get => _gameName;
            set
            {
                _gameName = value;
                OnPropertyChanged();
            }
        }
        
        private ObservableCollection<Account> _accounts = new();
        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasAccounts));
            }
        }

        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged();
            }
        }

        public bool HasAccounts => Accounts.Any();

        private bool _isAccountAddingAllowed;
        public bool IsAccountAddingAllowed
        {
            get => _isAccountAddingAllowed;
            set
            {
                _isAccountAddingAllowed = value;
                OnPropertyChanged();
            }
        }

        private MainViewModel _mainViewModel;

        public CancellationTokenSource CancellationTokenSource = new();

        public HomeViewModel(IAuthService authService, UserData userData, AppConfig appConfig)
        {
            _authService = authService;
            _userData = userData;
            _appConfig = appConfig;
            
            PlayCommand = new(async _ => await Play());

            AddAccountCommand = new(async _ => await AddAccount());
        }

        public async Task ChangeAccount(object o)
        {
            if (o == null || !IsLoggedIn)
                return;

            var account = (Account)((ComboBox)o).SelectedItem;
            
            if ((_userData.RiotUserData?.AccountInfo?.GameName.Equals(account.DisplayName, StringComparison.InvariantCulture) ?? false) &&
                (_userData.RiotUserData?.AccountInfo?.TagLine.Equals(account.Tag, StringComparison.InvariantCulture) ?? false))
            {
                return;
            }

            if (IsLoggedIn)
                Logout();
            
            var loginSuccess = await Login(account.Username, account.Password);
            if (loginSuccess)
            {
                SelectedAccount = account;
                OnPropertyChanged(nameof(Accounts));
                OnPropertyChanged(nameof(HasAccounts));
            }
            else
            {
                if (!IsLoggedIn && Accounts.Any())
                {
                    await Login(Accounts.First().Username, Accounts.First().Password);
                }
            }
        }

        public async Task RemoveAccount(object o)
        {
            if (o is ComboBoxItem obj)
            {
                var acc = obj.Content as Account;

                Accounts.Remove(acc);
                _appConfig.Accounts.Clear();
                foreach (var account in Accounts)
                {
                    _appConfig.Accounts.Add(account);
                }

                await _appConfig.SaveToFile();

                if (IsLoggedIn && (_userData.RiotUserData?.AccountInfo.GameName.Equals(acc?.DisplayName, StringComparison.InvariantCulture) ?? false) &&
                                  (_userData.RiotUserData?.AccountInfo.TagLine.Equals(acc?.Tag, StringComparison.InvariantCulture) ?? false))
                {
                    Logout();
                }

                if (Accounts.Any())
                {
                    var accToLogin = Accounts.First();
                    var loginSuccess = await Login(accToLogin.Username, accToLogin.Password);
                    if (loginSuccess)
                        SelectedAccount = accToLogin;
                    else
                    {
                        if (!IsLoggedIn && Accounts.Any())
                        {
                            await Login(Accounts.First().Username, Accounts.First().Password);
                        }
                    }
                }

                OnPropertyChanged(nameof(Accounts));
                OnPropertyChanged(nameof(HasAccounts));
            }
        }

        public async Task PopulateAccountList()
        {
            foreach (var acc in _appConfig.Accounts)
                if (!Accounts.Any(x =>
                        x.Username.Equals(acc.Username, StringComparison.InvariantCulture) &&
                        x.Password.Equals(acc.Password, StringComparison.InvariantCulture)))
                {
                    Accounts.Add(acc);
                }

            OnPropertyChanged(nameof(Accounts));
            OnPropertyChanged(nameof(HasAccounts));
            
            if (_appConfig.Accounts.Any())
            {
                if (_userData.RiotUserData != null)
                {
                    SelectedAccount = Accounts.FirstOrDefault(x =>
                        x.DisplayName.Equals(_userData.RiotUserData.AccountInfo.GameName, StringComparison.InvariantCulture) &&
                        x.Tag.Equals(_userData.RiotUserData.AccountInfo.TagLine, StringComparison.InvariantCulture));
                }
                else
                {
                    SelectedAccount = _appConfig.Accounts.First();
                }
            }

            IsAccountAddingAllowed = _appConfig.Accounts.Count < 5;

            if (SelectedAccount != null && !IsLoggedIn)
            {
                var loginSuccess = await Login(SelectedAccount.Username, SelectedAccount.Password);
                if (loginSuccess)
                {
                    OnPropertyChanged(nameof(Accounts));
                    OnPropertyChanged(nameof(HasAccounts));
                }
                else
                {
                    if (!IsLoggedIn && Accounts.Any())
                    {
                        await Login(Accounts.First().Username, Accounts.First().Password);
                    }
                }
            }
        }

        private async Task AddAccount()
        {
            var dialog = new AddAccountDialog(_appConfig, this, _userData);
            await DialogHost.Show(dialog, "MainDialogHost");
            await PopulateAccountList();
        }

        private async Task Play()
        {
            var riotClientExists = CheckForRiotClient(out var clientPath);
            if (!riotClientExists)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Riot Client not detected, is VALORANT installed?" });
                await DialogHost.Show(dialog, "MainDialogHost");
                return;
            }

            await AuthenticateRiotClient();

            var riotClient = new ProcessStartInfo(clientPath, " --launch-product=valorant --launch-patchline=live")
            {
                UseShellExecute = true
            };
            Process.Start(riotClient);

            Application.Current.Shutdown();
        }

        private async Task AuthenticateRiotClient()
        {
            var riotGamesSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Riot Games", "Riot Client", "Data", "RiotGamesPrivateSettings.yaml");
            var riotClientSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Riot Games", "Riot Client", "Data", "RiotClientPrivateSettings.yaml");
            var riotClientConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Riot Games", "Riot Client", "Config", "RiotClientSettings.yaml");

            var clientGameSettings = new ClientGameModel(_userData);
            var clientPrivateSettings = new ClientPrivateModel(_userData);
            var clientConfigSettings = new ClientConfigSettingsModel();

            await using (TextWriter writer = File.CreateText(riotGamesSettingsPath))
            {
                clientGameSettings.CreateFile(_userData).Save(writer, false);
            }

            await using (TextWriter writer = File.CreateText(riotClientSettingsPath))
            {
                clientPrivateSettings.CreateFile().Save(writer, false);
            }

            await using (TextWriter writer = File.CreateText(riotClientConfigPath))
            {
                clientConfigSettings.CreateSettings(_userData).Save(writer, false);
            }
        }

        private bool CheckForRiotClient(out string clientPath)
        {
            var defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Riot Games");
            var defaultFileName = "RiotClientInstalls.json";

            if (!File.Exists(Path.Combine(defaultPath, defaultFileName)))
            {
                clientPath = "";
                return false;
            }

            var config = JsonConvert.DeserializeObject<RiotClientInstalls>(File.ReadAllText(Path.Combine(defaultPath, defaultFileName)));

            clientPath = config.RcDefault;

            return true;
        }

        public void Logout()
        {
            _userData = _userData.Clear();
            GameNameText = "";
            UsernameText = "";
            IsLoggedIn = false;
            LogInFormVisible = true;

            _mainViewModel ??= Application.Current.MainWindow?.DataContext as MainViewModel;
            if (_mainViewModel != null)
                _mainViewModel.IsLoggedIn = false;
        }

        public async Task<bool> Login(string username, string password, bool isAddingAccount = false)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Invalid Username or Password." });
                await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                return false;
            }

            if (!isAddingAccount)
            {
                LoadingVisible = true;
                LogInFormVisible = false;
            }

            Logout();
            Account logInSuccessAccount = null;
            try
            {
                if (CancellationTokenSource.IsCancellationRequested)
                {
                    CancellationTokenSource = new();
                }
                logInSuccessAccount = await _authService.Login(CancellationTokenSource.Token, username, password, isAddingAccount);
            }
            catch (TaskCanceledException) { }

            if (logInSuccessAccount != null)
            {
                GameNameText = logInSuccessAccount.FullDisplayName;
                IsLoggedIn = true;

                _mainViewModel ??= Application.Current.MainWindow?.DataContext as MainViewModel;
                if (_mainViewModel != null)
                    _mainViewModel.IsLoggedIn = true;
            }
            else
            {
                LogInFormVisible = true;
            }

            if (!isAddingAccount)
            {
                LoadingVisible = false;
                LogInFormVisible = true;
            }

            return logInSuccessAccount != null;
        }
    }
}