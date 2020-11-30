using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class StreamController
    {
        public StreamController()
        {

        }

        public string ReadString(NetworkStream stream, byte[] buffer)
        {
            int buffer_size = 1024;
            int message_size = stream.Read(buffer, 0, buffer_size);
            return new ASCIIEncoding().GetString(buffer, 0, message_size);
        }

        public void SendString(string str, byte[] buffer, NetworkStream stream)
        {
            buffer = Encoding.ASCII.GetBytes(str);
            stream.Write(buffer, 0, str.Length);
        }
    }
}
