using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using ValorantLauncher.Constants;
using ValorantLauncher.Models;
using ValorantLauncher.Utils;

namespace ValorantLauncher.ViewModels
{
    public class SettingsViewModel : Observable
    {
        private readonly UserData _userData;
        private readonly AppConfig _appConfig;

        public RelayCommand<string> SystemButtonsCommand { get; }

        public SettingsViewModel(UserData userData, AppConfig appConfig)
        {
            _userData = userData;
            _appConfig = appConfig;

            SystemButtonsCommand = new(async s => await ChangeSystemButtons(s));
        }

        private async Task ChangeSystemButtons(string value)
        {
            if (Enum.TryParse(value, true, out SystemButtonsType systemButtonsType) && Enum.IsDefined(typeof(SystemButtonsType), systemButtonsType))
            {
                var mainWindow = Application.Current.MainWindow;
                var mainVm = (MainViewModel)mainWindow?.DataContext;
                if (mainVm != null)
                {
                    switch (systemButtonsType)
                    {
                        case SystemButtonsType.Colored:
                            mainVm.SystemButtonsStyle = Application.Current.TryFindResource("ColoredSystemButton") as Style;
                            break;
                        case SystemButtonsType.Simple:
                            mainVm.SystemButtonsStyle = Application.Current.TryFindResource("SimpleSystemButton") as Style;
                            break;
                    }
                    _appConfig.SystemButtonsType = systemButtonsType;
                    await SaveConfig();
                }
            }
        }

        private async Task SaveConfig()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var applicationName = Application.Current.TryFindResource("ApplicationName") as string;
            var configFileName = Application.Current.TryFindResource("ConfigFileName") as string;

            if (!string.IsNullOrEmpty(applicationName) && !string.IsNullOrEmpty(configFileName))
            {
                var folderPath = Path.Combine(localAppData, applicationName);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                
                var filePath = Path.Combine(folderPath, configFileName);

                var appConfigAsText = JsonConvert.SerializeObject(_appConfig, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, appConfigAsText);
            }
        }
    }
}
