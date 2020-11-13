using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Server
{
    public class ServerEchoAPM : ServerEcho
    {
        //string message = "podaj login: ";
        int buffer_size = 1024;
        UserManager manager;
        User current_user;

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
            TcpClient client = (TcpClient)ar.AsyncState;
            client.Close();
        }

        protected override string ReadString(NetworkStream stream, byte[] buffer)
        {
            int message_size = stream.Read(buffer, 0, buffer_size);
            stream.ReadByte();
            stream.ReadByte();
            return new ASCIIEncoding().GetString(buffer, 0, message_size);
        }

        protected override void SendString(string str, byte[] buffer, NetworkStream stream)
        {
            buffer = Encoding.ASCII.GetBytes(str);
            stream.Write(buffer, 0, str.Length);
        }

        protected override void BeginDataTransmission(NetworkStream stream)
        {
            byte[] buffer = new byte[Buffer_size];
         
            while (this.manager.session_is_logged != true)
            {
                try
                {
                    // LOGOWANIE
                    SendString("podaj login: ", buffer, stream);
                    string login = ReadString(stream, buffer);
                    SendString("podaj haslo: ", buffer, stream);
                    string password = ReadString(stream, buffer);
                    //authorization
                    current_user = this.manager.authorize(login, password, this.manager);
                    

                    /*Rejestracja
                    SendString("podaj login: ", buffer, stream);
                    string login = ReadString(stream, buffer);
                    SendString("podaj haslo: ", buffer, stream);
                    string password = ReadString(stream, buffer);
                    SendString("podaj ponownie haslo: ", buffer, stream);
                    string passwordCheck = ReadString(stream, buffer);

                    this.manager.register(login, password, passwordCheck);
                    */

                }
                catch (IOException e)
                {
                    break;
                }
            }
            while (this.manager.session_is_logged == true)
            {
                try
                {
                    SendString("Witaj "+this.current_user.getLogin()+"\r\n", buffer, stream);
                    SendString("Wpisz logout aby wyjsc / wpisz haslo aby sprawdzic swoje haslo\r\n", buffer, stream);
                    string str = ReadString(stream, buffer);
                    if (str.ToLower() == "logout")
                    {
                        break;
                    }
                    else if (str.ToLower() == "haslo")
                    {
                        SendString("nie pokaze lol ", buffer, stream);
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