using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using Radiant.Interfaces;
using Radiant.Models.AppConfigs;
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

        private ObservableCollection<FrameworkElement> _frameworkItems = new();
        public ObservableCollection<FrameworkElement> FrameworkItems
        {
            get => _frameworkItems;
            set
            {
                _frameworkItems = value;
                OnPropertyChanged();
            }
        }

        public PopupDialog(AppConfig appConfig, string title, IEnumerable<string> messages, IEnumerable<FrameworkElement> elements = null)
        {
            CloseCommand = new(_ => CloseDialog());

            this.DataContext = this;

            InitializeComponent();

            DialogTitle = title;

            switch (appConfig.Settings.SystemButtons)
            {
                case SystemButtons.Colored:
                    SystemButtonsStyle = Application.Current.TryFindResource("ColoredSystemButton") as Style;
                    break;
                case SystemButtons.Simple:
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
                    Foreground = Application.Current.TryFindResource("Text") as SolidColorBrush,
                    FontFamily = Application.Current.TryFindResource("RobotoSlabRegular") as FontFamily,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new(0, 5, 0, 5)
                };

                MessageItems.Add(textBlock);
            }

            if (elements != null)
            {
                foreach (var element in elements)
                {
                    FrameworkItems.Add(element);
                }
            }
        }

        private void CloseDialog()
        {
            try
            {
                if (DialogHost.IsDialogOpen("AddAccountDialogHost"))
                {
                    DialogHost.Close("AddAccountDialogHost");
                    return;
                }
            }
            catch
            {
                // ignored, if dialog host does not exist yet, an exception is thrown.
            }

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow?.CloseDialogs();
        }
    }
}
