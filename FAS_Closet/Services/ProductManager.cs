// This file defines the ProductManager class, which handles product-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Models;
using FASCloset.Data;

namespace FASCloset.Services
{
    public static class ProductManager
    {
        private const string DescriptionParameter = "@Description";
        
        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
        }

        // Get all products with category and manufacturer names
        public static List<Product> GetProducts(bool includeInactive = false)
        {
            // Define SQL parameter name constants
            const string includeInactiveParam = "@IncludeInactive";

            string query = @"
                SELECT p.*, 
                       c.CategoryName, 
                       m.ManufacturerName,
                       CASE WHEN p.Stock <= p.MinimumStockThreshold THEN 1 ELSE 0 END as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                WHERE (p.IsActive = 1 OR @IncludeInactive = 1)
                ORDER BY p.ProductID";

            var parameters = new Dictionary<string, object>
            {
                { includeInactiveParam, includeInactive ? 1 : 0 }
            };

            try
            {
                var products = DataAccessHelper.ExecuteReader(query, reader => MapProductWithDetails(reader), parameters);
                Console.WriteLine($"Retrieved {products.Count} products from database.");
                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving products: {ex.Message}");
                // Return empty list instead of throwing to avoid crashing UI
                return new List<Product>();
            }
        }

        // Get all products (alias for GetProducts for backward compatibility)
        public static List<Product> GetAllProducts(bool onlyActive = true)
        {
            // Inverted logic between methods - onlyActive = true means includeInactive = false
            return GetProducts(!onlyActive);
        }

        // Get products by category with additional details
        public static List<Product> GetProductsByCategory(int categoryId, bool includeInactive = false)
        {
            string query = @"
                SELECT p.*, 
                       c.CategoryName, 
                       m.ManufacturerName,
                       CASE WHEN p.Stock <= p.MinimumStockThreshold THEN 1 ELSE 0 END as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                WHERE p.CategoryID = @CategoryID AND (p.IsActive = 1 OR @IncludeInactive = 1)
                ORDER BY p.ProductID";

            var parameters = new Dictionary<string, object>
            {
                { "@CategoryID", categoryId },
                { "@IncludeInactive", includeInactive ? 1 : 0 }
            };

            return DataAccessHelper.ExecuteReader(query, reader => MapProductWithDetails(reader), parameters);
        }

        // Get product by name
        public static Product? GetProductByName(string name)
        {
            string query = "SELECT * FROM Product WHERE ProductName = @Name";
            var parameters = new Dictionary<string, object> { { "@Name", name } };

            return DataAccessHelper.ExecuteReaderSingle(query, reader => MapProduct(reader), parameters);
        }

        // Get product by ID with related information
        public static Product? GetProductById(int productId)
        {
            string query = @"
                SELECT p.*, 
                       c.CategoryName, 
                       m.ManufacturerName,
                       CASE WHEN p.Stock <= p.MinimumStockThreshold THEN 1 ELSE 0 END as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                WHERE p.ProductID = @ProductID";

            var parameters = new Dictionary<string, object> { { "@ProductID", productId } };

            return DataAccessHelper.ExecuteReaderSingle(query, reader => MapProductWithDetails(reader), parameters);
        }

        // Add a new product
        public static void AddProduct(Product product)
        {
            string query = @"
                INSERT INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description, IsActive, MinimumStockThreshold) 
                VALUES (@ProductName, @CategoryID, @ManufacturerID, @Price, @Stock, @Description, @IsActive, @MinimumStockThreshold);
                SELECT last_insert_rowid();";

            var parameters = new Dictionary<string, object>
            {
                { "@ProductName", product.ProductName },
                { "@CategoryID", product.CategoryID },
                { "@ManufacturerID", product.ManufacturerID ?? (object)DBNull.Value },
                { "@Price", product.Price },
                { "@Stock", product.Stock },
                { "@Description", product.Description },
                { "@IsActive", product.IsActive ? 1 : 0 },
                { "@MinimumStockThreshold", product.MinimumStockThreshold > 0 ? product.MinimumStockThreshold : 5 }
            };

            DataAccessHelper.ExecuteScalar<int>(query, parameters);
        }

        // Update an existing product
        public static void UpdateProduct(Product product)
        {
            // Get the current product to check if stock changed
            Product? currentProduct = GetProductById(product.ProductID);
            bool stockChanged = currentProduct != null && currentProduct.Stock != product.Stock;

            string query = @"
                UPDATE Product 
                SET ProductName = @ProductName, 
                    CategoryID = @CategoryID, 
                    ManufacturerID = @ManufacturerID, 
                    Price = @Price, 
                    Stock = @Stock, 
                    Description = @Description,
                    IsActive = @IsActive,
                    MinimumStockThreshold = @MinimumStockThreshold
                WHERE ProductID = @ProductID";

            var parameters = new Dictionary<string, object>
            {
                { "@ProductID", product.ProductID },
                { "@ProductName", product.ProductName },
                { "@CategoryID", product.CategoryID },
                { "@ManufacturerID", product.ManufacturerID ?? (object)DBNull.Value },
                { "@Price", product.Price },
                { "@Stock", product.Stock },
                { "@Description", product.Description },
                { "@IsActive", product.IsActive ? 1 : 0 },
                { "@MinimumStockThreshold", product.MinimumStockThreshold > 0 ? product.MinimumStockThreshold : 5 }
            };

            DataAccessHelper.ExecuteNonQuery(query, parameters);

            // If stock changed, we might need to trigger inventory notifications or other actions
            if (stockChanged)
            {
                // Log stock change or trigger notifications if needed
                // This would replace the previous call to InventoryManager.UpdateStock
            }
        }

        // Delete a product (or mark as inactive)
        public static void DeleteProduct(int productId)
        {
            // Instead of deleting, mark as inactive
            string query = "UPDATE Product SET IsActive = 0 WHERE ProductID = @ProductID";
            var parameters = new Dictionary<string, object> { { "@ProductID", productId } };
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }

        // Permanently delete a product (only for admin use)
        public static void PermanentlyDeleteProduct(int productId)
        {
            string query = "DELETE FROM Product WHERE ProductID = @ProductID";
            var parameters = new Dictionary<string, object> { { "@ProductID", productId } };
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }

        // Get all categories
        public static List<Category> GetCategories()
        {
            string query = "SELECT * FROM Category WHERE IsActive = 1 ORDER BY CategoryName";
            return DataAccessHelper.ExecuteReader(query, reader => new Category
            {
                CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
            });
        }

        // Get all manufacturers
        public static List<Manufacturer> GetManufacturers()
        {
            string query = "SELECT * FROM Manufacturer ORDER BY ManufacturerName";
            return DataAccessHelper.ExecuteReader(query, reader => new Manufacturer
            {
                ManufacturerID = reader.GetInt32(reader.GetOrdinal("ManufacturerID")),
                ManufacturerName = reader.GetString(reader.GetOrdinal("ManufacturerName")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"))
            });
        }

        // Add a category
        public static void AddCategory(Category category)
        {
            string query = "INSERT INTO Category (CategoryName, Description, IsActive) VALUES (@CategoryName, @Description, @IsActive)";
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryName", category.CategoryName },
                { "@Description", category.Description ?? (object)DBNull.Value },
                { "@IsActive", category.IsActive }
            };
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }

        // Add a manufacturer
        public static void AddManufacturer(Manufacturer manufacturer)
        {
            string query = "INSERT INTO Manufacturer (ManufacturerName, Description) VALUES (@ManufacturerName, @Description)";
            var parameters = new Dictionary<string, object>
            {
                { "@ManufacturerName", manufacturer.ManufacturerName },
                { "@Description", manufacturer.Description ?? (object)DBNull.Value }
            };
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }

        // Update a category 
        public static void UpdateCategory(Category category)
        {
            string query = "UPDATE Category SET CategoryName = @CategoryName, Description = @Description, IsActive = @IsActive WHERE CategoryID = @CategoryID";
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryID", category.CategoryID },
                { "@CategoryName", category.CategoryName },
                { "@Description", category.Description ?? (object)DBNull.Value },
                { "@IsActive", category.IsActive }
            };
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }

        // Update a manufacturer
        public static void UpdateManufacturer(Manufacturer manufacturer)
        {
            string query = "UPDATE Manufacturer SET ManufacturerName = @ManufacturerName, Description = @Description WHERE ManufacturerID = @ManufacturerID";
            var parameters = new Dictionary<string, object>
            {
                { "@ManufacturerID", manufacturer.ManufacturerID },
                { "@ManufacturerName", manufacturer.ManufacturerName },
                { "@Description", manufacturer.Description ?? (object)DBNull.Value }
            };
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }

        // Check if category name is already in use
        public static bool IsCategoryNameTaken(string categoryName)
        {
            string query = "SELECT COUNT(*) FROM Category WHERE CategoryName = @CategoryName";
            var parameters = new Dictionary<string, object> { { "@CategoryName", categoryName } };
            int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            return count > 0;
        }

        // Check if manufacturer name is already in use
        public static bool IsManufacturerNameTaken(string manufacturerName)
        {
            string query = "SELECT COUNT(*) FROM Manufacturer WHERE ManufacturerName = @ManufacturerName";
            var parameters = new Dictionary<string, object> { { "@ManufacturerName", manufacturerName } };
            int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            return count > 0;
        }

        // Search products by name or description
        public static List<Product> SearchProducts(string searchText, bool includeInactive = false)
        {
            string query = @"
                SELECT p.*, 
                       c.CategoryName, 
                       m.ManufacturerName,
                       CASE WHEN p.Stock <= p.MinimumStockThreshold THEN 1 ELSE 0 END as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                WHERE (p.ProductName LIKE @SearchText OR p.Description LIKE @SearchText) 
                AND (p.IsActive = 1 OR @IncludeInactive = 1)
                ORDER BY p.ProductName";

            var parameters = new Dictionary<string, object>
            {
                { "@SearchText", "%" + searchText + "%" },
                { "@IncludeInactive", includeInactive ? 1 : 0 }
            };

            return DataAccessHelper.ExecuteReader(query, reader => MapProductWithDetails(reader), parameters);
        }

        // Get all products with warehouse-specific stock information
        public static List<Product> GetProductsForWarehouse(int warehouseId, bool includeInactive = false)
        {
            // Define SQL parameter name constants
            const string warehouseIdParam = "@WarehouseID";
            const string includeInactiveParam = "@IncludeInactive";

            string query = @"
                SELECT p.*, 
                       c.CategoryName, 
                       m.ManufacturerName,
                       CASE WHEN p.Stock <= p.MinimumStockThreshold THEN 1 ELSE 0 END as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                WHERE (p.IsActive = 1 OR @IncludeInactive = 1)
                ORDER BY p.ProductName";

            var parameters = new Dictionary<string, object>
            {
                { includeInactiveParam, includeInactive ? 1 : 0 }
            };

            try
            {
                var products = DataAccessHelper.ExecuteReader(query, reader => MapProductWithDetails(reader), parameters);
                    
                Console.WriteLine($"Retrieved {products.Count} products for warehouse {warehouseId}");
                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving products for warehouse {warehouseId}: {ex.Message}");
                return new List<Product>();
            }
        }

        // Helper method to map reader to product
        private static Product MapProduct(IDataReader reader)
        {
            return new Product
            {
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                ManufacturerID = reader.IsDBNull(reader.GetOrdinal("ManufacturerID")) ? null : reader.GetInt32(reader.GetOrdinal("ManufacturerID")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                MinimumStockThreshold = reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                IsLowStock = reader.GetInt32(reader.GetOrdinal("Stock")) <= reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold"))
            };
        }

        // Helper method to map reader to product with additional details
        private static Product MapProductWithDetails(IDataReader reader)
        {
            if (reader == null) 
                throw new ArgumentNullException(nameof(reader));
                
            var product = MapProduct(reader);

            // Add related information - safely check for column existence first
            DataTable schemaTable = reader.GetSchemaTable();
            
            if (schemaTable != null)
            {
                bool hasCategory = SchemaContainsColumn(schemaTable, "CategoryName");
                bool hasManufacturer = SchemaContainsColumn(schemaTable, "ManufacturerName");
                bool hasLowStock = SchemaContainsColumn(schemaTable, "IsLowStock");
                
                if (hasCategory && !reader.IsDBNull(reader.GetOrdinal("CategoryName")))
                    product.CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"));

                if (hasManufacturer && !reader.IsDBNull(reader.GetOrdinal("ManufacturerName")))
                    product.ManufacturerName = reader.GetString(reader.GetOrdinal("ManufacturerName"));

                if (hasLowStock)
                    product.IsLowStock = reader.GetBoolean(reader.GetOrdinal("IsLowStock"));
                else
                    product.IsLowStock = product.Stock <= product.MinimumStockThreshold;
            }

            return product;
        }

        // Helper method to check if a column exists in schema
        private static bool SchemaContainsColumn(DataTable schema, string columnName)
        {
            foreach (DataRow row in schema.Rows)
            {
                if (string.Equals(row["ColumnName"].ToString(), columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private static void EnsureCategoriesTableExists(SqliteConnection connection)
        {
            string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Category';";
            using (var command = new SqliteCommand(checkTableQuery, connection))
            {
                if (command.ExecuteScalar() == null)
                {
                    using (var createCommand = new SqliteCommand(@"
                        CREATE TABLE IF NOT EXISTS Category (
                            CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                            CategoryName TEXT NOT NULL UNIQUE,
                            Description TEXT,
                            IsActive INTEGER NOT NULL DEFAULT 1,
                            CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                        );", connection))
                    {
                        createCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public static List<Product> GetProductsByCategory(int categoryId)
        {
            string query = @"
                SELECT p.*, c.CategoryName, m.ManufacturerName
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                WHERE p.CategoryID = @CategoryID";

            var parameters = new Dictionary<string, object>
            {
                { "@CategoryID", categoryId }
            };

            return DataAccessHelper.ExecuteReader(query, reader => new Product
            {
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                ManufacturerID = reader.IsDBNull(reader.GetOrdinal("ManufacturerID")) ? null : reader.GetInt32(reader.GetOrdinal("ManufacturerID")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                MinimumStockThreshold = reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoryName")),
                ManufacturerName = reader.IsDBNull(reader.GetOrdinal("ManufacturerName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ManufacturerName")),
                IsLowStock = reader.GetInt32(reader.GetOrdinal("Stock")) <= reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold"))
            }, parameters);
        }
    }
}
