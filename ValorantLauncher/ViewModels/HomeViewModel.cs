using ValorantLauncher.Interfaces;
using ValorantLauncher.Models;
using ValorantLauncher.Utils;

namespace ValorantLauncher.ViewModels
{
    public class HomeViewModel : Observable
    {
        private readonly IAuthService _authService;
        private readonly UserData _userData;

        public RelayCommand<object> LoginCommand { get; }

        private bool _signedIn = false;
        public bool SignedIn
        {
            get => _signedIn;
            set
            {
                _signedIn = value;
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

        private string _password = "";
        public string PasswordText
        {
            get => _password;
            set
            {
                if (!string.Equals(_password, value))
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public HomeViewModel(IAuthService authService, UserData userData)
        {
            _authService = authService;
            _userData = userData;

            LoginCommand = new RelayCommand<object>(Login);
        }

        private void Login(object _)
        {
            _authService.Login(UsernameText, PasswordText);
        }
    }
}
