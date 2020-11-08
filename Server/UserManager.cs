using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    class UserManager
    {
        [JsonProperty]
        private string login, password;
        private bool isLogged;

        public UserManager()
        {
            this.login = "maciej";
            this.password = "chajda";
            this.isLogged = false;
        }

        public UserManager(string login, string password)
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

        public string getLogin()
        {
            return this.login;
        }

        public string getPassword()
        {
            return this.password;
        }

        public void saveUser(string user)
        {
            string path = @"user.json";
            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(user);
                    Console.WriteLine("saved!");
                }
            }

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(user);
                Console.WriteLine("saved!");
            }
        }
    }
}
