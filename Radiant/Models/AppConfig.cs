using Newtonsoft.Json;
using Radiant.Utils;

namespace Radiant.Models
{
    public class AppConfig
    {
        public bool LoginAutomatically { get; set; } = false;

        public LoginDetails LoginDetails { get; set; } = new();

        public Settings Settings { get; set; } = new();
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
