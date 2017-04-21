using System.Net;
using System.Net.Sockets;

namespace Asoch.Sync
{
    public class Client : IClient
    {
        private Socket _socket;

        public Client(string host, int port) : base(host, port)
        {

        }

        public bool Connect()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                _socket.Connect(Host, Port);
                IsConnected = true;
            }
            catch (SocketException)
            {
                IsConnected = false;
            }
            return IsConnected;
        }

        public void Disconnect(bool reuse)
        {
            _socket.Disconnect(reuse);
        }

        public void Send(byte[] buffer, int offset, int size)
        {
            _socket.Send(buffer, offset, size, SocketFlags.None);
        }

        public byte[] Receive()
        {
            byte[] res = new byte[BUFFER_SIZE];
            _socket.Receive(res);
            return res;
        }
    }
}
