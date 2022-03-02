﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Radiant.Extensions;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Models.Auth;
using Radiant.Models.Enums;
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

        public async Task<bool> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {

                var dialog = new PopupDialog(_appConfig, "Error", new []{"Invalid username or password"});
                await DialogHost.Show(dialog, "MainDialogHost");
                return false;
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls11;

            var authInitCookieData = new AuthInitCookieData();
            var initResponse = await _userData.Client.PostAsync(_apiUris["AuthUri"], new StringContent(JsonConvert.SerializeObject(authInitCookieData), Encoding.UTF8, "application/json"));
            if (!initResponse.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in." });
                await DialogHost.Show(dialog, "MainDialogHost");
                return false;
            }

            if (initResponse.Headers.Contains("Set-Cookie"))
            {
                AddClientCookies(initResponse.Headers.GetValues("Set-Cookie"));
            }

            var authResponse = await initResponse.Content.ReadAsJsonAsync<AuthResponse>();

            if (authResponse.Type.Equals("auth", StringComparison.OrdinalIgnoreCase))
            {
                var authData = new AuthData
                {
                    Type = "auth",
                    Username = username,
                    Password = password,
                    Remember = "true"
                };

                var response = await _userData.Client.PutAsync(_apiUris["AuthUri"], new StringContent(JsonConvert.SerializeObject(authData), Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in." });
                    await DialogHost.Show(dialog, "MainDialogHost");
                    return false;
                }

                if (response.Headers.Contains("Set-Cookie"))
                {
                    AddClientCookies(response.Headers.GetValues("Set-Cookie"));
                }

                authResponse = await response.Content.ReadAsJsonAsync<AuthResponse>();
            }

            if (!string.IsNullOrEmpty(authResponse.Error))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to log in.", $"Error: {authResponse.Error}"});
                await DialogHost.Show(dialog, "MainDialogHost");
                return false;
            }

            if (authResponse.Type.Equals("multifactor"))
            {
                authResponse = await Show2StepAuthDialog(authResponse.Multifactor.Email);
            }

            if (string.IsNullOrEmpty(authResponse.Response?.Parameters.Uri))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to log in."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return false;
            }

            var regex = new Regex(Regex.Escape("#"));
            var newUri = regex.Replace(authResponse.Response.Parameters.Uri, "?", 1);
            var responseUri = new Uri(newUri);
            var paramaters = responseUri.DecodeQueryParameters();
            var accessToken = paramaters["access_token"];
            var idToken = paramaters["id_token"];
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(idToken))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to log in.", "Access or ID Token was invalid."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return false;
            }

            _userData.TokenData = new UserData.TokenDataObject
            {
                AccessToken = accessToken,
                IdToken = idToken
            };

            _userData.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var entitlementToken = await GetEntitlementToken();
            if (string.IsNullOrEmpty(entitlementToken))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to log in.", "Entitlement Token was invalid."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return false;
            }
            _userData.TokenData.EntitlementToken = entitlementToken;
            _userData.Client.DefaultRequestHeaders.Add("X-Riot-Entitlements-JWT", entitlementToken);

            _userData.RiotUserData = await GetUserData();
            if (_userData.RiotUserData == null)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", "Failed to retrieve user data." });
                await DialogHost.Show(dialog, "MainDialogHost");
                return false;
            }

            var region = await GetUserRegion();
            if (region == null)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to log in.", "Failed to retrieve user region."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return false;
            }
            _userData.RiotRegion = region.Value;

            var addHeaderSuccess = await AddClientHeaders();
            if (!addHeaderSuccess)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", "Failed to retrieve client headers." });
                await DialogHost.Show(dialog, "MainDialogHost");
                return false;
            }

            return true;
        }

        private async Task<AuthResponse> Show2StepAuthDialog(string email)
        {
            var authCode = "";
            AuthResponse authResponse = null;

            var textBox = new TextBox
            {
                Tag = "Code: ",
                Text = "",
                Style = Application.Current.TryFindResource("LoginFormTextBox") as Style,
                Foreground = Brushes.Black,
                FontFamily = Application.Current.TryFindResource("RobotoSlabBold") as FontFamily,
                MaxLength = 6
            };
            HintAssist.SetHint(textBox, "Code:");
            HintAssist.SetForeground(textBox, Application.Current.TryFindResource("BackgroundDark") as SolidColorBrush);
            HintAssist.SetFontFamily(textBox, Application.Current.TryFindResource("RobotoSlabBold") as FontFamily);

            var button = new Button
            {
                Content = "Submit",
                Width = 100,
                Height = 35,
                IsDefault = true,
                Background = Application.Current.TryFindResource("ValorantRed") as SolidColorBrush,
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                IsEnabled = false
            };
            ButtonAssist.SetCornerRadius(button, new CornerRadius(5));

            textBox.TextChanged += (sender, e) =>
            {
                var tb = (TextBox)sender;
                var cursorPos = tb.CaretIndex;
                var regex = new Regex("[^0-9]+");
                var fullText = tb.Text;
                var cleanedText = Regex.Replace(fullText, "[^0-9]+", "");
                tb.Text = cleanedText;
                tb.CaretIndex = Math.Min(cursorPos, tb.Text.Length);
                authCode = cleanedText;
                button.IsEnabled = tb.Text.Length == 6;
            };
            button.Click += async (sender, e) =>
            {
                var button = (Button)sender;
                button.IsEnabled = false;
                var mfaData = new MFAData
                {
                    Code = authCode
                };
                var mfaDataAsJson = JsonConvert.SerializeObject(mfaData);
                var mfaResponse = await _userData.Client.PutAsync(_apiUris["AuthUri"], new StringContent(mfaDataAsJson, Encoding.UTF8, "application/json"));
                if (!mfaResponse.IsSuccessStatusCode)
                {
                    var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", "Could not verify 2fa code." });
                    DialogHost.Close("MainDialogHost");
                    await DialogHost.Show(dialog, "MainDialogHost");
                    return;
                }

                if (mfaResponse.Headers.Contains("Set-Cookie"))
                {
                    AddClientCookies(mfaResponse.Headers.GetValues("Set-Cookie"));
                }

                authResponse = await mfaResponse.Content.ReadAsJsonAsync<AuthResponse>();
                DialogHost.Close("MainDialogHost");
            };

            var grid = new Grid
            {
                Children =
                {
                    new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Children =
                        {
                            textBox,
                            button
                        }
                    }
                }
            };
            
            var dialog = new PopupDialog(_appConfig, "2-Step Auth", new[] { $"Enter the code that was sent to: {email}" }, new[] { grid });
            await DialogHost.Show(dialog, "MainDialogHost");

            return authResponse;
        }

        private void AddClientCookies(IEnumerable<string> cookieHeaders)
        {
            foreach (var cookie in cookieHeaders)
            {
                _userData.ClientHandler.CookieContainer.SetCookies(_apiUris["AuthUri"], cookie);
            }
        }

        private async Task<bool> AddClientHeaders()
        {
            var response = await _userData.Client.GetAsync(_apiUris["ClientVersionUri"]);
            if (!response.IsSuccessStatusCode)
                return false;

            var versions = await response.Content.ReadAsJsonAsync<RiotClientVersions>();
            _userData.Client.DefaultRequestHeaders.Add("X-Riot-ClientVersion", versions.Data.RiotClientVersion);
            _userData.Client.DefaultRequestHeaders.Add("X-Riot-ClientPlatform", "ew0KCSJwbGF0Zm9ybVR5cGUiOiAiUEMiLA0KCSJwbGF0Zm9ybU9TIjogIldpbmRvd3MiLA0KCSJwbGF0Zm9ybU9TVmVyc2lvbiI6ICIxMC4wLjE5MDQyLjEuMjU2LjY0Yml0IiwNCgkicGxhdGZvcm1DaGlwc2V0IjogIlVua25vd24iDQp9");

            return true;
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
                if (response.Headers.Contains("Set-Cookie"))
                {
                    AddClientCookies(response.Headers.GetValues("Set-Cookie"));
                }
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
                    return RiotRegionEnum.EU;
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

        private async Task<UserData.RiotUserDataObject> GetUserData()
        {
            var response = await _userData.Client.GetAsync(_apiUris["UserInfoUri"]);
            if (response.Headers.Contains("Set-Cookie"))
            {
                AddClientCookies(response.Headers.GetValues("Set-Cookie"));
            }
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsJsonAsync<UserData.RiotUserDataObject>();
        }

        private async Task<string> GetEntitlementToken()
        {
            var response = await _userData.Client.PostAsync(_apiUris["EntitlementUri"], new StringContent("", Encoding.UTF8, "application/json"));

            if (response.Headers.Contains("Set-Cookie"))
            {
                AddClientCookies(response.Headers.GetValues("Set-Cookie"));
            }

            if (!response.IsSuccessStatusCode)
                return null;
            var token = JsonConvert.DeserializeObject<EntitlementResponse>(await response.Content.ReadAsStringAsync())?.EntitlementsToken;
            
            return token;
        }
    }
}
