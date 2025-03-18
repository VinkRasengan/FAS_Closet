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
    }
}
