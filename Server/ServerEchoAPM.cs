﻿using System;
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
        PasswordGenerator PasswordGenerator;

        public delegate void TransmissionDataDelegate(NetworkStream stream);

        public ServerEchoAPM(IPAddress IP, int port) : base(IP, port)
        {
            manager = new UserManager();
            PasswordGenerator = new PasswordGenerator();
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
            //stream.ReadByte();
            //stream.ReadByte();
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
                    switch(ReadString(stream, buffer))
                    {
                        case "register":
                            //Rejestracja
                            Register(buffer, stream);
                            break;
                        case "login":
                            // LOGOWANIE
                            LogIn(buffer, stream);
                            break;
                        case "generuj":
                            SendString(PasswordGenerator.GeneratePassword(8), buffer, stream);
                            break;
                        default:
                            break;
                    }
                }
                catch (IOException e)
                {
                    break;
                }
                catch (Exception exc)
                {
                    SendString(exc.Message, buffer, stream);
                    if (exc.Message == "login success")
                        break;
                }
            }
            while (this.manager.session_is_logged == true)
            {
                try
                {
                    string str = ReadString(stream, buffer);
                    if (str.ToLower() == "logout")
                    {
                        this.manager.session_is_logged = false;
                    }
                    else if (str.ToLower() == "haslo")
                    {
                        SendString("nie pokaze lol ", buffer, stream);
                    }
                    /*
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
                    */
                }
                catch (IOException e)
                {
                    // e.Message;
                    break;
                }
            }
        }

        private void Register(byte[] buffer, NetworkStream stream)
        {
            SendString("login", buffer, stream);
            string login = ReadString(stream, buffer);
            SendString("password", buffer, stream);
            string password = ReadString(stream, buffer);
            SendString("password confirm", buffer, stream);
            string passwordCheck = ReadString(stream, buffer);

            this.manager.register(login, password, passwordCheck);
        }

        private void LogIn(byte[] buffer, NetworkStream stream)
        {
            SendString("podaj login: ", buffer, stream);
            string login = ReadString(stream, buffer);
            SendString("podaj haslo: ", buffer, stream);
            string password = ReadString(stream, buffer);
            //authorization
            current_user = this.manager.authorize(login, password, this.manager);
        }

        public override void Start()
        {
            StartListening();
            //transmission starts within the accept function
            AcceptClient();
        }
    }
}