using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class ClientController
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

        public void ChangeUsername(String password, String new_login)
        {
            System.Console.WriteLine(this.SessionController.getUser().getLogin());
            if (this.SessionController.getUser().getPassword() == password)
            {
                this.SessionController.changeLogin(SessionController.getUser().getLogin(), new_login, password);
                throw new Exception("changed username");
            }
            else
            {
                throw new Exception("wrong password");
            }
        }

        public bool Register(String login, String password, String passwordCheck)
        {
            if (this.SessionController.register(login, password, passwordCheck))
                return true;
            else return false;
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

        public bool LogIn(String login, String password)
        {
            this.SessionController.authorize(login, password);

            if (!(this.SessionController.getLoginStatus()))
                throw new Exception("login failed");
            else throw new Exception("login success");
        }

        //admin methods
        public void ShowUsers()
        {

        }

        public void DeleteUser(string login)
        {
            if (this.SessionController.getAdminStatus() == true)
            {
                this.SessionController.deleteUser(login);
            }
        }

        public void EditUser(string login, string new_login, string new_password)
        {
            this.SessionController.editUser(login, new_login, new_password);
            throw new Exception("user data edited");
        }
    }
}
