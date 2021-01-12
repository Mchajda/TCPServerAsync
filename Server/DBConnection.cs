using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class DBConnection
    {
        public string server, port, db_login, db_password, db_name;
        public MySqlConnection connection;

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

        public bool processQuery(string query)
        {
            this.startConnection();

            MySqlCommand comm = this.connection.CreateCommand();

            comm = new MySqlCommand(query, this.connection);

            if (comm.ExecuteNonQuery() > 0)
            {
                this.closeConnection();
                return true;
            }
            else
            {
                this.closeConnection();
                return false;
            }
        }
    }
}
