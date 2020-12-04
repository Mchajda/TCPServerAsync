using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using MySql.Data.MySqlClient;

namespace Server
{
    class UsersManager
    {
        DBConnection DBConnection;
        private ArrayList users;
        private string usersPath;

        public UsersManager()
        {
            this.usersPath = @"users.json";
            this.users = new ArrayList();
            //getting users to program
            DBConnection = new DBConnection("localhost", "3306", "root", "", "io_2020");
            
            this.readUsers();   
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

        public MySqlDataReader fetchRows(string query)
        {
            MySqlCommand comm = this.DBConnection.connection.CreateCommand();
            comm.CommandType = System.Data.CommandType.Text;
            comm.CommandText = query;
            MySqlDataReader reader = comm.ExecuteReader();
            return reader;
        }

        public void insertRow(string login, string password, string role)
        {
            DBConnection.startConnection();

            MySqlCommand comm = this.DBConnection.connection.CreateCommand();
            string query = "INSERT INTO users(username, password, role) VALUES('"+login+ "', '" + password + "', '" + role + "')"; 
            comm = new MySqlCommand(query, DBConnection.connection);
            comm.ExecuteNonQuery();

            DBConnection.closeConnection();
        }
        
        public void deleteRow(string login, string password, string role)
        {
            DBConnection.startConnection();

            MySqlCommand comm = this.DBConnection.connection.CreateCommand();
            string query = "DELETE FROM users WHERE username='" + login + "' ";
            comm = new MySqlCommand(query, DBConnection.connection);
            comm.ExecuteNonQuery();

            DBConnection.closeConnection();
        }
        public void readUsers()
        {
            DBConnection.startConnection();
            MySqlDataReader reader = fetchRows("SELECT * FROM users");
            System.Console.WriteLine("Users read");
            this.users.Clear();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    User newUser = new User(reader.GetString(1), reader.GetString(2), reader.GetString(3));
                    this.users.Add(newUser);
                }
            }

            DBConnection.closeConnection();
        }

        public ArrayList getUsers()
        {
            return this.users;
        }

        public void setUsers(ArrayList users)
        {
            this.users.Clear();
            this.users = users;
        }
    }
}
