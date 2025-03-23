// This file defines the OrderManager class, which handles order-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Data;
using FASCloset.Models;

namespace FASCloset.Services
{
    public class OrderManager
    {
        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
        }

        public static List<Order> GetOrders()
        {
            string query = @"
                SELECT o.*, c.Name as CustomerName
                FROM Orders o
                JOIN Customer c ON o.CustomerID = c.CustomerID
                ORDER BY o.OrderDate DESC";
                
            return DataAccessHelper.ExecuteReader(query, reader => new Order
            {
                OrderID = Convert.ToInt32(reader["OrderID"]),
                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                CustomerName = reader["CustomerName"].ToString(),
                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                PaymentMethod = reader["PaymentMethod"].ToString()
            });
        }
        
        public static Order GetOrderById(int orderId)
        {
            string query = @"
                SELECT o.*, c.Name as CustomerName
                FROM Orders o
                JOIN Customer c ON o.CustomerID = c.CustomerID
                WHERE o.OrderID = @OrderID";
                
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", orderId }
            };
            
            return DataAccessHelper.ExecuteReaderSingle(query, reader => new Order
            {
                OrderID = Convert.ToInt32(reader["OrderID"]),
                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                CustomerName = reader["CustomerName"].ToString(),
                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                PaymentMethod = reader["PaymentMethod"].ToString()
            }, parameters);
        }
        
        public static List<Order> GetOrdersByCustomerId(int customerId)
        {
            string query = @"
                SELECT o.*, c.Name as CustomerName
                FROM Orders o
                JOIN Customer c ON o.CustomerID = c.CustomerID
                WHERE o.CustomerID = @CustomerID
                ORDER BY o.OrderDate DESC";
                
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", customerId }
            };
            
            return DataAccessHelper.ExecuteReader(query, reader => new Order
            {
                OrderID = Convert.ToInt32(reader["OrderID"]),
                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                CustomerName = reader["CustomerName"].ToString(),
                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                PaymentMethod = reader["PaymentMethod"].ToString()
            }, parameters);
        }
        
        public static void AddOrder(Order order)
        {
            string query = @"
                INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod)
                VALUES (@CustomerID, @OrderDate, @TotalAmount, @PaymentMethod);
                SELECT last_insert_rowid();";
                
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", order.CustomerID },
                { "@OrderDate", order.OrderDate },
                { "@TotalAmount", order.TotalAmount },
                { "@PaymentMethod", order.PaymentMethod }
            };
            
            int orderId = DataAccessHelper.ExecuteScalar<int>(query, parameters);
            order.OrderID = orderId;
        }
        
        public static void UpdateOrder(Order order)
        {
            string query = @"
                UPDATE Orders 
                SET CustomerID = @CustomerID, 
                    OrderDate = @OrderDate, 
                    TotalAmount = @TotalAmount, 
                    PaymentMethod = @PaymentMethod
                WHERE OrderID = @OrderID";
                
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", order.OrderID },
                { "@CustomerID", order.CustomerID },
                { "@OrderDate", order.OrderDate },
                { "@TotalAmount", order.TotalAmount },
                { "@PaymentMethod", order.PaymentMethod }
            };
            
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }
        
        public static void DeleteOrder(int orderId)
        {
            string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
            
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", orderId }
            };
            
            DataAccessHelper.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Creates a new order with details using a transaction
        /// </summary>
        public static int CreateOrderWithDetails(Order order, List<OrderDetail> orderDetails)
        {
            int orderId = 0;
            
            try
            {
                // Use the helper method from DatabaseConnection
                DatabaseConnection.ExecuteDbOperation(connection =>
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Insert the order
                            string orderQuery = @"
                                INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod) 
                                VALUES (@CustomerID, @OrderDate, @TotalAmount, @PaymentMethod);
                                SELECT last_insert_rowid();";
                                
                            using (var command = new SqliteCommand(orderQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                                command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                                command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                                command.Parameters.AddWithValue("@PaymentMethod", order.PaymentMethod);
                                
                                // Get the inserted order ID
                                orderId = Convert.ToInt32(command.ExecuteScalar());
                            }
                            
                            // Insert order details
                            string detailQuery = @"
                                INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice)
                                VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice)";
                                
                            using (var command = new SqliteCommand(detailQuery, connection, transaction))
                            {
                                foreach (var detail in orderDetails)
                                {
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@OrderID", orderId);
                                    command.Parameters.AddWithValue("@ProductID", detail.ProductID);
                                    command.Parameters.AddWithValue("@Quantity", detail.Quantity);
                                    command.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
                                    command.ExecuteNonQuery();
                                    
                                    // Update inventory
                                    UpdateInventory(connection, transaction, detail.ProductID, detail.Quantity);
                                }
                            }
                            
                            // Commit the transaction
                            transaction.Commit();
                        }
                        catch
                        {
                            // Rollback on error
                            transaction.Rollback();
                            throw;
                        }
                    }
                });
                
                return orderId;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create order with details", ex);
            }
        }
        
        /// <summary>
        /// Updates inventory after an order is placed
        /// </summary>
        private static void UpdateInventory(SqliteConnection connection, SqliteTransaction transaction, int productId, int quantity)
        {
            string query = @"
                UPDATE Inventory SET StockQuantity = StockQuantity - @Quantity 
                WHERE ProductID = @ProductID AND StockQuantity >= @Quantity";
                
            using (var command = new SqliteCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@ProductID", productId);
                command.Parameters.AddWithValue("@Quantity", quantity);
                
                int rowsAffected = command.ExecuteNonQuery();
                
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"Not enough stock for product ID {productId}");
                }
            }
        }
    }
}
