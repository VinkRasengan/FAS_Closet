using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Services;

namespace FASCloset.Data
{
    /// <summary>
    /// Helper class for common database operations
    /// </summary>
    public static class DataAccessHelper
    {
        /// <summary>
        /// Executes a query and returns the results mapped to a list of objects
        /// </summary>
        public static List<T> ExecuteReader<T>(string query, Func<IDataReader, T> mapper, 
            Dictionary<string, object>? parameters = null) where T : class
        {
            return DatabaseConnection.ExecuteDbOperation(connection => 
            {
                var results = new List<T>();
                using (var command = new SqliteCommand(query, connection))
                {
                    // Add parameters if provided
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(mapper(reader));
                        }
                    }
                }
                return results;
            });
        }
        
        /// <summary>
        /// Executes a query and returns a single object
        /// </summary>
        public static T? ExecuteReaderSingle<T>(string query, Func<IDataReader, T> mapper, 
            Dictionary<string, object>? parameters = null) where T : class
        {
            return DatabaseConnection.ExecuteDbOperation(connection => 
            {
                using (var command = new SqliteCommand(query, connection))
                {
                    // Add parameters if provided
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return mapper(reader);
                        }
                    }
                }
                return null;
            });
        }
        
        /// <summary>
        /// Executes a non-query command
        /// </summary>
        public static int ExecuteNonQuery(string query, Dictionary<string, object>? parameters = null)
        {
            return DatabaseConnection.ExecuteDbOperation(connection => 
            {
                using (var command = new SqliteCommand(query, connection))
                {
                    // Add parameters if provided
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    
                    return command.ExecuteNonQuery();
                }
            });
        }
        
        /// <summary>
        /// Executes a scalar command
        /// </summary>
        public static T ExecuteScalar<T>(string query, Dictionary<string, object>? parameters = null)
        {
            return DatabaseConnection.ExecuteDbOperation(connection => 
            {
                using (var command = new SqliteCommand(query, connection))
                {
                    // Add parameters if provided
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    
                    var result = command.ExecuteScalar();
                    return (result == DBNull.Value) ? default : (T)Convert.ChangeType(result, typeof(T));
                }
            });
        }
    }
}
