using System;
using System.IO;
using System.Windows.Forms;
using FASCloset.Forms;
using FASCloset.Services;
using FASCloset.Data;
using Microsoft.Data.Sqlite;

namespace FASCloset
{
    internal static class Program
    {
        // Changed to use Data subdirectory in project folder
        private static readonly string databaseDir = @"c:\Project\FAS_Closet\FAS_Closet\Data";
        private static readonly string dbPath = Path.Combine(databaseDir, "FASClosetDB.sqlite");
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Ensure database directory exists
                if (!Directory.Exists(databaseDir))
                {
                    Directory.CreateDirectory(databaseDir);
                }
                
                bool isNewDatabase = !File.Exists(dbPath);
                
                // Initialize database if needed
                if (isNewDatabase)
                {
                    using (FileStream fs = File.Create(dbPath))
                    {
                        fs.Close(); // Ensure file handle is released
                    }
                    
                    using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                    {
                        connection.Open();
                        DatabaseInitializer.InitializeDatabaseSchema(connection);
                        DatabaseInitializer.CreateDemoData(connection);
                    }
                }
                
                // Verify database connection before starting the application
                if (!VerifyDatabaseConnection())
                {
                    MessageBox.Show("Cannot connect to the database. Please check your database configuration.",
                        "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // To prevent issues with access to the Visual Studio designer
                ApplicationConfiguration.Initialize();
                
                // Initialize and run the Windows Forms application
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // Start with the authentication form
                Application.Run(new Forms.AuthForm());
                
                #if DEBUG
                // For debugging: Verify database location
                Console.WriteLine($"Database location: {dbPath}");
                Console.WriteLine($"Database exists: {File.Exists(dbPath)}");
                #endif
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                    "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private static bool VerifyDatabaseConnection()
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    
                    // Verify we can query the database
                    using (var command = new SqliteCommand("SELECT 1", connection))
                    {
                        command.ExecuteScalar();
                    }
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection error: {ex.Message}");
                return false;
            }
        }
        
        private static void CopyDatabaseToOutputDirectory(string sourcePath)
        {
            try
            {
                string outputDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string destinationPath = Path.Combine(outputDirectory, "FASClosetDB.sqlite");
                
                // Only copy if source exists and is different from destination
                if (File.Exists(sourcePath) && (!File.Exists(destinationPath) || 
                    File.GetLastWriteTime(sourcePath) > File.GetLastWriteTime(destinationPath)))
                {
                    File.Copy(sourcePath, destinationPath, true);
                    Console.WriteLine($"Database copied to: {destinationPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to copy database to output directory: {ex.Message}");
                // This is just for debugging, so we don't want to throw an exception
            }
        }
    }
}