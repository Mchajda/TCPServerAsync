using System;
using System.Collections;
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

        public SessionController getSession() //parametr username, żeby zwrócić sesję konkretnego użytkownika
        {
            return SessionController;
        }

        public void ChangeUsername(String password, String new_login)
        {
            System.Console.WriteLine(this.SessionController.getUser().getLogin());
            if (SessionController.getUser().getPassword() == password)
            {
                SessionController.changeLogin(SessionController.getUser().getLogin(), new_login, password);
                throw new Exception("changed username");
            }
            else
            {
                throw new Exception("wrong password");
            }
        }

        public bool Register(String login, String password, String passwordCheck)
        {
            if (SessionController.register(login, password, passwordCheck))
                return true;
            else return false;
        }

        public void ChangePassword(String oldpassword, String password, String passwordCheck)
        {
            if (SessionController.getUser().getPassword() == oldpassword)
            {
                SessionController.changePassword(SessionController.getUser().getLogin(), oldpassword, password);
                throw new Exception("changed password");
            }
            else
            {
                throw new Exception("wrong password");
            }
        }

        public bool LogIn(String login, String password)
        {
            // tworzenie nowej sesji

            SessionController.authorize(login, password);

            if (!(SessionController.getLoginStatus()))
                throw new Exception("login failed");
            else throw new Exception("login success");
        }

        //admin methods
        public void ShowUsers()
        {

        }

        public void DeleteUser(string login)
        {
            if (SessionController.getAdminStatus() == true)
            {
                SessionController.deleteUser(login);
            }
        }

        public void EditUser(string login, string new_login, string new_password)
        {
            SessionController.editUser(login, new_login, new_password);
            throw new Exception("user data edited");
        }

        public void AddFriend(string login) //parametr username, by dodać relację znajomości konkretnego użytkownika z podanym loginem
        {
            SessionController.addToFriends(login);
            throw new Exception("added friend");
        }

        public ArrayList getFriends(string username)
        {
            return SessionController.getFriends(username);
        }

        public void deleteFriend(string username, string friend)
        {
            SessionController.deleteFriend(username, friend);
            throw new Exception("friend deleted");
        }
    }
}
