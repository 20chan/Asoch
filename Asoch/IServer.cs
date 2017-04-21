using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asoch
{
    public abstract class IServer
    {
        public const int BUFFER_SIZE = 2048;
        public const int LISTEN_BACKLOG = 20;

        public int Port { get; protected set; }
        public bool IsRunning { get; protected set; } = false;

        public IServer(int port)
        {
            Port = port;
        }
    }
}
