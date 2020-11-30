using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;
using System.Net.Sockets;

namespace Server
{
    class UserManager
    {
        private ArrayList users;
        public bool session_is_logged;
        private string usersPath;
        public User curr_user;

        public UserManager()
        {
            this.usersPath = @"users.json";
            this.users = new ArrayList();
            //getting users to program
            this.readUsers();
            this.users = this.getUsers();
            //flags
            this.session_is_logged = false;

        }

        public void saveUsers()
        {
            File.WriteAllText(usersPath, "");
            foreach (User user in this.getUsers())
            {
                string json = JsonConvert.SerializeObject(user);
                this.saveUser(json);
            }
        }

        public void saveUser(string user)
        {
            if (!File.Exists(this.usersPath))
            {
                using (StreamWriter sw = File.CreateText(this.usersPath))
                {
                    sw.WriteLine(user);
                    Console.WriteLine("saved!");
                }
            }

            using (StreamWriter sw = File.AppendText(this.usersPath))
            {
                sw.WriteLine(user);
                Console.WriteLine("saved!");
            }
        }

        public void readUsers()
        {
            string line;
            this.users.Clear();

            System.IO.StreamReader file = new System.IO.StreamReader(this.usersPath);
            while ((line = file.ReadLine()) != null)
            {
                string resultJson = String.Empty;
                resultJson = line;
                User resultUser = JsonConvert.DeserializeObject<User>(resultJson);
                //adding users to users array
                this.users.Add(resultUser);
                //Console.WriteLine();
            }

            file.Close();
            //System.Console.WriteLine();
        }

        public ArrayList getUsers()
        {
            return this.users;
        }

        public User authorize(string login, string password)
        {
            User current_user = new User(login, password);

            foreach (User user in this.getUsers())
            {
                if (user.getLogin() == login && user.getPassword() == password)
                {
                    this.session_is_logged = true;
                    current_user.setLogged();
                    this.curr_user = current_user;
                    break;
                }
                else
                {
                    current_user.unSetLogged();
                }
            }

            return current_user;
        }

        public void register(string login, string password, string passwordCheck)
        {
            bool is_valid_user = true;

            foreach (User user in this.getUsers())
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
                    this.saveUser(json);
                    Console.WriteLine("poprawnie zarejestrowano nowego uzytkownika!");
                    throw new Exception("registration successful");
                }
                else Console.WriteLine("podane hasla sie nie zgadzaja");
            }
            else Console.WriteLine("istnieje użytkownik o podanym loginie");
        }

        public void changePassword(string login, string password, string passwordCheck)
        {
            foreach (User user in this.getUsers())
            {
                if (user.getLogin() == login)
                {
                    if (password == passwordCheck)
                    {
                        user.setPassword(password);
                        saveUsers();
                    }
                    else
                        throw new Exception("podane hasla sie nie zgadzaja");
                }
            }
        }

        public void changeLogin(string curr_login, string new_login)
        {
            foreach (User user in this.getUsers())
            {
                if (user.getLogin() == curr_login)
                {
                    user.setLogin(new_login);
                    saveUsers();
                }
            }
        }

        public void ChangeUsername(String oldpassword, String login)
        {
            
            string passwordCheck = oldpassword;

            if (this.curr_user.getPassword() == oldpassword)
            {
                this.changeLogin(curr_user.getLogin(), login);
                throw new Exception("changed username");
            }
            else
            {
                throw new Exception("wrong password");
            }
        }

        public void ChangePassword(String oldpassword, String password, String passwordCheck)
        {
            if (this.curr_user.getPassword() == oldpassword)
            {
                this.changePassword(curr_user.getLogin(), password, passwordCheck);
                throw new Exception("changed password");
            }
            else
            {
                throw new Exception("wrong password");
            }
        }

        public void Register(String login, String password, String passwordCheck)
        {
            this.register(login, password, passwordCheck);
        }

        public void LogIn(String login, String password)
        {
            //authorization
            this.curr_user = this.authorize(login, password);
            if (!(this.session_is_logged))
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
