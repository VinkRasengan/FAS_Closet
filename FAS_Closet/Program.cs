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
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // Ensure database directory exists
                string databaseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                if (!Directory.Exists(databaseDir))
                {
                    Directory.CreateDirectory(databaseDir);
                }
                
                // Database path
                string dbPath = Path.Combine(databaseDir, "FASClosetDB.sqlite");
                
                // Initialize database if it doesn't exist
                DatabaseInitializer.Initialize(dbPath);
                
                // Copy db file to output directory if running in debug mode
                #if DEBUG
                    CopyDatabaseToOutputDirectory(dbPath);
                #endif
                
                Application.Run(new AuthForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during application startup: {ex.Message}", 
                    "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private static void CopyDatabaseToOutputDirectory(string sourcePath)
        {
            string? targetDir = Path.GetDirectoryName(Application.ExecutablePath);
            if (targetDir == null) return;
            
            string targetPath = Path.Combine(targetDir, "FASClosetDB.sqlite");
            
            try
            {
                if (File.Exists(targetPath))
                    File.Delete(targetPath);
                    
                File.Copy(sourcePath, targetPath);
            }
            catch (IOException ex)
            {
                // Just log the error, don't prevent application from running
                Console.WriteLine($"Error copying database: {ex.Message}");
            }
        }
    }
}