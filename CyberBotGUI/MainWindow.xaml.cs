using CyberBotGUI.Core;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CyberBotGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ChatEngine _engine = new ChatEngine();
        public MainWindow()
        {
            InitializeComponent();


            //Play original voice greeting on startup 

            ConsoleBridge.PlayGreeting();

            //Welcome Message on load
            AddBotBubble("Hello! I am CyberBot - your cybersecurity awareness assistant.\n\nWhat is your name?");
        }
        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SendButton_Click(object sender, RoutedEventArgs e)

            => SubmitMessage();


        private void InputBox_TextChange(object sender, TextChangedEventArgs e)

            => SendButton.IsEnabled = !string.IsNullOrWhiteSpace(InputBox.Text);

        private void SubmitMessage()
        {
            string userText = InputBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(userText)) return;

            AddUserBubble(userText);
            InputBox.Clear();
            SendButton.IsEnabled = false;

            string botText = _engine.ProcessInput(userText);
            AddBotBubble(botText);

            ChatScroll.ScrollToBottom();
        }

        private void AddUserBubble(string userText)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0, 80, 20)),
                CornerRadius = new CornerRadius(16, 16, 4, 16),
                Padding = new Thickness(12, 8, 12, 8),
                Margin = new Thickness(60, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 340

            };

            var textblock = new TextBlock
            {
                Text = userText,
                Foreground = new SolidColorBrush(Color.FromRgb(240, 246, 252)),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
            };

            border.Child = textblock;
            ChatPanel.Children.Add(border);
        }

        private void AddBotBubble(string botText)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(22, 27, 34)),
                CornerRadius = new CornerRadius(16, 16, 16, 4),
                Padding = new Thickness(12, 8, 12, 8),
                Margin = new Thickness(8, 4, 60, 4),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 340,
                BorderBrush = new SolidColorBrush(Color.FromRgb(48, 54, 61)),
                BorderThickness = new Thickness(1)
            };

            var textblock = new TextBlock
            {
                Text = botText,
                Foreground = new SolidColorBrush(Color.FromRgb(240, 246, 252)),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,

            };
            border.Child = textblock;
            ChatPanel.Children.Add(border);
        }
        
        }
    }

