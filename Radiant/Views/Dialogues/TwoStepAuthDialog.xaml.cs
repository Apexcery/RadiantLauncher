using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Radiant.Extensions;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Models.Auth;
using Radiant.Models.Enums;
using Radiant.Utils;
using Radiant.ViewModels;

namespace Radiant.Views.Dialogues
{
    public partial class TwoStepAuthDialog : ObservableUserControl
    {
        public RelayCommand<ICloseable> CloseCommand { get; }
        public RelayCommand<object> SubmitCommand { get; }

        private Style _systemButtonsStyle;
        public Style SystemButtonsStyle
        {
            get => _systemButtonsStyle;
            set
            {
                _systemButtonsStyle = value;
                OnPropertyChanged();
            }
        }

        private string _accountEmail;
        public string AccountEmail
        {
            get => _accountEmail;
            set
            {
                _accountEmail = value;
                OnPropertyChanged();
            }
        }

        private string _authCode;
        public string AuthCode
        {
            get => _authCode;
            set
            {
                _authCode = value;
                OnPropertyChanged();
            }
        }
        
        private readonly AppConfig _appConfig;
        private readonly UserData _userData;
        private readonly CancellationToken _cancellationToken;
        private readonly bool _isAddingAccount;
        private readonly Dictionary<string, Uri> _apiUris;
        
        private readonly string _username;
        private readonly string _password;

        private bool loginSuccess = false;

        public TwoStepAuthDialog(AppConfig appConfig, UserData userData, CancellationToken cancellationToken, bool isAddingAccount, string email, string username, string password)
        {
            _appConfig = appConfig;
            _userData = userData;
            _cancellationToken = cancellationToken;
            _isAddingAccount = isAddingAccount;
            _username = username;
            _password = password;
            _apiUris = ApiURIs.URIs;

            this.DataContext = this;
            
            CloseCommand = new(_ => CloseDialog());
            SubmitCommand = new(_ => HandleSubmit());

            InitializeComponent();

            switch (_appConfig.Settings.SystemButtons)
            {
                case SystemButtons.Colored:
                    SystemButtonsStyle = Application.Current.TryFindResource("ColoredSystemButton") as Style;
                    break;
                case SystemButtons.Simple:
                    SystemButtonsStyle = Application.Current.TryFindResource("SimpleSystemButton") as Style;
                    break;
            }

            AccountEmail = email;

            AuthCodeTextBox.TextChanged += AuthCodeTextBoxOnTextChanged;
        }

        private void HandleSubmit()
        {
            SubmitButton.IsEnabled = false;
            SubmitButton.Content = new ProgressBar
            {
                IsIndeterminate = true,
                Width = 25,
                Height = 25,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Task.Run(SubmitAuthCode, _cancellationToken).Wait(Convert.ToInt32(TimeSpan.FromMinutes(1).TotalMilliseconds));
            SubmitButton.Content = "Submit";
            SubmitButton.IsEnabled = true;
            this.CloseDialog();
        }

        private async Task SubmitAuthCode()
        {
            var mfaData = new MFAData
            {
                Code = AuthCode
            };
            var mfaDataAsJson = JsonConvert.SerializeObject(mfaData);
            var mfaResponse = await _userData.Client.PutAsync(ApiURIs.URIs["AuthUri"], new StringContent(mfaDataAsJson, Encoding.UTF8, "application/json"), _cancellationToken);

            if (!mfaResponse.IsSuccessStatusCode)
            {
                loginSuccess = false;
                return;
            }
            
            if (mfaResponse.Headers.Contains("Set-Cookie"))
            {
                var cookies = mfaResponse.Headers.GetValues("Set-Cookie");
                AddClientCookies(cookies);
            }

            var authResponse = await mfaResponse.Content.ReadAsJsonAsync<AuthResponse>(_cancellationToken);

            if (string.IsNullOrEmpty(authResponse?.Response?.Parameters.Uri)) // authResponse can be null if the 2-step prompt was closed.
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", authResponse?.Error ?? "" });
                await DialogHost.Show(dialog, "TwoStepDialogHost");
                loginSuccess = false;
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
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", "Access or ID Token was invalid." });
                await DialogHost.Show(dialog, "TwoStepDialogHost");
                loginSuccess = false;
                return;
            }

            _userData.TokenData = new UserData.TokenDataObject
            {
                AccessToken = accessToken,
                IdToken = idToken
            };

            _userData.Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var entitlementToken = await GetEntitlementToken(_cancellationToken);
            if (string.IsNullOrEmpty(entitlementToken))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", "Entitlement Token was invalid." });
                await DialogHost.Show(dialog, "TwoStepDialogHost");
                loginSuccess = false;
                return;
            }
            _userData.TokenData.EntitlementToken = entitlementToken;
            _userData.Client.DefaultRequestHeaders.Add("X-Riot-Entitlements-JWT", entitlementToken);

            _userData.RiotUserData = await GetUserData(_cancellationToken);
            if (_userData.RiotUserData == null)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", "Failed to retrieve user data." });
                await DialogHost.Show(dialog, "TwoStepDialogHost");
                loginSuccess = false;
                return;
            }

            var region = await GetUserRegion(_cancellationToken);
            if (region == null)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", "Failed to retrieve user region." });
                await DialogHost.Show(dialog, "TwoStepDialogHost");
                loginSuccess = false;
                return;
            }
            _userData.RiotRegion = region.Value;

            var addHeaderSuccess = await AddClientHeaders(_cancellationToken);
            if (!addHeaderSuccess)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new[] { "Failed to log in.", "Failed to retrieve client headers." });
                await DialogHost.Show(dialog, "TwoStepDialogHost");
                loginSuccess = false;
                return;
            }

            var account = new Account
            {
                Username = _username,
                Password = _password,
                DisplayName = _userData.RiotUserData.AccountInfo.GameName,
                Tag = _userData.RiotUserData.AccountInfo.TagLine
            };

            HomeViewModel.LoggedInAccount = account;

            loginSuccess = true;
        }
        private void AddClientCookies(IEnumerable<string> cookieHeaders)
        {
            foreach (var cookie in cookieHeaders)
            {
                _userData.ClientHandler.CookieContainer.SetCookies(_apiUris["AuthUri"], cookie);
            }
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

        private async Task<RiotRegionEnum?> GetUserRegion(CancellationToken cancellationToken, string region = null)
        {
            string liveRegion;
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

            switch (liveRegion.ToLowerInvariant())
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

        private void AuthCodeTextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            var cursorPos = tb.CaretIndex;
            var fullText = tb.Text;
            var cleanedText = Regex.Replace(fullText, "[^0-9]", "");
            tb.Text = cleanedText;
            tb.CaretIndex = Math.Min(cursorPos, tb.Text.Length);
            SubmitButton.IsEnabled = tb.Text.Length == 6;
        }
        
        private void CloseDialog()
        {
            try
            {
                if (DialogHost.IsDialogOpen(_isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost"))
                {
                    DialogHost.Close(_isAddingAccount ? "AddAccountDialogHost" : "MainDialogHost");
                }
            }
            catch
            {
                // ignored, if dialog host does not exist yet, an exception is thrown.
            }
        } 
    }
}
