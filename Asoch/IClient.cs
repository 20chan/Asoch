using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asoch
{
    public abstract class IClient
    {
        public const int BUFFER_SIZE = 2048;

        public string Host { get; protected set; }
        public int Port { get; protected set; }

        public bool IsConnected { get; protected set; } = false;
        
        public IClient(string hostIP, int port)
        {
            Host = hostIP; Port = port;
        }
    }
}
