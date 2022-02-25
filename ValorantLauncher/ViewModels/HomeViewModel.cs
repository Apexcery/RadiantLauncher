using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
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

        public HomeViewModel(IAuthService authService, UserData userData)
        {
            _authService = authService;
            _userData = userData;

            LoginCommand = new RelayCommand<object>(async o => await Login(o));
            LogoutCommand = new RelayCommand<object>(Logout);
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

        private void Logout(object obj)
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
        }

        private async Task Login(object obj)
        {
            var passwordBox = (PasswordBox)obj;
            LoadingVisible = true;
            LogInFormVisible = false;
            var logInSuccess = await _authService.Login(UsernameText, passwordBox.Password);
            if (logInSuccess)
            {
                GameNameText = _userData.RiotUserData.AccountInfo.GameName;
                PlayFormVisible = true;
            }
            else
            {
                LogInFormVisible = true;
            }
            LoadingVisible = false;
        }
    }
}
