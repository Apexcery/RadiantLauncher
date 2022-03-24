using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Models.AppConfigs;
using Radiant.Services;
using Radiant.ViewModels;
using Radiant.Views.ContentViews;

namespace Radiant
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
                    var hasOldConfig = true;
                    try
                    {
                        JsonConvert.DeserializeObject<AppConfigOld20220322>(File.ReadAllText(filePath),
                            new JsonSerializerSettings
                            {
                                MissingMemberHandling = MissingMemberHandling.Error,

                            });
                    }
                    catch (JsonSerializationException)
                    {
                        hasOldConfig = false;
                    }

                    if (hasOldConfig)
                    {
                        File.Delete(filePath);
                        File.WriteAllText(filePath, JsonConvert.SerializeObject(appConfig, Formatting.Indented));
                    }

                    var existingConfig = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(filePath));
                    if (existingConfig != null)
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
