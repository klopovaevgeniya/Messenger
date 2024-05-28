using System.Windows;
using System.Windows.Controls;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        public enum UserType
        {
            Client,
            Host
        }
        
        public ChatWindow(UserType userType)
        {
            InitializeComponent();
            
            switch(userType)
            {
                case UserType.Client:
                    frame.Content = new ClientPage(frame);
                    break;
                case UserType.Host:
                    frame.Content = new HostPage(frame);
                    break;
                default: 
                    throw new ArgumentException();
            }
        }

        public void AddUser(string user)
        {
            var users = listBox.Items
                .Cast<string>()
                .Append(user)
                .ToList();
            listBox.Items.Clear();
            listBox.ItemsSource = users;
        }

        public void RemoveUser(string user)
        {
            var users = listBox.Items
                .Cast<string>()
                .Where(x => x != user)
                .ToList();
            listBox.Items.Clear();
            listBox.ItemsSource = users;
        }
    }
}
