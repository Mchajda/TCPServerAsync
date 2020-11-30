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
        int buffer_size = 1024;
        UserManager manager;
        User current_user;
        PasswordGenerator PasswordGenerator;
        StreamController StreamController;

        public delegate void TransmissionDataDelegate(NetworkStream stream);

        public ServerEchoAPM(IPAddress IP, int port) : base(IP, port)
        {
            manager = new UserManager();
            PasswordGenerator = new PasswordGenerator();
            StreamController = new StreamController();
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
                while (this.manager.session_is_logged != true)
                {
                    manager.readUsers();
                    try
                    {
                        switch (this.StreamController.ReadString(stream, buffer))
                        {
                            case "register":
                                //Rejestracja
                                Register(buffer, stream);
                                break;
                            case "login":
                                // LOGOWANIE
                                LogIn(buffer, stream);
                                break;
                            case "generate":
                                //Generowanie hasła
                                this.StreamController.SendString(PasswordGenerator.GeneratePassword(8), buffer, stream);
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
                        this.StreamController.SendString(exc.Message, buffer, stream);
                    }
                }

                while (this.manager.session_is_logged == true)
                {
                    try
                    {
                        string str = this.StreamController.ReadString(stream, buffer);
                        switch (str)
                        {
                            case "logout":
                                this.manager.session_is_logged = false;
                                current_user.unSetLogged();
                                break;
                            case "generate":
                                //Generowanie hasła
                                this.StreamController.SendString(PasswordGenerator.GeneratePassword(8), buffer, stream);
                                break;
                            case "change password":
                                ChangePassword(buffer, stream);
                                break;
                            case "change username":
                                ChangeUsername(buffer, stream);
                                break;
                            case "username":
                                this.StreamController.SendString(current_user.getLogin(), buffer, stream);
                                break;
                            default:
                                break;
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

        private void ChangeUsername(byte[] buffer, NetworkStream stream)
        {
            this.StreamController.SendString("oldpassword", buffer, stream);
            string oldpassword = this.StreamController.ReadString(stream, buffer);
            this.StreamController.SendString("newlogin", buffer, stream);
            string login = this.StreamController.ReadString(stream, buffer);
            string passwordCheck = oldpassword;

            if (this.current_user.getPassword() == oldpassword)
            {
                this.manager.changeLogin(current_user.getLogin(), login);
                throw new Exception("changed username");
            }
            else
            {
                throw new Exception("wrong password");
            }
        }

        private void ChangePassword(byte[] buffer, NetworkStream stream)
        {
            this.StreamController.SendString("oldpassword", buffer, stream);
            string oldpassword = this.StreamController.ReadString(stream, buffer);
            this.StreamController.SendString("newpassword", buffer, stream);
            string password = this.StreamController.ReadString(stream, buffer);
            this.StreamController.SendString("password confirm", buffer, stream);
            string passwordCheck = this.StreamController.ReadString(stream, buffer);

            if (this.current_user.getPassword() == oldpassword)
            {
                this.manager.changePassword(current_user.getLogin(), password, passwordCheck);
                throw new Exception("changed password");
            }
            else { 
                throw new Exception("wrong password");
            }                      
        }

        private void Register(byte[] buffer, NetworkStream stream)
        {
            this.StreamController.SendString("login", buffer, stream);
            string login = this.StreamController.ReadString(stream, buffer);
            this.StreamController.SendString("password", buffer, stream);
            string password = this.StreamController.ReadString(stream, buffer);
            this.StreamController.SendString("password confirm", buffer, stream);
            string passwordCheck = this.StreamController.ReadString(stream, buffer);

            this.manager.register(login, password, passwordCheck);
        }

        private void LogIn(byte[] buffer, NetworkStream stream)
        {
            this.StreamController.SendString("podaj login: ", buffer, stream);
            string login = this.StreamController.ReadString(stream, buffer);
            this.StreamController.SendString("podaj haslo: ", buffer, stream);
            string password = this.StreamController.ReadString(stream, buffer);
            //authorization
            this.current_user = this.manager.authorize(login, password, this.manager);
            if (!(manager.session_is_logged))
            {
                throw new Exception("login failed");
            }
            else
            {
                throw new Exception("login success");
            }
        }

        public override void Start()
        {
            StartListening();
            AcceptClient();
        }
    }
}