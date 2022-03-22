using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using Radiant.Constants;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Models.Career;
using Radiant.Utils;
using Radiant.Views.UserControls;

namespace Radiant.ViewModels
{
    public class CareerViewModel : Observable
    {
        private readonly ICareerService _careerService;
        public readonly UserData UserData;

        private ObservableCollection<Image> _rankHistoryItems = new();
        public ObservableCollection<Image> RankHistoryItems
        {
            get => _rankHistoryItems;
            set
            {
                _rankHistoryItems = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<TextBlock> _rankHistoryName = new();
        public ObservableCollection<TextBlock> RankHistoryName
        {
            get => _rankHistoryName;
            set
            {
                _rankHistoryName = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<TextBlock> _rankHistoryRank = new();
        public ObservableCollection<TextBlock> RankHistoryRank
        {
            get => _rankHistoryRank;
            set
            {
                _rankHistoryRank = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MatchHistoryItem> _matchHistoryItems = new();
        public ObservableCollection<MatchHistoryItem> MatchHistoryItems
        {
            get => _matchHistoryItems;
            set
            {
                _matchHistoryItems = value;
                OnPropertyChanged();
            }
        }

        private ChartValues<double> _rankChartValues = new();
        public ChartValues<double> RankChartValues
        {
            get => _rankChartValues;
            set
            {
                _rankChartValues = value;
                OnPropertyChanged();
            }
        }
        private ChartValues<string> _rankChartLabels = new();
        public ChartValues<string> RankChartLabels
        {
            get => _rankChartLabels;
            set
            {
                _rankChartLabels = value;
                OnPropertyChanged();
            }
        }
        private ChartValues<string> _rankupChartLabels = new();
        public ChartValues<string> RankupChartLabels
        {
            get => _rankupChartLabels;
            set
            {
                _rankupChartLabels = value;
                OnPropertyChanged();
            }
        }

        public CancellationTokenSource CancellationTokenSource = new();

        public CareerViewModel(ICareerService careerService, UserData userData)
        {
            _careerService = careerService;
            UserData = userData;
        }

        public void ClearCareerData()
        {
            RankHistoryItems.Clear();
            RankHistoryName.Clear();
            RankHistoryRank.Clear();
            MatchHistoryItems.Clear();
            RankChartValues.Clear();
            RankChartLabels.Clear();
            RankupChartLabels.Clear();
        }

        public async Task GetRankData()
        {
            ClearCareerData();

            if (RankHistoryItems.Any())
                return;

            if (CancellationTokenSource.IsCancellationRequested)
            {
                CancellationTokenSource = new();
            }

            var rankInfoTask = _careerService.GetPlayerRankInfo(CancellationTokenSource.Token);
            var playerRankUpdatesTask = _careerService.GetPlayerRankUpdates(CancellationTokenSource.Token, 10, "competitive");
            var playerMatchHistoryTask = _careerService.GetPlayerMatchHistory(CancellationTokenSource.Token, 10);

            PlayerRankInfo rankInfo = null;
            PlayerRankUpdates playerRankUpdates = null;
            PlayerMatchHistory playerMatchHistory = null;
            try
            {
                await Task.WhenAll(rankInfoTask, playerRankUpdatesTask, playerMatchHistoryTask);

                rankInfo = await rankInfoTask;
                playerRankUpdates = await playerRankUpdatesTask;
                playerMatchHistory = await playerMatchHistoryTask;
            }
            catch (TaskCanceledException taskCanceledException) { }

            if (rankInfo == null || playerRankUpdates == null || playerMatchHistory == null)
                return;

            SetRankHistoryIcons(rankInfo);
            PopulateGraph(playerRankUpdates);
            await PopulateMatchHistory(playerMatchHistory, playerRankUpdates);
        }

        private void SetRankHistoryIcons(PlayerRankInfo rankInfo)
        {
            var currentSeasonName = ValorantConstants.SeasonNameById[rankInfo.LatestCompetitiveUpdate.SeasonID];

            var previousActTiers = new Dictionary<string, int>
            {
                { "Episode 3 Act 2", rankInfo.QueueData.Competitive?.SeasonalInfoBySeasonID?.Episode3Act2?.CompetitiveTier ?? 0 },
                { "Episode 3 Act 3", rankInfo.QueueData.Competitive?.SeasonalInfoBySeasonID?.Episode3Act3?.CompetitiveTier ?? 0 },
                { "Episode 4 Act 1", rankInfo.QueueData.Competitive?.SeasonalInfoBySeasonID?.Episode4Act1?.CompetitiveTier ?? 0 },
                { "Episode 4 Act 2", rankInfo.QueueData.Competitive?.SeasonalInfoBySeasonID?.Episode4Act2?.CompetitiveTier ?? 0 },
                { "Episode 4 Act 3", rankInfo.QueueData.Competitive?.SeasonalInfoBySeasonID?.Episode4Act3?.CompetitiveTier ?? 0 }
            };

            foreach (var (actName, actTier) in previousActTiers)
            {
                var rankName = ValorantConstants.RankNameByTier[actTier];
                var rankResourceName = $"{rankName.Replace(" ", "")}Image";

                var rankImage = new Image
                {
                    Source = Application.Current.TryFindResource(rankResourceName) as ImageSource,
                    Width = actName.Equals(currentSeasonName) ? 75 : 50,
                    Height = actName.Equals(currentSeasonName) ? 75 : 50
                };

                var rankHistoryName = new TextBlock
                {
                    Text = $"{(actName.Equals(currentSeasonName) ? "[Current Act]" : actName)}",
                    Foreground = (SolidColorBrush)Application.Current.TryFindResource("Text"),
                    FontSize = (double)Application.Current.TryFindResource("LoginFormTextSize"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(10, 0, 10, 10)
                };

                var rankHistoryRank = new TextBlock
                {
                    Text = $"{rankName}",
                    Foreground = (SolidColorBrush)Application.Current.TryFindResource("Text"),
                    FontSize = (double)Application.Current.TryFindResource("LoginFormTextSize"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(10, 5, 10, 5)
                };

                RankHistoryItems.Add(rankImage);
                RankHistoryName.Add(rankHistoryName);
                RankHistoryRank.Add(rankHistoryRank);
            }
        }

        private void PopulateGraph(PlayerRankUpdates playerRankUpdates)
        {
            RankChartValues.Clear();
            RankChartLabels.Clear();
            var recentMatches = playerRankUpdates.Matches.Take(5).OrderBy(x => x.MatchStartTime).ToList();
            for (var i = 0; i < recentMatches.Count; i++)
            {
                var rankUpdate = recentMatches[i];
                RankChartValues.Add(rankUpdate.RankedRatingAfterUpdate);
                RankChartLabels.Add($"Game {i + 1}");
                if (rankUpdate.TierBeforeUpdate > rankUpdate.TierAfterUpdate)
                {
                    // Rank tier decrease
                    RankupChartLabels.Add("[De-Rank]");
                }
                else if (rankUpdate.TierBeforeUpdate < rankUpdate.TierAfterUpdate)
                {
                    // Rank tier increase
                    RankupChartLabels.Add("[Rank-Up]");
                }
                else
                {
                    // Rank tier didn't change
                    RankupChartLabels.Add("");
                }
            }
        }

        private async Task PopulateMatchHistory(PlayerMatchHistory playerMatchHistory, PlayerRankUpdates playerRankUpdates)
        {
            var matchDataTasks = playerMatchHistory.Matches.Select(match => _careerService.GetMatchData(CancellationTokenSource.Token, match.MatchID)).ToList();
            MatchData[] matchDataResults = null;
            try
            {
                matchDataResults = await Task.WhenAll(matchDataTasks);
            }
            catch (TaskCanceledException taskCanceledException) { }

            if (matchDataResults is null)
                return;

            foreach (var matchData in matchDataResults)
            {
                var matchingRankUpdate = playerRankUpdates.Matches.FirstOrDefault(x => x.MatchID.Equals(matchData.MatchInfo.MatchId));
                var matchHistoryItem = new MatchHistoryItem(matchData, matchingRankUpdate, UserData);
                MatchHistoryItems.Add(matchHistoryItem);
            }
        }
    }
}
