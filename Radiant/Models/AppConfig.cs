using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Radiant.Utils;

namespace Radiant.Models
{
    public class AppConfig
    {
        public Settings Settings { get; set; } = new();

        public List<Account> Accounts { get; set; } = new();

        public async Task SaveToFile()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var applicationName = Application.Current.TryFindResource("ApplicationName") as string;
            var configFileName = Application.Current.TryFindResource("ConfigFileName") as string;

            if (!string.IsNullOrEmpty(localAppData) && !string.IsNullOrEmpty(applicationName) && !string.IsNullOrEmpty(configFileName))
            {
                var folderPath = Path.Combine(localAppData, applicationName);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, configFileName);

                var appConfigAsText = JsonConvert.SerializeObject(this, Formatting.Indented);

                await File.WriteAllTextAsync(filePath, "// Any changes made to this file are your own responsibility and could lead to the app failing to run correctly.\n");
                await File.AppendAllTextAsync(filePath, appConfigAsText);
            }
        }
    }
    public class Settings
    {
        [JsonConverter(typeof(TolerantEnumConverter))]
        public SystemButtons SystemButtons { get; set; } = SystemButtons.Colored;
        
        [JsonConverter(typeof(TolerantEnumConverter))]
        public ColorTheme ColorTheme { get; set; } = ColorTheme.Dark;
    }
    
    public class Account : INotifyPropertyChanged
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Tag { get; set; }

        [JsonIgnore]
        public string FullDisplayName => DisplayName + "#" + Tag;

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum SystemButtons
    {
        Colored,
        Simple
    }

    public enum ColorTheme
    {
        Dark,
        Light
    }
}
