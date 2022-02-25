using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using ValorantLauncher.Constants;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models;
using ValorantLauncher.Models.Client;
using ValorantLauncher.Utils;

namespace ValorantLauncher.ViewModels
{
    public class HomeViewModel : Observable
    {
        private readonly IAuthService _authService;
        private UserData _userData;
        private readonly AppConfig _appConfig;

        public RelayCommand<object> LoginCommand { get; }
        public RelayCommand<object> LogoutCommand { get; }
        public RelayCommand<object> PlayCommand { get; }

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

        private bool _playFormVisible = false;
        public bool PlayFormVisible
        {
            get => _playFormVisible;
            set
            {
                _playFormVisible = value;
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

        private bool _loginAutomatically = false;
        public bool LoginAutomatically
        {
            get => _loginAutomatically;
            set
            {
                _loginAutomatically = value;
                OnPropertyChanged();
            }
        }

        private MainViewModel _mainViewModel;

        public HomeViewModel(IAuthService authService, UserData userData, AppConfig appConfig)
        {
            _authService = authService;
            _userData = userData;
            _appConfig = appConfig;

            LoginCommand = new RelayCommand<object>(async o => await LoginWithWindowData(o));
            LogoutCommand = new RelayCommand<object>(async o => await Logout(o));
            PlayCommand = new RelayCommand<object>(async _ => await Play());
        }

        private async Task Play()
        {
            var clientPath = "";
            var riotClientExists = CheckForRiotClient(out clientPath);
            if (!riotClientExists)
            {
                MessageBox.Show("Riot Client not detected, is VALORANT installed?");
                return;
            }

            await AuthenticateRiotClient();

            var riotClient = new ProcessStartInfo(clientPath, " --launch-product=valorant --launch-patchline=live");
            Process.Start(riotClient);

            Application.Current.Shutdown();
        }

        private async Task AuthenticateRiotClient()
        {
            var riotGamesSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Riot Games", "Riot Client", "Data", "RiotGamesPrivateSettings.yaml");
            var riotClientSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Riot Games", "Riot Client", "Data", "RiotClientPrivateSettings.yaml");

            var clientGameSettings = new ClientGameModel(_userData);
            var clientPrivateSettings = new ClientPrivateModel(_userData);

            await using (TextWriter writer = File.CreateText(riotGamesSettingsPath))
            {
                clientGameSettings.CreateFile().Save(writer, false);
            }

            await using (TextWriter writer = File.CreateText(riotClientSettingsPath))
            {
                clientPrivateSettings.CreateFile().Save(writer, false);
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

        private async Task Logout(object obj)
        {
            var passwordBox = (PasswordBox)obj;
            _userData = _userData.Clear();
            GameNameText = "";
            UsernameText = "";
            passwordBox.Clear();
            passwordBox.SecurePassword.Clear();
            PasswordBoxHelper.SetPassword(passwordBox, "");
            PlayFormVisible = false;
            LogInFormVisible = true;

            // Clear saved login details
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var applicationName = Application.Current.TryFindResource("ApplicationName") as string;
            var configFileName = Application.Current.TryFindResource("ConfigFileName") as string;

            if (!string.IsNullOrEmpty(applicationName) && !string.IsNullOrEmpty(configFileName))
            {
                var folderPath = Path.Combine(localAppData, applicationName);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                
                var filePath = Path.Combine(folderPath, configFileName);

                _appConfig.LoginAutomatically = LoginAutomatically;
                _appConfig.LoginDetails.Username = "";
                _appConfig.LoginDetails.Password = "";

                var appConfigAsText = JsonConvert.SerializeObject(_appConfig, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, appConfigAsText);
            }

            _mainViewModel ??= Application.Current.MainWindow?.DataContext as MainViewModel;
            if (_mainViewModel != null)
                _mainViewModel.IsLoggedIn = false;
        }

        public async Task LoginWithSavedData()
        {
            // Login automatically
            if (_appConfig.LoginAutomatically && _appConfig.LoginDetails.IsValid())
            {
                await Login(_appConfig.LoginDetails.Username, _appConfig.LoginDetails.Password);
            }
        }

        private async Task LoginWithWindowData(object obj)
        {
            var passwordBox = (PasswordBox)obj;
            var username = UsernameText;
            var password = passwordBox.Password;

            var loginSuccess = await Login(username, password);
            if (loginSuccess)
            {
                // Save login details
                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var applicationName = Application.Current.TryFindResource("ApplicationName") as string;
                var configFileName = Application.Current.TryFindResource("ConfigFileName") as string;

                if (!string.IsNullOrEmpty(applicationName) && !string.IsNullOrEmpty(configFileName))
                {
                    var folderPath = Path.Combine(localAppData, applicationName);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    
                    var filePath = Path.Combine(folderPath, configFileName);

                    _appConfig.LoginAutomatically = LoginAutomatically;
                    _appConfig.LoginDetails.Username = LoginAutomatically ? username : "";
                    _appConfig.LoginDetails.Password = LoginAutomatically ? password : "";

                    var appConfigAsText = JsonConvert.SerializeObject(_appConfig, Formatting.Indented);
                    await File.WriteAllTextAsync(filePath, appConfigAsText);
                }
            }
        }

        private async Task<bool> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Invalid Username or Password.");
                return false;
            }
            
            LoadingVisible = true;
            LogInFormVisible = false;

            var logInSuccess = await _authService.Login(username, password);
            if (logInSuccess)
            {
                // Change partial view
                GameNameText = _userData.RiotUserData.AccountInfo.GameName;
                PlayFormVisible = true;

                _mainViewModel ??= Application.Current.MainWindow?.DataContext as MainViewModel;
                if (_mainViewModel != null)
                    _mainViewModel.IsLoggedIn = true;
            }
            else
            {
                LogInFormVisible = true;
            }

            LoadingVisible = false;

            return logInSuccess;
        }
    }
}
