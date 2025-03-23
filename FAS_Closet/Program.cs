using System;
using System.IO;
using System.Windows.Forms;
using FASCloset.Config;
using FASCloset.Data;
using FASCloset.Forms;
using FASCloset.Services;

namespace FASCloset
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                // Ensure database directory exists
                string dbDirectory = Path.GetDirectoryName(AppSettings.DatabasePath);
                if (!string.IsNullOrEmpty(dbDirectory) && !Directory.Exists(dbDirectory))
                {
                    Directory.CreateDirectory(dbDirectory);
                }

                Console.WriteLine($"Database path: {AppSettings.DatabasePath}");

                // Initialize database with proper error handling
                try {
                    DatabaseInitializer.Initialize(AppSettings.DatabasePath);
                    Console.WriteLine("Database initialized successfully.");
                }
                catch (Exception dbEx)
                {
                    MessageBox.Show($"Database initialization failed: {dbEx.Message}\n\nDetails: {dbEx.StackTrace}\n\nThe application will now exit.", 
                        "Database Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                    return;
                }
                
                // Start with authentication form
                Application.Run(new AuthForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Application failed to start: {ex.Message}\n\nDetails: {ex.StackTrace}", 
                    "Startup Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }
    }
}