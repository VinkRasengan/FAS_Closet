// This file defines the InventoryManager class, which handles inventory-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Models;

namespace FASCloset.Services
{
    public static class InventoryManager
    {
        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
        }

        public static void UpdateStock(int productId, int stockQuantity)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "UPDATE Inventory SET StockQuantity = @StockQuantity WHERE ProductID = @ProductID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                        command.Parameters.AddWithValue("@ProductID", productId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while updating stock.", ex);
            }
        }

        public static void SetMinimumStockThreshold(int productId, int minimumStockThreshold)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "UPDATE Inventory SET MinimumStockThreshold = @MinimumStockThreshold WHERE ProductID = @ProductID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MinimumStockThreshold", minimumStockThreshold);
                        command.Parameters.AddWithValue("@ProductID", productId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while setting minimum stock threshold.", ex);
            }
        }

        public static List<Inventory> GetLowStockProducts()
        {
            var lowStockProducts = new List<Inventory>();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT ProductID, StockQuantity, MinimumStockThreshold FROM Inventory WHERE StockQuantity < MinimumStockThreshold";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lowStockProducts.Add(new Inventory
                                {
                                    ProductID = reader.GetInt32(0),
                                    StockQuantity = reader.GetInt32(1),
                                    MinimumStockThreshold = reader.GetInt32(2)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving low stock products.", ex);
            }
            return lowStockProducts;
        }
    }
}
