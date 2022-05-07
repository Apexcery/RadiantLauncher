using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Radiant.Models.Career;
using Radiant.Models.Enums;
using Radiant.Models.Store;

namespace Radiant.Models
{
    public class UserData
    {
        public HttpClientHandler ClientHandler;

        public TokenDataObject TokenData = new();
        public RiotUserDataObject RiotUserData;
        public RiotRegionEnum RiotRegion;
        public RiotUrlObject RiotUrl = new();
        
        public PlayerStore PlayerStore = new();
        public StoreOffers StoreOffers = new();

        public PlayerRankInfo PlayerRankInfo = new();
        public PlayerRankUpdates PlayerRankUpdates = new();
        public PlayerMatchHistory PlayerMatchHistory = new();
        public List<MatchData> PlayerMatchHistoryData = new ();

        public UserData()
        {
            this.ClientHandler = new()
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            this.Client = new HttpClient
            {
                DefaultRequestHeaders =
                {
                    CacheControl = new()
                    {
                        NoCache = true,
                        NoStore = true
                    }
                },
                Timeout = TimeSpan.FromSeconds(30)
            };
            this.Client.DefaultRequestHeaders.Add("User-Agent", "RiotClient/43.0.1.4195386.4190634 rso-auth (Windows;10;;Professional, x64)");
        }

        public HttpClient Client;

        public class RiotUrlObject
        {
            [JsonProperty("glzURL")]
            public string GlzUrl;

            [JsonProperty("pdURL")]
            public string PdUrl;
        }

        public class TokenDataObject
        {
            public string AccessToken;
            public string IdToken;
            public string EntitlementToken;
        }
        public class RiotUserDataObject
        {
            [JsonProperty("country")]
            public string Country;

            [JsonProperty("sub")]
            public string Puuid;

            [JsonProperty("email_verified")]
            public bool EmailVerified;

            [JsonProperty("player_plocale")]
            public string PlayerPlocale;

            [JsonProperty("country_at")]
            public long? CountryAt;

            [JsonProperty("pw")]
            public PwInfoObject PwInfo;

            [JsonProperty("phone_number_verified")]
            public bool PhoneNumberVerified;

            [JsonProperty("account_verified")]
            public bool AccountVerified;

            [JsonProperty("ppid")]
            public object Ppid;

            [JsonProperty("player_locale")]
            public string PlayerLocale;

            [JsonProperty("acct")]
            public AccountInfoObject AccountInfo;

            [JsonProperty("age")]
            public int Age;

            [JsonProperty("jti")]
            public string Jti;

            public class PwInfoObject
            {
                [JsonProperty("cnt_at")]
                public long CngAt;

                [JsonProperty("reset")]
                public bool Reset;

                [JsonProperty("must_reset")]
                public bool MustReset;
            }

            public class AccountInfoObject
            {
                [JsonProperty("type")]
                public int Type;

                [JsonProperty("state")]
                public string State;

                [JsonProperty("adm")]
                public bool Adm;

                [JsonProperty("game_name")]
                public string GameName;

                [JsonProperty("tag_line")]
                public string TagLine;

                [JsonProperty("created_at")]
                public long CreatedAt;
            }
        }

        public UserData Clear()
        {
            this.ClientHandler = new()
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };
            
            this.TokenData = new();
            this.RiotUserData = null;
            this.RiotUrl = new();

            this.PlayerStore = new();
            this.StoreOffers = new();

            this.PlayerRankInfo = new();
            this.PlayerRankUpdates = new();
            this.PlayerMatchHistory = new();
            this.PlayerMatchHistoryData = new();

            this.Client = new HttpClient
            {
                DefaultRequestHeaders =
                {
                    CacheControl = new()
                    {
                        NoCache = true,
                        NoStore = true
                    }
                },
                Timeout = TimeSpan.FromSeconds(30)
            };
            this.Client.DefaultRequestHeaders.Add("User-Agent", "RiotClient/43.0.1.4195386.4190634 rso-auth (Windows;10;;Professional, x64)");

            return this;
        }
    }
}
