using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public static string IpServer { get; private set; } = string.Empty;
        public static string Username { get; private set; } = string.Empty;

        public AuthorizationWindow()
        {
            InitializeComponent();
            ipServerTextBox.Text = "127.0.0.1";
        }

        private void createChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUsernameValid(usernameTextBox.Text))
            {
                MessageBox.Show("Некорректное имя пользователя");
                return;
            }

            Username = usernameTextBox.Text;
            WindowManager.Instance.OpenNewWindow(new ChatWindow(ChatWindow.UserType.Host), this);
        }

        private void connectToChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUsernameValid(usernameTextBox.Text))
            {
                MessageBox.Show("Некорректное имя пользователя");
                return;
            }
            if (!IsIpValid(ipServerTextBox.Text))
            {
                MessageBox.Show("Некорректный ip");
                return;
            }

            Username = usernameTextBox.Text;
            IpServer = ipServerTextBox.Text;
            WindowManager.Instance.OpenNewWindow(new ChatWindow(ChatWindow.UserType.Client), this);
        }

        private bool IsUsernameValid(string username) 
            => new Regex("^[A-Za-zА-Яа-яЁё][A-Za-z0-9А-Яа-яЁё]{2,15}$").IsMatch(username);
        private bool IsIpValid(string ip) 
            => new Regex("^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$").IsMatch(ip);
    }
}