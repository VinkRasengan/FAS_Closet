// This file defines the OrderDetailManager class, which handles order detail-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Data;
using FASCloset.Models;

namespace FASCloset.Services
{
    public static class OrderDetailManager
    {
        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
        }
        
        /// <summary>
        /// Adds a new order detail
        /// </summary>
        /// <param name="orderDetail">The order detail to add</param>
        /// <returns>The ID of the newly added order detail</returns>
        public static int AddOrderDetail(OrderDetail orderDetail)
        {
            string query = @"
                INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice)
                VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice);
                SELECT last_insert_rowid();
            ";
            
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", orderDetail.OrderID },
                { "@ProductID", orderDetail.ProductID },
                { "@Quantity", orderDetail.Quantity },
                { "@UnitPrice", orderDetail.UnitPrice }
            };
            
            int orderDetailId = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            
            // Update the Product stock quantity
            string updateProductQuery = @"
                UPDATE Product 
                SET Stock = Stock - @Quantity 
                WHERE ProductID = @ProductID AND Stock >= @Quantity
            ";
            
            int rowsAffected = DataAccessHelper.ExecuteNonQuery(updateProductQuery, parameters);
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"Not enough stock for product ID {orderDetail.ProductID}");
            }
            
            return orderDetailId;
        }

        /// <summary>
        /// Updates an existing order detail
        /// </summary>
        /// <param name="orderDetail">The order detail to update</param>
        public static void UpdateOrderDetail(OrderDetail orderDetail)
        {
            // First get the existing order detail to calculate stock adjustment
            OrderDetail existingDetail = GetOrderDetailById(orderDetail.OrderDetailID);
            int stockDifference = orderDetail.Quantity - existingDetail.Quantity;
            
            // Start a transaction to ensure consistency
            DatabaseConnection.ExecuteWithTransaction((connection, transaction) =>
            {
                // Update the order detail
                string updateDetailQuery = @"
                    UPDATE OrderDetails 
                    SET OrderID = @OrderID,
                        ProductID = @ProductID,
                        Quantity = @Quantity,
                        UnitPrice = @UnitPrice
                    WHERE OrderDetailID = @OrderDetailID
                ";
                
                using (var updateCmd = new SqliteCommand(updateDetailQuery, connection, transaction))
                {
                    updateCmd.Parameters.AddWithValue("@OrderDetailID", orderDetail.OrderDetailID);
                    updateCmd.Parameters.AddWithValue("@OrderID", orderDetail.OrderID);
                    updateCmd.Parameters.AddWithValue("@ProductID", orderDetail.ProductID);
                    updateCmd.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);
                    updateCmd.Parameters.AddWithValue("@UnitPrice", orderDetail.UnitPrice);
                    
                    updateCmd.ExecuteNonQuery();
                }
                
                // Only update stock if there's a change in quantity
                if (stockDifference != 0)
                {
                    string updateStockQuery = @"
                        UPDATE Product 
                        SET Stock = Stock - @StockDifference 
                        WHERE ProductID = @ProductID AND (Stock >= @StockDifference OR @StockDifference < 0)
                    ";
                    
                    using (var stockCmd = new SqliteCommand(updateStockQuery, connection, transaction))
                    {
                        stockCmd.Parameters.AddWithValue("@ProductID", orderDetail.ProductID);
                        stockCmd.Parameters.AddWithValue("@StockDifference", stockDifference);
                        
                        int rowsAffected = stockCmd.ExecuteNonQuery();
                        if (rowsAffected == 0 && stockDifference > 0)
                        {
                            throw new InvalidOperationException($"Not enough stock for product ID {orderDetail.ProductID}");
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Deletes an order detail
        /// </summary>
        /// <param name="orderDetailId">The ID of the order detail to delete</param>
        public static void DeleteOrderDetail(int orderDetailId)
        {
            // First get the existing order detail to restore stock
            OrderDetail existingDetail = GetOrderDetailById(orderDetailId);
            
            // Start a transaction to ensure consistency
            DatabaseConnection.ExecuteWithTransaction((connection, transaction) =>
            {
                // Restore product stock
                string updateStockQuery = @"
                    UPDATE Product 
                    SET Stock = Stock + @Quantity 
                    WHERE ProductID = @ProductID
                ";
                
                using (var stockCmd = new SqliteCommand(updateStockQuery, connection, transaction))
                {
                    stockCmd.Parameters.AddWithValue("@ProductID", existingDetail.ProductID);
                    stockCmd.Parameters.AddWithValue("@Quantity", existingDetail.Quantity);
                    
                    stockCmd.ExecuteNonQuery();
                }
                
                // Delete the order detail
                string deleteQuery = @"
                    DELETE FROM OrderDetails 
                    WHERE OrderDetailID = @OrderDetailID
                ";
                
                using (var deleteCmd = new SqliteCommand(deleteQuery, connection, transaction))
                {
                    deleteCmd.Parameters.AddWithValue("@OrderDetailID", orderDetailId);
                    
                    deleteCmd.ExecuteNonQuery();
                }
            });
        }

        /// <summary>
        /// Gets a specific order detail by ID
        /// </summary>
        /// <param name="orderDetailId">The order detail ID</param>
        /// <returns>The order detail or null if not found</returns>
        public static OrderDetail GetOrderDetailById(int orderDetailId)
        {
            string query = @"
                SELECT od.OrderDetailID, od.OrderID, od.ProductID, od.Quantity, od.UnitPrice,
                       p.ProductName
                FROM OrderDetails od
                JOIN Product p ON od.ProductID = p.ProductID
                WHERE od.OrderDetailID = @OrderDetailID
            ";
            
            var parameters = new Dictionary<string, object>
            {
                { "@OrderDetailID", orderDetailId }
            };
            
            return DataAccessHelper.ExecuteReaderSingle(query, reader => new OrderDetail
            {
                OrderDetailID = Convert.ToInt32(reader["OrderDetailID"]),
                OrderID = Convert.ToInt32(reader["OrderID"]),
                ProductID = Convert.ToInt32(reader["ProductID"]),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                ProductName = reader["ProductName"].ToString()
            }, parameters);
        }
        
        /// <summary>
        /// Gets all order details for a specific order, redirecting to OrderManager
        /// to maintain a single source of truth
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>A list of order details</returns>
        public static List<OrderDetail> GetOrderDetails(int orderId)
        {
            string query = @"
                SELECT od.OrderDetailID, od.OrderID, od.ProductID, od.Quantity, od.UnitPrice,
                       p.ProductName
                FROM OrderDetails od
                JOIN Product p ON od.ProductID = p.ProductID
                WHERE od.OrderID = @OrderID
            ";
            
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", orderId }
            };
            
            return DataAccessHelper.ExecuteReader(query, reader => new OrderDetail
            {
                OrderDetailID = Convert.ToInt32(reader["OrderDetailID"]),
                OrderID = Convert.ToInt32(reader["OrderID"]),
                ProductID = Convert.ToInt32(reader["ProductID"]),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                ProductName = reader["ProductName"].ToString()
            }, parameters);
        }
    }
}
