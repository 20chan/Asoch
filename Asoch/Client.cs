using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Asoch
{
    public class Client
    {
        #region Private members
        private ManualResetEvent _connectEvent;
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
            _connectEvent = new ManualResetEvent(false);
        }

        #region Connect
        public bool Connect()
        {
            _connectEvent.Reset();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _socket.BeginConnect(Host, Port, new AsyncCallback(ConnectCallback), _socket);
            _connectEvent.WaitOne();
            return IsConnected;
        }

        private void ConnectCallback(IAsyncResult res)
        {
            try
            {
                Socket client = res.AsyncState as Socket;
                client.EndConnect(res);
            }
            catch(SocketException)
            {
                IsConnected = false;
            }
            finally
            {
                _connectEvent.Set();
            }
        }

        public void DisConnect()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
        #endregion

        #region Send

        #endregion

        #region Receive
        #endregion
    }
}
