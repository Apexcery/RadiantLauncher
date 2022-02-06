using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeView>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<StoreView>();
            services.AddSingleton<StoreViewModel>();

            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<UserData>();

            var serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
