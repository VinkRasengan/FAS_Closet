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
        /// <summary>
        /// Adds a new order detail
        /// </summary>
        /// <param name="orderDetail">The order detail to add</param>
        /// <returns>The ID of the newly added order detail</returns>
        public static int AddOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                string query = @"
                    INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice)
                    VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice);
                    SELECT last_insert_rowid();";
                    
                var parameters = new Dictionary<string, object>
                {
                    { "@OrderID", orderDetail.OrderID },
                    { "@ProductID", orderDetail.ProductID },
                    { "@Quantity", orderDetail.Quantity },
                    { "@UnitPrice", orderDetail.UnitPrice }
                };
                
                int orderDetailId = DataAccessHelper.ExecuteScalar<int>(query, parameters);
                orderDetail.OrderDetailID = orderDetailId;
                return orderDetailId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order detail: {ex.Message}");
                throw new InvalidOperationException("Database error occurred while adding order detail.", ex);
            }
        }

        /// <summary>
        /// Updates an existing order detail
        /// </summary>
        /// <param name="orderDetail">The order detail to update</param>
        public static void UpdateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                string query = @"
                    UPDATE OrderDetails 
                    SET OrderID = @OrderID, 
                        ProductID = @ProductID, 
                        Quantity = @Quantity, 
                        UnitPrice = @UnitPrice 
                    WHERE OrderDetailID = @OrderDetailID";
                    
                var parameters = new Dictionary<string, object>
                {
                    { "@OrderDetailID", orderDetail.OrderDetailID },
                    { "@OrderID", orderDetail.OrderID },
                    { "@ProductID", orderDetail.ProductID },
                    { "@Quantity", orderDetail.Quantity },
                    { "@UnitPrice", orderDetail.UnitPrice }
                };
                
                DataAccessHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order detail: {ex.Message}");
                throw new InvalidOperationException("Database error occurred while updating order detail.", ex);
            }
        }

        /// <summary>
        /// Deletes an order detail
        /// </summary>
        /// <param name="orderDetailId">The ID of the order detail to delete</param>
        public static void DeleteOrderDetail(int orderDetailId)
        {
            try
            {
                string query = "DELETE FROM OrderDetails WHERE OrderDetailID = @OrderDetailID";
                
                var parameters = new Dictionary<string, object>
                {
                    { "@OrderDetailID", orderDetailId }
                };
                
                DataAccessHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting order detail: {ex.Message}");
                throw new InvalidOperationException("Database error occurred while deleting order detail.", ex);
            }
        }

        /// <summary>
        /// Gets a specific order detail by ID
        /// </summary>
        /// <param name="orderDetailId">The order detail ID</param>
        /// <returns>The order detail or null if not found</returns>
        public static OrderDetail GetOrderDetailById(int orderDetailId)
        {
            try
            {
                string query = @"
                    SELECT OrderDetailID, OrderID, ProductID, Quantity, UnitPrice 
                    FROM OrderDetails 
                    WHERE OrderDetailID = @OrderDetailID";
                    
                var parameters = new Dictionary<string, object>
                {
                    { "@OrderDetailID", orderDetailId }
                };
                
                return DataAccessHelper.ExecuteReaderSingle(query, reader => new OrderDetail
                {
                    OrderDetailID = reader.GetInt32(0),
                    OrderID = reader.GetInt32(1),
                    ProductID = reader.GetInt32(2),
                    Quantity = reader.GetInt32(3),
                    UnitPrice = reader.GetDecimal(4)
                }, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting order detail: {ex.Message}");
                throw new InvalidOperationException("Database error occurred while retrieving order detail.", ex);
            }
        }
        
        /// <summary>
        /// Gets all order details for a specific order, redirecting to OrderManager
        /// to maintain a single source of truth
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>A list of order details</returns>
        public static List<OrderDetail> GetOrderDetails(int orderId)
        {
            // This method now delegates to OrderManager to maintain a single source of truth
            return OrderManager.GetOrderDetails(orderId);
        }
    }
}
