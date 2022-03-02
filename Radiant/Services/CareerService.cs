using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Radiant.Extensions;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Models.Career;
using Radiant.Views.Dialogues;

namespace Radiant.Services
{
    public class CareerService : ICareerService
    {
        private readonly UserData _userData;
        private readonly AppConfig _appConfig;

        public CareerService(UserData userData, AppConfig appConfig)
        {
            _userData = userData;
            _appConfig = appConfig;
        }

        public async Task<PlayerRankInfo> GetPlayerRankInfo()
        {
            if (string.IsNullOrEmpty(_userData.RiotUrl?.PdUrl) || string.IsNullOrEmpty(_userData.RiotUserData.Puuid))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Login to your account before trying to view your career."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/mmr/v1/players/{_userData.RiotUserData.Puuid}");
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to get player's career rank.", response.ReasonPhrase});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var playerRankInfo = await response.Content.ReadAsJsonAsync<PlayerRankInfo>();
            return playerRankInfo;
        }

        public async Task<PlayerRankUpdates> GetPlayerRankUpdates(int amount = 15, string queue = null)
        {
            if (string.IsNullOrEmpty(_userData.RiotUrl?.PdUrl) || string.IsNullOrEmpty(_userData.RiotUserData.Puuid))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Login to your account before trying to view your career."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var requestUrl = $"{baseAddress}/mmr/v1/players/{_userData.RiotUserData.Puuid}/competitiveupdates?startIndex=0&endIndex={amount}";
            if (!string.IsNullOrEmpty(queue))
                requestUrl += $"&queue={queue}";
            var response = await _userData.Client.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to get player's rank updates.", response.ReasonPhrase});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var playerRankInfo = await response.Content.ReadAsJsonAsync<PlayerRankUpdates>();
            return playerRankInfo;
        }

        public async Task<PlayerMatchHistory> GetPlayerMatchHistory(int amount = 15, string queue = null)
        {
            if (string.IsNullOrEmpty(_userData.RiotUrl?.PdUrl) || string.IsNullOrEmpty(_userData.RiotUserData.Puuid))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Login to your account before trying to view your career."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var requestUrl = $"{baseAddress}/match-history/v1/history/{_userData.RiotUserData.Puuid}?startIndex=0&endIndex={amount}";
            if (!string.IsNullOrEmpty(queue))
                requestUrl += $"&queue={queue}";

            var response = await _userData.Client.GetAsync($"{requestUrl}");
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to get player's match history.", response.ReasonPhrase});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var playerMatchHistory = await response.Content.ReadAsJsonAsync<PlayerMatchHistory>();
            return playerMatchHistory;
        }

        public async Task<MatchData> GetMatchData(string matchId)
        {
            if (string.IsNullOrEmpty(_userData.RiotUrl?.PdUrl) || string.IsNullOrEmpty(_userData.RiotUserData.Puuid))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Login to your account before trying to view your career."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/match-details/v1/matches/{matchId}");
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to get match details for match ID:", matchId, response.ReasonPhrase});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var matchData = await response.Content.ReadAsJsonAsync<MatchData>();
            return matchData;
        }
    }
}
