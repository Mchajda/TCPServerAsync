using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    public class User
    {
        [JsonProperty]
        private string login, password, role;

        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
        }

        public User(string login, string password, string role)
        {
            this.login = login;
            this.password = password;
            this.role = role;
        }        

        public string getLogin()
        {
            return login;
        }

        public string getPassword()
        {
            return password;
        }

        public string getRole()
        {
            return role;
        }

        public void setPassword(string passwd)
        {
            this.password = passwd;
        }

        public void setLogin(string login)
        {
            this.login = login;
        }
        public void setRole(string role)
        {
            this.role = role;
        }
    }
}
