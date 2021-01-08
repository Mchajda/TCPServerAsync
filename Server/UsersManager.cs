﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using MySql.Data.MySqlClient;

namespace Server
{
    public class UsersManager
    {
        public DBConnection DBConnection;
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

        public MySqlDataReader fetchRows(string query)
        {
            MySqlCommand comm = this.DBConnection.connection.CreateCommand();
            comm.CommandType = System.Data.CommandType.Text;
            comm.CommandText = query;
            MySqlDataReader reader = comm.ExecuteReader();
            return reader;
        }

        public bool checkIfExists(string login)
        {
            foreach (User u in this.getUsers())
            {
                if (u.getLogin() == login)
                    return false;
            }
            return true;
        }

        public bool insertRow(string login, string password, string role)
        {
            DBConnection.startConnection();

            MySqlCommand comm = this.DBConnection.connection.CreateCommand();
            string query = "INSERT INTO users(username, password, role) VALUES('"+login+ "', '" + password + "', '" + role + "')"; 

            if(this.checkIfExists(login))
            {
                comm = new MySqlCommand(query, DBConnection.connection);
                comm.ExecuteNonQuery();

                if (comm.ExecuteNonQuery() > 0)
                {
                    DBConnection.closeConnection();
                    return true;
                }
                else
                {
                    DBConnection.closeConnection();
                    return false;
                }
            }
            else return false;
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
    }
}
