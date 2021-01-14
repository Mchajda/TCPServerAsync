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
    public class UsersManager
    {
        public DBConnection DBConnection;
        private ArrayList users;

        public UsersManager()
        {
            this.users = new ArrayList();
            //getting users to program
            DBConnection = new DBConnection("localhost", "3306", "root", "", "io_2020");
            
            this.readUsers();   
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
                    return true;
            }
            return false;
        }

        public bool insertRow(string login, string password, string role)
        {
            string query = "INSERT INTO users(username, password, role) VALUES('"+login+ "', '" + password + "', '" + role + "')"; 

            if(!this.checkIfExists(login))
            {
                return this.DBConnection.processQuery(query);
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

        public ArrayList readFriends(string username)
        {
            DBConnection.startConnection();
            MySqlDataReader reader = fetchRows("SELECT second_user FROM friendships WHERE first_user = '" + username + "'");
            System.Console.WriteLine("Friends read");
            ArrayList friends = new ArrayList();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    friends.Add(reader.GetString(1));
                }
            }
            DBConnection.closeConnection();
            return friends;
        }

        public ArrayList getUsers()
        {
            return this.users;
        }

        public ArrayList getFriends(string login)
        {
            ArrayList friends = new ArrayList();
            DBConnection.startConnection();
            MySqlDataReader reader = fetchRows("SELECT * FROM friendships WHERE first_user='"+login+"' OR second_user='"+login+"';");
            System.Console.WriteLine("Users read");

            //szukamy znajomych uzytkownika o loginie podanym w parametrze funckji
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string friend_one = reader.GetString(1);
                    string friend_two = reader.GetString(2);

                    //jezeli znajdziemy login uzytkownika w pierwszej kolumnie to znaczy ze uzytkownik z drugiej kolumny jest jego znajomym
                    if (friend_one == login)
                        friends.Add(friend_two);
                    else if (friend_two == login)
                        friends.Add(friend_one);
                }
            }

            DBConnection.closeConnection();
            return friends;
        }

        public bool deleteFriend(string user_login, string friend_to_delete)
        {
            ArrayList friends = new ArrayList();
            friends = this.getFriends(user_login);

            foreach(string friend in friends)
            {
                if(friend_to_delete == friend)
                {
                    string query = "DELETE FROM friendships WHERE (first_user='" + user_login + "' AND second_user='"+friend_to_delete+ "') OR (first_user='" + friend_to_delete + "' AND second_user='" + user_login + "');";
                    if (this.DBConnection.processQuery(query))
                        return true;
                    else return false;
                }
            }
            return false;
        }
    }
}
