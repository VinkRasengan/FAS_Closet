using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace FASCloset.Config
{
    public static class AppSettings
    {
        // Đường dẫn cơ sở tới project source code (không phải thư mục bin)
        private static readonly string _projectDirectory = GetProjectDirectory();
        
        // Đường dẫn tới thư mục Data trong project source code
        private static readonly string _sourceDataDirectory = Path.Combine(_projectDirectory, "Data");
        
        // Đường dẫn mặc định tới file database trong thư mục Data của project source code
        private static readonly string _defaultDatabasePath = Path.Combine(_sourceDataDirectory, "FASClosetDB.sqlite");

        /// <summary>
        /// Đường dẫn tới file database - ưu tiên sử dụng thư mục Data trong project source code
        /// </summary>
        public static string DatabasePath
        {
            get
            {
                try
                {
                    // Kiểm tra cấu hình trong App.config
                    string? configPath = ConfigurationManager.AppSettings["DatabasePath"];
                    
                    // Nếu đường dẫn được chỉ định trong cấu hình
                    if (!string.IsNullOrEmpty(configPath))
                    {
                        // Thay thế biến môi trường trong đường dẫn
                        configPath = Environment.ExpandEnvironmentVariables(configPath);
                        
                        // Nếu là đường dẫn tương đối, chuyển thành tuyệt đối dựa trên project directory
                        if (!Path.IsPathRooted(configPath))
                        {
                            configPath = Path.Combine(_projectDirectory, configPath);
                        }
                        
                        // Đảm bảo thư mục chứa file tồn tại
                        string? directory = Path.GetDirectoryName(configPath);
                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        
                        Console.WriteLine($"Using configured database path: {configPath}");
                        return configPath;
                    }
                    
                    // Tạo thư mục Data trong project directory nếu chưa tồn tại
                    if (!Directory.Exists(_sourceDataDirectory))
                    {
                        try
                        {
                            Directory.CreateDirectory(_sourceDataDirectory);
                            Console.WriteLine($"Created source data directory: {_sourceDataDirectory}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error creating source data directory: {ex.Message}");
                        }
                    }
                    
                    // Sử dụng đường dẫn mặc định trong project source code
                    Console.WriteLine($"Using default database path in source code directory: {_defaultDatabasePath}");
                    
                    // Hiển thị thông tin đầy đủ về đường dẫn file
                    if (File.Exists(_defaultDatabasePath))
                    {
                        var fileInfo = new FileInfo(_defaultDatabasePath);
                        Console.WriteLine($"Database file exists: {fileInfo.Length} bytes, last modified: {fileInfo.LastWriteTime}");
                    }
                    else
                    {
                        Console.WriteLine("Database file does not exist and will be created");
                    }
                    
                    return _defaultDatabasePath;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error determining DatabasePath: {ex.Message}");
                    return _defaultDatabasePath;
                }
            }
        }

        /// <summary>
        /// Lấy đường dẫn tới thư mục project source code (không phải bin directory)
        /// </summary>
        private static string GetProjectDirectory()
        {
            try
            {
                // Bắt đầu từ vị trí file thực thi (.exe)
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Console.WriteLine($"Base directory: {baseDirectory}");
                
                // Đi lên để tìm thư mục project
                DirectoryInfo? directory = new DirectoryInfo(baseDirectory);
                
                // Tìm thư mục gốc của project (thư mục chứa file .csproj)
                while (directory != null && 
                       !File.Exists(Path.Combine(directory.FullName, "FAS_Closet.csproj")) && 
                       !Directory.Exists(Path.Combine(directory.FullName, "Data")))
                {
                    directory = directory.Parent;
                }
                
                // Nếu tìm thấy thư mục project
                if (directory != null && 
                    (File.Exists(Path.Combine(directory.FullName, "FAS_Closet.csproj")) || 
                     Directory.Exists(Path.Combine(directory.FullName, "Data"))))
                {
                    Console.WriteLine($"Found project directory: {directory.FullName}");
                    return directory.FullName;
                }
                
                // Nếu không tìm thấy, giải quyết bằng cách sử dụng đường dẫn cứng cho môi trường phát triển
                string hardcodedPath = @"d:\Project\FAS_Closet\FAS_Closet";
                if (Directory.Exists(hardcodedPath))
                {
                    Console.WriteLine($"Using hardcoded development path: {hardcodedPath}");
                    return hardcodedPath;
                }
                
                // Cuối cùng, sử dụng chính thư mục chứa file thực thi
                Console.WriteLine($"Using application base directory: {baseDirectory}");
                return baseDirectory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetProjectDirectory: {ex.Message}");
                return AppDomain.CurrentDomain.BaseDirectory;
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
