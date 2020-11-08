using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    class UserManager
    {
        public UserManager()
        {
            
        }

        public void saveUser(string user)
        {
            string path = @"users.json";
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
