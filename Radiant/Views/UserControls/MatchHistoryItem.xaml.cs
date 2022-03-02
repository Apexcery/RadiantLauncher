using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Radiant.Constants;
using Radiant.Models;
using Radiant.Models.Career;
using Radiant.Utils;
using Radiant.Utils.Extensions;

namespace Radiant.Views.UserControls
{
    public partial class MatchHistoryItem : ObservableUserControl
    {
        private string _victoryDefeatText;
        public string VictoryDefeatText
        {
            get => _victoryDefeatText;
            set
            {
                _victoryDefeatText = value;
                OnPropertyChanged();
            }
        }
        private SolidColorBrush _victoryDefeatTextColor;
        public SolidColorBrush VictoryDefeatTextColor
        {
            get => _victoryDefeatTextColor;
            set
            {
                _victoryDefeatTextColor = value;
                OnPropertyChanged();
            }
        }
        private ImageSource _agentIcon;
        public ImageSource AgentIcon
        {
            get => _agentIcon;
            set
            {
                _agentIcon = value;
                OnPropertyChanged();
            }
        }
        private ToolTip _agentIconTooltip;
        public ToolTip AgentIconTooltip
        {
            get => _agentIconTooltip;
            set
            {
                _agentIconTooltip = value;
                OnPropertyChanged();
            }
        }
        private string _mapName;
        public string MapName
        {
            get => _mapName;
            set
            {
                _mapName = value;
                OnPropertyChanged();
            }
        }
        private string _gameModeName;
        public string GameModeName
        {
            get => _gameModeName;
            set
            {
                _gameModeName = value;
                OnPropertyChanged();
            }
        }
        private string _rrEarned;
        public string RREarned
        {
            get => _rrEarned;
            set
            {
                _rrEarned = value;
                OnPropertyChanged();
            }
        }
        private string _blueTeamScore;
        public string BlueTeamScore
        {
            get => _blueTeamScore;
            set
            {
                _blueTeamScore = value;
                OnPropertyChanged();
            }
        }
        private string _redTeamScore;
        public string RedTeamScore
        {
            get => _redTeamScore;
            set
            {
                _redTeamScore = value;
                OnPropertyChanged();
            }
        }
        private string _kda;
        public string KDA
        {
            get => _kda;
            set
            {
                _kda = value;
                OnPropertyChanged();
            }
        }
        private string _kdRatio;
        public string KDRatio
        {
            get => _kdRatio;
            set
            {
                _kdRatio = value;
                OnPropertyChanged();
            }
        }
        private SolidColorBrush _kdRatioColor;
        public SolidColorBrush KDRatioColor
        {
            get => _kdRatioColor;
            set
            {
                _kdRatioColor = value;
                OnPropertyChanged();
            }
        }
        private ToolTip _matchDateTooltip;
        public ToolTip MatchDateTooltip
        {
            get => _matchDateTooltip;
            set
            {
                _matchDateTooltip = value;
                OnPropertyChanged();
            }
        }
        private string _matchDuration;
        public string MatchDuration
        {
            get => _matchDuration;
            set
            {
                _matchDuration = value;
                OnPropertyChanged();
            }
        }

        private bool _isRanked;
        public bool IsRanked
        {
            get => _isRanked;
            set
            {
                _isRanked = value;
                OnPropertyChanged();
            }
        }
        
        private readonly MatchData _match;
        private readonly CompetitiveUpdate _rankUpdate;
        private readonly UserData _userData;

        public MatchHistoryItem(MatchData match, CompetitiveUpdate rankUpdate, UserData userData)
        {
            _match = match;
            _rankUpdate = rankUpdate;
            _userData = userData;

            this.DataContext = this;

            InitializeComponent();

            IsRanked = rankUpdate != null;

            DecorateItem();
        }

        private void DecorateItem()
        {
            var numRoundsRedTeamWon = _match.RoundResults.Count(x => x.WinningTeam.Equals("Red"));
            var numRoundsBlueTeamWon = _match.RoundResults.Count(x => x.WinningTeam.Equals("Blue"));

            BlueTeamScore = $"{numRoundsBlueTeamWon}";
            RedTeamScore = $"{numRoundsRedTeamWon}";

            var gameDraw = numRoundsRedTeamWon == numRoundsBlueTeamWon;
            var redTeamWon = numRoundsRedTeamWon > numRoundsBlueTeamWon;

            var currentPlayer = _match.Players.First(x => x.Subject.Equals(_userData.RiotUserData.Puuid));

            var playersTeam = currentPlayer.TeamId;
            var agentId = currentPlayer.CharacterId;
            var agentName = ValorantConstants.AgentNameById[agentId];
            var cleanedAgentName = agentName.ReplaceAll(new[] { "/", ".", ",", ":", ";", "'", "\"", "\\", "@", "#", "~", "!", "?" }, "");
            AgentIcon = (ImageSource)Application.Current.TryFindResource($"{cleanedAgentName}Image");
            AgentIconTooltip = new ToolTip
            {
                Content = agentName,
                Placement = PlacementMode.Right
            };

            var mapAssetPath = _match.MatchInfo.MapId;
            var mapName = ValorantConstants.Maps.FirstOrDefault(x => x.AssetPath.Equals(mapAssetPath))?.Name ?? "Map Name";
            MapName = mapName;

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            GameModeName = textInfo.ToTitleCase(_match.MatchInfo.QueueID);

            if (IsRanked)
            {
                var rrEarned = _rankUpdate.RankedRatingEarned;
                switch (rrEarned)
                {
                    case 0:
                        RREarned = $"+/-{rrEarned} RR";
                        break;
                    case > 0:
                        RREarned = $"+{rrEarned} RR";
                        break;
                    case < 0:
                        RREarned = $"{rrEarned} RR";
                        break;
                }
            }

            var playerKills = currentPlayer.Stats.Kills;
            var playerDeaths = currentPlayer.Stats.Deaths;
            var playerAssists = currentPlayer.Stats.Assists;
            KDA = $"{playerKills} / {playerDeaths} / {playerAssists}";

            double kdRatio;
            if (playerDeaths == 0)
                kdRatio = playerKills;
            else
                kdRatio = playerKills / playerDeaths;

            KDRatio = $"{kdRatio:0.00}";
            switch (kdRatio)
            {
                case 1.0:
                    KDRatioColor = (SolidColorBrush)Application.Current.TryFindResource("MatchDrawBrush");
                    break;
                case > 1.0:
                    KDRatioColor = (SolidColorBrush)Application.Current.TryFindResource("MatchVictoryBrush");
                    break;
                case < 1.0:
                    KDRatioColor = (SolidColorBrush)Application.Current.TryFindResource("MatchDefeatBrush");
                    break;
            }

            var matchDate = DateTime.UnixEpoch.AddMilliseconds(_match.MatchInfo.GameStartMillis).ToLocalTime();
            MatchDateTooltip = new ToolTip
            {
                Content = new TextBlock
                {
                    Text = $"{matchDate.ToLongDateString()}\n{matchDate.ToShortTimeString()}",
                    TextAlignment = TextAlignment.Center
                },
                HorizontalContentAlignment = HorizontalAlignment.Center
            };

            var matchDuration = TimeSpan.FromMilliseconds(_match.MatchInfo.GameLengthMillis);
            MatchDuration = matchDuration.ToString(matchDuration.Hours > 0 ? @"hh\:mm\:ss" : @"mm\:ss");

            if (gameDraw)
            {
                // Draw

                this.Background = new LinearGradientBrush(new GradientStopCollection
                {
                    new((Color)Application.Current.TryFindResource("MatchDrawColor"), 0),
                    new((Color)Application.Current.TryFindResource("BackgroundColor"), 0.05)
                }, new Point(0, 0), new Point(1, 1));

                VictoryDefeatText = "Draw";
                VictoryDefeatTextColor = (SolidColorBrush)Application.Current.TryFindResource("MatchDrawBrush");
            }
            else switch (playersTeam)
            {
                case "Red" when redTeamWon:
                case "Blue" when !redTeamWon:

                    // Victory

                    this.Background = new LinearGradientBrush(new GradientStopCollection
                    {
                        new((Color)Application.Current.TryFindResource("MatchVictoryColor"), 0),
                        new((Color)Application.Current.TryFindResource("BackgroundColor"), 0.05)
                    }, new Point(0, 0), new Point(1, 1));

                    VictoryDefeatText = "Victory";
                    VictoryDefeatTextColor = (SolidColorBrush)Application.Current.TryFindResource("MatchVictoryBrush");
                    break;

                case "Red":
                case "Blue":

                    // Defeat

                    this.Background = new LinearGradientBrush(new GradientStopCollection
                    {
                        new((Color)Application.Current.TryFindResource("MatchDefeatColor"), 0),
                        new((Color)Application.Current.TryFindResource("BackgroundColor"), 0.05)
                    }, new Point(0, 0), new Point(1, 1));

                    VictoryDefeatText = "Defeat";
                    VictoryDefeatTextColor = (SolidColorBrush)Application.Current.TryFindResource("MatchDefeatBrush");
                    break;
            }
        }
    }
}
