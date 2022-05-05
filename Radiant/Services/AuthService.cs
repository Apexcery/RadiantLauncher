using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Radiant.Extensions;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Models.Auth;
using Radiant.Models.Enums;
using Radiant.ViewModels;
using Radiant.Views.Dialogues;

namespace Radiant.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserData _userData;
        private readonly AppConfig _appConfig;

        private readonly Dictionary<string, Uri> _apiUris;

        public AuthService(UserData userData, AppConfig appConfig)
        {
            _userData = userData;
            _appConfig = appConfig;
            _apiUris = ApiURIs.URIs;
        }

        public async Task Login(CancellationToken cancellationToken, string username, string password, bool isAddingAccount = false)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Invalid username or password"});
                await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                return;
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var authInitCookieData = new AuthInitCookieData();
            var initResponse = await _userData.Client.PostAsync(_apiUris["AuthUri"], new StringContent(JsonConvert.SerializeObject(authInitCookieData), Encoding.UTF8, "application/json"), cancellationToken);
            if (!initResponse.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", initResponse.ReasonPhrase });
                await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                return;
            }

            if (initResponse.Headers.Contains("Set-Cookie"))
            {
                AddClientCookies(initResponse.Headers.GetValues("Set-Cookie"));
            }

            var authResponse = await initResponse.Content.ReadAsJsonAsync<AuthResponse>(cancellationToken);

            if (authResponse.Type.Equals("auth", StringComparison.OrdinalIgnoreCase))
            {
                var authData = new AuthData
                {
                    Type = "auth",
                    Username = username,
                    Password = password,
                    Remember = "true"
                };

                var response = await _userData.Client.PutAsync(_apiUris["AuthUri"], new StringContent(JsonConvert.SerializeObject(authData), Encoding.UTF8, "application/json"), cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", response.ReasonPhrase });
                    await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                    return;
                }

                if (response.Headers.Contains("Set-Cookie"))
                {
                    AddClientCookies(response.Headers.GetValues("Set-Cookie"));
                }

                authResponse = await response.Content.ReadAsJsonAsync<AuthResponse>(cancellationToken);
            }

            if (!string.IsNullOrEmpty(authResponse.Error))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to log in.", "Error: Username or Password was incorrect."});
                await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                return;
            }

            if (authResponse.Type.Equals("multifactor"))
            {
                await FinishLoginWithTwoStepAuth(cancellationToken, authResponse.Multifactor.Email, username, password, isAddingAccount);
                return;
            }

            if (string.IsNullOrEmpty(authResponse?.Response?.Parameters.Uri))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to log in.", authResponse.Error });
                await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                return;
            }

            var regex = new Regex(Regex.Escape("#"));
            var newUri = regex.Replace(authResponse.Response.Parameters.Uri, "?", 1);
            var responseUri = new Uri(newUri);
            var parameters = responseUri.DecodeQueryParameters();
            var accessToken = parameters["access_token"];
            var idToken = parameters["id_token"];
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(idToken))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to log in.", "Access or ID Token was invalid."});
                await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                return;
            }

            _userData.TokenData = new UserData.TokenDataObject
            {
                AccessToken = accessToken,
                IdToken = idToken
            };

            _userData.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var entitlementToken = await GetEntitlementToken(cancellationToken);
            if (string.IsNullOrEmpty(entitlementToken))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to log in.", "Entitlement Token was invalid."});
                await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                return;
            }
            _userData.TokenData.EntitlementToken = entitlementToken;
            _userData.Client.DefaultRequestHeaders.Add("X-Riot-Entitlements-JWT", entitlementToken);

            _userData.RiotUserData = await GetUserData(cancellationToken);
            if (_userData.RiotUserData == null)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", "Failed to retrieve user data." });
                await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                return;
            }

            var region = await GetUserRegion(cancellationToken);
            if (region == null)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to log in.", "Failed to retrieve user region."});
                await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                return;
            }
            _userData.RiotRegion = region.Value;

            var addHeaderSuccess = await AddClientHeaders(cancellationToken);
            if (!addHeaderSuccess)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", "Failed to retrieve client headers." });
                await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                return;
            }

            var account = new Account
            {
                Username = username,
                Password = password,
                DisplayName = _userData.RiotUserData.AccountInfo.GameName,
                Tag = _userData.RiotUserData.AccountInfo.TagLine
            };

            HomeViewModel.LoggedInAccount = account;
        }

        private async Task FinishLoginWithTwoStepAuth(CancellationToken cancellationToken, string email, string username, string password, bool isAddingAccount)
        {
            var dialog = new TwoStepAuthDialog(_appConfig, _userData, cancellationToken, isAddingAccount, email, username, password);
            await DialogHost.Show(dialog, isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
        }

        private void AddClientCookies(IEnumerable<string> cookieHeaders)
        {
            foreach (var cookie in cookieHeaders)
            {
                _userData.ClientHandler.CookieContainer.SetCookies(_apiUris["AuthUri"], cookie);
            }
        }

        private async Task<bool> AddClientHeaders(CancellationToken cancellationToken)
        {
            var response = await _userData.Client.GetAsync(_apiUris["ClientVersionUri"], cancellationToken);
            if (!response.IsSuccessStatusCode)
                return false;

            var versions = await response.Content.ReadAsJsonAsync<RiotClientVersions>(cancellationToken);
            _userData.Client.DefaultRequestHeaders.Add("X-Riot-ClientVersion", versions.Data.RiotClientVersion);
            _userData.Client.DefaultRequestHeaders.Add("X-Riot-ClientPlatform", "ew0KCSJwbGF0Zm9ybVR5cGUiOiAiUEMiLA0KCSJwbGF0Zm9ybU9TIjogIldpbmRvd3MiLA0KCSJwbGF0Zm9ybU9TVmVyc2lvbiI6ICIxMC4wLjE5MDQyLjEuMjU2LjY0Yml0IiwNCgkicGxhdGZvcm1DaGlwc2V0IjogIlVua25vd24iDQp9");

            return true;
        }

        private async Task<RiotRegionEnum?> GetUserRegion(CancellationToken cancellationToken, string region = null)
        {
            var liveRegion = "";
            if (region is null)
            {
                var token = new
                {
                    id_token = _userData.TokenData.IdToken
                };
                var response = await _userData.Client.PutAsync(_apiUris["UserRegionUri"], new StringContent(JsonConvert.SerializeObject(token)), cancellationToken);
                if (response.Headers.Contains("Set-Cookie"))
                {
                    AddClientCookies(response.Headers.GetValues("Set-Cookie"));
                }
                if (!response.IsSuccessStatusCode)
                    return null;

                var regionData = await response.Content.ReadAsJsonAsync<RegionResponse>(cancellationToken);
                liveRegion = regionData.Affinities["live"];
            }
            else
            {
                liveRegion = region;
            }

            switch (liveRegion)
            {
                case "na":
                    _userData.RiotUrl = new()
                    {
                        GlzUrl = "https://glz-na-1.na.a.pvp.net",
                        PdUrl = "https://pd.na.a.pvp.net"
                    };
                    return RiotRegionEnum.NA;
                case "eu":
                    _userData.RiotUrl = new()
                    {
                        GlzUrl = "https://glz-eu-1.eu.a.pvp.net",
                        PdUrl = "https://pd.eu.a.pvp.net"
                    };
                    return RiotRegionEnum.EUW;
                case "kr":
                    _userData.RiotUrl = new()
                    {
                        GlzUrl = "https://glz-kr-1.kr.a.pvp.net",
                        PdUrl = "https://pd.kr.a.pvp.net"
                    };
                    return RiotRegionEnum.KR;
                case "latam":
                    _userData.RiotUrl = new()
                    {
                        GlzUrl = "https://glz-latam-1.na.a.pvp.net",
                        PdUrl = "https://pd.na.a.pvp.net"
                    };
                    return RiotRegionEnum.LATAM;
                case "br":
                    _userData.RiotUrl = new()
                    {
                        GlzUrl = "https://glz-br-1.na.a.pvp.net",
                        PdUrl = "https://pd.na.a.pvp.net"
                    };
                    return RiotRegionEnum.BR;
                case "ap":
                    _userData.RiotUrl = new()
                    {
                        GlzUrl = "https://glz-ap-1.ap.a.pvp.net",
                        PdUrl = "https://pd.ap.a.pvp.net"
                    };
                    return RiotRegionEnum.AP;
            }

            return RiotRegionEnum.AP;
        }

        private async Task<UserData.RiotUserDataObject> GetUserData(CancellationToken cancellationToken)
        {
            var response = await _userData.Client.GetAsync(_apiUris["UserInfoUri"], cancellationToken);
            if (response.Headers.Contains("Set-Cookie"))
            {
                AddClientCookies(response.Headers.GetValues("Set-Cookie"));
            }
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsJsonAsync<UserData.RiotUserDataObject>(cancellationToken);
        }

        private async Task<string> GetEntitlementToken(CancellationToken cancellationToken)
        {
            var response = await _userData.Client.PostAsync(_apiUris["EntitlementUri"], new StringContent("", Encoding.UTF8, "application/json"), cancellationToken);

            if (response.Headers.Contains("Set-Cookie"))
            {
                AddClientCookies(response.Headers.GetValues("Set-Cookie"));
            }

            if (!response.IsSuccessStatusCode)
                return null;
            var token = JsonConvert.DeserializeObject<EntitlementResponse>(await response.Content.ReadAsStringAsync(cancellationToken))?.EntitlementsToken;
            
            return token;
        }
    }
}
