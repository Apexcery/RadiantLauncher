using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Radiant.Utils;

namespace Radiant.Models
{
    public class AppConfig
    {
        public bool LoginAutomatically { get; set; } = false;

        public LoginDetails LoginDetails { get; set; } = new();

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

    public class LoginDetails
    {
        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                return true;

            return false;
        }
    }

    public class Settings
    {
        [JsonConverter(typeof(TolerantEnumConverter))]
        public SystemButtonsType SystemButtonsType { get; set; } = SystemButtonsType.Colored;
        
        [JsonConverter(typeof(TolerantEnumConverter))]
        public ColorThemeType ColorThemeType { get; set; } = ColorThemeType.Dark;
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

    public enum SystemButtonsType
    {
        Colored,
        Simple
    }

    public enum ColorThemeType
    {
        Dark,
        Light
    }
}
