// This file defines the InventoryManager class, which handles stock operations for Products.

using System;
using System.Collections.Generic;
using FASCloset.Models;
using FASCloset.Data;
using FASCloset.Config;
using Microsoft.Data.Sqlite;

namespace FASCloset.Services
{
    public static class InventoryManager
    {
        private static string GetConnectionString()
        {
            return $"Data Source={AppSettings.DatabasePath}";
        }

        // Update stock quantity for a product
        public static void UpdateStock(int productId, int newStock)
        {
            DatabaseConnection.ExecuteWithTransaction((connection, transaction) =>
            {
                try
                {
                    string updateProductQuery = "UPDATE Product SET Stock = @Stock WHERE ProductID = @ProductID";
                    using var productCmd = new SqliteCommand(updateProductQuery, connection, transaction);
                    productCmd.Parameters.AddWithValue("@ProductID", productId);
                    productCmd.Parameters.AddWithValue("@Stock", newStock);
                    productCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Error updating stock: {ex.Message}", ex);
                }
            });
        }

        // Get products with low stock (less than 5)
        public static List<Product> GetLowStockProducts()
        {
            string query = @"
                SELECT p.*, c.CategoryName, m.ManufacturerName
                FROM Product p
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                LEFT JOIN Manufacturer m ON p.ManufacturerID = m.ManufacturerID
                WHERE p.Stock < 5 AND p.IsActive = 1
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
                Description = reader.GetString(reader.GetOrdinal("Description")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoryName")),
                ManufacturerName = reader.IsDBNull(reader.GetOrdinal("ManufacturerName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ManufacturerName")),
                IsLowStock = reader.GetInt32(reader.GetOrdinal("Stock")) < 5
            });
        }
    }
}
