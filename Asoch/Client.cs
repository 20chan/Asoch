using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Asoch
{
    public class Client
    {
        #region Private members
        private Socket _socket;
        #endregion

        #region Properties
        public string Host { get; private set; }
        public int Port { get; private set; }

        public bool IsConnected { get; private set; } = false;
        #endregion
        
        public Client(string hostIP, int port)
        {
            Host = hostIP; Port = port;
        }
        
        public async Task<bool> Connect()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                await _socket.ConnectTaskAsync(new IPEndPoint(IPAddress.Parse(Host), Port));
                IsConnected = true;
            }
            catch(SocketException)
            {
                IsConnected = false;
            }
            return IsConnected;
        }
        
        public async Task<bool> Send(byte[] buffer, int offset, int size)
        {
            try
            {
                await _socket.SendTaskAsync(buffer, offset, size, SocketFlags.None);
                return true;
            }
            catch(SocketException)
            {
                return false;
            }
        }
    }

    public static class AsyncSocket
    {
        public static Task ConnectTaskAsync(this Socket socket, EndPoint endpoint)
        {
            return Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, endpoint, null);
        }

        public static Task<int> SendTaskAsync(this Socket socket, byte[] buffer, int offset, int size, SocketFlags flags)
        {
            // TODO: 도대체 씨발 뭐가 문제인지 찾기
            // return Task.Factory.FromAsync(() => socket.BeginSend(buffer, offset, size, flags), socket.EndSend, buffer, offset, size, flags, null);
            throw new NotImplementedException();
        }
    }
}
