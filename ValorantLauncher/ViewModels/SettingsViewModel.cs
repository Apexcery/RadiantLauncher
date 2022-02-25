using ValorantLauncher.Models;
using ValorantLauncher.Utils;

namespace ValorantLauncher.ViewModels
{
    public class SettingsViewModel : Observable
    {
        private readonly UserData _userData;
        private readonly AppConfig _appConfig;

        public SettingsViewModel(UserData userData, AppConfig appConfig)
        {
            _userData = userData;
            _appConfig = appConfig;
        }
    }
}
