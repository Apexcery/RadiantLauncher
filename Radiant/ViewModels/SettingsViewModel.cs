using System;
using System.Threading.Tasks;
using System.Windows;
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
            if (Enum.TryParse(value, true, out ColorTheme colorThemeType) && Enum.IsDefined(typeof(ColorTheme), colorThemeType))
            {
                var mainWindow = Application.Current.MainWindow;
                var mainVm = (MainViewModel)mainWindow?.DataContext;
                if (mainVm != null)
                {
                    var dict = new ResourceDictionary();
                    switch (colorThemeType)
                    {
                        case ColorTheme.Dark:
                            dict.Source = new("Resources/Values/Colors/DarkThemecolors.xaml", UriKind.Relative);
                            break;
                        case ColorTheme.Light:
                            dict.Source = new("Resources/Values/Colors/LightThemecolors.xaml", UriKind.Relative);
                            break;
                    }
                    _appConfig.Settings.ColorTheme = colorThemeType;
                    for (var i = Application.Current.Resources.MergedDictionaries.Count - 1; i > 0; i--)
                    {
                        var currentDict = Application.Current.Resources.MergedDictionaries[i];
                        if (currentDict.Source.ToString().Contains("DarkThemeColors.xaml") || currentDict.Source.ToString().Contains("LightThemeColors.xaml"))
                        {
                            Application.Current.Resources.MergedDictionaries.RemoveAt(i);
                        }
                    }
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                    await _appConfig.SaveToFile();
                }
            }
        }

        private async Task ChangeSystemButtons(string value)
        {
            if (Enum.TryParse(value, true, out SystemButtons systemButtonsType) && Enum.IsDefined(typeof(SystemButtons), systemButtonsType))
            {
                var mainWindow = Application.Current.MainWindow;
                var mainVm = (MainViewModel)mainWindow?.DataContext;
                if (mainVm != null)
                {
                    switch (systemButtonsType)
                    {
                        case SystemButtons.Colored:
                            mainVm.SystemButtonsStyle = Application.Current.TryFindResource("ColoredSystemButton") as Style;
                            break;
                        case SystemButtons.Simple:
                            mainVm.SystemButtonsStyle = Application.Current.TryFindResource("SimpleSystemButton") as Style;
                            break;
                    }
                    _appConfig.Settings.SystemButtons = systemButtonsType;
                    await _appConfig.SaveToFile();
                }
            }
        }
    }
}
