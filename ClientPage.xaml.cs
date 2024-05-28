using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Messenger
{
    public partial class ClientPage : Page
    {
        private Client client;
        private Frame mainFrame;

        public ClientPage(Frame mainFrame)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;
            string ipServer = AuthorizationWindow.IpServer;
            string username = AuthorizationWindow.Username;

            client = new Client(ipServer, username, Dispatcher);
            client.NewMessageHasBeenReceived += ClientPage_NewMessageHasBeenReceived;
            client.UserHasBeenConnected += ClientPage_UserHasBeenConnected;
            client.UserHasBeenDisconnected += ClientPage_UserHasBeenDisconnected;

            Loaded += ClientPage_Loaded; // Добавляем обработчик события Loaded
        }

        private void ClientPage_Loaded(object sender, RoutedEventArgs e)
        {
            Unloaded += ClientPage_Unloaded; // Добавляем обработчик события Unloaded
        }

        private void ClientPage_Unloaded(object sender, RoutedEventArgs e)
        {
            client.Disconnect();
        }

        private void ClientPage_UserHasBeenDisconnected(object sender, string e)
        {
            Dispatcher.Invoke(() =>
            {
                if (mainFrame.Content is ChatWindow chatWindow)
                {
                    chatWindow.RemoveUser(e);
                }
            });
        }

        private void ClientPage_UserHasBeenConnected(object sender, string e)
        {
            Dispatcher.Invoke(() =>
            {
                if (mainFrame.Content is ChatWindow chatWindow)
                {
                    chatWindow.AddUser(e);
                }
            });
        }

        private void ClientPage_NewMessageHasBeenReceived(object sender, string e)
        {
            Dispatcher.Invoke(() =>
            {
                messagesListBox.Items.Add(e);
                messagesListBox.ScrollIntoView(e);
            });
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(messageTextBox.Text))
            {
                await client.SendMessage(messageTextBox.Text);
                messageTextBox.Text = string.Empty;
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            client.Disconnect();
            Window.GetWindow(this)?.Close();
        }
    }
}
