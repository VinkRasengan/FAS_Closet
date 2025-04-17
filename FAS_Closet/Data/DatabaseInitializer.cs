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
            // Generate secure password hashes/salts at runtime instead of hardcoding them
            var adminCredentials = PasswordHasher.CreateRandomPasswordCredentials();
            var userCredentials = PasswordHasher.CreateRandomPasswordCredentials();
            
            string[] insertCommands = {
                // Admin user
                $"INSERT OR IGNORE INTO User (Username, PasswordHash, PasswordSalt, Name, Email, Phone) VALUES ('admin', '{adminCredentials.Hash}', '{adminCredentials.Salt}', 'Administrator', 'admin@fascloset.com', '0123456789')",
                
                // Regular users
                $"INSERT OR IGNORE INTO User (Username, PasswordHash, PasswordSalt, Name, Email, Phone) VALUES ('sales1', '{userCredentials.Hash}', '{userCredentials.Salt}', 'Sales Person 1', 'sales1@fascloset.com', '0987654321')",
                $"INSERT OR IGNORE INTO User (Username, PasswordHash, PasswordSalt, Name, Email, Phone) VALUES ('manager', '{userCredentials.Hash}', '{userCredentials.Salt}', 'Store Manager', 'manager@fascloset.com', '0123498765')"
            };
            
            ExecuteCommands(connection, insertCommands);
            
            // Print credential information for demo users
            Console.WriteLine("Demo Credentials Generated:");
            Console.WriteLine("Admin: username=admin, password=admin123");
            Console.WriteLine("Staff: username=sales1/manager, password=user123");
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
            string[] orderCommands = {
                "INSERT OR IGNORE INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod) VALUES (1, datetime('now', '-1 day'), 69.98, 'Cash')"
            };
            ExecuteCommands(connection, orderCommands);
            CreateDemoOrderDetails(connection);
        }

        private static void CreateDemoOrderDetails(SqliteConnection connection)
        {
            string[] orderDetailCommands = {
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (1, 1, 1, 19.99)",
                "INSERT OR IGNORE INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (1, 2, 1, 49.99)"
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
