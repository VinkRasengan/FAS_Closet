using System;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using FASCloset.Config;

namespace FASCloset.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(string dbPath)
        {
            try
            {
                Console.WriteLine($"Initializing database at: {dbPath}");
                
                // Kiểm tra và tạo thư mục chứa database nếu chưa tồn tại
                string directory = Path.GetDirectoryName(dbPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    try
                    {
                        Directory.CreateDirectory(directory);
                        Console.WriteLine($"Created directory: {directory}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Không thể tạo thư mục database: {ex.Message}", ex);
                    }
                }
                
                bool newDatabase = !File.Exists(dbPath);
                Console.WriteLine($"Database file exists: {!newDatabase}");
                
                // Tạo kết nối đến database
                string connectionString = $"Data Source={dbPath}";
                using (var connection = new SqliteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine("Successfully opened database connection");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Không thể mở kết nối tới database: {ex.Message}", ex);
                    }
                    
                    // Luôn khởi tạo schema để đảm bảo đầy đủ bảng và cột mới nhất
                    InitializeDatabaseSchema(connection);
                    Console.WriteLine("Initialized database schema");
                    
                    // Tạo dữ liệu demo nếu là database mới
                    if (newDatabase)
                    {
                        CreateDemoData(connection);
                        Console.WriteLine("Created demo data");
                    }
                }
                
                // Xác minh database sau khi tạo
                VerifyDatabase(dbPath);
                
                Console.WriteLine("Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                Debug.WriteLine($"Database initialization error: {ex}");
                
                // Hiển thị thông báo lỗi
                try
                {
                    System.Windows.Forms.MessageBox.Show(
                        $"Lỗi khởi tạo cơ sở dữ liệu: {ex.Message}\n\nĐường dẫn database: {dbPath}\n\nVui lòng kiểm tra lại đường dẫn và quyền truy cập.",
                        "Lỗi Khởi Tạo CSDL",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
                catch
                {
                    // Nếu không hiển thị được MessageBox, bỏ qua
                }
                
                // Ném lại ngoại lệ để mã gọi có thể xử lý
                throw;
            }
        }
        
        /// <summary>
        /// Xác minh database đã được khởi tạo đúng với tất cả các bảng cần thiết
        /// </summary>
        private static void VerifyDatabase(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                throw new FileNotFoundException($"Database file không tồn tại sau khi khởi tạo: {dbPath}");
            }
            
            using (var connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                
                // Kiểm tra xem các bảng chính có tồn tại không
                string[] requiredTables = {
                    "User", "Category", "Customer", "Manufacturer", 
                    "Product", "Orders", "OrderDetails"
                };
                
                foreach (var table in requiredTables)
                {
                    string query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{table}'";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        var result = command.ExecuteScalar();
                        if (result == null)
                        {
                            throw new Exception($"Bảng '{table}' không tồn tại trong database sau khi khởi tạo");
                        }
                    }
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
                    Stock INTEGER NOT NULL DEFAULT 0,
                    Description TEXT NOT NULL,
                    IsActive BOOLEAN NOT NULL DEFAULT 1,
                    MinimumStockThreshold INTEGER NOT NULL DEFAULT 5,
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
                try
                {
                    using (var cmd = new SqliteCommand(command, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi tạo schema: {ex.Message}\nLệnh SQL: {command}", ex);
                }
            }
            
            // Kiểm tra cập nhật schema cho bảng Product
            Console.WriteLine("Checking for schema updates...");
            UpdateProductTableSchema(connection);
        }
        
        /// <summary>
        /// Cập nhật schema cho bảng Product nếu cần
        /// </summary>
        private static void UpdateProductTableSchema(SqliteConnection connection)
        {
            // Kiểm tra các cột cần thiết trong bảng Product
            using (var cmd = new SqliteCommand("PRAGMA table_info(Product);", connection))
            {
                bool hasIsActiveColumn = false;
                bool hasMinimumStockThresholdColumn = false;
                bool hasStockColumn = false;
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string columnName = reader.GetString(1);
                        if (columnName.Equals("IsActive", StringComparison.OrdinalIgnoreCase))
                        {
                            hasIsActiveColumn = true;
                        }
                        else if (columnName.Equals("MinimumStockThreshold", StringComparison.OrdinalIgnoreCase))
                        {
                            hasMinimumStockThresholdColumn = true;
                        }
                        else if (columnName.Equals("Stock", StringComparison.OrdinalIgnoreCase))
                        {
                            hasStockColumn = true;
                        }
                    }
                }
                
                // Thêm cột IsActive nếu chưa có
                if (!hasIsActiveColumn)
                {
                    try
                    {
                        using (var alterCmd = new SqliteCommand(
                            "ALTER TABLE Product ADD COLUMN IsActive BOOLEAN NOT NULL DEFAULT 1;", 
                            connection))
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
                
                // Thêm cột MinimumStockThreshold nếu chưa có
                if (!hasMinimumStockThresholdColumn)
                {
                    try
                    {
                        using (var alterCmd = new SqliteCommand(
                            "ALTER TABLE Product ADD COLUMN MinimumStockThreshold INTEGER NOT NULL DEFAULT 5;", 
                            connection))
                        {
                            alterCmd.ExecuteNonQuery();
                            Console.WriteLine("Added MinimumStockThreshold column to existing Product table.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding MinimumStockThreshold column: {ex.Message}");
                    }
                }
                
                // Thêm cột Stock nếu chưa có
                if (!hasStockColumn)
                {
                    try
                    {
                        using (var alterCmd = new SqliteCommand(
                            "ALTER TABLE Product ADD COLUMN Stock INTEGER NOT NULL DEFAULT 0;", 
                            connection))
                        {
                            alterCmd.ExecuteNonQuery();
                            Console.WriteLine("Added Stock column to existing Product table.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding Stock column: {ex.Message}");
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
            string[] insertProductCommands = {
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, MinimumStockThreshold, Description, IsActive) VALUES ('Blue Button-Up Shirt', 1, 1, 29.99, 50, 5, 'Classic blue button-up shirt for all occasions', 1)",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, MinimumStockThreshold, Description, IsActive) VALUES ('Black Slim Pants', 2, 2, 39.99, 30, 5, 'Stylish slim-fit pants in black', 1)",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, MinimumStockThreshold, Description, IsActive) VALUES ('Summer Floral Dress', 3, 3, 49.99, 25, 5, 'Beautiful floral pattern dress for summer', 1)",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, MinimumStockThreshold, Description, IsActive) VALUES ('Winter Jacket', 4, 4, 89.99, 20, 5, 'Warm winter jacket with hood', 1)",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, MinimumStockThreshold, Description, IsActive) VALUES ('Leather Belt', 5, 2, 19.99, 40, 5, 'Quality leather belt', 1)",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, MinimumStockThreshold, Description, IsActive) VALUES ('Striped T-Shirt', 1, 4, 24.99, 35, 5, 'Casual striped t-shirt', 1)",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, MinimumStockThreshold, Description, IsActive) VALUES ('Jeans', 2, 1, 59.99, 45, 5, 'Classic blue jeans', 1)",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, MinimumStockThreshold, Description, IsActive) VALUES ('Evening Gown', 3, 3, 129.99, 15, 5, 'Elegant evening gown for special occasions', 1)",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, MinimumStockThreshold, Description, IsActive) VALUES ('Wool Scarf', 5, 3, 34.99, 30, 5, 'Soft wool scarf for winter', 1)",
                "INSERT OR IGNORE INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, MinimumStockThreshold, Description, IsActive) VALUES ('Denim Jacket', 4, 2, 69.99, 25, 5, 'Classic denim jacket', 1)"
            };
            ExecuteCommands(connection, insertProductCommands);
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
