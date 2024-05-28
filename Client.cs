using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Messenger
{
    class Client
    {
        private Socket server;
        private Dispatcher dispatcher;
        private string username;
        private bool isConnected;

        public event EventHandler<string> NewMessageHasBeenReceived;
        public event EventHandler<string> UserHasBeenConnected;
        public event EventHandler<string> UserHasBeenDisconnected;

        public Client(string ipServer, string username, Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.username = username;
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Connect(ipServer, 8888);
            SendMessage("/connect " + username);
            isConnected = true;
            Task.Run(() => ReceiveMessages());
        }

        private void ReceiveMessages()
        {
            while (isConnected)
            {
                byte[] buffer = new byte[1024];
                int received = server.Receive(buffer);
                string message = Encoding.UTF8.GetString(buffer, 0, received).Trim('\0');

                if (!message.StartsWith("/"))
                {
                    dispatcher.Invoke(() => NewMessageHasBeenReceived?.Invoke(this, message));
                }
                else if (message.StartsWith("/connect "))
                {
                    string newUser = message.Substring(9);
                    dispatcher.Invoke(() => UserHasBeenConnected?.Invoke(this, newUser));
                }
                else if (message.StartsWith("/disconnect "))
                {
                    string disconnectedUser = message.Substring(12);
                    dispatcher.Invoke(() => UserHasBeenDisconnected?.Invoke(this, disconnectedUser));
                }
                else if (message == "/disconnect")
                {
                    Disconnect();
                    break;
                }
            }
        }

        public void Disconnect()
        {
            if (isConnected)
            {
                isConnected = false;
                SendMessage("/disconnect");
                server.Close();
                dispatcher.Invoke(() => UserHasBeenDisconnected?.Invoke(this, username));
            }
        }

        public async Task SendMessage(string message)
        {
            if (isConnected)
            {
                if (message == "/disconnect")
                {
                    Disconnect();
                    return;
                }

                message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}";
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await server.SendAsync(buffer, SocketFlags.None);
            }
        }
    }
}
// бла бла это проверка что я не дундук