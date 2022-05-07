using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using Radiant.Constants;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Models.Career;
using Radiant.Models.Valorant;
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

        private ChartValues<ObservableValue> _rankChartValues = new();
        public ChartValues<ObservableValue> RankChartValues
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

            PlayerRankInfo rankInfo = null;
            PlayerRankUpdates playerRankUpdates = null;
            List<MatchData> playerMatchHistoryData = null;
            try
            {
                rankInfo = UserData.PlayerRankInfo ?? await _careerService.GetPlayerRankInfo(CancellationTokenSource.Token);
                playerRankUpdates = UserData.PlayerRankUpdates ?? await _careerService.GetPlayerRankUpdates(CancellationTokenSource.Token, 10, "competitive");
                playerMatchHistoryData = UserData.PlayerMatchHistoryData;

                if (playerMatchHistoryData is null)
                {
                    var playerMatchHistory = UserData.PlayerMatchHistory ?? await _careerService.GetPlayerMatchHistory(CancellationTokenSource.Token, 10);
                    if (playerMatchHistory is null)
                        return;
                    playerMatchHistoryData = (await Task.WhenAll(playerMatchHistory.Matches.Select(match => _careerService.GetMatchData(CancellationTokenSource.Token, match.MatchID)))).ToList();
                }
            }
            catch (TaskCanceledException) { }

            if (rankInfo is null || playerRankUpdates is null || playerMatchHistoryData is null)
                return;

            SetRankHistoryIcons(rankInfo);
            PopulateGraph(playerRankUpdates);
            PopulateMatchHistory(playerRankUpdates, playerMatchHistoryData);
        }

        private void SetRankHistoryIcons(PlayerRankInfo rankInfo)
        {
            var currentSeason = ValorantConstants.SeasonById[rankInfo.LatestCompetitiveUpdate.SeasonID];
            var currentSeasonFullName = Seasons.GetSeasonFullName(currentSeason.Id, ValorantConstants.SeasonById.Values);

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
                    Width = actName.Equals(currentSeasonFullName) ? 75 : 50,
                    Height = actName.Equals(currentSeasonFullName) ? 75 : 50
                };

                var rankHistoryName = new TextBlock
                {
                    Text = $"{(actName.Equals(currentSeasonFullName) ? "[Current Act]" : actName)}",
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
            RankChartValues.AddRange(recentMatches.Select(match => new ObservableValue(match.RankedRatingAfterUpdate)));
            RankChartLabels.AddRange(recentMatches.Select((_, index) => $"Game {index + 1}"));
            RankupChartLabels.AddRange(recentMatches.Select(match =>
                match.TierBeforeUpdate > match.TierAfterUpdate ? "[De-Rank]" :
                match.TierBeforeUpdate < match.TierAfterUpdate ? "[Rank-Up]" :
                ""));
        }

        private void PopulateMatchHistory(PlayerRankUpdates playerRankUpdates, List<MatchData> playerMatchHistoryData)
        {
            foreach (var matchData in playerMatchHistoryData)
            {
                var matchingRankUpdate = playerRankUpdates.Matches.FirstOrDefault(x => x.MatchID.Equals(matchData.MatchInfo.MatchId));
                var matchHistoryItem = new MatchHistoryItem(matchData, matchingRankUpdate, UserData);
                MatchHistoryItems.Add(matchHistoryItem);
            }
        }
    }
}
