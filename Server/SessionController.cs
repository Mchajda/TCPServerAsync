using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class SessionController
    {
        User current_user;
        UsersManager UsersManager;
        public bool session_is_logged, session_admin;

        public SessionController()
        {
            UsersManager = new UsersManager();
            session_is_logged = false;
            session_admin = false;
        }

        public User getUser()
        {
            return current_user;
        }

        public void setUser(User user)
        {
            current_user = user;
        }

        public bool getLoginStatus()
        {
            return session_is_logged;
        }
        public bool getAdminStatus()
        {
            return session_admin;
        }

        public void setStatus(bool var)
        {
            session_is_logged = var;
        }

        public void setAdminStatus(bool var)
        {
            session_admin = var;
        }

        public bool authorize(string login, string password)
        {
            foreach (User user in UsersManager.getUsers())
            {
                if (user.getLogin() == login && user.getPassword() == password)
                {

                    setUser(new User(login, password));
                    setStatus(true);

                    if(user.getRole() == "ROLE_ADMIN")
                        session_admin = true;

                    return true;
                }
            }
            setStatus(false);
            return false;
        }

        public bool register(string login, string password, string passwordCheck)
        {
            bool is_valid_user = true;

            foreach (User user in UsersManager.getUsers())
            {
                if (user.getLogin() == login)
                {
                    is_valid_user = false;
                    throw new Exception("user exists");
                }
            }

            if (is_valid_user == true)
            {
                if (password == passwordCheck)
                {
                    if (UsersManager.insertRow(login, password, "ROLE_USER"))
                        return true;
                    else return false;

                    throw new Exception("registration successful");
                }
                else throw new Exception("podane hasla sie nie zgadzaja");
            }
            else throw new Exception("istnieje użytkownik o podanym loginie");
        }

        public void changePassword(string login, string password, string passwordCheck)
        {
            if (password == current_user.getPassword())
            {
                string query = "UPDATE users SET password='" + password + "' WHERE username='" + login + "' ";
                UsersManager.DBConnection.processQuery(query);
                getUser().setPassword(password);
            }
        }

        public void addToFriends(string username)
        {
            string query = "INSERT INTO friendships(first_user, second_user) VALUES('" + current_user.getLogin() + "', '" + username + "')";
            UsersManager.DBConnection.processQuery(query);
        }

        public void deleteFriend(string username, string friend)
        {
            string query = "DELETE FROM friendships WHERE first_user='" + username + "' AND second_user='" + friend + "'";
            UsersManager.DBConnection.processQuery(query);
        }

        //admin methods
        public void deleteUser(string login)
        {
            string query = "DELETE FROM users WHERE username='" + login + "' ";
            UsersManager.DBConnection.processQuery(query);
        }

        public void changeLogin(string login, string newlogin, string password)
        {
            if(password == current_user.getPassword())
            {
                string query = "UPDATE users SET username='" + newlogin + "' WHERE username='" + login + "' ";
                UsersManager.DBConnection.processQuery(query);
                getUser().setLogin(newlogin);
            }
        }

        public void editUser(string login, string new_login, string new_password)
        {
            if (!UsersManager.checkIfExists(new_login))
            {
                string query = "";
                if (new_login != "" && new_password != "")
                {
                    query = "UPDATE users SET username='" + new_login + "', password='" + new_password + "' WHERE username='" + login + "' ";
                }
                else if (new_login != "" && new_password == "")
                {
                    query = "UPDATE users SET username='" + new_login + "' WHERE username='" + login + "' ";
                }
                else if (new_login == "" && new_password != "")
                {
                    query = "UPDATE users SET password='" + new_password + "' WHERE username='" + login + "' ";
                }

                UsersManager.DBConnection.processQuery(query);
            }
            else throw new Exception("user already exists");
        }

        public ArrayList getFriends(string username)
        {
            return UsersManager.getFriends(username);
        }
    }
}
