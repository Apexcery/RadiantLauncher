using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ValorantLauncher.Interfaces;
using ValorantLauncher.Models.Store.Career;
using ValorantLauncher.Utils;

namespace ValorantLauncher.ViewModels
{
    public class CareerViewModel : Observable
    {
        private readonly ICareerService _careerService;

        private ImageSource _rankImageSource = Application.Current.TryFindResource("UnrankedImage") as ImageSource; //Set the unranked image as the default.
        public ImageSource RankImageSource
        {
            get => _rankImageSource;
            set
            {
                _rankImageSource = value;
                OnPropertyChanged();
            }
        }
        private ToolTip _rankImageTooltip;
        public ToolTip RankImageTooltip
        {
            get => _rankImageTooltip;
            set
            {
                _rankImageTooltip = value;
                OnPropertyChanged();
            }
        }

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

        private readonly Dictionary<int, string> TierToRank = new Dictionary<int, string>
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
        internal class SeasonInfo
        {
            internal string SeasonID { get; set; }
            internal string SeasonName { get; set; }
        }
        internal List<SeasonInfo> Seasons = new()
        {
            new SeasonInfo
            {
                SeasonID = "0df5adb9-4dcb-6899-1306-3e9860661dd3",
                SeasonName = "Closed Beta"
            },
            new SeasonInfo
            {
                SeasonID = "3f61c772-4560-cd3f-5d3f-a7ab5abda6b3",
                SeasonName = "Episode 1 Act 1"
            },
            new SeasonInfo
            {
                SeasonID = "0530b9c4-4980-f2ee-df5d-09864cd00542",
                SeasonName = "Episode 1 Act 2"
            },
            new SeasonInfo
            {
                SeasonID = "46ea6166-4573-1128-9cea-60a15640059b",
                SeasonName = "Episode 1 Act 3"
            },
            new SeasonInfo
            {
                SeasonID = "97b6e739-44cc-ffa7-49ad-398ba502ceb0",
                SeasonName = "Episode 2 Act 1"
            },
            new SeasonInfo
            {
                SeasonID = "ab57ef51-4e59-da91-cc8d-51a5a2b9b8ff",
                SeasonName = "Episode 2 Act 2"
            },
            new SeasonInfo
            {
                SeasonID = "52e9749a-429b-7060-99fe-4595426a0cf7",
                SeasonName = "Episode 2 Act 3"
            },
            new SeasonInfo
            {
                SeasonID = "2a27e5d2-4d30-c9e2-b15a-93b8909a442c",
                SeasonName = "Episode 3 Act 1"
            },
            new SeasonInfo
            {
                SeasonID = "4cb622e1-4244-6da3-7276-8daaf1c01be2",
                SeasonName = "Episode 3 Act 2"
            },
            new SeasonInfo
            {
                SeasonID = "a16955a5-4ad0-f761-5e9e-389df1c892fb",
                SeasonName = "Episode 3 Act 3"
            },
            new SeasonInfo
            {
                SeasonID = "573f53ac-41a5-3a7d-d9ce-d6a6298e5704",
                SeasonName = "Episode 4 Act 1"
            },
            new SeasonInfo
            {
                SeasonID = "d929bc38-4ab6-7da4-94f0-ee84f8ac141e",
                SeasonName = "Episode 4 Act 2"
            },
            new SeasonInfo
            {
                SeasonID = "3e47230a-463c-a301-eb7d-67bb60357d4f",
                SeasonName = "Episode 4 Act 3"
            },
        };

        public CareerViewModel(ICareerService careerService)
        {
            _careerService = careerService;
        }

        public async Task GetRankData()
        {
            var rankInfo = await _careerService.GetPlayerRankInfo();

            if (rankInfo == null)
                return;

            await SetRankHistoryIcons(rankInfo);
        }

        private async Task SetRankHistoryIcons(PlayerRankInfo rankInfo)
        {
            var currentSeason = Seasons.First(x => x.SeasonID.Equals(rankInfo.LatestCompetitiveUpdate.SeasonID));

            var currentSeasonRank = TierToRank[rankInfo.LatestCompetitiveUpdate.TierAfterUpdate];
            var currentSeasonName = currentSeason.SeasonName;

            var resourceName = $"{currentSeasonRank.Replace(" ", "")}Image";
            RankImageSource = Application.Current.TryFindResource(resourceName) as ImageSource;
            RankImageTooltip = new ToolTip
            {
                Content = new TextBlock
                {
                    Text = $"{currentSeasonName}\n{currentSeasonRank}",
                    TextAlignment = TextAlignment.Center
                },
                Placement = PlacementMode.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var previousActRanks = new Dictionary<string, int>
            {
                { "Episode 3 Act 2", rankInfo.QueueData.Competitive?.SeasonalInfoBySeasonID?.Episode3Act2?.CompetitiveTier ?? 0 },
                { "Episode 3 Act 3", rankInfo.QueueData.Competitive?.SeasonalInfoBySeasonID?.Episode3Act3?.CompetitiveTier ?? 0 },
                { "Episode 4 Act 1", rankInfo.QueueData.Competitive?.SeasonalInfoBySeasonID?.Episode4Act1?.CompetitiveTier ?? 0 },
                { "Episode 4 Act 2", rankInfo.QueueData.Competitive?.SeasonalInfoBySeasonID?.Episode4Act2?.CompetitiveTier ?? 0 },
                { "Episode 4 Act 3", rankInfo.QueueData.Competitive?.SeasonalInfoBySeasonID?.Episode4Act3?.CompetitiveTier ?? 0 }
            };

            foreach (var (actName, actRank) in previousActRanks)
            {
                var rankName = TierToRank[actRank];
                var rankResourceName = $"{rankName.Replace(" ", "")}Image";

                var rankImage = new Image
                {
                    Source = Application.Current.TryFindResource(rankResourceName) as ImageSource,
                    Width = 50,
                    Height = 50,
                    ToolTip = new ToolTip
                    {
                        Content = new TextBlock
                        {
                            Text = $"{actName}\n{rankName}",
                            TextAlignment = TextAlignment.Center
                        },
                        Placement = PlacementMode.Bottom,
                        HorizontalAlignment = HorizontalAlignment.Center
                    },
                };

                RankHistoryItems.Add(rankImage);
            }
        }
    }
}
