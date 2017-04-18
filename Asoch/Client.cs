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
        public async Task<bool> DisConnect(bool reuse)
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
    }
}
