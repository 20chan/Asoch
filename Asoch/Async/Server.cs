using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Asoch
{
    public class Server : IServer
    {
        private class StateObject
        {
            public Socket Sock = null;
            public byte[] Buffer = new byte[BUFFER_SIZE];
        }

        static bool IsSocketConnected(Socket s)
        {
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
        }

        private Socket _socket;
        private List<StateObject> _clients;

        public Server(int port) : base(port)
        {
            _clients = new List<StateObject>();
        }

        public void Run()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, Port));
            _socket.Listen(LISTEN_BACKLOG);
            _socket.BeginAccept(new AsyncCallback(acceptCallback), _socket);
        }
       
        private void acceptCallback(IAsyncResult res)
        {
            Socket listener = res.AsyncState as Socket;
            if(listener != null)
            {
                Socket handler = listener.EndAccept(res);
                StateObject o = new StateObject();
                o.Sock = handler;
                _clients.Add(o);
                handler.BeginReceive(o.Buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(receiveCallback), o);
            }
        }

        private void receiveCallback(IAsyncResult res)
        {
            StateObject state = res.AsyncState as StateObject;
            if(IsSocketConnected(state.Sock))
            {
                _clients.Remove(state);
            }
        }

        private void Receive(object sender, SocketAsyncEventArgs e)
        {
            byte[] res = new byte[BUFFER_SIZE];
            Task.Factory.FromAsync(_socket.BeginReceive(res, 0, BUFFER_SIZE, SocketFlags.None, Receive, null),
                _socket.EndReceive);
        }

        public async Task<byte[]> Receive()
        {
            byte[] res = new byte[BUFFER_SIZE];
            await _socket.ReceiveTask(res, 0, BUFFER_SIZE, SocketFlags.None);
            return res;
        }
    }
}
