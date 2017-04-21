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
        public class StateObject
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

        public event Action<StateObject, byte[]> Received;

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
                state.Sock.Close();
            }

            int read = state.Sock.EndReceive(res);
            if (read > 0)
            {
                byte[] result = new byte[read];
                Array.Copy(state.Buffer, result, read);
                Received?.Invoke(state, result);

                state.Sock.BeginReceive(state.Buffer, 0, BUFFER_SIZE, 0, new AsyncCallback(receiveCallback), state);
            }
        }

        public void Send(StateObject client, byte[] data)
        {
            client.Sock.BeginSend(data, 0, data.Length, SocketFlags.None
                , new AsyncCallback(sendCallback), client);
        }

        private void sendCallback(IAsyncResult res)
        {
            StateObject state = res.AsyncState as StateObject;
            state.Sock.EndSend(res);
        }
    }
}
