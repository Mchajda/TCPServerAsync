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

                                this.ClientController.Register(login, password, passwordCheck, "ROLE_USER");
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

                            case "close":
                                {
                                    return;
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
                            case "delete_user":
                                {
                                    this.StreamController.SendString("login", buffer, stream);
                                    string login = this.StreamController.ReadString(stream, buffer);

                                    this.ClientController.DeleteUser(login);
                                    break;
                                }

                            case "edit_user":
                                {
                                    this.StreamController.SendString("login", buffer, stream);
                                    string login = this.StreamController.ReadString(stream, buffer);
                                    this.StreamController.SendString("new_login", buffer, stream);
                                    string new_login = this.StreamController.ReadString(stream, buffer);
                                    this.StreamController.SendString("new password", buffer, stream);
                                    string password = this.StreamController.ReadString(stream, buffer);
                                    this.StreamController.SendString("role", buffer, stream);
                                    string role = this.StreamController.ReadString(stream, buffer);

                                    this.ClientController.EditUser(login, new_login, password, role);
                                    break;
                                }

                            case "close":
                                {
                                    this.ClientController.getSession().LogOut();
                                    return;
                                }

                            case "add_user":
                                {
                                    this.StreamController.SendString("login", buffer, stream);
                                    string login = this.StreamController.ReadString(stream, buffer);
                                    this.StreamController.SendString("password", buffer, stream);
                                    string password = this.StreamController.ReadString(stream, buffer);
                                    this.StreamController.SendString("password confirm", buffer, stream);
                                    string passwordCheck = this.StreamController.ReadString(stream, buffer);
                                    this.StreamController.SendString("role", buffer, stream);
                                    string role = this.StreamController.ReadString(stream, buffer);

                                    this.ClientController.Register(login, password, passwordCheck, role);
                                    break;
                                }

                            case "is_admin":
                            {
                                    if(this.ClientController.getSession().session_admin)
                                        this.StreamController.SendString("true", buffer, stream);
                                    else
                                        this.StreamController.SendString("false", buffer, stream);
                                    break;
                            }                                

                            case "logout":
                            {
                                this.ClientController.getSession().LogOut();
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