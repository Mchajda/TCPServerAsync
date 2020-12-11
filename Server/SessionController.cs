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

        //admin methods
        public void deleteUser(User u)
        {
            ArrayList users = this.UsersManager.getUsers();
            ArrayList newUsers = new ArrayList();

            foreach (User user in users)
            {
                if (!(u.getLogin() == user.getLogin() && u.getPassword() == user.getPassword()))
                {
                    newUsers.Add(user);
                }
            }

            this.UsersManager.setUsers(newUsers);
        }
    }
}
