// This file defines the InventoryManager class, which handles inventory-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using FASCloset.Models;
using FASCloset.Data;
using FASCloset.Config;

namespace FASCloset.Services
{
    public static class InventoryManager
    {
        /// <summary>
        /// Gets the connection string for the database
        /// </summary>
        private static string GetConnectionString()
        {
            return $"Data Source={AppSettings.DatabasePath}";
        }

        // Get inventory by product ID
        public static Inventory? GetInventoryByProductId(int productId)
        {
            string query = "SELECT * FROM Inventory WHERE ProductID = @ProductID";
            var parameters = new Dictionary<string, object> { { "@ProductID", productId } };

            return DataAccessHelper.ExecuteReaderSingle(query, reader => new Inventory
            {
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                MinimumStockThreshold = reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold"))
            }, parameters);
        }

        // Update stock quantity for a product
        public static void UpdateStock(int productId, int newStock)
        {
            // Use DatabaseConnection.ExecuteWithTransaction instead
            DatabaseConnection.ExecuteWithTransaction((connection, transaction) =>
            {
                try
                {
                    // Update Product table
                    string updateProductQuery = "UPDATE Product SET Stock = @Stock WHERE ProductID = @ProductID";
                    using var productCmd = new SqliteCommand(updateProductQuery, connection, transaction);
                    productCmd.Parameters.AddWithValue("@ProductID", productId);
                    productCmd.Parameters.AddWithValue("@Stock", newStock);
                    productCmd.ExecuteNonQuery();

                    // Check if inventory record exists
                    string checkQuery = "SELECT COUNT(*) FROM Inventory WHERE ProductID = @ProductID";
                    using var checkCmd = new SqliteCommand(checkQuery, connection, transaction);
                    checkCmd.Parameters.AddWithValue("@ProductID", productId);
                    int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                    
                    if (exists > 0)
                    {
                        // Update existing inventory record
                        string updateInvQuery = "UPDATE Inventory SET StockQuantity = @StockQuantity WHERE ProductID = @ProductID";
                        using var updateCmd = new SqliteCommand(updateInvQuery, connection, transaction);
                        updateCmd.Parameters.AddWithValue("@ProductID", productId);
                        updateCmd.Parameters.AddWithValue("@StockQuantity", newStock);
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // Insert new inventory record
                        string insertInvQuery = @"
                            INSERT INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold) 
                            VALUES (@ProductID, @StockQuantity, @MinimumStockThreshold)";
                        using var insertCmd = new SqliteCommand(insertInvQuery, connection, transaction);
                        insertCmd.Parameters.AddWithValue("@ProductID", productId);
                        insertCmd.Parameters.AddWithValue("@StockQuantity", newStock);
                        insertCmd.Parameters.AddWithValue("@MinimumStockThreshold", 10); // Default threshold
                        insertCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Let the ExecuteWithTransaction handle the rollback
                    throw new ApplicationException($"Error updating stock: {ex.Message}", ex);
                }
            });
        }

        // Update stock quantity for a product in specific warehouse
        public static void UpdateStock(int productId, int warehouseId, int newStock)
        {
            // Use DatabaseConnection.ExecuteWithTransaction instead
            DatabaseConnection.ExecuteWithTransaction((connection, transaction) =>
            {
                try
                {
                    // Update Product table - aggregate stock from all warehouses
                    string updateProductQuery = @"
                        UPDATE Product SET Stock = (
                            SELECT SUM(StockQuantity) 
                            FROM Inventory 
                            WHERE ProductID = @ProductID 
                            AND WarehouseID <> @WarehouseID
                        ) + @Stock
                        WHERE ProductID = @ProductID";
                        
                    using var productCmd = new SqliteCommand(updateProductQuery, connection, transaction);
                    productCmd.Parameters.AddWithValue("@ProductID", productId);
                    productCmd.Parameters.AddWithValue("@WarehouseID", warehouseId);
                    productCmd.Parameters.AddWithValue("@Stock", newStock);
                    productCmd.ExecuteNonQuery();

                    // Check if inventory record exists for this product and warehouse
                    string checkQuery = "SELECT COUNT(*) FROM Inventory WHERE ProductID = @ProductID AND WarehouseID = @WarehouseID";
                    using var checkCmd = new SqliteCommand(checkQuery, connection, transaction);
                    checkCmd.Parameters.AddWithValue("@ProductID", productId);
                    checkCmd.Parameters.AddWithValue("@WarehouseID", warehouseId);
                    int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                    
                    if (exists > 0)
                    {
                        // Update existing inventory record
                        string updateInvQuery = "UPDATE Inventory SET StockQuantity = @StockQuantity WHERE ProductID = @ProductID AND WarehouseID = @WarehouseID";
                        using var updateCmd = new SqliteCommand(updateInvQuery, connection, transaction);
                        updateCmd.Parameters.AddWithValue("@ProductID", productId);
                        updateCmd.Parameters.AddWithValue("@WarehouseID", warehouseId);
                        updateCmd.Parameters.AddWithValue("@StockQuantity", newStock);
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // Insert new inventory record
                        string insertInvQuery = @"
                            INSERT INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold) 
                            VALUES (@ProductID, @WarehouseID, @StockQuantity, @MinimumStockThreshold)";
                        using var insertCmd = new SqliteCommand(insertInvQuery, connection, transaction);
                        insertCmd.Parameters.AddWithValue("@ProductID", productId);
                        insertCmd.Parameters.AddWithValue("@WarehouseID", warehouseId);
                        insertCmd.Parameters.AddWithValue("@StockQuantity", newStock);
                        insertCmd.Parameters.AddWithValue("@MinimumStockThreshold", 10); // Default threshold
                        insertCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Let the ExecuteWithTransaction handle the rollback
                    throw new ApplicationException($"Error updating stock: {ex.Message}", ex);
                }
            });
        }

        // Get inventory by product ID and warehouse
        public static Inventory? GetInventoryByProductAndWarehouse(int productId, int warehouseId)
        {
            string query = @"
                SELECT i.*, p.ProductName, w.Name as WarehouseName
                FROM Inventory i
                JOIN Product p ON i.ProductID = p.ProductID
                JOIN Warehouse w ON i.WarehouseID = w.WarehouseID
                WHERE i.ProductID = @ProductID AND i.WarehouseID = @WarehouseID";
                
            var parameters = new Dictionary<string, object> 
            { 
                { "@ProductID", productId },
                { "@WarehouseID", warehouseId }
            };

            return DataAccessHelper.ExecuteReaderSingle(query, reader => new Inventory
            {
                InventoryID = reader.GetInt32(reader.GetOrdinal("InventoryID")),
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                WarehouseID = reader.GetInt32(reader.GetOrdinal("WarehouseID")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                MinimumStockThreshold = reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold")),
                Product = new Product
                {
                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName"))
                },
                Warehouse = new Warehouse
                {
                    WarehouseID = reader.GetInt32(reader.GetOrdinal("WarehouseID")),
                    Name = reader.GetString(reader.GetOrdinal("WarehouseName"))
                }
            }, parameters);
        }

        // Get inventory for a specific warehouse
        public static List<Inventory> GetWarehouseInventory(int warehouseId)
        {
            string query = @"
                SELECT i.*, p.ProductName, p.Description as ProductDescription, 
                       p.Price, p.IsActive, c.CategoryName, 
                       w.Name as WarehouseName
                FROM Inventory i
                JOIN Product p ON i.ProductID = p.ProductID
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                JOIN Warehouse w ON i.WarehouseID = w.WarehouseID
                WHERE i.WarehouseID = @WarehouseID
                ORDER BY p.ProductName";
                
            var parameters = new Dictionary<string, object> { { "@WarehouseID", warehouseId } };

            return DataAccessHelper.ExecuteReader(query, reader => {
                // Create the inventory object
                var inventory = new Inventory
                {
                    InventoryID = reader.GetInt32(reader.GetOrdinal("InventoryID")),
                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    WarehouseID = reader.GetInt32(reader.GetOrdinal("WarehouseID")),
                    StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                    MinimumStockThreshold = reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold")),
                    Product = new Product
                    {
                        ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        Description = reader.GetString(reader.GetOrdinal("ProductDescription")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                        CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                    },
                    Warehouse = new Warehouse
                    {
                        WarehouseID = reader.GetInt32(reader.GetOrdinal("WarehouseID")),
                        Name = reader.GetString(reader.GetOrdinal("WarehouseName"))
                    }
                    // IsLowStock is calculated automatically
                };
                
                return inventory;
            }, parameters);
        }

        // Get products with stock below minimum threshold
        public static List<Product> GetLowStockProducts()
        {
            string query = @"
                SELECT p.*, c.CategoryName, m.ManufacturerName, 1 as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                LEFT JOIN Inventory i ON p.ProductID = i.ProductID
                WHERE p.Stock <= COALESCE(i.MinimumStockThreshold, 10) AND p.IsActive = 1
                ORDER BY p.Stock ASC";

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
                IsLowStock = true
            });
        }

        // Get products with stock below minimum threshold for a specific warehouse
        public static List<Inventory> GetLowStockProducts(int warehouseId)
        {
            string query = @"
                SELECT i.*, p.ProductName, p.CategoryID, p.Price, p.Description, p.IsActive,
                       c.CategoryName,
                       i.StockQuantity, i.MinimumStockThreshold,
                       CASE WHEN i.StockQuantity <= i.MinimumStockThreshold THEN 1 ELSE 0 END AS IsLowStockFlag
                FROM Inventory i
                JOIN Product p ON i.ProductID = p.ProductID
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                WHERE i.WarehouseID = @WarehouseID AND p.IsActive = 1
                ORDER BY i.StockQuantity ASC";
            
            var parameters = new Dictionary<string, object>
            {
                { "@WarehouseID", warehouseId }
            };
            
            return DataAccessHelper.ExecuteReader(query, reader => {
                // Create the inventory object with all properties except IsLowStock
                // since it's a calculated property that will be set automatically
                var inventory = new Inventory
                {
                    InventoryID = reader.GetInt32(reader.GetOrdinal("InventoryID")),
                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    WarehouseID = reader.GetInt32(reader.GetOrdinal("WarehouseID")),
                    StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                    MinimumStockThreshold = reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold")),
                    Product = new Product
                    {
                        ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                        CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoryName"))
                    }
                    // IsLowStock is calculated automatically based on StockQuantity and MinimumStockThreshold
                };
                
                return inventory;
            }, parameters);
        }

        // Get out of stock products (Stock = 0)
        public static List<Product> GetOutOfStockProducts()
        {
            string query = @"
                SELECT p.*, c.CategoryName, m.ManufacturerName, 1 as IsLowStock
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                WHERE p.Stock = 0 AND p.IsActive = 1
                ORDER BY p.ProductName";

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
                IsLowStock = true
            });
        }

        // Set minimum stock threshold for a product
        public static void SetMinimumStockThreshold(int productId, int threshold)
        {
            string query = @"
                INSERT INTO Inventory (ProductID, StockQuantity, MinimumStockThreshold)
                VALUES (@ProductID, (SELECT Stock FROM Product WHERE ProductID = @ProductID), @MinimumStockThreshold)
                ON CONFLICT(ProductID) 
                DO UPDATE SET MinimumStockThreshold = @MinimumStockThreshold";

            var parameters = new Dictionary<string, object>
            {
                { "@ProductID", productId },
                { "@MinimumStockThreshold", threshold }
            };

            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }

        // Get current minimum stock threshold for a product
        public static int GetMinimumThreshold(int productId)
        {
            string query = "SELECT MinimumStockThreshold FROM Inventory WHERE ProductID = @ProductID";
            var parameters = new Dictionary<string, object> { { "@ProductID", productId } };

            object result = DataAccessHelper.ExecuteScalar<object>(query, parameters);
            return result == null ? 10 : Convert.ToInt32(result); // Default threshold is 10
        }

        // Get total stock across all warehouses for a product
        public static int GetTotalStock(int productId)
        {
            string query = @"
                SELECT SUM(StockQuantity) as TotalStock
                FROM Inventory 
                WHERE ProductID = @ProductID";
                
            var parameters = new Dictionary<string, object> { { "@ProductID", productId } };

            object result = DataAccessHelper.ExecuteScalar<object>(query, parameters);
            return result == null ? 0 : Convert.ToInt32(result);
        }

        // Get inventory records for a product across all warehouses
        public static List<Inventory> GetProductInventory(int productId)
        {
            string query = @"
                SELECT i.*, w.Name as WarehouseName
                FROM Inventory i
                JOIN Warehouse w ON i.WarehouseID = w.WarehouseID
                WHERE i.ProductID = @ProductID
                ORDER BY i.WarehouseID";
                
            var parameters = new Dictionary<string, object> { { "@ProductID", productId } };

            return DataAccessHelper.ExecuteReader(query, reader => new Inventory
            {
                InventoryID = reader.GetInt32(reader.GetOrdinal("InventoryID")),
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                WarehouseID = reader.GetInt32(reader.GetOrdinal("WarehouseID")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                MinimumStockThreshold = reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold")),
                Warehouse = new Warehouse
                {
                    WarehouseID = reader.GetInt32(reader.GetOrdinal("WarehouseID")),
                    Name = reader.GetString(reader.GetOrdinal("WarehouseName"))
                }
            }, parameters);
        }

        // Transfer stock between warehouses
        public static void TransferStock(int productId, int fromWarehouseId, int toWarehouseId, int quantity)
        {
            if (fromWarehouseId == toWarehouseId)
                throw new ArgumentException("Source and destination warehouses must be different");
                
            if (quantity <= 0)
                throw new ArgumentException("Transfer quantity must be positive");
                
            // Get current stock in source warehouse
            var sourceInventory = GetInventoryByProductAndWarehouse(productId, fromWarehouseId);
            if (sourceInventory == null || sourceInventory.StockQuantity < quantity)
                throw new InvalidOperationException("Insufficient stock in source warehouse");
                
            // Execute as a transaction
            DatabaseConnection.ExecuteWithTransaction((connection, transaction) =>
            {
                try
                {
                    // Reduce stock in source warehouse
                    string reduceQuery = @"
                        UPDATE Inventory 
                        SET StockQuantity = StockQuantity - @Quantity 
                        WHERE ProductID = @ProductID AND WarehouseID = @WarehouseID";
                        
                    using (var reduceCmd = new SqliteCommand(reduceQuery, connection, transaction))
                    {
                        reduceCmd.Parameters.AddWithValue("@ProductID", productId);
                        reduceCmd.Parameters.AddWithValue("@WarehouseID", fromWarehouseId);
                        reduceCmd.Parameters.AddWithValue("@Quantity", quantity);
                        reduceCmd.ExecuteNonQuery();
                    }
                    
                    // Check if destination has an inventory record
                    string checkQuery = @"
                        SELECT COUNT(*) 
                        FROM Inventory 
                        WHERE ProductID = @ProductID AND WarehouseID = @WarehouseID";
                        
                    using (var checkCmd = new SqliteCommand(checkQuery, connection, transaction))
                    {
                        checkCmd.Parameters.AddWithValue("@ProductID", productId);
                        checkCmd.Parameters.AddWithValue("@WarehouseID", toWarehouseId);
                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                        
                        if (exists > 0)
                        {
                            // Increase stock in destination warehouse
                            string increaseQuery = @"
                                UPDATE Inventory 
                                SET StockQuantity = StockQuantity + @Quantity 
                                WHERE ProductID = @ProductID AND WarehouseID = @WarehouseID";
                                
                            using (var increaseCmd = new SqliteCommand(increaseQuery, connection, transaction))
                            {
                                increaseCmd.Parameters.AddWithValue("@ProductID", productId);
                                increaseCmd.Parameters.AddWithValue("@WarehouseID", toWarehouseId);
                                increaseCmd.Parameters.AddWithValue("@Quantity", quantity);
                                increaseCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Create new inventory record for destination
                            string insertQuery = @"
                                INSERT INTO Inventory (ProductID, WarehouseID, StockQuantity, MinimumStockThreshold)
                                VALUES (@ProductID, @WarehouseID, @Quantity, 10)";
                                
                            using (var insertCmd = new SqliteCommand(insertQuery, connection, transaction))
                            {
                                insertCmd.Parameters.AddWithValue("@ProductID", productId);
                                insertCmd.Parameters.AddWithValue("@WarehouseID", toWarehouseId);
                                insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                    
                    // Update the total stock in the Product table
                    string updateTotalQuery = @"
                        UPDATE Product 
                        SET Stock = (SELECT SUM(StockQuantity) FROM Inventory WHERE ProductID = @ProductID)
                        WHERE ProductID = @ProductID";
                        
                    using (var updateTotalCmd = new SqliteCommand(updateTotalQuery, connection, transaction))
                    {
                        updateTotalCmd.Parameters.AddWithValue("@ProductID", productId);
                        updateTotalCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Error transferring stock: {ex.Message}", ex);
                }
            });
        }
    }
}
