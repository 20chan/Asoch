using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Asoch
{
    public class Server
    {
        public const int BUFFER_SIZE = 2048;
        public const int LISTEN_BACKLOG = 20;
        private Socket _socket;
        private List<Socket> _clients;

        public int Port { get; private set; }

        public Server(int port)
        {
            Port = port;
            _clients = new List<Socket>();
        }

        public async void Run()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, Port));
            _socket.Listen(LISTEN_BACKLOG);
            Accept();
        }
        
        private async void Accept()
        {
            await _socket.AcceptTask(null, BUFFER_SIZE);
        }

        private void Receive(object sender, SocketAsyncEventArgs e)
        {
            byte[] res = new byte[BUFFER_SIZE];
            Task.Factory.FromAsync(_socket.BeginReceive(res, 0, BUFFER_SIZE, SocketFlags.None, Receive, null),
                _socket.EndReceive);
        }

        public async Task<byte[]> Receive()
        {
            await _socket.ReceiveTask(res, 0, BUFFER_SIZE, SocketFlags.None);
            return res;
        }
    }
}
