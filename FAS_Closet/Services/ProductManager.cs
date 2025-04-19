// This file defines the ProductManager class, which handles product-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Models;
using FASCloset.Data;

namespace FASCloset.Services
{
    /// <summary>
    /// Provides methods for managing products, categories, and manufacturers.
    /// </summary>
    public static class ProductManager
    {
        private const string DescriptionParameter = "@Description";

        /// <summary>
        /// Gets the connection string for the database.
        /// </summary>
        /// <returns>Connection string.</returns>
        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
        }

        /// <summary>
        /// Retrieves all products with category and manufacturer names.
        /// </summary>
        /// <param name="includeInactive">Whether to include inactive products.</param>
        /// <returns>List of products.</returns>
        public static List<Product> GetProducts(bool includeInactive = false)
        {
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
                return new List<Product>();
            }
        }

        /// <summary>
        /// Retrieves all products regardless of status.
        /// This is an alias for GetProducts(true) for better code readability.
        /// </summary>
        /// <returns>List of all products.</returns>
        public static List<Product> GetAllProducts()
        {
            // Include inactive products by default
            return GetProducts(true);
        }

        /// <summary>
        /// Gets all products with control over including inactive products.
        /// </summary>
        /// <param name="includeInactive">Whether to include inactive products</param>
        /// <returns>List of products</returns>
        public static List<Product> GetAllProducts(bool includeInactive)
        {
            return GetProducts(includeInactive);
        }

        /// <summary>
        /// Retrieves all products with optional filtering by category.
        /// </summary>
        /// <param name="categoryId">Category ID to filter by, or null for all products.</param>
        /// <param name="includeInactive">Whether to include inactive products.</param>
        /// <returns>List of products filtered by category if specified.</returns>
        public static List<Product> GetAllProducts(int? categoryId, bool includeInactive = false)
        {
            if (categoryId.HasValue)
            {
                return GetProductsByCategory(categoryId.Value, includeInactive);
            }
            else
            {
                return GetProducts(includeInactive);
            }
        }

        /// <summary>
        /// Retrieves products by category with additional details.
        /// </summary>
        /// <param name="categoryId">Category ID to filter by.</param>
        /// <param name="includeInactive">Whether to include inactive products.</param>
        /// <returns>List of products in the specified category.</returns>
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

        /// <summary>
        /// Retrieves a product by its name.
        /// </summary>
        /// <param name="name">Name of the product.</param>
        /// <returns>Product object or null if not found.</returns>
        public static Product? GetProductByName(string name)
        {
            string query = "SELECT * FROM Product WHERE ProductName = @Name";
            var parameters = new Dictionary<string, object> { { "@Name", name } };

            return DataAccessHelper.ExecuteReaderSingle(query, reader => MapProduct(reader), parameters);
        }

        /// <summary>
        /// Retrieves a product by its ID with related information.
        /// </summary>
        /// <param name="productId">ID of the product.</param>
        /// <returns>Product object or null if not found.</returns>
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

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="product">Product object to add.</param>
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

            string inventoryQuery = @"
                INSERT OR IGNORE INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) 
                VALUES (@ProductID, @StockQuantity, @MinimumStockThreshold)";

            var inventoryParams = new Dictionary<string, object>
            {
                { "@ProductID", productId },
                { "@StockQuantity", product.Stock },
                { "@MinimumStockThreshold", 10 }
            };

            DataAccessHelper.ExecuteNonQuery(inventoryQuery, inventoryParams);
        }

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="product">Updated product information.</param>
        public static void UpdateProduct(Product product)
        {
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

            if (stockChanged)
            {
                InventoryManager.UpdateStock(product.ProductID, product.Stock);
            }
        }

        /// <summary>
        /// Marks a product as inactive in the database.
        /// </summary>
        /// <param name="productId">ID of the product to mark as inactive.</param>
        public static void DeleteProduct(int productId)
        {
            string query = "UPDATE Product SET IsActive = 0 WHERE ProductID = @ProductID";
            var parameters = new Dictionary<string, object> { { "@ProductID", productId } };
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Permanently deletes a product from the database.
        /// </summary>
        /// <param name="productId">ID of the product to delete.</param>
        public static void PermanentlyDeleteProduct(int productId)
        {
            string query = "DELETE FROM Product WHERE ProductID = @ProductID";
            var parameters = new Dictionary<string, object> { { "@ProductID", productId } };
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Retrieves all active categories.
        /// </summary>
        /// <returns>List of categories.</returns>
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

        /// <summary>
        /// Retrieves all manufacturers.
        /// </summary>
        /// <returns>List of manufacturers.</returns>
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

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="category">Category object to add.</param>
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

        /// <summary>
        /// Adds a new manufacturer to the database.
        /// </summary>
        /// <param name="manufacturer">Manufacturer object to add.</param>
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

        /// <summary>
        /// Updates an existing category in the database.
        /// </summary>
        /// <param name="category">Updated category information.</param>
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

        /// <summary>
        /// Updates an existing manufacturer in the database.
        /// </summary>
        /// <param name="manufacturer">Updated manufacturer information.</param>
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

        /// <summary>
        /// Checks if a category name is already in use.
        /// </summary>
        /// <param name="categoryName">Category name to check.</param>
        /// <returns>True if the name is taken, otherwise false.</returns>
        public static bool IsCategoryNameTaken(string categoryName)
        {
            string query = "SELECT COUNT(*) FROM Category WHERE CategoryName = @CategoryName";
            var parameters = new Dictionary<string, object> { { "@CategoryName", categoryName } };
            int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            return count > 0;
        }

        /// <summary>
        /// Checks if a manufacturer name is already in use.
        /// </summary>
        /// <param name="manufacturerName">Manufacturer name to check.</param>
        /// <returns>True if the name is taken, otherwise false.</returns>
        public static bool IsManufacturerNameTaken(string manufacturerName)
        {
            string query = "SELECT COUNT(*) FROM Manufacturer WHERE ManufacturerName = @ManufacturerName";
            var parameters = new Dictionary<string, object> { { "@ManufacturerName", manufacturerName } };
            int count = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            return count > 0;
        }

        /// <summary>
        /// Searches products by name or description.
        /// </summary>
        /// <param name="searchText">Text to search for.</param>
        /// <param name="includeInactive">Whether to include inactive products.</param>
        /// <returns>List of matching products.</returns>
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
        /// Retrieves all products with warehouse-specific stock information.
        /// </summary>
        /// <param name="warehouseId">ID of the warehouse.</param>
        /// <param name="includeInactive">Whether to include inactive products.</param>
        /// <returns>List of products with warehouse-specific stock information.</returns>
        public static List<Product> GetProductsForWarehouse(int warehouseId, bool includeInactive = false)
        {
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

        /// <summary>
        /// Maps a database record to a Product object.
        /// </summary>
        /// <param name="reader">Data reader containing product data.</param>
        /// <returns>Populated Product object.</returns>
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

        /// <summary>
        /// Maps a database record to a Product object with additional details.
        /// </summary>
        /// <param name="reader">Data reader containing product data.</param>
        /// <returns>Populated Product object with additional details.</returns>
        private static Product MapProductWithDetails(IDataReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var product = MapProduct(reader);

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

        /// <summary>
        /// Checks if a column exists in the schema.
        /// </summary>
        /// <param name="schema">Schema table.</param>
        /// <param name="columnName">Column name to check.</param>
        /// <returns>True if the column exists, otherwise false.</returns>
        private static bool SchemaContainsColumn(DataTable schema, string columnName)
        {
            foreach (DataRow row in schema.Rows)
            {
                if (string.Equals(row["ColumnName"].ToString(), columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Ensures the Categories table exists in the database.
        /// </summary>
        /// <param name="connection">Database connection.</param>
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

        /// <summary>
        /// Retrieves products by category ID.
        /// </summary>
        /// <param name="categoryId">Category ID to filter by.</param>
        /// <returns>List of products in the specified category.</returns>
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
