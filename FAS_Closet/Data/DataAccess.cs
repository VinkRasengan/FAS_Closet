using System.Data;
using System.Data.SQLite;
using System.IO;
using System;

namespace FASCloset.Data
{
    public class DataAccess
    {
        private readonly string _connectionString;

        public DataAccess()
        {
            string dataDir = @"c:\Project\FAS_Closet\FAS_Closet\Data";
            string databasePath = Path.Combine(dataDir, "FASClosetDB.sqlite");
            _connectionString = $"Data Source={databasePath};Version=3;";
        }

        public IDbConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString);
        }

        public void CreateTables()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        PRAGMA foreign_keys = ON;

                        -- User Table
                        CREATE TABLE IF NOT EXISTS User (
                            UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL UNIQUE,
                            PasswordHash TEXT NOT NULL,
                            PasswordSalt TEXT NOT NULL,
                            Name TEXT NOT NULL,
                            Email TEXT NOT NULL UNIQUE,
                            Phone TEXT NOT NULL UNIQUE
                        );

                        -- Category Table
                        CREATE TABLE IF NOT EXISTS Category (
                            CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                            CategoryName TEXT NOT NULL UNIQUE,
                            Description TEXT,
                            IsActive BOOLEAN NOT NULL DEFAULT 1,
                            CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                        );

                        -- Customer Table
                        CREATE TABLE IF NOT EXISTS Customer (
                            CustomerID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            Email TEXT NOT NULL UNIQUE,
                            Phone TEXT NOT NULL UNIQUE,
                            Address TEXT NOT NULL
                        );

                        -- Manufacturer Table
                        CREATE TABLE IF NOT EXISTS Manufacturer (
                            ManufacturerID INTEGER PRIMARY KEY AUTOINCREMENT,
                            ManufacturerName TEXT NOT NULL UNIQUE,
                            Description TEXT
                        );

                        -- Product Table
                        CREATE TABLE IF NOT EXISTS Product (
                            ProductID INTEGER PRIMARY KEY AUTOINCREMENT,
                            ProductName TEXT NOT NULL,
                            CategoryID INTEGER NOT NULL,
                            ManufacturerID INTEGER,
                            Price DECIMAL(10, 2) NOT NULL,
                            Stock INTEGER NOT NULL,
                            Description TEXT NOT NULL,
                            FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID) ON DELETE CASCADE,
                            FOREIGN KEY (ManufacturerID) REFERENCES Manufacturer(ManufacturerID) ON DELETE SET NULL
                        );

                        -- Inventory Table
                        CREATE TABLE IF NOT EXISTS Inventory (
                            ProductID INTEGER PRIMARY KEY,
                            StockQuantity INTEGER NOT NULL,
                            MinimumStockThreshold INTEGER NOT NULL,
                            FOREIGN KEY (ProductID) REFERENCES Product(ProductID) ON DELETE CASCADE
                        );

                        -- Order Table
                        CREATE TABLE IF NOT EXISTS OrderTable (
                            OrderID INTEGER PRIMARY KEY AUTOINCREMENT,
                            CustomerID INTEGER NOT NULL,
                            OrderDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                            TotalAmount DECIMAL(10, 2) NOT NULL,
                            PaymentMethod TEXT NOT NULL,
                            FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) ON DELETE CASCADE
                        );

                        -- OrderDetail Table
                        CREATE TABLE IF NOT EXISTS OrderDetail (
                            OrderDetailID INTEGER PRIMARY KEY AUTOINCREMENT,
                            OrderID INTEGER NOT NULL,
                            ProductID INTEGER NOT NULL,
                            Quantity INTEGER NOT NULL,
                            UnitPrice DECIMAL(10, 2) NOT NULL,
                            FOREIGN KEY (OrderID) REFERENCES OrderTable(OrderID) ON DELETE CASCADE,
                            FOREIGN KEY (ProductID) REFERENCES Product(ProductID) ON DELETE CASCADE
                        );
                    ";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
