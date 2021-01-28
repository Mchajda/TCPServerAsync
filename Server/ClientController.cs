using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class ClientController
    {
        //User current_user;
        Dictionary<string, SessionController> SessionControllers = new Dictionary<string, SessionController>();

        public ClientController()
        {
            SessionControllers["_anyone"] = new SessionController();
        }

        public SessionController getSession(String username) //parametr username, żeby zwrócić sesję konkretnego użytkownika
        {
            return SessionControllers[username];
        }

        public void ChangeUsername(String username, String password, String new_login)
        {
            Console.WriteLine(SessionControllers[username].getUser().getLogin());
            if (SessionControllers[username].getUser().getPassword() == password)
            {
                SessionControllers[username].changeLogin(username, new_login, password);
                throw new Exception("changed username");
            }
            else
            {
                throw new Exception("wrong password");
            }
        }

        public bool Register(String login, String password, String passwordCheck, String role)
        {
            if (SessionControllers["_anyone"].register(login, password, passwordCheck, role))
                return true;
            else return false;
        }

        public void ChangePassword(String username, String oldpassword, String password, String passwordCheck)
        {
            if (SessionControllers[username].getUser().getPassword() == oldpassword)
            {
                SessionControllers[username].changePassword(username, oldpassword, password);
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

            SessionControllers[login] = new SessionController();
            SessionControllers[login].authorize(login, password);

            if (!(SessionControllers[login].getLoginStatus()))
                throw new Exception("login failed");
            else throw new Exception("login success");
        }

        //admin methods
        public void ShowUsers()
        {

        }

        public void DeleteUser(string username, string login)
        {
            if (SessionControllers[username].getAdminStatus() == true)
            {
                SessionControllers[username].deleteUser(login);
                throw new Exception("user deleted");
            }
            else
            {
                throw new Exception("no permission");
            }
        }

        public void EditUser(string username, string login, string new_login, string new_password)
        {
            if (SessionControllers[username].getAdminStatus() == true)
            {
                SessionControllers[username].editUser(login, new_login, new_password);
                throw new Exception("user data edited");
            }
            else
            {
                throw new Exception("no permission");
            }
        }

        public void AddFriend(string username, string login) //parametr username, by dodać relację znajomości konkretnego użytkownika z podanym loginem
        {
            SessionControllers[username].addToFriends(login);
            throw new Exception("added friend");
        }

        public ArrayList getFriends(string username)
        {
            return SessionControllers[username].getFriends(username);
        }

        public void deleteFriend(string username, string friend)
        {
            SessionControllers[username].deleteFriend(username, friend);
            throw new Exception("friend deleted");
        }

        public void LogOut(string username)
        {
            SessionControllers.Remove(username);
        }
    }
}
