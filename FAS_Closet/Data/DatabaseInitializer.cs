using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace FASCloset.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(string dbPath)
        {
            bool newDatabase = !File.Exists(dbPath);
            
            // Create database file if it doesn't exist
            if (newDatabase)
            {
                using (File.Create(dbPath)) { }
                
                // Initialize schema
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    InitializeDatabaseSchema(connection);
                    
                    // Create demo data for a new database
                    CreateDemoData(connection);
                }
            }
        }
        
        public static void InitializeDatabaseSchema(SqliteConnection connection)
        {
            string[] createTableCommands = {
                @"PRAGMA foreign_keys = ON;",
                
                @"CREATE TABLE IF NOT EXISTS User (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    PasswordSalt TEXT NOT NULL,
                    Name TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    Phone TEXT NOT NULL UNIQUE
                );",
                
                @"CREATE TABLE IF NOT EXISTS Category (
                    CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CategoryName TEXT NOT NULL UNIQUE,
                    Description TEXT,
                    IsActive BOOLEAN NOT NULL DEFAULT 1,
                    CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                );",
                
                @"CREATE TABLE IF NOT EXISTS Customer (
                    CustomerID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    Phone TEXT NOT NULL UNIQUE,
                    Address TEXT NOT NULL,
                    LoyaltyPoints INTEGER DEFAULT 0
                );",
                
                @"CREATE TABLE IF NOT EXISTS Manufacturer (
                    ManufacturerID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ManufacturerName TEXT NOT NULL UNIQUE,
                    Description TEXT
                );",
                
                @"CREATE TABLE IF NOT EXISTS Product (
                    ProductID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductName TEXT NOT NULL,
                    CategoryID INTEGER NOT NULL,
                    ManufacturerID INTEGER,
                    Price DECIMAL(10, 2) NOT NULL,
                    Stock INTEGER NOT NULL,
                    Description TEXT NOT NULL,
                    FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID) ON DELETE CASCADE,
                    FOREIGN KEY (ManufacturerID) REFERENCES Manufacturer(ManufacturerID) ON DELETE SET NULL
                );",
                
                @"CREATE TABLE IF NOT EXISTS Inventory (
                    ProductID INTEGER PRIMARY KEY,
                    StockQuantity INTEGER NOT NULL,
                    MinimumStockThreshold INTEGER NOT NULL,
                    FOREIGN KEY (ProductID) REFERENCES Product(ProductID) ON DELETE CASCADE
                );",
                
                @"CREATE TABLE IF NOT EXISTS Orders (
                    OrderID INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerID INTEGER NOT NULL,
                    OrderDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    TotalAmount DECIMAL(10, 2) NOT NULL,
                    PaymentMethod TEXT NOT NULL,
                    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) ON DELETE CASCADE
                );",
                
                @"CREATE TABLE IF NOT EXISTS OrderDetails (
                    OrderDetailID INTEGER PRIMARY KEY AUTOINCREMENT,
                    OrderID INTEGER NOT NULL,
                    ProductID INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    UnitPrice DECIMAL(10, 2) NOT NULL,
                    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
                    FOREIGN KEY (ProductID) REFERENCES Product(ProductID) ON DELETE CASCADE
                );"
            };
            
            foreach (var command in createTableCommands)
            {
                using (var cmd = new SqliteCommand(command, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        private static void CreateDemoData(SqliteConnection connection)
        {
            try
            {
                // Create demo users
                CreateDemoUsers(connection);
                
                // Create product categories
                CreateDemoCategories(connection);
                
                // Create manufacturers
                CreateDemoManufacturers(connection);
                
                // Create products
                CreateDemoProducts(connection);
                
                // Create inventory records
                CreateDemoInventory(connection);
                
                // Create customers
                CreateDemoCustomers(connection);
                
                // Create orders
                CreateDemoOrders(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating demo data: {ex.Message}");
            }
        }
        
        private static void CreateDemoUsers(SqliteConnection connection)
        {
            // Hash and salt for password "admin123"
            string adminPasswordHash = "9K8yYGFnUT1nFXEap8/QGuPvNkwHZw+VY7qaX9grSuLAce50bvJfF9uXvMUKDU7oJgxSGNxydKp9q60dO50wJg==";
            string adminPasswordSalt = "m2K/HaFXsZ1ZvGCqAB23K7FU59Jdl7wMESxCyQyPGUDGycgP5I/XQ0zGXrKkuL9HsJEkC3R9K52I6vnM9TAUpZPNjiF0QXbwQF0eZ0gEGZTiEJ7+gQ5fa09V/UuwAlwLH+tv6wUUETX3+WSwJVrGKM5UTjvUctGpLlg3y5zZxNM=";
            
            // Hash and salt for password "user123"
            string userPasswordHash = "e22PgUToxl0tr2MJnQ8MVzh5jZPD8UFOa5ZF6/EEn0JKQMX4BlUQW9Gjr0BbWQqxLrMrSOFt3J+isxSoZIP+Mw==";
            string userPasswordSalt = "o46B2C5T4+MNnwVRzVoDPiG3klTZpOYwP5URpQOluDh0tYZe3jIsXk8Y9gmcnet2+/NPQu9/fASYq9T3hf3Y2gKXKwEJAhF1OsJ5ItYdAUPVpuW5YXwAgGtO5f6FQhesaA3Nsr10JrKDxKE0mtwy9+MKC4n24vFXVlA+N4ZDN2s=";
            
            string[] insertCommands = {
                // Admin user
                $"INSERT OR IGNORE INTO User (Username, PasswordHash, PasswordSalt, Name, Email, Phone) VALUES ('admin', '{adminPasswordHash}', '{adminPasswordSalt}', 'Administrator', 'admin@fascloset.com', '0123456789')",
                
                // Regular users
                $"INSERT OR IGNORE INTO User (Username, PasswordHash, PasswordSalt, Name, Email, Phone) VALUES ('sales1', '{userPasswordHash}', '{userPasswordSalt}', 'Sales Person 1', 'sales1@fascloset.com', '0987654321')",
                $"INSERT OR IGNORE INTO User (Username, PasswordHash, PasswordSalt, Name, Email, Phone) VALUES ('manager', '{userPasswordHash}', '{userPasswordSalt}', 'Store Manager', 'manager@fascloset.com', '0123498765')"
            };
            
            ExecuteCommands(connection, insertCommands);
        }
        
        private static void CreateDemoCategories(SqliteConnection connection)
        {
            string[] insertCommands = {
                "INSERT OR IGNORE INTO Category (CategoryName, Description, IsActive, CreatedDate) VALUES ('Shirts', 'All types of shirts', 1, datetime('now'))",
                "INSERT OR IGNORE INTO Category (CategoryName, Description, IsActive, CreatedDate) VALUES ('Pants', 'All types of pants', 1, datetime('now'))",
                "INSERT OR IGNORE INTO Category (CategoryName, Description, IsActive, CreatedDate) VALUES ('Dresses', 'All types of dresses', 1, datetime('now'))",
                "INSERT OR IGNORE INTO Category (CategoryName, Description, IsActive, CreatedDate) VALUES ('Outerwear', 'Jackets and coats', 1, datetime('now'))",
                "INSERT OR IGNORE INTO Category (CategoryName, Description, IsActive, CreatedDate) VALUES ('Accessories', 'Hats, scarves, and more', 1, datetime('now'))"
            };
            
            ExecuteCommands(connection, insertCommands);
        }
        
        private static void CreateDemoManufacturers(SqliteConnection connection)
        {
            string[] insertCommands = {
                "INSERT OR IGNORE INTO Manufacturer (ManufacturerName, Description) VALUES ('Fashion Plus', 'Premium clothing manufacturer')",
                "INSERT OR IGNORE INTO Manufacturer (ManufacturerName, Description) VALUES ('Style Co.', 'Modern clothing styles')",
                "INSERT OR IGNORE INTO Manufacturer (ManufacturerName, Description) VALUES ('Elegance', 'Elegant and formal wear')",
                "INSERT OR IGNORE INTO Manufacturer (ManufacturerName, Description) VALUES ('Casual Comfort', 'Comfortable casual clothing')"
            };
            
            ExecuteCommands(connection, insertCommands);
        }
        
        private static void CreateDemoProducts(SqliteConnection connection)
        {
            string[] insertCommands = {
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description) VALUES ('Blue Button-Up Shirt', 1, 1, 29.99, 50, 'Classic blue button-up shirt for all occasions')",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description) VALUES ('Black Slim Pants', 2, 2, 39.99, 30, 'Stylish slim-fit pants in black')",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description) VALUES ('Summer Floral Dress', 3, 3, 49.99, 25, 'Beautiful floral pattern dress for summer')",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description) VALUES ('Winter Jacket', 4, 4, 89.99, 20, 'Warm winter jacket with hood')",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description) VALUES ('Leather Belt', 5, 2, 19.99, 40, 'Quality leather belt')",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description) VALUES ('Striped T-Shirt', 1, 4, 24.99, 35, 'Casual striped t-shirt')",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description) VALUES ('Jeans', 2, 1, 59.99, 45, 'Classic blue jeans')",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description) VALUES ('Evening Gown', 3, 3, 129.99, 15, 'Elegant evening gown for special occasions')",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description) VALUES ('Wool Scarf', 5, 3, 34.99, 30, 'Soft wool scarf for winter')",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description) VALUES ('Denim Jacket', 4, 2, 69.99, 25, 'Classic denim jacket')"
            };
            
            ExecuteCommands(connection, insertCommands);
        }
        
        private static void CreateDemoInventory(SqliteConnection connection)
        {
            string[] insertCommands = {
                "INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) VALUES (1, 50, 10)",
                "INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) VALUES (2, 30, 8)",
                "INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) VALUES (3, 25, 5)",
                "INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) VALUES (4, 20, 5)",
                "INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) VALUES (5, 40, 10)",
                "INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) VALUES (6, 35, 10)",
                "INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) VALUES (7, 45, 10)",
                "INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) VALUES (8, 15, 3)",
                "INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) VALUES (9, 30, 8)",
                "INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) VALUES (10, 25, 5)",
                "INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) VALUES (11, 5, 10)" // This will appear in low stock warnings
            };
            
            ExecuteCommands(connection, insertCommands);
        }
        
        private static void CreateDemoCustomers(SqliteConnection connection)
        {
            string[] insertCommands = {
                "INSERT OR IGNORE INTO Customer (Name, Email, Phone, Address, LoyaltyPoints) VALUES ('John Smith', 'john.smith@email.com', '0901234567', '123 Main St, City', 150)",
                "INSERT OR IGNORE INTO Customer (Name, Email, Phone, Address, LoyaltyPoints) VALUES ('Jane Doe', 'jane.doe@email.com', '0912345678', '456 Park Ave, Town', 220)",
                "INSERT OR IGNORE INTO Customer (Name, Email, Phone, Address, LoyaltyPoints) VALUES ('Robert Johnson', 'robert.j@email.com', '0923456789', '789 Oak St, Village', 75)",
                "INSERT OR IGNORE INTO Customer (Name, Email, Phone, Address, LoyaltyPoints) VALUES ('Sarah Williams', 'sarah.w@email.com', '0934567890', '321 Pine St, City', 300)",
                "INSERT OR IGNORE INTO Customer (Name, Email, Phone, Address, LoyaltyPoints) VALUES ('Michael Brown', 'michael.b@email.com', '0945678901', '654 Maple Ave, Town', 180)"
            };
            
            ExecuteCommands(connection, insertCommands);
        }
        
        private static void CreateDemoOrders(SqliteConnection connection)
        {
            // Insert orders
            string[] orderCommands = {
                "INSERT OR IGNORE INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod) VALUES (1, datetime('now', '-10 days'), 89.97, 'Credit Card')",
                "INSERT OR IGNORE INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod) VALUES (2, datetime('now', '-7 days'), 179.97, 'Cash')",
                "INSERT OR IGNORE INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod) VALUES (3, datetime('now', '-5 days'), 49.99, 'Bank Transfer')",
                "INSERT OR IGNORE INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod) VALUES (4, datetime('now', '-3 days'), 159.98, 'Credit Card')",
                "INSERT OR IGNORE INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod) VALUES (5, datetime('now', '-1 day'), 94.98, 'Cash')"
            };
            
            ExecuteCommands(connection, orderCommands);
            
            // Insert order details
            string[] orderDetailCommands = {
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (1, 1, 1, 29.99)",
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (1, 5, 1, 19.99)",
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (1, 6, 1, 24.99)",
                
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (2, 7, 1, 59.99)",
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (2, 8, 1, 129.99)",
                
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (3, 3, 1, 49.99)",
                
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (4, 4, 1, 89.99)",
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (4, 9, 2, 34.99)",
                
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (5, 2, 1, 39.99)",
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (5, 10, 1, 69.99)"
            };
            
            ExecuteCommands(connection, orderDetailCommands);
        }
        
        private static void ExecuteCommands(SqliteConnection connection, string[] commands)
        {
            foreach (var command in commands)
            {
                using (var cmd = new SqliteCommand(command, connection))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error executing command: {command}\nError: {ex.Message}");
                    }
                }
            }
        }
    }
}
