using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Asoch
{
    public static class SocketTaskAsync
    {
        public static Task<Socket> AcceptTask(this Socket socket, Socket accept, int recv)
            => Task.Factory.FromAsync(socket.BeginAccept(accept, recv, null, null), socket.EndAccept);
        
        public static Task ConnectTask(this Socket socket, string host, int port)
            => Task.Factory.FromAsync(socket.BeginConnect(host, port, null, null), socket.EndConnect);

        public static Task DisConnectTask(this Socket socket, bool reuse)
            => Task.Factory.FromAsync(socket.BeginDisconnect(reuse, null, null), socket.EndDisconnect);

        public static Task<int> SendTask(this Socket socket, byte[] buffer, int offset, int size, SocketFlags flags)
            => Task.Factory.FromAsync(socket.BeginSend(buffer, offset, size, flags, null, socket), socket.EndSend);

        public static Task<int> ReceiveTask(this Socket socket, byte[] buffer, int offset, int size, SocketFlags flags)
            => Task.Factory.FromAsync(socket.BeginReceive(buffer, offset, size, flags, null, null), socket.EndReceive);
    }
}
