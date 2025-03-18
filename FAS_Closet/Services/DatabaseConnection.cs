// This file defines the DatabaseConnection class, which handles the database connection.

using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace FASCloset.Services
{
    public static class DatabaseConnection
    {
        public static string GetConnectionString()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory ?? throw new InvalidOperationException("Base directory is null.");
            string projectDir = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName ?? throw new InvalidOperationException("Project directory is null.");

            // Use a fixed path for the database file
            string dbPath = Path.Combine(projectDir, "Data", "FASClosetDB.sqlite");
            return $"Data Source={dbPath};";
        }

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(GetConnectionString());
        }

        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();
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
                    );
                    CREATE TABLE IF NOT EXISTS Users (
                        UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL,
                        PasswordHash BLOB NOT NULL,
                        PasswordSalt BLOB NOT NULL,
                        Name TEXT NOT NULL,
                        Email TEXT,
                        Phone TEXT
                    );";
                using (var command = new SqliteCommand(createTablesQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
