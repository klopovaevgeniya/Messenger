using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Messenger
{
    class Server
    {
        private Socket socket;
        private List<Socket> clients = new List<Socket>();
        private List<string> userList = new List<string>();

        public Server()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1000);

            ListenToClients();
        }

        public async Task ListenToClients()
        {
            while (true)
            {
                var client = await socket.AcceptAsync();
                clients.Add(client);
                //SendMessage(client, "N");
                ReceiveMessage(client);
            }
        }

        public async Task ReceiveMessage(Socket client)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await client.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);

                foreach (var item in clients)
                {
                    SendMessage(item, message);
                }
            }
        }

        private async Task SendMessage(Socket client, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(bytes, SocketFlags.None); 
        }
    }
}
