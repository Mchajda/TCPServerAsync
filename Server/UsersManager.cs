using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;
using System.Net.Sockets;

namespace Server
{
    class UsersManager
    {
        private ArrayList users;
        private string usersPath;

        public UsersManager()
        {
            this.usersPath = @"users.json";
            this.users = new ArrayList();
            //getting users to program
            this.readUsers();
        }

        public void saveUsers()
        {
            File.WriteAllText(usersPath, "");
            foreach (User user in users)
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

                this.users.Add(resultUser);
            }

            System.Console.WriteLine(this.getUsers().Count);
            foreach (User user in this.getUsers())
            {
                System.Console.WriteLine(user.getLogin());
            }

            file.Close();
        }

        public ArrayList getUsers()
        {
            return this.users;
        }

        public void setUsers(ArrayList users)
        {
            this.users = users;
        }
    }
}
