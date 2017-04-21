using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Asoch.Sync
{
    public class Server : IServer
    {
        private Socket _socket;
        private Thread _acceptThread;
        public int MaxClients { get; set; }

        public List<Socket> Clients { get; private set; }
        public int ClientsCount => Clients.Count;

        public Server(int port, int maxClients = -1) : base(port)
        {
            MaxClients = maxClients;
        }

        public void Run()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, Port));
            _socket.Listen(LISTEN_BACKLOG);
            IsRunning = true;
        }

        public void StartAccepting()
        {

        }

        private void accept()
        {
            while (IsRunning)
            {
                Socket client = _socket.Accept();
                Clients.Add(client);
            }
        }
    }
}
