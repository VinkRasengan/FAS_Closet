// This file defines the DatabaseConnection class, which handles the database connection.

using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace FASCloset.Services
{
    public static class DatabaseConnection
    {
        public static string GetConnectionString()
        {
            string databasePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "FASClosetDB.sqlite");
            // Remove the Version=3 parameter as it's not supported by Microsoft.Data.Sqlite
            return $"Data Source={databasePath}";
        }
    }
}
