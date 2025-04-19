// This file defines the InventoryManager class, which handles stock operations for Products.

using System;
using System.Collections.Generic;
using System.Data;
using FASCloset.Data;
using FASCloset.Models;

namespace FASCloset.Services
{
    /// <summary>
    /// Manages inventory operations including stock management and tracking
    /// </summary>
    public static class InventoryManager
    {
        /// <summary>
        /// Updates stock quantity for a product
        /// </summary>
        /// <param name="productId">ID of the product to update</param>
        /// <param name="newQuantity">New stock quantity</param>
        /// <returns>True if update was successful</returns>
        public static bool UpdateStock(int productId, int newQuantity)
        {
            try
            {
                // Update product table stock
                string queryProduct = "UPDATE Product SET Stock = @Quantity WHERE ProductID = @ProductID";
                var parametersProduct = new Dictionary<string, object>
                {
                    { "@ProductID", productId },
                    { "@Quantity", newQuantity }
                };
                
                int rowsAffectedProduct = DataAccessHelper.ExecuteNonQuery(queryProduct, parametersProduct);
                
                // Update inventory table stock
                string queryInventory = "UPDATE Inventory SET StockQuantity = @Quantity WHERE ProductID = @ProductID";
                var parametersInventory = new Dictionary<string, object>
                {
                    { "@ProductID", productId },
                    { "@Quantity", newQuantity }
                };
                
                int rowsAffectedInventory = DataAccessHelper.ExecuteNonQuery(queryInventory, parametersInventory);
                
                return (rowsAffectedProduct > 0) || (rowsAffectedInventory > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating stock: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Gets all products with low stock (below their minimum threshold)
        /// </summary>
        /// <returns>List of products with low stock</returns>
        public static List<Product> GetLowStockProducts()
        {
            string query = @"
                SELECT p.*, c.CategoryName, m.ManufacturerName, 
                       i.StockQuantity, i.MinimumStockThreshold
                FROM Product p
                INNER JOIN Inventory i ON p.ProductID = i.ProductID
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                WHERE p.Stock <= i.MinimumStockThreshold AND p.IsActive = 1
                ORDER BY p.Stock ASC";
            
            return DataAccessHelper.ExecuteReader(query, reader => {
                var product = new Product
                {
                    Id = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    Name = reader.GetString(reader.GetOrdinal("ProductName")),
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                    CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? "" : reader.GetString(reader.GetOrdinal("CategoryName")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    MinimumStockThreshold = reader.GetInt32(reader.GetOrdinal("MinimumStockThreshold"))
                };
                
                if (!reader.IsDBNull(reader.GetOrdinal("ManufacturerID")))
                {
                    product.ManufacturerId = reader.GetInt32(reader.GetOrdinal("ManufacturerID"));
                    product.ManufacturerName = reader.IsDBNull(reader.GetOrdinal("ManufacturerName")) ? "" : reader.GetString(reader.GetOrdinal("ManufacturerName"));
                }
                
                return product;
            });
        }
        
        /// <summary>
        /// Gets the count of products with stock below their minimum threshold
        /// </summary>
        /// <returns>Number of products with low stock</returns>
        public static int GetLowStockCount()
        {
            string query = @"
                SELECT COUNT(*) 
                FROM Product p
                INNER JOIN Inventory i ON p.ProductID = i.ProductID
                WHERE p.Stock <= i.MinimumStockThreshold AND p.IsActive = 1";
            
            return DataAccessHelper.ExecuteScalar<int>(query);
        }
        
        /// <summary>
        /// Updates the minimum stock threshold for a product
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <param name="threshold">New minimum threshold value</param>
        /// <returns>True if update was successful</returns>
        public static bool UpdateMinimumStockThreshold(int productId, int threshold)
        {
            try
            {
                string query = "UPDATE Inventory SET MinimumStockThreshold = @Threshold WHERE ProductID = @ProductID";
                var parameters = new Dictionary<string, object>
                {
                    { "@ProductID", productId },
                    { "@Threshold", threshold }
                };
                
                int rowsAffected = DataAccessHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating minimum stock threshold: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Transfers stock between products
        /// </summary>
        /// <param name="sourceProductId">ID of the source product</param>
        /// <param name="targetProductId">ID of the target product</param>
        /// <param name="quantity">Amount to transfer</param>
        /// <returns>True if transfer was successful</returns>
        public static bool TransferStock(int sourceProductId, int targetProductId, int quantity)
        {
            try
            {
                // Get current stock levels
                Product sourceProduct = ProductManager.GetProductById(sourceProductId);
                Product targetProduct = ProductManager.GetProductById(targetProductId);
                
                if (sourceProduct == null || targetProduct == null)
                    return false;
                
                if (sourceProduct.Stock < quantity)
                    return false; // Not enough stock to transfer
                
                // Perform transfer
                UpdateStock(sourceProductId, sourceProduct.Stock - quantity);
                UpdateStock(targetProductId, targetProduct.Stock + quantity);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error transferring stock: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Gets the minimum stock threshold for a product
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <returns>Minimum stock threshold value or 0 if not found</returns>
        public static int GetMinimumStockThreshold(int productId)
        {
            string query = "SELECT MinimumStockThreshold FROM Inventory WHERE ProductID = @ProductID";
            var parameters = new Dictionary<string, object> { { "@ProductID", productId } };
            
            try
            {
                return DataAccessHelper.ExecuteScalar<int>(query, parameters);
            }
            catch
            {
                return 0;
            }
        }
        
        /// <summary>
        /// Gets all products in the inventory
        /// </summary>
        /// <returns>List of all products</returns>
        public static List<Product> GetAllProducts()
        {
            // Delegate to ProductManager for consistency
            return ProductManager.GetAllProducts();
        }

        /// <summary>
        /// Gets all active products or optionally includes inactive ones
        /// </summary>
        /// <param name="includeInactive">Whether to include inactive products</param>
        /// <returns>List of products based on active status</returns>
        public static List<Product> GetAllProducts(bool includeInactive)
        {
            // Delegate to ProductManager for consistency
            return ProductManager.GetAllProducts(includeInactive);
        }
    }
}
