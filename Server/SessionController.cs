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
            this.session_is_logged = false;
            this.session_admin = false;
        }

        public User getUser()
        {
            return this.current_user;
        }

        public void setUser(User user)
        {
            this.current_user = user;
        }

        public bool getLoginStatus()
        {
            return this.session_is_logged;
        }
        public bool getAdminStatus()
        {
            return this.session_admin;
        }

        public void setStatus(bool var)
        {
            this.session_is_logged = var;
        }

        public void setAdminStatus(bool var)
        {
            this.session_admin = var;
        }

        public bool authorize(string login, string password)
        {
            foreach (User user in this.UsersManager.getUsers())
            {
                if (user.getLogin() == login && user.getPassword() == password)
                {

                    this.setUser(new User(login, password));
                    this.setStatus(true);

                    if(user.getRole() == "ROLE_ADMIN")
                        this.session_admin = true;

                    return true;
                }
            }
            this.setStatus(false);
            return false;
        }

        public bool register(string login, string password, string passwordCheck)
        {
            bool is_valid_user = true;

            foreach (User user in this.UsersManager.getUsers())
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
                    if (this.UsersManager.insertRow(login, password, "ROLE_USER"))
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
                this.UsersManager.DBConnection.startConnection();

                MySqlCommand comm = this.UsersManager.DBConnection.connection.CreateCommand();
                string query = "UPDATE users SET password='" + password + "' WHERE username='" + login + "' ";
                comm = new MySqlCommand(query, this.UsersManager.DBConnection.connection);
                comm.ExecuteNonQuery();

                this.UsersManager.DBConnection.closeConnection();

                this.getUser().setPassword(password);
            }
        }

        public void addToFriends(string username)
        {
            this.UsersManager.DBConnection.startConnection();

            MySqlCommand comm = this.UsersManager.DBConnection.connection.CreateCommand();
            string query = "INSERT INTO friendships(first_user, second_user) VALUES('" + this.current_user.getLogin() + "', '" + username + "')";
            comm = new MySqlCommand(query, this.UsersManager.DBConnection.connection);
            comm.ExecuteNonQuery();

            this.UsersManager.DBConnection.closeConnection();
        }

        //admin methods
        public void deleteUser(string login)
        {
            this.UsersManager.DBConnection.startConnection();

            MySqlCommand comm = this.UsersManager.DBConnection.connection.CreateCommand();
            string query = "DELETE FROM users WHERE username='"+login+"' ";
            comm = new MySqlCommand(query, this.UsersManager.DBConnection.connection);
            comm.ExecuteNonQuery();

            this.UsersManager.DBConnection.closeConnection();
        }

        public void changeLogin(string login, string newlogin, string password)
        {
            if(password == current_user.getPassword())
            {
                this.UsersManager.DBConnection.startConnection();

                MySqlCommand comm = this.UsersManager.DBConnection.connection.CreateCommand();
                string query = "UPDATE users SET username='" + newlogin + "' WHERE username='" + login + "' ";
                comm = new MySqlCommand(query, this.UsersManager.DBConnection.connection);
                comm.ExecuteNonQuery();

                this.UsersManager.DBConnection.closeConnection();

                this.getUser().setLogin(newlogin);
            }
        }

        public void editUser(string login, string new_login, string new_password)
        {
            string query = "";
            if (new_login != "" && new_password != "")
            {
                query = "UPDATE users SET username='" + new_login + "', password='"+ new_password +"' WHERE username='" + login + "' ";
            }
            else if (new_login != "" && new_password == "")
            {
                query = "UPDATE users SET username='" + new_login + "' WHERE username='" + login + "' ";
            }
            else if (new_login == "" && new_password != "")
            {
                query = "UPDATE users SET password='" + new_password + "' WHERE username='" + login + "' ";
            }

            this.UsersManager.DBConnection.startConnection();

            MySqlCommand comm = this.UsersManager.DBConnection.connection.CreateCommand();
            
            comm = new MySqlCommand(query, this.UsersManager.DBConnection.connection);
            comm.ExecuteNonQuery();

            this.UsersManager.DBConnection.closeConnection();
        }
    }
}
