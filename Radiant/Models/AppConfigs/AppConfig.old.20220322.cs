using Newtonsoft.Json;

namespace Radiant.Models.AppConfigs
{
    public class AppConfigOld20220322
    {
        [JsonProperty("LoginAutomatically")]
        public bool LoginAutomaticallyOld20220322 { get; set; }
        [JsonProperty("LoginDetails")]
        public LoginDetailsOld20220322 LoginDetailsOld { get; set; }
        [JsonProperty("Settings")]
        public SettingsOld20220322 SettingsOld { get; set; }

        public class LoginDetailsOld20220322
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class SettingsOld20220322
        {
            [JsonProperty("SystemButtonsType")]
            public SystemButtonsTypeOld20220322 SystemButtonsTypeOld { get; set; }

            [JsonProperty("ColorThemeType")]
            public ColorThemeTypeOld20220322 ColorThemeTypeOld { get; set; }
        }

        public enum SystemButtonsTypeOld20220322
        {
            Colored,
            Simple
        }

        public enum ColorThemeTypeOld20220322
        {
            Dark,
            Light
        }
    }
}
