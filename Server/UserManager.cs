using Newtonsoft.Json;
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

        private string usersPath;
        public UserManager()
        {
            this.usersPath = @"users.json";
            this.users = new ArrayList();
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
    }
}
