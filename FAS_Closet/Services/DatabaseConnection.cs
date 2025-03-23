// This file defines the DatabaseConnection class, which handles the database connection.

using System;
using System.IO;
using Microsoft.Data.Sqlite;
using FASCloset.Config;

namespace FASCloset.Services
{
    public static class DatabaseConnection
    {
        public static string GetConnectionString()
        {
            string databasePath;
            
            // Always prioritize the project's Data directory path
            string dataDirectoryPath = @"c:\Project\FAS_Closet\FAS_Closet\Data";
            string defaultPath = Path.Combine(dataDirectoryPath, "FASClosetDB.sqlite");
            
            if (!Directory.Exists(dataDirectoryPath))
            {
                try
                {
                    Directory.CreateDirectory(dataDirectoryPath);
                }
                catch
                {
                    // If creating directory fails, fall back to other options
                }
            }
            
            if (File.Exists(defaultPath))
            {
                databasePath = defaultPath;
            }
            else
            {
                // Fall back to configured path if needed
                try
                {
                    databasePath = AppSettings.DatabasePath;
                    if (!File.Exists(databasePath))
                    {
                        databasePath = defaultPath;
                    }
                }
                catch
                {
                    databasePath = defaultPath;
                }
            }
            
            // Return the SQLite connection string
            return $"Data Source={databasePath}";
        }
        
        public static bool TestConnection()
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Executes a database operation with proper connection handling and error management
        /// </summary>
        /// <typeparam name="T">The return type of the operation</typeparam>
        /// <param name="databaseOperation">The operation to execute</param>
        /// <returns>The result of the operation</returns>
        public static T ExecuteDbOperation<T>(Func<SqliteConnection, T> databaseOperation)
        {
            try
            {
                string connectionString = GetConnectionString();
                Console.WriteLine($"Connection string: {connectionString}");
                
                using (var connection = new SqliteConnection(connectionString))
                {
                    Console.WriteLine("Opening database connection...");
                    connection.Open();
                    Console.WriteLine("Connection opened successfully");
                    return databaseOperation(connection);
                }
            }
            catch (SqliteException ex)
            {
                string message = $"Database operation failed: {ex.Message}";
                Console.WriteLine(message);
                throw new InvalidOperationException(message, ex);
            }
        }
        
        /// <summary>
        /// Executes a database operation with proper connection handling and error management (void version)
        /// </summary>
        /// <param name="databaseOperation">The operation to execute</param>
        public static void ExecuteDbOperation(Action<SqliteConnection> databaseOperation)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    databaseOperation(connection);
                }
            }
            catch (SqliteException ex)
            {
                string message = $"Database operation failed: {ex.Message}";
                Console.WriteLine(message);
                throw new InvalidOperationException(message, ex);
            }
        }

        // Execute database operations within a transaction
        public static void ExecuteWithTransaction(Action<SqliteConnection, SqliteTransaction> operations)
        {
            using var connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            
            using var transaction = connection.BeginTransaction();
            try
            {
                operations(connection, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
