using System.Threading.Tasks;
using System.Windows;
using ValorantLauncher.Extensions;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models;
using ValorantLauncher.Models.Store.Career;

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
            if (_userData.RiotUrl?.PdUrl == null)
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
    }
}
