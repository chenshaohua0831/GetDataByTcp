using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int port = 8888;
            AsyncTcpServer server = new AsyncTcpServer(port);
            await  server.Start();
        }
    }
}
