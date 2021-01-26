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
                string connectionstring = "server=" + server + ";port=" + port + ";uid=" + db_login + ";pwd=" + db_password + ";database=" + db_name + ";charset=utf8;SslMode=none;";
                connection = new MySqlConnection(connectionstring);

                connection.Open();
                Console.WriteLine("connection is " + connection.State.ToString());
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("connection failed due to: " + ex.ToString());
            }
        }

        public void closeConnection()
        {
            connection.Close();
            Console.WriteLine("connection is " + connection.State.ToString());
        }

        public bool processQuery(string query)
        {
            startConnection();

            MySqlCommand comm = connection.CreateCommand();

            comm = new MySqlCommand(query, connection);

            if (comm.ExecuteNonQuery() > 0)
            {
                closeConnection();
                return true;
            }
            else
            {
                closeConnection();
                return false;
            }
        }
    }
}
