using System.Threading.Tasks;
using System.Net.Sockets;

namespace Asoch
{
    public class Client : IClient
    {
        #region Private members
        private Socket _socket;
        #endregion

        public Client(string host, int port) : base(host, port)
        {

        }
        
        /// <summary>
        /// Connect to server
        /// </summary>
        /// <returns>Succeed of connection</returns>
        public async Task<bool> Connect()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                await _socket.ConnectTask(Host, Port);
                IsConnected = true;
            }
            catch(SocketException)
            {
                IsConnected = false;
            }
            return IsConnected;
        }

        /// <summary>
        /// Disconnect to server
        /// </summary>
        /// <param name="reuse"></param>
        /// <returns>Succeed of disconnection</returns>
        public async Task<bool> Disconnect(bool reuse)
        {
            try
            {
                await _socket.DisConnectTask(reuse);
                return true;
            }
            catch(SocketException)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<int> Send(byte[] buffer, int offset, int size)
        {
            try
            {
                return await _socket.SendTask(buffer, offset, size, SocketFlags.None);
            }
            catch(SocketException)
            {
                return -1;
            }
        }

        /// <summary>
        /// Receive data
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> Receive()
        {
            byte[] res = new byte[BUFFER_SIZE];
            await _socket.ReceiveTask(res, 0, BUFFER_SIZE, SocketFlags.None);
            return res;
        }
    }
}
