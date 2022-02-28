using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Radiant.Models;
using Radiant.Utils;

namespace Radiant.ViewModels
{
    public class SettingsViewModel : Observable
    {
        private readonly UserData _userData;
        private readonly AppConfig _appConfig;

        public RelayCommand<string> SystemButtonsCommand { get; }

        public RelayCommand<string> ColorThemeCommand { get; }

        public SettingsViewModel(UserData userData, AppConfig appConfig)
        {
            _userData = userData;
            _appConfig = appConfig;

            SystemButtonsCommand = new(async s => await ChangeSystemButtons(s));
            ColorThemeCommand = new(async s => await ChangeColorTheme(s));
        }

        private async Task ChangeColorTheme(string value)
        {
            if (Enum.TryParse(value, true, out ColorThemeType colorThemeType) && Enum.IsDefined(typeof(ColorThemeType), colorThemeType))
            {
                var mainWindow = Application.Current.MainWindow;
                var mainVm = (MainViewModel)mainWindow?.DataContext;
                if (mainVm != null)
                {
                    var dict = new ResourceDictionary();
                    switch (colorThemeType)
                    {
                        case ColorThemeType.Dark:
                            dict.Source = new("Resources/Values/Colors/DarkThemecolors.xaml", UriKind.Relative);
                            break;
                        case ColorThemeType.Light:
                            dict.Source = new("Resources/Values/Colors/LightThemecolors.xaml", UriKind.Relative);
                            break;
                    }
                    _appConfig.Settings.ColorThemeType = colorThemeType;
                    for (var i = Application.Current.Resources.MergedDictionaries.Count - 1; i > 0; i--)
                    {
                        var currentDict = Application.Current.Resources.MergedDictionaries[i];
                        if (currentDict.Source.ToString().Contains("DarkThemeColors.xaml") || currentDict.Source.ToString().Contains("LightThemeColors.xaml"))
                        {
                            Application.Current.Resources.MergedDictionaries.RemoveAt(i);
                        }
                    }
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                    await SaveConfig();
                }
            }
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
                    _appConfig.Settings.SystemButtonsType = systemButtonsType;
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
