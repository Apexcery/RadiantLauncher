using System.Threading.Tasks;
using System.Windows.Controls;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models;
using ValorantLauncher.Utils;
using ValorantLauncher.Views.ContentViews;

namespace ValorantLauncher.ViewModels
{
    public class HomeViewModel : Observable
    {
        private readonly IAuthService _authService;
        private UserData _userData;
        private readonly HomeView _view;

        public RelayCommand<object> LoginCommand { get; }
        public RelayCommand<object> LogoutCommand { get; }

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

        public HomeViewModel(IAuthService authService, UserData userData, HomeView view)
        {
            _authService = authService;
            _userData = userData;
            _view = view;

            LoginCommand = new RelayCommand<object>(async o => await Login(o));
            LogoutCommand = new RelayCommand<object>(async o => await Logout(o));
        }

        private async Task Logout(object obj)
        {
            var passwordBox = (PasswordBox)obj;
            //TODO: Clear UserData object.
            _userData = _userData.Clear();
            GameNameText = "";
            PlayFormVisible = false;
            UsernameText = "";
            passwordBox.Clear();
            passwordBox.SecurePassword.Clear();
            PasswordBoxHelper.SetPassword(passwordBox, "");
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
