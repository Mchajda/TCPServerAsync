using System;
using System.Collections;
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

        public delegate void TransmissionDataDelegate(NetworkStream stream);

        public ServerEchoAPM(IPAddress IP, int port) : base(IP, port)
        {
            PasswordGenerator = new PasswordGenerator();
            StreamController = new StreamController();
            ClientController = new ClientController();
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
            client.Close();
        }

        protected override void BeginDataTransmission(NetworkStream stream)
        {
            byte[] buffer = new byte[Buffer_size];

            while (true)
            {
                try
                {
                    //if (!ClientController.getSession().getLoginStatus())

                    string username = StreamController.ReadString(stream, buffer);
                    StreamController.SendString("StreamOpened", buffer, stream);

                    switch (StreamController.ReadString(stream, buffer))
                    {
                        case "register":
                            {
                                //Rejestracja użytkownika
                                StreamController.SendString("login", buffer, stream);
                                string login = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("password", buffer, stream);
                                string password = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("password confirm", buffer, stream);
                                string passwordCheck = StreamController.ReadString(stream, buffer);

                                ClientController.Register(login, password, passwordCheck);
                                break;
                            }

                        case "login":
                            {
                                //LOGOWANIE - tworzenie nowej sesji
                                StreamController.SendString("podaj login: ", buffer, stream);
                                string loginl = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("podaj haslo: ", buffer, stream);
                                string passwordl = StreamController.ReadString(stream, buffer);

                                ClientController.LogIn(loginl, passwordl);
                                break;
                            }

                        case "close":
                            {
                                return;
                            }

                        case "add a friend":
                            {
                                //DODAWANIE ZNAJOMYCH - nazwa zalogowanego użytkownika + login znajomego

                                StreamController.SendString("login", buffer, stream);
                                string login = StreamController.ReadString(stream, buffer);

                                ClientController.AddFriend(login);
                                break;
                            }

                        case "friends":
                            {
                                ArrayList friends = ClientController.getFriends(ClientController.getSession().getUser().getLogin()); //parametr username

                                for (int i = 0; i < friends.Count; i++)
                                {
                                    StreamController.SendString(friends[i].ToString(), buffer, stream);
                                    StreamController.ReadString(stream, buffer);
                                }

                                throw new Exception("_");
                            }

                        case "delete friend":
                            {
                                StreamController.SendString("friend", buffer, stream);
                                string friend = StreamController.ReadString(stream, buffer);

                                ClientController.deleteFriend(ClientController.getSession().getUser().getLogin(), friend);
                                break;
                            }

                        case "delete_user":
                            {
                                StreamController.SendString("login", buffer, stream);
                                string login = StreamController.ReadString(stream, buffer);
                                ClientController.DeleteUser(login);
                                break;
                            }

                        case "edit_user":
                            {
                                StreamController.SendString("login", buffer, stream);
                                string login = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("new_login", buffer, stream);
                                string new_login = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("new password", buffer, stream);
                                string password = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("role", buffer, stream);
                                string role = StreamController.ReadString(stream, buffer);

                                ClientController.EditUser(login, new_login, password);
                                break;
                            }

                        case "add_user":
                            {
                                StreamController.SendString("login", buffer, stream);
                                string login = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("password", buffer, stream);
                                string password = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("password confirm", buffer, stream);
                                string passwordCheck = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("role", buffer, stream);
                                string role = StreamController.ReadString(stream, buffer);

                                ClientController.Register(login, password, passwordCheck);
                                break;
                            }

                        case "is_admin":
                            {
                                if (ClientController.getSession().session_admin)
                                    throw new Exception("true");
                                else
                                    throw new Exception("false");
                            }

                        case "logout":
                            {
                                ClientController.getSession().session_is_logged = false;
                                break;
                            }

                        case "generate":
                            {
                                //Generowanie hasła
                                throw new Exception(PasswordGenerator.GeneratePassword(8));
                            }

                        case "change password":
                            {
                                StreamController.SendString("oldpassword", buffer, stream);
                                string oldpassword = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("newpassword", buffer, stream);
                                string password = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("password confirm", buffer, stream);
                                string passwordCheck = StreamController.ReadString(stream, buffer);

                                ClientController.ChangePassword(oldpassword, password, passwordCheck);
                                break;
                            }

                        case "change username":
                            {
                                StreamController.SendString("oldpassword", buffer, stream);
                                string oldpassword1 = StreamController.ReadString(stream, buffer);
                                StreamController.SendString("newlogin", buffer, stream);
                                string login = StreamController.ReadString(stream, buffer);

                                ClientController.ChangeUsername(oldpassword1, login);
                                break;
                            }

                        case "check password":
                            {
                                throw new Exception(ClientController.getSession().getUser().getPassword());
                            }

                        default:
                            {
                                break;
                            }

                    }
                }
                catch (Exception exc)
                {
                    StreamController.SendString(exc.Message, buffer, stream);
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