using System.Threading.Tasks;
using System.Windows;
using ValorantLauncher.Extensions;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models;
using ValorantLauncher.Models.Career;

namespace ValorantLauncher.Services
{
    public class CareerService : ICareerService
    {
        private readonly UserData _userData;

        public CareerService(UserData userData)
        {
            _userData = userData;
        }

        public async Task<PlayerRankInfo> GetPlayerRankInfo()
        {
            if (string.IsNullOrEmpty(_userData.RiotUrl?.PdUrl) || string.IsNullOrEmpty(_userData.RiotUserData.Puuid))
            {
                MessageBox.Show("Login to your account before trying to view your career."); //TODO: Better error handling.
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/mmr/v1/players/{_userData.RiotUserData.Puuid}");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to get player's career rank."); //TODO: Better error messages.
                return null;
            }

            var playerRankInfo = await response.Content.ReadAsJsonAsync<PlayerRankInfo>();
            return playerRankInfo;
        }

        public async Task<PlayerRankUpdates> GetPlayerRankUpdates(int amount = 15, string queue = null)
        {
            if (string.IsNullOrEmpty(_userData.RiotUrl?.PdUrl) || string.IsNullOrEmpty(_userData.RiotUserData.Puuid))
            {
                MessageBox.Show("Login to your account before trying to view your career."); //TODO: Better error handling.
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var requestUrl = $"{baseAddress}/mmr/v1/players/{_userData.RiotUserData.Puuid}/competitiveupdates?startIndex=0&endIndex={amount}";
            if (!string.IsNullOrEmpty(queue))
                requestUrl += $"&queue={queue}";
            var response = await _userData.Client.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to get player's rank updates."); //TODO: Better error messages.
                return null;
            }

            var playerRankInfo = await response.Content.ReadAsJsonAsync<PlayerRankUpdates>();
            return playerRankInfo;
        }

        public async Task<PlayerMatchHistory> GetPlayerMatchHistory(int amount = 15, string queue = null)
        {
            if (string.IsNullOrEmpty(_userData.RiotUrl?.PdUrl) || string.IsNullOrEmpty(_userData.RiotUserData.Puuid))
            {
                MessageBox.Show("Login to your account before trying to view your career."); //TODO: Better error handling.
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var requestUrl = $"{baseAddress}/match-history/v1/history/{_userData.RiotUserData.Puuid}?startIndex=0&endIndex={amount}";
            if (!string.IsNullOrEmpty(queue))
                requestUrl += $"&queue={queue}";

            var response = await _userData.Client.GetAsync($"{requestUrl}");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Failed to get player's match history."); //TODO: Better error messages.
                return null;
            }

            var playerMatchHistory = await response.Content.ReadAsJsonAsync<PlayerMatchHistory>();
            return playerMatchHistory;
        }

        public async Task<MatchData> GetMatchData(string matchId)
        {
            if (string.IsNullOrEmpty(_userData.RiotUrl?.PdUrl) || string.IsNullOrEmpty(_userData.RiotUserData.Puuid))
            {
                MessageBox.Show("Login to your account before trying to view your career."); //TODO: Better error handling.
                return null;
            }

            var baseAddress = _userData.RiotUrl.PdUrl;
            var response = await _userData.Client.GetAsync($"{baseAddress}/match-details/v1/matches/{matchId}");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Failed to get match details for match: {matchId}."); //TODO: Better error messages.
                return null;
            }

            var matchData = await response.Content.ReadAsJsonAsync<MatchData>();
            return matchData;
        }
    }
}
