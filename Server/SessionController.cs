using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class SessionController
    {
        User current_user;
        UsersManager UsersManager;
        public bool session_is_logged;

        public SessionController()
        {
            UsersManager = new UsersManager();
            this.session_is_logged = false;
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
                    System.Console.Write("wszedlem");
                    User current_user = new User(login, password);
                    this.setUser(current_user);
                    this.setStatus(true);
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
                    Console.WriteLine("jest juz taki uzytkownik o podanym loginie!");
                    is_valid_user = false;
                    throw new Exception("user exists");
                }
            }

            if (is_valid_user == true)
            {
                if (password == passwordCheck)
                {
                    User newUser = new User(login, password);
                    newUser.setRole("ROLE_USER");
                    string json = JsonConvert.SerializeObject(newUser);
                    this.UsersManager.saveUser(json);
                    Console.WriteLine("poprawnie zarejestrowano nowego uzytkownika!");
                    throw new Exception("registration successful");
                }
                else Console.WriteLine("podane hasla sie nie zgadzaja");
            }
            else Console.WriteLine("istnieje użytkownik o podanym loginie");
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
    }
}
