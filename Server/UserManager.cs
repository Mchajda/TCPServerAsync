using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class UserManager
    {
        private string login, password;
        private bool isLogged;

        public UserManager()
        {
            this.login = "maciej";
            this.password = "chajda";
            this.isLogged = false;
        }

        public bool getIsLogged()
        {
            return this.isLogged;
        }

        public void setLogged()
        {
            this.isLogged = true;
        }

        public string getLogin()
        {
            return this.login;
        }

        public string getPassword()
        {
            return this.password;
        }
    }
}
