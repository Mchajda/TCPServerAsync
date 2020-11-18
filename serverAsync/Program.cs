using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace serverAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerEchoAPM server = new ServerEchoAPM(new System.Net.IPAddress(new byte[] { 127, 0, 0, 1 }), 2311);
            server.Start();
        }
    }
}
