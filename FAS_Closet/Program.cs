using System;
using System.IO;
using System.Windows.Forms;
using FASCloset.Forms;
using FASCloset.Services; // Add this line
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
            InitializeDatabase();
            Application.Run(new AuthForm());
        }

        private static void InitializeDatabase()
        {
            DatabaseConnection.InitializeDatabase();
        }
    }
}