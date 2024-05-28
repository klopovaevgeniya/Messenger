using System.Windows;
using System.Windows.Controls;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для HostPage.xaml
    /// </summary>
    public partial class HostPage : Page
    {
        private Server server;
        private Client client;
        private Frame mainFrame;
        public HostPage(Frame mainFrame)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;

            server = new Server();
            client = new Client(AuthorizationWindow.IpServer);

            client.NewMessageHasBeenReceived += HostPage_NewMessageHasBeenReceived;
            client.UserHasBeenConnected += HostPage_UserHasBeenConnected;
            client.UserHasBeenDisconnected += HostPage_UserHasBeenDisconnected;
        }

        private void HostPage_UserHasBeenDisconnected(object? sender, string e)
        {
            var mainWindow = WindowManager.Instance.GetWindowByFrame(mainFrame);
            if (mainWindow != null && mainWindow is ChatWindow chatWindow)
            {
                chatWindow.RemoveUser(e);
            }
        }

        private void HostPage_UserHasBeenConnected(object? sender, string e)
        {
            var mainWindow = WindowManager.Instance.GetWindowByFrame(mainFrame);
            if (mainWindow != null && mainWindow is ChatWindow chatWindow)
            {
                chatWindow.AddUser(e);
            }
        }

        private void HostPage_NewMessageHasBeenReceived(object? sender, string e) 
            => messagesListBox.Items.Add(e);

        private async void sendButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await client.SendMessage(messageTextBox.Text);
            messageTextBox.Text = string.Empty;
        }

        private void exitButton_Click(object sender, System.Windows.RoutedEventArgs e)
            => WindowManager.Instance.CloseWindowByFrame(mainFrame);
    }
}
