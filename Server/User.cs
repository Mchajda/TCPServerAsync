using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    class User
    {
        [JsonProperty]
        private string login, password, role;
        private bool isLogged { get; set; }

        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
        }        

        public void setLogged()
        {
            this.isLogged = true;
        }

        public void unSetLogged()
        {
            this.isLogged = false;
        }

        public string getLogin()
        {
            return this.login;
        }

        public string getPassword()
        {
            return this.password;
        }

        public string getRole()
        {
            return this.role;
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
