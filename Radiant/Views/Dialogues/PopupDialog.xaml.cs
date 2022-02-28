using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Radiant.Interfaces;
using Radiant.Models;
using Radiant.Utils;

namespace Radiant.Views.Dialogues
{
    public partial class PopupDialog : ObservableUserControl
    {
        public RelayCommand<ICloseable> CloseCommand { get; }

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

        private string _dialogTitle;
        public string DialogTitle
        {
            get => _dialogTitle;
            set
            {
                _dialogTitle = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TextBlock> _messageItems = new();
        public ObservableCollection<TextBlock> MessageItems
        {
            get => _messageItems;
            set
            {
                _messageItems = value;
                OnPropertyChanged();
            }
        }

        public PopupDialog(AppConfig appConfig, string title, params string[] messages)
        {
            CloseCommand = new(_ => CloseDialog());

            this.DataContext = this;

            InitializeComponent();

            DialogTitle = title;

            switch (appConfig.Settings.SystemButtonsType)
            {
                case SystemButtonsType.Colored:
                    SystemButtonsStyle = Application.Current.TryFindResource("ColoredSystemButton") as Style;
                    break;
                case SystemButtonsType.Simple:
                    SystemButtonsStyle = Application.Current.TryFindResource("SimpleSystemButton") as Style;
                    break;
            }

            foreach (var message in messages)
            {
                var textBlock = new TextBlock
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    FontSize = (double)Application.Current.TryFindResource("LoginFormTextSize"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new(0, 5, 0, 5)
                };

                MessageItems.Add(textBlock);
            }
        }

        private void CloseDialog()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow?.CloseDialogs();
        }
    }
}
