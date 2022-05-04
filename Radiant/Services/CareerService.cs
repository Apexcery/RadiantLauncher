using System.Threading;
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

        public async Task<PlayerRankInfo> GetPlayerRankInfo(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_userData.RiotUrl?.PdUrl) || string.IsNullOrEmpty(_userData.RiotUserData.Puuid))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Login to your account before trying to view your career."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/mmr/v1/players/{_userData.RiotUserData.Puuid}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to get player's career rank.", response.ReasonPhrase});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var playerRankInfo = await response.Content.ReadAsJsonAsync<PlayerRankInfo>(cancellationToken);
            return playerRankInfo;
        }

        public async Task<PlayerRankUpdates> GetPlayerRankUpdates(CancellationToken cancellationToken, int amount = 15, string queue = null)
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
            var response = await _userData.Client.GetAsync(requestUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to get player's rank updates.", response.ReasonPhrase});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var playerRankInfo = await response.Content.ReadAsJsonAsync<PlayerRankUpdates>(cancellationToken);
            return playerRankInfo;
        }

        public async Task<PlayerMatchHistory> GetPlayerMatchHistory(CancellationToken cancellationToken, int amount = 15, string queue = null)
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

            var response = await _userData.Client.GetAsync($"{requestUrl}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to get player's match history.", response.ReasonPhrase});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var playerMatchHistory = await response.Content.ReadAsJsonAsync<PlayerMatchHistory>(cancellationToken);
            return playerMatchHistory;
        }

        public async Task<MatchData> GetMatchData(CancellationToken cancellationToken, string matchId)
        {
            if (string.IsNullOrEmpty(_userData.RiotUrl?.PdUrl) || string.IsNullOrEmpty(_userData.RiotUserData.Puuid))
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Login to your account before trying to view your career."});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/match-details/v1/matches/{matchId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var dialog = new PopupDialog(_appConfig, "Error", new []{"Failed to get match details for match ID:", matchId, response.ReasonPhrase});
                await DialogHost.Show(dialog, "MainDialogHost");
                return null;
            }

            var matchData = await response.Content.ReadAsJsonAsync<MatchData>(cancellationToken);
            return matchData;
        }
    }
}
