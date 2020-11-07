using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ServerEchoAPM : ServerEcho
    {
        //string message = "podaj login: ";
        int buffer_size = 1024;
        UserManager manager;

        public delegate void TransmissionDataDelegate(NetworkStream stream);

        public ServerEchoAPM(IPAddress IP, int port) : base(IP, port)
        {
            manager = new UserManager();
        }

        protected override void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = TcpListener.AcceptTcpClient();
                Stream = tcpClient.GetStream();
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);

                //callback style

                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, tcpClient);

                // async result style
                //IAsyncResult result = transmissionDelegate.BeginInvoke(Stream, null, null);
                ////operacje......
                //while (!result.IsCompleted) ;
                ////sprzątanie
            }
        }

        private void TransmissionCallback(IAsyncResult ar)
        {
            // sprzątanie
        }

        private string ReadString(NetworkStream stream, byte[] buffer)
        {
            int message_size = stream.Read(buffer, 0, buffer_size);
            stream.ReadByte();
            stream.ReadByte();
            return new ASCIIEncoding().GetString(buffer, 0, message_size);
        }

        private void sendString(string str, byte[] buffer, NetworkStream stream)
        {
            buffer = Encoding.ASCII.GetBytes(str);
            stream.Write(buffer, 0, str.Length);
        }

        protected override void BeginDataTransmission(NetworkStream stream)
        {
            byte[] buffer = new byte[Buffer_size];

            while (manager.getIsLogged() != true)
            {
                try
                {
                    sendString("podaj login: ", buffer, stream);
                    string login = ReadString(stream, buffer);

                    if (login == manager.getLogin())
                    {
                        sendString("podaj haslo: ", buffer, stream);
                        string password = ReadString(stream, buffer);

                        if (password == manager.getPassword())
                        {
                            sendString("Zostales poprawnie zalogowany\r\n", buffer, stream);
                            manager.setLogged();

                            string json;

                            break;
                        }
                        else
                        {
                            sendString("Niepoprawne haslo\r\n", buffer, stream);
                            continue;
                        }
                    }
                    else
                    {
                        sendString("Niepoprawny login\r\n", buffer, stream);
                        continue;
                    }
                }
                catch (IOException e)
                {
                    break;
                }
            }
            while (manager.getIsLogged() == true)
            {
                try
                {
                    sendString("Wpisz logout aby wyjsc / wpisz haslo aby sprawdzic swoje haslo\r\n", buffer, stream);
                    string str = ReadString(stream, buffer);
                    if (str.ToLower() == "logout")
                    {
                        break;
                    }
                    else if(str.ToLower() == "haslo")
                    {
                        sendString("twoje haslo to: " + manager.getPassword(), buffer, stream);
                    }
                }
                catch (IOException e)
                {
                    // e.Message;
                    break;
                }
            }
        }
        public override void Start()
        {
            StartListening();
            //transmission starts within the accept function
            AcceptClient();
        }
    }
}