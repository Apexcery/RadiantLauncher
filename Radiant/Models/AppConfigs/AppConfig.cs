using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Radiant.Utils;

namespace Radiant.Models.AppConfigs
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

            if (!string.IsNullOrEmpty(applicationName) && !string.IsNullOrEmpty(configFileName))
            {
                var folderPath = Path.Combine(localAppData, applicationName);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, configFileName);

                var appConfigAsText = JsonConvert.SerializeObject(this, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, appConfigAsText);
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
    
    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Tag { get; set; }

        [JsonIgnore]
        public string FullDisplayName => DisplayName + "#" + Tag;
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
