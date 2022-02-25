using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ValorantLauncher.Models
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
        [JsonConverter(typeof(StringEnumConverter))]
        public SystemButtonsType SystemButtonsType { get; set; } = SystemButtonsType.Colored;
    }

    public enum SystemButtonsType
    {
        Colored,
        Simple
    }
}
