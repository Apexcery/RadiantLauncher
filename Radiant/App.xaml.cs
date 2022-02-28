using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models;
using ValorantLauncher.Services;
using ValorantLauncher.ViewModels;
using ValorantLauncher.Views.ContentViews;

namespace ValorantLauncher
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            var appConfig = new AppConfig();
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var applicationName = Application.Current.TryFindResource("ApplicationName") as string;
            var configFileName = Application.Current.TryFindResource("ConfigFileName") as string;

            if (!string.IsNullOrEmpty(applicationName) && !string.IsNullOrEmpty(configFileName))
            {
                var folderPath = Path.Combine(localAppData, applicationName);
                var filePath = Path.Combine(localAppData, applicationName, configFileName);

                if (Directory.Exists(folderPath) && File.Exists(filePath))
                {
                    var existingConfig = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(filePath));
                    appConfig = existingConfig;
                }
            }

            for (var i = Application.Current.Resources.MergedDictionaries.Count - 1; i > 0 ; i--)
            {
                var dict = Application.Current.Resources.MergedDictionaries[i];
                if (dict.Source.ToString().Contains("DarkThemeColors.xaml"))
                {
                    Application.Current.Resources.MergedDictionaries.RemoveAt(i);
                }
            }

            services.AddSingleton(appConfig);

            services.AddSingleton<UserData>();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<HomeView>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<StoreView>();
            services.AddSingleton<StoreViewModel>();
            services.AddSingleton<CareerView>();
            services.AddSingleton<CareerViewModel>();

            services.AddSingleton<SettingsView>();
            services.AddSingleton<SettingsViewModel>();

            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IStoreService, StoreService>();
            services.AddSingleton<ICareerService, CareerService>();

            var serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
