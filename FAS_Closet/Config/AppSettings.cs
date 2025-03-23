using System;
using System.Configuration;
using System.IO;

namespace FASCloset.Config
{
    public static class AppSettings
    {
        // Changed to use Data subdirectory in project folder
        private static readonly string _defaultDatabaseDirectory = Path.Combine(
            @"c:\Project\FAS_Closet\FAS_Closet\Data");
            
        private static readonly string _defaultDatabasePath = Path.Combine(
            _defaultDatabaseDirectory,
            "FASClosetDB.sqlite");

        /// <summary>
        /// Gets the database path from configuration or returns the default path
        /// </summary>
        public static string DatabasePath
        {
            get
            {
                try
                {
                    string? path = ConfigurationManager.AppSettings["DatabasePath"];
                    
                    // Replace environment variables in the path
                    if (!string.IsNullOrEmpty(path))
                    {
                        path = Environment.ExpandEnvironmentVariables(path);
                    }
                    
                    return string.IsNullOrEmpty(path) ? _defaultDatabasePath : path;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading DatabasePath from configuration: {ex.Message}");
                    return _defaultDatabasePath;
                }
            }
        }

        /// <summary>
        /// Gets the application name from configuration
        /// </summary>
        public static string ApplicationName
        {
            get
            {
                try
                {
                    string? appName = ConfigurationManager.AppSettings["ApplicationName"];
                    return appName ?? "FASCloset";
                }
                catch
                {
                    return "FASCloset";
                }
            }
        }

        /// <summary>
        /// Gets the application version from configuration
        /// </summary>
        public static string ApplicationVersion
        {
            get
            {
                try
                {
                    string? appVersion = ConfigurationManager.AppSettings["ApplicationVersion"];
                    return appVersion ?? "1.0.0";
                }
                catch
                {
                    return "1.0.0";
                }
            }
        }
    }
}
