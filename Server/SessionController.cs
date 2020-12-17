﻿using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class SessionController
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

        public bool getStatus()
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

        public void authorize(string login, string password)
        {
            foreach (User user in this.UsersManager.getUsers())
            {
                if (user.getLogin() == login && user.getPassword() == password)
                {
                    User current_user = new User(login, password);
                    this.setUser(current_user);
                    this.setStatus(true);

                    if(user.getRole() == "ROLE_ADMIN")
                    {
                        this.session_admin = true;
                    }

                    break;
                }
                else
                {
                    this.setStatus(false);
                }
            }
        }

        public void register(string login, string password, string passwordCheck)
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
                    User newUser = new User(login, password, "ROLE_USER");
                    
                    this.UsersManager.insertRow(login, password, "ROLE_USER");

                    throw new Exception("registration successful");
                }
                else throw new Exception("podane hasla sie nie zgadzaja");
            }
            else throw new Exception("istnieje użytkownik o podanym loginie");
        }

        public void changePassword(string login, string password, string passwordCheck)
        {
            foreach (User user in this.UsersManager.getUsers())
            {
                if (user.getLogin() == login)
                {
                    if (password == passwordCheck)
                    {
                        user.setPassword(password);
                        this.UsersManager.saveUsers();
                    }
                    else
                    {
                        throw new Exception("podane hasla sie nie zgadzaja");
                    }
                }
            }
        }

        public void changeLogin(string curr_login, string new_login)
        {
            foreach (User user in this.UsersManager.getUsers())
            {
                if (user.getLogin() == curr_login)
                {
                    user.setLogin(new_login);
                    this.UsersManager.saveUsers();
                }
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

        public void editUser(string newlogin, string login, string password)
        {
            if(password == current_user.getPassword())
            {
                this.UsersManager.DBConnection.startConnection();

                MySqlCommand comm = this.UsersManager.DBConnection.connection.CreateCommand();
                string query = "UPDATE users SET username='" + newlogin + "' WHERE username='" + login + "' ";
                comm = new MySqlCommand(query, this.UsersManager.DBConnection.connection);
                comm.ExecuteNonQuery();

                this.UsersManager.DBConnection.closeConnection();
            }
        }
    }
}
