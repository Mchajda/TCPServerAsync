using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class DBConnection
    {
        public string server, port, db_login, db_password, db_name;
        MySqlConnection connection;

        public DBConnection(string server, string port, string db_login, string db_password, string db_name)
        {
            this.server = server;
            this.port = port;
            this.db_login = db_login;
            this.db_password = db_password;
            this.db_name = db_name;
        }

        public void startConnection()
        {
            try
            {
                string connectionstring = "server="+this.server+";port="+this.port+";uid="+this.db_login+";pwd="+this.db_password+";database="+this.db_name+";charset=utf8;SslMode=none;";
                this.connection = new MySqlConnection(connectionstring);

                this.connection.Open();
                Console.WriteLine("connection is " + this.connection.State.ToString());
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("connection failed due to: " + ex.ToString());
            }
        }

        public void closeConnection()
        {
            this.connection.Close();
            Console.WriteLine("connection is " + this.connection.State.ToString());
        }

        public void fetchData()
        {
            Console.WriteLine("connection is " + this.connection.State.ToString());

            MySqlCommand comm = this.connection.CreateCommand();
            comm.CommandType = System.Data.CommandType.Text;
            comm.CommandText = "SELECT * FROM user";
            MySqlDataReader reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    System.Console.WriteLine(reader.GetString(1) + Environment.NewLine);
                }
            }
        }
    }
}
