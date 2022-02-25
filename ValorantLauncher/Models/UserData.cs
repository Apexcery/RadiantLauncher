using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using ValorantLauncher.Models.Enums;

namespace ValorantLauncher.Models
{
    public class UserData
    {
        private readonly IHttpClientFactory _httpClientFactory;
        
        public HttpClientHandler ClientHandler;


        public TokenDataObject TokenData { get; set; }
        public RiotUserDataObject RiotUserData { get; set; }
        public RiotRegionEnum RiotRegion { get; set; }
        public RiotUrlObject RiotUrl { get; set; }

        public UserData(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            ClientHandler = new()
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };
        }

        public HttpClient Client { get; set; }

        public class RiotUrlObject
        {
            [JsonProperty("glzURL")]
            public string GlzUrl { get; set; }

            [JsonProperty("pdURL")]
            public string PdUrl { get; set; }
        }

        public class TokenDataObject
        {
            public string AccessToken { get; set; }
            public string IdToken { get; set; }
            public string EntitlementToken { get; set; }
        }
        public class RiotUserDataObject
        {
            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("sub")]
            public string Puuid { get; set; }

            [JsonProperty("email_verified")]
            public bool EmailVerified { get; set; }

            [JsonProperty("player_plocale")]
            public string PlayerPlocale { get; set; }

            [JsonProperty("country_at")]
            public long? CountryAt { get; set; }

            [JsonProperty("pw")]
            public PwInfoObject PwInfo { get; set; }

            [JsonProperty("phone_number_verified")]
            public bool PhoneNumberVerified { get; set; }

            [JsonProperty("account_verified")]
            public bool AccountVerified { get; set; }

            [JsonProperty("ppid")]
            public object Ppid { get; set; }

            [JsonProperty("player_locale")]
            public string PlayerLocale { get; set; }

            [JsonProperty("acct")]
            public AccountInfoObject AccountInfo { get; set; }

            [JsonProperty("age")]
            public int Age { get; set; }

            [JsonProperty("jti")]
            public string Jti { get; set; }

            public class PwInfoObject
            {
                [JsonProperty("cnt_at")]
                public long CngAt { get; set; }

                [JsonProperty("reset")]
                public bool Reset { get; set; }

                [JsonProperty("must_reset")]
                public bool MustReset { get; set; }
            }

            public class AccountInfoObject
            {
                [JsonProperty("type")]
                public int Type { get; set; }

                [JsonProperty("state")]
                public string State { get; set; }

                [JsonProperty("adm")]
                public bool Adm { get; set; }

                [JsonProperty("game_name")]
                public string GameName { get; set; }

                [JsonProperty("tag_line")]
                public string TagLine { get; set; }

                [JsonProperty("created_at")]
                public long CreatedAt { get; set; }
            }
        }

        public UserData Clear()
        {
            this.TokenData = null;
            this.RiotUserData = null;
            this.RiotUrl = null;
            this.ClientHandler.CookieContainer = new();

            this.Client = _httpClientFactory.CreateClient("ValClient");

            return this;
        }
    }
}
