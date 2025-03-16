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
                InitializeDatabase();
                // Khởi chạy AuthForm
                Application.Run(new AuthForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void InitializeDatabase()
        {
            string? baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (baseDirectory == null)
            {
                throw new InvalidOperationException("Base directory is null.");
            }

            string? projectDir = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;
            if (projectDir == null)
            {
                throw new InvalidOperationException("Project directory is null.");
            }

            string dataDir = Path.Combine(projectDir, "Data");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            string dbPath = Path.Combine(dataDir, "FASClosetDB.sqlite");
            string connectionString = $"Data Source={dbPath};";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand("PRAGMA foreign_keys = ON;", connection))
                {
                    command.ExecuteNonQuery();
                }
                string createTablesQuery = @"
                    CREATE TABLE IF NOT EXISTS Category (
                        category_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        description TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Manufacturer (
                        manufacturer_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        description TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Product (
                        product_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        category_id INTEGER NOT NULL,
                        manufacturer_id INTEGER,
                        price REAL NOT NULL,
                        stock INTEGER DEFAULT 0,
                        description TEXT,
                        FOREIGN KEY (category_id) REFERENCES Category(category_id),
                        FOREIGN KEY (manufacturer_id) REFERENCES Manufacturer(manufacturer_id)
                    );
                    CREATE TABLE IF NOT EXISTS Customer (
                        customer_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        phone TEXT,
                        email TEXT,
                        address TEXT
                    );
                    CREATE TABLE IF NOT EXISTS ""Order"" (
                        order_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        customer_id INTEGER,
                        order_date DATETIME DEFAULT CURRENT_TIMESTAMP,
                        total_amount REAL,
                        status TEXT DEFAULT 'pending',
                        FOREIGN KEY (customer_id) REFERENCES Customer(customer_id)
                    );
                    CREATE TABLE IF NOT EXISTS Order_Detail (
                        order_detail_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        order_id INTEGER NOT NULL,
                        product_id INTEGER NOT NULL,
                        quantity INTEGER NOT NULL,
                        unit_price REAL NOT NULL,
                        FOREIGN KEY (order_id) REFERENCES ""Order""(order_id),
                        FOREIGN KEY (product_id) REFERENCES Product(product_id)
                    );
                    CREATE TABLE IF NOT EXISTS Payment (
                        payment_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        order_id INTEGER NOT NULL,
                        payment_date DATETIME DEFAULT CURRENT_TIMESTAMP,
                        amount REAL NOT NULL,
                        method TEXT,
                        FOREIGN KEY (order_id) REFERENCES ""Order""(order_id)
                    );";
                using (var command = new SqliteCommand(createTablesQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}