using System;
using System.Configuration;
using System.IO;

namespace FASCloset.Config
{
    public static class AppSettings
    {
        // Get the project directory path (not the bin directory)
        private static readonly string _projectDirectory = GetProjectDirectory();
        
        // Use the project directory to locate the database file
        private static readonly string _defaultDatabasePath = Path.Combine(
            _projectDirectory, 
            "Data", 
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

        // Get the project directory (not the bin directory)
        private static string GetProjectDirectory()
        {
            try
            {
                // Start with the directory where the application is running
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                
                // Navigate up from bin\Debug or bin\Release to get to the project directory
                DirectoryInfo? directory = new DirectoryInfo(baseDirectory);
                
                // Go up until we find the project directory that contains "Data" folder
                while (directory != null && !Directory.Exists(Path.Combine(directory.FullName, "Data")))
                {
                    directory = directory.Parent;
                }
                
                // If we found the directory with Data folder, use that
                if (directory != null && Directory.Exists(Path.Combine(directory.FullName, "Data")))
                {
                    Console.WriteLine($"Found project directory: {directory.FullName}");
                    return directory.FullName;
                }
                
                // If we didn't find it, go back to a fixed location
                // Uses d:\Project\FAS_Closet as the base directory
                string fixedProjectPath = @"d:\Project\FAS_Closet\FAS_Closet";
                if (Directory.Exists(fixedProjectPath))
                {
                    Console.WriteLine($"Using fixed project directory: {fixedProjectPath}");
                    return fixedProjectPath;
                }
                
                // Final fallback to the running directory
                Console.WriteLine($"Falling back to base directory: {baseDirectory}");
                return baseDirectory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error determining project directory: {ex.Message}");
                // Final fallback to d:\Project\FAS_Closet if available
                return @"d:\Project\FAS_Closet\FAS_Closet";
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

        #region Notification Settings
        /// <summary>
        /// Gets whether email notifications are enabled
        /// </summary>
        public static bool EmailNotificationsEnabled
        {
            get
            {
                try
                {
                    string? setting = ConfigurationManager.AppSettings["EmailNotificationsEnabled"];
                    return !string.IsNullOrEmpty(setting) && bool.Parse(setting);
                }
                catch
                {
                    return false;
                }
            }
        }
        
        /// <summary>
        /// Gets whether SMS notifications are enabled
        /// </summary>
        public static bool SmsNotificationsEnabled
        {
            get
            {
                try
                {
                    string? setting = ConfigurationManager.AppSettings["SmsNotificationsEnabled"];
                    return !string.IsNullOrEmpty(setting) && bool.Parse(setting);
                }
                catch
                {
                    return false;
                }
            }
        }
        
        /// <summary>
        /// Gets the email address from which notifications are sent
        /// </summary>
        public static string NotificationEmailAddress
        {
            get
            {
                try
                {
                    string? email = ConfigurationManager.AppSettings["NotificationEmailAddress"];
                    return email ?? "noreply@fascloset.com";
                }
                catch
                {
                    return "noreply@fascloset.com";
                }
            }
        }
        
        /// <summary>
        /// Gets the email address to which notifications are sent
        /// </summary>
        public static string NotificationRecipientEmail
        {
            get
            {
                try
                {
                    string? email = ConfigurationManager.AppSettings["NotificationRecipientEmail"];
                    return email ?? "admin@fascloset.com";
                }
                catch
                {
                    return "admin@fascloset.com";
                }
            }
        }
        
        /// <summary>
        /// Gets the phone number to which SMS notifications are sent
        /// </summary>
        public static string SmsRecipientNumber
        {
            get
            {
                try
                {
                    string? number = ConfigurationManager.AppSettings["SmsRecipientNumber"];
                    return number ?? "";
                }
                catch
                {
                    return "";
                }
            }
        }
        
        /// <summary>
        /// Gets SMTP server address
        /// </summary>
        public static string SmtpServer
        {
            get
            {
                try
                {
                    string? server = ConfigurationManager.AppSettings["SmtpServer"];
                    return server ?? "smtp.gmail.com";
                }
                catch
                {
                    return "smtp.gmail.com";
                }
            }
        }
        
        /// <summary>
        /// Gets SMTP port
        /// </summary>
        public static int SmtpPort
        {
            get
            {
                try
                {
                    string? port = ConfigurationManager.AppSettings["SmtpPort"];
                    return !string.IsNullOrEmpty(port) ? int.Parse(port) : 587;
                }
                catch
                {
                    return 587;
                }
            }
        }
        
        /// <summary>
        /// Gets whether SMTP should use SSL
        /// </summary>
        public static bool SmtpUseSsl
        {
            get
            {
                try
                {
                    string? useSsl = ConfigurationManager.AppSettings["SmtpUseSsl"];
                    return !string.IsNullOrEmpty(useSsl) ? bool.Parse(useSsl) : true;
                }
                catch
                {
                    return true;
                }
            }
        }
        
        /// <summary>
        /// Gets SMTP username
        /// </summary>
        public static string SmtpUsername
        {
            get
            {
                try
                {
                    string? username = ConfigurationManager.AppSettings["SmtpUsername"];
                    return username ?? "";
                }
                catch
                {
                    return "";
                }
            }
        }
        
        /// <summary>
        /// Gets SMTP password
        /// </summary>
        public static string SmtpPassword
        {
            get
            {
                try
                {
                    string? password = ConfigurationManager.AppSettings["SmtpPassword"];
                    return password ?? "";
                }
                catch
                {
                    return "";
                }
            }
        }
        #endregion
    }
}
