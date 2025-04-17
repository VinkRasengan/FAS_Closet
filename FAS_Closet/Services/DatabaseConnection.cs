// This file defines the DatabaseConnection class, which handles the database connection.

using System;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;
using FASCloset.Config;

namespace FASCloset.Services
{
    public static class DatabaseConnection
    {
        /// <summary>
        /// Gets the connection string to the database
        /// </summary>
        /// <returns>The connection string</returns>
        public static string GetConnectionString()
        {
            // Ensure database directory exists
            string dbDirectory = Path.GetDirectoryName(AppSettings.DatabasePath);
            if (!Directory.Exists(dbDirectory) && !string.IsNullOrEmpty(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }
            
            return $"Data Source={AppSettings.DatabasePath}";
        }
        
        /// <summary>
        /// Tests the database connection
        /// </summary>
        /// <returns>True if the connection is successful</returns>
        public static bool TestConnection()
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection error: {ex.Message}");
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
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    return databaseOperation(connection);
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"SQLite error: {ex.Message}");
                    throw new InvalidOperationException($"Database error: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in database operation: {ex.Message}");
                    throw;
                }
            }
        }
        
        /// <summary>
        /// Executes a database operation with proper connection handling and error management (void version)
        /// </summary>
        /// <param name="databaseOperation">The operation to execute</param>
        public static void ExecuteDbOperation(Action<SqliteConnection> databaseOperation)
        {
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    databaseOperation(connection);
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"SQLite error: {ex.Message}");
                    throw new InvalidOperationException($"Database error: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in database operation: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Executes a database operation within a transaction
        /// </summary>
        /// <typeparam name="T">The return type of the operation</typeparam>
        /// <param name="transactionOperation">The operation to execute within a transaction</param>
        /// <returns>The result of the operation</returns>
        public static T ExecuteWithTransaction<T>(Func<SqliteConnection, SqliteTransaction, T> transactionOperation)
        {
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        T result = transactionOperation(connection, transaction);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Transaction error: {ex.Message}");
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Executes a database operation within a transaction (void version)
        /// </summary>
        /// <param name="transactionOperation">The operation to execute within a transaction</param>
        public static void ExecuteWithTransaction(Action<SqliteConnection, SqliteTransaction> transactionOperation)
        {
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        transactionOperation(connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Transaction error: {ex.Message}");
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
