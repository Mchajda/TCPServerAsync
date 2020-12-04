using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Server
{
    public class ServerEchoAPM : ServerEcho
    {
        PasswordGenerator PasswordGenerator;
        StreamController StreamController;
        ClientController ClientController;
        DBConnection DBConnection;

        public delegate void TransmissionDataDelegate(NetworkStream stream);

        public ServerEchoAPM(IPAddress IP, int port) : base(IP, port)
        {
            PasswordGenerator = new PasswordGenerator();
            StreamController = new StreamController();
            ClientController = new ClientController();
            DBConnection = new DBConnection("localhost", "3306", "root", "", "imgo");
            DBConnection.startConnection();
            DBConnection.fetchData();
        }

        protected override void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = TcpListener.AcceptTcpClient();
                Stream = tcpClient.GetStream();
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);

                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, tcpClient);
            }
        }

        private void TransmissionCallback(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            DBConnection.closeConnection();
            client.Close();
        }

        protected override void BeginDataTransmission(NetworkStream stream)
        {
            byte[] buffer = new byte[Buffer_size];

            while (true)
            {
                while (this.ClientController.getSession().getStatus() != true)
                {
                    try
                    {
                        switch (this.StreamController.ReadString(stream, buffer))
                        {
                            case "register":
                            {
                                //Rejestracja
                                this.StreamController.SendString("login", buffer, stream);
                                string login = this.StreamController.ReadString(stream, buffer);
                                this.StreamController.SendString("password", buffer, stream);
                                string password = this.StreamController.ReadString(stream, buffer);
                                this.StreamController.SendString("password confirm", buffer, stream);
                                string passwordCheck = this.StreamController.ReadString(stream, buffer);

                                this.ClientController.Register(login, password, passwordCheck);
                                break;
                            }
                                
                            case "login":
                            {
                                // LOGOWANIE
                                this.StreamController.SendString("podaj login: ", buffer, stream);
                                string loginl = this.StreamController.ReadString(stream, buffer);
                                this.StreamController.SendString("podaj haslo: ", buffer, stream);
                                string passwordl = this.StreamController.ReadString(stream, buffer);

                                this.ClientController.LogIn(loginl, passwordl);
                                break;
                            }
                                
                            case "generate":
                            {
                                //Generowanie hasła
                                this.StreamController.SendString(PasswordGenerator.GeneratePassword(8), buffer, stream);
                                break;
                            }

                            default:
                            {
                                break;
                            }
                                
                        }
                    }
                    catch (IOException e)
                    {
                        break;
                    }
                    catch (Exception exc)
                    {
                        this.StreamController.SendString(exc.Message, buffer, stream);
                    }
                }

                while (this.ClientController.getSession().getStatus() == true)
                {
                    try
                    {
                        string str = this.StreamController.ReadString(stream, buffer);
                        switch (str)
                        {
                            case "logout":
                            {
                                this.ClientController.getSession().setStatus(false);
                                break;
                            }
                                
                            case "generate":
                            {
                                //Generowanie hasła
                                this.StreamController.SendString(PasswordGenerator.GeneratePassword(8), buffer, stream);
                                break;
                            }
                                
                            case "change password":
                            {
                                this.StreamController.SendString("oldpassword", buffer, stream);
                                string oldpassword = this.StreamController.ReadString(stream, buffer);
                                this.StreamController.SendString("newpassword", buffer, stream);
                                string password = this.StreamController.ReadString(stream, buffer);
                                this.StreamController.SendString("password confirm", buffer, stream);
                                string passwordCheck = this.StreamController.ReadString(stream, buffer);

                                this.ClientController.ChangePassword(oldpassword, password, passwordCheck);
                                break;
                            }

                            case "change username":
                            {
                                this.StreamController.SendString("oldpassword", buffer, stream);
                                string oldpassword1 = this.StreamController.ReadString(stream, buffer);
                                this.StreamController.SendString("newlogin", buffer, stream);
                                string login = this.StreamController.ReadString(stream, buffer);

                                this.ClientController.ChangeUsername(oldpassword1, login);
                                break;
                            }
                                
                            case "username":
                            {
                                this.StreamController.SendString(this.ClientController.getSession().getUser().getLogin(), buffer, stream);
                                break;
                            }

                            default:
                            {
                                break;
                            }
                                
                        }
                    }
                    catch (IOException e)
                    {
                        // e.Message;
                        break;
                    }
                    catch (Exception exc)
                    {
                        this.StreamController.SendString(exc.Message, buffer, stream);
                    }
                }
            }
        }

        public override void Start()
        {
            StartListening();
            AcceptClient();
        }
    }
}