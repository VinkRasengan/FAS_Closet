using System;
using System.IO;
using System.Windows.Forms;
using FASCloset.Forms;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                if (!Directory.Exists(dataDir))
                {
                    Directory.CreateDirectory(dataDir);
                }
                string dbPath = Path.Combine(dataDir, "FASClosetDB.sqlite");
                string connectionString = $"Data Source={dbPath};";
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL,
                            PasswordHash BLOB NOT NULL,
                            PasswordSalt BLOB NOT NULL,
                            Name TEXT NOT NULL,
                            Email TEXT,
                            Phone TEXT
                        );";
                    using (var command = new SqliteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}