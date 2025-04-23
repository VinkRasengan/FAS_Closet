// This file defines the InventoryManager class, which handles stock operations for Products.

using System;
using System.Collections.Generic;
using FASCloset.Models;
using FASCloset.Data;
using FASCloset.Config;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace FASCloset.Services
{
    public static class InventoryManager
    {
        private static string GetConnectionString()
        {
            return $"Data Source={AppSettings.DatabasePath}";
        }

        // Update stock quantity for a product - optimized version
        public static void UpdateStock(int productId, int newStock)
        {
            string connectionString = GetConnectionString();
            string productUpdate = "UPDATE Product SET Stock = @Stock WHERE ProductID = @ProductID";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        // Update Product table
                        using (var cmd = new SqliteCommand(productUpdate, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ProductID", productId);
                            cmd.Parameters.AddWithValue("@Stock", newStock);
                            cmd.ExecuteNonQuery();
                        }
                        
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating stock: {ex.Message}");
                throw new ApplicationException($"Failed to update stock quantity: {ex.Message}", ex);
            }
        }

        // Update minimum stock threshold for a product
        public static void UpdateMinimumStockThreshold(int productId, int newThreshold)
        {
            string connectionString = GetConnectionString();
            string productUpdate = "UPDATE Product SET MinimumStockThreshold = @MinimumStockThreshold WHERE ProductID = @ProductID";

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        using (var cmd = new SqliteCommand(productUpdate, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ProductID", productId);
                            cmd.Parameters.AddWithValue("@MinimumStockThreshold", newThreshold);
                            cmd.ExecuteNonQuery();
                        }
                        
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating minimum stock threshold: {ex.Message}");
                throw new ApplicationException($"Failed to update minimum stock threshold: {ex.Message}", ex);
            }
        }

        // Async version of UpdateStock to prevent UI freezing
        public static async Task UpdateStockAsync(int productId, int newStock)
        {
            await Task.Run(() => UpdateStock(productId, newStock));
        }

        // Async version of UpdateMinimumStockThreshold
        public static async Task UpdateMinimumStockThresholdAsync(int productId, int newThreshold)
        {
            await Task.Run(() => UpdateMinimumStockThreshold(productId, newThreshold));
        }

        // Get products with low stock (based on individual product thresholds)
        public static List<LowStockProductView> GetLowStockProducts()
        {
            try
            {
                string query = @"
                    SELECT 
                        p.ProductID, 
                        p.ProductName, 
                        p.CategoryID, 
                        p.ManufacturerID,
                        p.Price, 
                        p.Stock AS StockQuantity, 
                        p.Description, 
                        p.IsActive,
                        p.MinimumStockThreshold,
                        c.CategoryName, 
                        m.ManufacturerName
                    FROM Product p
                    LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                    LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                    WHERE p.Stock <= p.MinimumStockThreshold AND p.IsActive = 1
                    ORDER BY 
                        CASE WHEN p.Stock = 0 THEN 0 ELSE 1 END, -- Out of stock items first
                        (1.0 * p.Stock / p.MinimumStockThreshold) ASC, -- Then by % of threshold
                        p.ProductName -- Then alphabetically
                    ";

                var lowStockProducts = DataAccessHelper.ExecuteReader(query, reader => {
                    try {
                        // Get all required fields safely
                        int productId = reader.GetInt32(reader.GetOrdinal("ProductID"));
                        string productName = reader.GetString(reader.GetOrdinal("ProductName"));
                        int categoryId = reader.GetInt32(reader.GetOrdinal("CategoryID")); 
                        int? manufacturerId = reader.IsDBNull(reader.GetOrdinal("ManufacturerID")) ? 
                            null : reader.GetInt32(reader.GetOrdinal("ManufacturerID"));
                        decimal price = reader.GetDecimal(reader.GetOrdinal("Price"));
                        int stockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"));
                        int minimumStockThreshold = reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold"));
                        string description = reader.GetString(reader.GetOrdinal("Description"));
                        bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                        
                        // Optional fields
                        string categoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? 
                            string.Empty : reader.GetString(reader.GetOrdinal("CategoryName"));
                        string manufacturerName = reader.IsDBNull(reader.GetOrdinal("ManufacturerName")) ? 
                            string.Empty : reader.GetString(reader.GetOrdinal("ManufacturerName"));
                        
                        // Create the object with all retrieved values
                        return new LowStockProductView
                        {
                            ProductID = productId,
                            ProductName = productName,
                            CategoryID = categoryId,
                            ManufacturerID = manufacturerId,
                            Price = price,
                            StockQuantity = stockQuantity,
                            Description = description,
                            IsActive = isActive,
                            CategoryName = categoryName,
                            ManufacturerName = manufacturerName,
                            MinimumStockThreshold = minimumStockThreshold
                        };
                    }
                    catch (Exception ex) {
                        Console.WriteLine($"Error mapping low stock product: {ex.Message}");
                        return null;
                    }
                });
                
                // Filter out any nulls from mapping errors
                return lowStockProducts.Where(p => p != null).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting low stock products: {ex.Message}");
                return new List<LowStockProductView>();
            }
        }

        // Async version of GetLowStockProducts
        public static async Task<List<LowStockProductView>> GetLowStockProductsAsync()
        {
            return await Task.Run(() => GetLowStockProducts());
        }

        // Get out of stock products (Stock = 0)
        public static List<Product> GetOutOfStockProducts()
        {
            string query = @"
                SELECT p.*, c.CategoryName, m.ManufacturerName
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
                MinimumStockThreshold = reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoryName")),
                ManufacturerName = reader.IsDBNull(reader.GetOrdinal("ManufacturerName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ManufacturerName")),
                IsLowStock = true
            });
        }

        // Get all products (optionally only active ones)
        public static List<Product> GetAllProducts(bool onlyActive = true)
        {
            string query = @"
                SELECT p.*, c.CategoryName, m.ManufacturerName
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                " + (onlyActive ? "WHERE p.IsActive = 1" : string.Empty) + @"
                ORDER BY p.ProductName";

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
            });
        }

        // Async version of GetAllProducts
        public static async Task<List<Product>> GetAllProductsAsync(bool onlyActive = true)
        {
            return await Task.Run(() => GetAllProducts(onlyActive));
        }
    }
}
