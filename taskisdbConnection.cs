using System;
using MySql.Data.MySqlClient;

namespace tasked_forms
{
    public class taskisdbConnection
    {
        private static taskisdbConnection _instance = null;
        private static readonly object _lock = new object();
        private MySqlConnection _connection;
        private string _connectionString;

        // Private constructor (Singleton pattern)
        private taskisdbConnection()
        {
            // Update this if your MySQL has a password
            _connectionString = "Server=localhost;Database=task_is;Uid=root;Pwd=;";
            _connection = new MySqlConnection(_connectionString);
        }

        // Get the single instance of this class
        public static taskisdbConnection GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new taskisdbConnection();
                    }
                }
            }
            return _instance;
        }

        // Get the MySQL connection object
        public MySqlConnection GetConnection()
        {
            return _connection;
        }

        // Open the connection if closed
        public void OpenConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        // Close the connection if open
        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        // Test if connection is working
        public bool TestConnection()
        {
            try
            {
                OpenConnection();
                CloseConnection();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}