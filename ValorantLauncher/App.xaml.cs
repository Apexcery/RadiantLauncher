using System;
using System.Net;
using System.Net.Http;
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
            
            services.AddSingleton<UserData>();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeView>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<StoreView>();
            services.AddSingleton<StoreViewModel>();
            services.AddSingleton<CareerView>();
            services.AddSingleton<CareerViewModel>();

            services.AddHttpClient("ValClient").ConfigureHttpClient(client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent", "RiotClient/43.0.1.4195386.4190634 rso-auth (Windows;10;;Professional, x64)");
                client.Timeout = TimeSpan.FromSeconds(30);
            }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true
            });

            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IStoreService, StoreService>();
            services.AddSingleton<ICareerService, CareerService>();

            var serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
