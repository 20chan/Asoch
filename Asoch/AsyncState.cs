using System.Net.Sockets;

namespace Asoch
{
    public class AsyncState
    {
        public byte[] Buffer;
        public Socket Socket;
        public AsyncState(int size)
        {
            Buffer = new byte[size];
        }
    }
}
