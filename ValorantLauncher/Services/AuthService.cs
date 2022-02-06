using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using Newtonsoft.Json;
using RestSharp;
using ValorantLauncher.Extensions;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models;
using ValorantLauncher.Models.Auth;
using ValorantLauncher.Models.Enums;

namespace ValorantLauncher.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserData _userData;

        private readonly Dictionary<string, Uri> _apiUris = new()
        {
            {
                "AuthUri", new Uri("https://auth.riotgames.com/api/v1/authorization")
            },
            {
                "EntitlementUri", new Uri("https://entitlements.auth.riotgames.com/api/token/v1")
            },
            {
                "UserInfoUri", new Uri("https://auth.riotgames.com/userinfo")
            },
            {
                "UserRegionUri", new Uri("https://riot-geo.pas.si.riotgames.com/pas/v1/product/valorant")
            },
            {
                "ClientVersionUri", new Uri("https://valorant-api.com/v1/version")
            }
        };

        public AuthService(UserData userData)
        {
            _userData = userData;
        }

        public async void Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Invalid username or password"); //TODO: Better error popup.
                return;
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls11;

            var authInitCookieData = new AuthInitCookieData();
            var initResponse = await _userData.Client.PostAsync(_apiUris["AuthUri"], new StringContent(JsonConvert.SerializeObject(authInitCookieData), Encoding.UTF8, "application/json"));

            var authData = new AuthData
            {
                Type = "auth",
                Username = username,
                Password = password,
                Remember = "true"
            };

            var response = await _userData.Client.PutAsync(_apiUris["AuthUri"], new StringContent(JsonConvert.SerializeObject(authData), Encoding.UTF8, "application/json"));
            var authResponse = await response.Content.ReadAsJsonAsync<AuthResponse>();

            if (!string.IsNullOrEmpty(authResponse.Error))
            {
                MessageBox.Show("Failed to log in."); //TODO: Better error popup.
                return;
            }

            if (authResponse.Type.Equals("multifactor"))
            {
                //TODO: Need to deal with 2 step auth.
            }

            if (string.IsNullOrEmpty(authResponse.Response?.Parameters.Uri))
            {
                MessageBox.Show("Failed to log in."); //TODO: Better error popup.
                return;
            }

            var regex = new Regex(Regex.Escape("#"));
            var newUri = regex.Replace(authResponse.Response.Parameters.Uri, "?", 1);

            var responseUri = new Uri(newUri);
            var paramaters = responseUri.DecodeQueryParameters();
            var accessToken = paramaters["access_token"];
            var idToken = paramaters["id_token"];
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(idToken))
            {
                MessageBox.Show("Failed to log in."); //TODO: Better error popup.
                return;
            }

            _userData.TokenData.AccessToken = accessToken;
            _userData.TokenData.IdToken = idToken;

            _userData.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var entitlementToken = await GetEntitlementToken();
            if (string.IsNullOrEmpty(entitlementToken))
            {
                MessageBox.Show("Failed to log in."); //TODO: Better error popup.
                return;
            }
            _userData.TokenData.EntitlementToken = entitlementToken;
            _userData.Client.DefaultRequestHeaders.Add("X-Riot-Entitlements-JWT", entitlementToken);

            _userData.RiotUserData = await GetUserData();

            var region = await GetUserRegion();
            if (region == null)
            {
                MessageBox.Show("Failed to log in."); //TODO: Better error popup.
                return;
            }
            _userData.RiotRegion = region.Value;

            await AddClientHeaders();
        }

        private async Task AddClientHeaders()
        {
            var response = await _userData.Client.GetAsync(_apiUris["ClientVersionUri"]);
            if (!response.IsSuccessStatusCode)
                return;

            var versions = await response.Content.ReadAsJsonAsync<RiotClientVersions>();
            _userData.Client.DefaultRequestHeaders.Add("X-Riot-ClientVersion", versions.Data.RiotClientVersion);
            _userData.Client.DefaultRequestHeaders.Add("X-Riot-ClientPlatform", "ew0KCSJwbGF0Zm9ybVR5cGUiOiAiUEMiLA0KCSJwbGF0Zm9ybU9TIjogIldpbmRvd3MiLA0KCSJwbGF0Zm9ybU9TVmVyc2lvbiI6ICIxMC4wLjE5MDQyLjEuMjU2LjY0Yml0IiwNCgkicGxhdGZvcm1DaGlwc2V0IjogIlVua25vd24iDQp9");
        }

        private async Task<RiotRegionEnum?> GetUserRegion(string region = null)
        {
            var liveRegion = "";
            if (region is null)
            {
                var token = new
                {
                    id_token = _userData.TokenData.IdToken
                };
                var response = await _userData.Client.PutAsync(_apiUris["UserRegionUri"], new StringContent(JsonConvert.SerializeObject(token)));
                if (!response.IsSuccessStatusCode)
                    return null;

                var regionData = await response.Content.ReadAsJsonAsync<RegionResponse>();
                liveRegion = regionData.Affinities["live"];
            }
            else
            {
                liveRegion = region;
            }

            switch (liveRegion)
            {
                case "na":
                    _userData.RiotUrl.GlzUrl = "https://glz-na-1.na.a.pvp.net";
                    _userData.RiotUrl.PdUrl = "https://pd.na.a.pvp.net";
                    return RiotRegionEnum.NA;
                case "eu":
                    _userData.RiotUrl.GlzUrl = "https://glz-eu-1.eu.a.pvp.net";
                    _userData.RiotUrl.PdUrl = "https://pd.eu.a.pvp.net";
                    return RiotRegionEnum.EU;
                case "kr":
                    _userData.RiotUrl.GlzUrl = "https://glz-kr-1.kr.a.pvp.net";
                    _userData.RiotUrl.PdUrl = "https://pd.kr.a.pvp.net";
                    return RiotRegionEnum.KR;
                case "latam":
                    _userData.RiotUrl.GlzUrl = "https://glz-latam-1.na.a.pvp.net";
                    _userData.RiotUrl.PdUrl = "https://pd.na.a.pvp.net";
                    return RiotRegionEnum.LATAM;
                case "br":
                    _userData.RiotUrl.GlzUrl = "https://glz-br-1.na.a.pvp.net";
                    _userData.RiotUrl.PdUrl = "https://pd.na.a.pvp.net";
                    return RiotRegionEnum.BR;
                case "ap":
                    _userData.RiotUrl.GlzUrl = "https://glz-ap-1.ap.a.pvp.net";
                    _userData.RiotUrl.PdUrl = "https://pd.ap.a.pvp.net";
                    return RiotRegionEnum.AP;
            }

            return RiotRegionEnum.AP;
        }

        private async Task<UserData.RiotUserDataObject> GetUserData()
        {
            var response = await _userData.Client.GetAsync(_apiUris["UserInfoUri"]);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsJsonAsync<UserData.RiotUserDataObject>();
        }

        private async Task<string> GetEntitlementToken()
        {
            var response = await _userData.Client.PostAsync(_apiUris["EntitlementUri"], new StringContent("", Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
                return null;
            var token = JsonConvert.DeserializeObject<EntitlementResponse>(await response.Content.ReadAsStringAsync())?.EntitlementsToken;
            
            return token;
        }
    }
}
