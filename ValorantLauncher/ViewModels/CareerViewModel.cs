using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models.Career;
using ValorantLauncher.Utils;

namespace ValorantLauncher.ViewModels
{
    public class CareerViewModel : Observable
    {
        private readonly ICareerService _careerService;

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

        private ObservableCollection<object> _matchHistoryItems = new();
        public ObservableCollection<object> MatchHistoryItems
        {
            get => _matchHistoryItems;
            set
            {
                _matchHistoryItems = value;
                OnPropertyChanged();
            }
        }

        private readonly Dictionary<int, string> _rankIds = new()
        {
            { 0,"Unranked" },
            { 3,"Iron 1" },
            { 4,"Iron 2" },
            { 5,"Iron 3" },
            { 6,"Bronze 1" },
            { 7,"Bronze 2" },
            { 8,"Bronze 3" },
            { 9,"Silver 1" },
            { 10,"Silver 2" },
            { 11,"Silver 3" },
            { 12,"Gold 1" },
            { 13,"Gold 2" },
            { 14,"Gold 3" },
            { 15,"Platinum 1" },
            { 16,"Platinum 2" },
            { 17,"Platinum 3" },
            { 18,"Diamond 1" },
            { 19,"Diamond 2" },
            { 20,"Diamond 3" },
            { 21,"Immortal 1" },
            { 22,"Immortal 2" },
            { 23,"Immortal 3" },
            { 24,"Radiant" }
        };
        private readonly Dictionary<string, string> _seasonIds = new()
        {
            { "0df5adb9-4dcb-6899-1306-3e9860661dd3", "Closed Beta" },
            { "3f61c772-4560-cd3f-5d3f-a7ab5abda6b3", "Episode 1 Act 1" },
            { "0530b9c4-4980-f2ee-df5d-09864cd00542", "Episode 1 Act 2" },
            { "46ea6166-4573-1128-9cea-60a15640059b", "Episode 1 Act 3" },
            { "97b6e739-44cc-ffa7-49ad-398ba502ceb0", "Episode 2 Act 1" },
            { "ab57ef51-4e59-da91-cc8d-51a5a2b9b8ff", "Episode 2 Act 2" },
            { "52e9749a-429b-7060-99fe-4595426a0cf7", "Episode 2 Act 3" },
            { "2a27e5d2-4d30-c9e2-b15a-93b8909a442c", "Episode 3 Act 1" },
            { "4cb622e1-4244-6da3-7276-8daaf1c01be2", "Episode 3 Act 2" },
            { "a16955a5-4ad0-f761-5e9e-389df1c892fb", "Episode 3 Act 3" },
            { "573f53ac-41a5-3a7d-d9ce-d6a6298e5704", "Episode 4 Act 1" },
            { "d929bc38-4ab6-7da4-94f0-ee84f8ac141e", "Episode 4 Act 2" },
            { "3e47230a-463c-a301-eb7d-67bb60357d4f", "Episode 4 Act 3" }
        };

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

        public CareerViewModel(ICareerService careerService)
        {
            _careerService = careerService;
        }

        public async Task GetRankData()
        {
            if (RankHistoryItems.Any())
                return;

            //Initial Graph Values
            for (var i = 0; i < 5; i++)
            {
                RankChartValues.Add(50);
                RankChartLabels.Add($"Game {i + 1}");
            }

            var rankInfo = await _careerService.GetPlayerRankInfo();
            var playerRankUpdates = await _careerService.GetPlayerRankUpdates();

            if (rankInfo == null)
                return;

            SetRankHistoryIcons(rankInfo);
            PopulateGraph(playerRankUpdates);
        }

        private void PopulateGraph(PlayerRankUpdates playerRankUpdates)
        {
            RankChartValues.Clear();
            RankChartLabels.Clear();
            for (var i = 0; i < 5; i++)
            {
                var rankUpdate = playerRankUpdates.Matches[i];
                RankChartValues.Add(rankUpdate.RankedRatingAfterUpdate);
                RankChartLabels.Add($"Game {i + 1}");
            }
        }

        private void SetRankHistoryIcons(PlayerRankInfo rankInfo)
        {
            var currentSeasonName = _seasonIds[rankInfo.LatestCompetitiveUpdate.SeasonID];

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
                var rankName = _rankIds[actTier];
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
    }
}
