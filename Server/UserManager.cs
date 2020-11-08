﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;

namespace Server
{
    class UserManager
    {
        private ArrayList users;
        public bool session_is_logged;
        public User current_user;

        private string usersPath;
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

        public void saveUser(string user)
        {
            // This text is added only once to the file.
            if (!File.Exists(this.usersPath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(this.usersPath))
                {
                    sw.WriteLine(user);
                    Console.WriteLine("saved!");
                }
            }

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(this.usersPath))
            {
                sw.WriteLine(user);
                Console.WriteLine("saved!");
            }
        }

        public void readUsers()
        {
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(this.usersPath);
            while ((line = file.ReadLine()) != null)
            {
                string resultJson = String.Empty;
                resultJson = line;
                User resultUser = JsonConvert.DeserializeObject<User>(resultJson);
                //adding users to users array
                this.users.Add(resultUser);
                Console.WriteLine();
            }

            file.Close();
            System.Console.WriteLine();
        }

        public ArrayList getUsers()
        {
            return this.users;
        }

        public User authorize(string login, string password, UserManager manager)
        {
            User current_user = new User(login, password);

            foreach (User user in this.getUsers())
            {
                if (user.getLogin() == login && user.getPassword() == password)
                {
                    manager.session_is_logged = true;
                    current_user.setLogged();
                }
                else
                {
                    current_user.unSetLogged();
                }
            }

            return current_user;
        }
    }
}
