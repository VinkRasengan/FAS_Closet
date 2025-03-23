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
                using (FileStream fs = File.Create(dbPath))
                {
                    fs.Close(); // Ensure file handle is released
                }
            }
            
            // Always initialize schema to ensure all required tables exist
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                InitializeDatabaseSchema(connection);
                
                // Create demo data only for a new database
                if (newDatabase)
                {
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
                    IsActive BOOLEAN NOT NULL DEFAULT 1,
                    FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID) ON DELETE CASCADE,
                    FOREIGN KEY (ManufacturerID) REFERENCES Manufacturer(ManufacturerID) ON DELETE SET NULL
                );",
                
                // Create Warehouse table BEFORE Inventory table
                @"CREATE TABLE IF NOT EXISTS Warehouse (
                    WarehouseID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Address TEXT,
                    ManagerUserID INTEGER NOT NULL,
                    CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    IsActive BOOLEAN NOT NULL DEFAULT 1,
                    Description TEXT,
                    FOREIGN KEY (ManagerUserID) REFERENCES User(UserID)
                );",
                
                // Now create Inventory table which references Warehouse
                @"CREATE TABLE IF NOT EXISTS Inventory (
                    InventoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductID INTEGER NOT NULL,
                    WarehouseID INTEGER NOT NULL DEFAULT 1,
                    StockQuantity INTEGER NOT NULL DEFAULT 0,
                    MinimumStockThreshold INTEGER NOT NULL DEFAULT 10,
                    FOREIGN KEY (ProductID) REFERENCES Product(ProductID) ON DELETE CASCADE,
                    FOREIGN KEY (WarehouseID) REFERENCES Warehouse(WarehouseID) ON DELETE CASCADE,
                    UNIQUE(ProductID, WarehouseID)
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
                    WarehouseID INTEGER DEFAULT 1,
                    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
                    FOREIGN KEY (ProductID) REFERENCES Product(ProductID) ON DELETE CASCADE,
                    FOREIGN KEY (WarehouseID) REFERENCES Warehouse(WarehouseID) ON DELETE SET DEFAULT
                );",
                
                @"CREATE TABLE IF NOT EXISTS NotificationLog (
                    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Type TEXT NOT NULL,
                    Subject TEXT NOT NULL,
                    Message TEXT NOT NULL,
                    Timestamp DATETIME NOT NULL
                );"
            };
            
            foreach (var command in createTableCommands)
            {
                using (var cmd = new SqliteCommand(command, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            
            // Check for existing Product table that needs to be updated with IsActive column
            using (var cmd = new SqliteCommand("PRAGMA table_info(Product);", connection))
            {
                bool hasIsActiveColumn = false;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string columnName = reader.GetString(1);
                        if (columnName.Equals("IsActive", StringComparison.OrdinalIgnoreCase))
                        {
                            hasIsActiveColumn = true;
                            break;
                        }
                    }
                }
                
                // If IsActive column doesn't exist, add it
                if (!hasIsActiveColumn)
                {
                    try
                    {
                        using (var alterCmd = new SqliteCommand("ALTER TABLE Product ADD COLUMN IsActive BOOLEAN NOT NULL DEFAULT 1;", connection))
                        {
                            alterCmd.ExecuteNonQuery();
                            Console.WriteLine("Added IsActive column to existing Product table.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding IsActive column: {ex.Message}");
                    }
                }
            }
            
            // Check if we need to migrate old Inventory data
            using (var cmd = new SqliteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='OldInventory'", connection))
            {
                if (cmd.ExecuteScalar() == null)
                {
                    // No migration needed, continue
                }
                else
                {
                    // Migration needed
                    string migrateQuery = @"
                        INSERT INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold)
                        SELECT ProductID, 1, StockQuantity, MinimumStockThreshold
                        FROM OldInventory;
                        DROP TABLE OldInventory;";
                    using (var migrateCmd = new SqliteCommand(migrateQuery, connection))
                    {
                        migrateCmd.ExecuteNonQuery();
                    }
                }
            }
        }
        
        public static void CreateDemoData(SqliteConnection connection)
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
            // First create the warehouses
            CreateDemoWarehouses(connection);
            
            string[] insertCommands = {
                // Main Warehouse (WarehouseID = 1) inventory
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (1, 1, 50, 10)",
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (2, 1, 30, 8)",
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (3, 1, 25, 5)",
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (4, 1, 20, 5)",
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (5, 1, 40, 10)",
                
                // North Branch (WarehouseID = 2) inventory
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (1, 2, 25, 8)",
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (2, 2, 15, 5)",
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (6, 2, 35, 10)",
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (7, 2, 45, 10)",
                
                // South Branch (WarehouseID = 3) inventory
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (8, 3, 15, 3)",
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (9, 3, 30, 8)",
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (10, 3, 25, 5)",
                "INSERT OR IGNORE INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) VALUES (11, 1, 5, 10)" // This will appear in low stock warnings
            };
            
            ExecuteCommands(connection, insertCommands);
        }
        
        private static void CreateDemoWarehouses(SqliteConnection connection)
        {
            string[] insertCommands = {
                "INSERT OR IGNORE INTO Warehouse (WarehouseID, Name, Address, ManagerUserID, Description) VALUES (1, 'Main Warehouse', '123 Main Street, City', 1, 'The main central warehouse')",
                "INSERT OR IGNORE INTO Warehouse (Name, Address, ManagerUserID, Description) VALUES ('North Branch', '456 North Ave, Town', 2, 'Northern distribution center')",
                "INSERT OR IGNORE INTO Warehouse (Name, Address, ManagerUserID, Description) VALUES ('South Branch', '789 South Blvd, Village', 3, 'Southern distribution center')"
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
            CreateDemoOrderDetails(connection);
        }
        
        private static void CreateDemoOrderDetails(SqliteConnection connection)
        {
            // Insert order details with warehouse information
            string[] orderDetailCommands = {
                // Order 1 details - from Warehouse 1
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, WarehouseID) VALUES (1, 1, 1, 29.99, 1)",
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, WarehouseID) VALUES (1, 5, 1, 19.99, 1)",
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, WarehouseID) VALUES (1, 6, 1, 24.99, 1)",
                
                // Order 2 details - from Warehouse 2
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, WarehouseID) VALUES (2, 7, 1, 59.99, 2)",
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, WarehouseID) VALUES (2, 8, 1, 129.99, 2)",
                
                // Order 3 details - from Warehouse 3
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, WarehouseID) VALUES (3, 3, 1, 49.99, 3)",
                
                // Order 4 details - mixed warehouses
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, WarehouseID) VALUES (4, 4, 1, 89.99, 1)",
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, WarehouseID) VALUES (4, 9, 2, 34.99, 3)",
                
                // Order 5 details - from Warehouse 1
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, WarehouseID) VALUES (5, 2, 1, 39.99, 1)",
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, WarehouseID) VALUES (5, 10, 1, 69.99, 1)"
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
