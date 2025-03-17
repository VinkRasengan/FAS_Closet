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
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory ?? throw new InvalidOperationException("Base directory is null.");
            string projectDir = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName ?? throw new InvalidOperationException("Project directory is null.");

            // Use a fixed path for the database file
            string dbPath = Path.Combine(projectDir, "Data", "FASClosetDB.sqlite");
            return $"Data Source={dbPath};";
        }

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(GetConnectionString());
        }

        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Categories (
                        CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                        CategoryName TEXT NOT NULL,
                        Description TEXT,
                        IsActive INTEGER NOT NULL,
                        CreatedDate DATETIME NOT NULL
                    );";
                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
