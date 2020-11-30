using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class ClientController
    {
        //User current_user;
        SessionController SessionController;
        public ClientController() 
        {
            SessionController = new SessionController();
        }

        public SessionController getSession()
        {
            return this.SessionController;
        }

        public void ChangeUsername(String oldpassword, String login)
        {
            System.Console.WriteLine(this.SessionController.getUser().getLogin());
            if (this.SessionController.getUser().getPassword() == oldpassword)
            {
                this.SessionController.changeLogin(SessionController.getUser().getLogin(), login);
                throw new Exception("changed username");
            }
            else
            {
                throw new Exception("wrong password");
            }
        }

        public void Register(String login, String password, String passwordCheck)
        {
            this.SessionController.register(login, password, passwordCheck);
        }

        public void ChangePassword(String oldpassword, String password, String passwordCheck)
        {
            if (this.SessionController.getUser().getPassword() == oldpassword)
            {
                this.SessionController.changePassword(this.SessionController.getUser().getLogin(), password, passwordCheck);
                throw new Exception("changed password");
            }
            else
            {
                throw new Exception("wrong password");
            }
        }

        public void LogIn(String login, String password)
        {
            this.SessionController.authorize(login, password);

            if (!(this.SessionController.getStatus()))
            {
                throw new Exception("login failed");
            }
            else
            {
                throw new Exception("login success");
            }
        }
    }
}
