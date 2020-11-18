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
        private string login, password;
        private bool isLogged;

        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
        }

        public bool getIsLogged()
        {
            return this.isLogged;
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

        public void setPassword(string passwd)
        {
            this.password = passwd;
        }
    }
}
