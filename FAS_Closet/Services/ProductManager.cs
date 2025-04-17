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
                       CASE WHEN p.Stock <= COALESCE(i.MinimumStockThreshold, 10) THEN 1 ELSE 0 END as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                LEFT JOIN Inventory i ON p.ProductID = i.ProductID
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

        // Get products by category with additional details
        public static List<Product> GetProductsByCategory(int categoryId, bool includeInactive = false)
        {
            string query = @"
                SELECT p.*, 
                       c.CategoryName, 
                       m.ManufacturerName,
                       CASE WHEN p.Stock <= COALESCE(i.MinimumStockThreshold, 10) THEN 1 ELSE 0 END as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                LEFT JOIN Inventory i ON p.ProductID = i.ProductID
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
                       CASE WHEN p.Stock <= COALESCE(i.MinimumStockThreshold, 10) THEN 1 ELSE 0 END as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                LEFT JOIN Inventory i ON p.ProductID = i.ProductID
                WHERE p.ProductID = @ProductID";

            var parameters = new Dictionary<string, object> { { "@ProductID", productId } };

            return DataAccessHelper.ExecuteReaderSingle(query, reader => MapProductWithDetails(reader), parameters);
        }

        // Add a new product
        public static void AddProduct(Product product)
        {
            string query = @"
                INSERT INTO Product (ProductName, CategoryID, ManufacturerID, Price, Stock, Description, IsActive) 
                VALUES (@ProductName, @CategoryID, @ManufacturerID, @Price, @Stock, @Description, @IsActive);
                SELECT last_insert_rowid();";

            var parameters = new Dictionary<string, object>
            {
                { "@ProductName", product.ProductName },
                { "@CategoryID", product.CategoryID },
                { "@ManufacturerID", product.ManufacturerID ?? (object)DBNull.Value },
                { "@Price", product.Price },
                { "@Stock", product.Stock },
                { "@Description", product.Description },
                { "@IsActive", product.IsActive ? 1 : 0 }
            };

            int productId = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            
            // Add default inventory record
            string inventoryQuery = @"
                INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) 
                VALUES (@ProductID, @StockQuantity, @MinimumStockThreshold)";

            var inventoryParams = new Dictionary<string, object>
            {
                { "@ProductID", productId },
                { "@StockQuantity", product.Stock },
                { "@MinimumStockThreshold", 10 } // Default threshold
            };

            DataAccessHelper.ExecuteNonQuery(inventoryQuery, inventoryParams);
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
                    IsActive = @IsActive
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
                { "@IsActive", product.IsActive ? 1 : 0 }
            };

            DataAccessHelper.ExecuteNonQuery(query, parameters);

            // If stock changed, update the inventory table as well
            if (stockChanged)
            {
                InventoryManager.UpdateStock(product.ProductID, product.Stock);
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

        // Search products by name or description
        public static List<Product> SearchProducts(string searchText, bool includeInactive = false)
        {
            string query = @"
                SELECT p.*, 
                       c.CategoryName, 
                       m.ManufacturerName,
                       CASE WHEN p.Stock <= COALESCE(i.MinimumStockThreshold, 10) THEN 1 ELSE 0 END as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                LEFT JOIN Inventory i ON p.ProductID = i.ProductID
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

        /// <summary>
        /// Get all products with warehouse-specific stock information
        /// </summary>
        public static List<Product> GetProductsForWarehouse(int warehouseId, bool includeInactive = false)
        {
            // Define SQL parameter name constants
            const string warehouseIdParam = "@WarehouseID";
            const string includeInactiveParam = "@IncludeInactive";

            string query = @"
                SELECT p.*, 
                       c.CategoryName, 
                       m.ManufacturerName,
                       i.StockQuantity as WarehouseStock,
                       CASE WHEN i.StockQuantity <= COALESCE(i.MinimumStockThreshold, 10) THEN 1 ELSE 0 END as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                LEFT JOIN Inventory i ON p.ProductID = i.ProductID AND i.WarehouseID = @WarehouseID
                WHERE (p.IsActive = 1 OR @IncludeInactive = 1)
                ORDER BY p.ProductName";

            var parameters = new Dictionary<string, object>
            {
                { warehouseIdParam, warehouseId },
                { includeInactiveParam, includeInactive ? 1 : 0 }
            };

            try
            {
                var products = DataAccessHelper.ExecuteReader(query, reader =>
                {
                    var product = MapProductWithDetails(reader);
                    
                    // Get warehouse-specific stock if available
                    if (!reader.IsDBNull(reader.GetOrdinal("WarehouseStock")))
                        product.Stock = reader.GetInt32(reader.GetOrdinal("WarehouseStock"));

                    return product;
                }, parameters);
                    
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
                Description = reader.GetString(reader.GetOrdinal("Description")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
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
                Description = reader.GetString(reader.GetOrdinal("Description")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoryName")),
                ManufacturerName = reader.IsDBNull(reader.GetOrdinal("ManufacturerName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ManufacturerName")),
            }, parameters);
        }

    }
}
