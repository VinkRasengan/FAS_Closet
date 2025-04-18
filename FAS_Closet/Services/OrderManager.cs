using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Data;
using FASCloset.Models;

namespace FASCloset.Services
{
    public static class OrderManager
    {
        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
        }
        
        /// <summary>
        /// Gets all orders from the database
        /// </summary>
        /// <returns>A list of orders</returns>
        public static List<Order> GetOrders()
        {
            const string query = @"
                SELECT o.OrderID, o.CustomerID, c.Name as CustomerName, o.OrderDate, o.TotalAmount, o.PaymentMethod
                FROM Orders o
                JOIN Customer c ON o.CustomerID = c.CustomerID
                ORDER BY o.OrderDate DESC
            ";
            
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
        
        /// <summary>
        /// Gets an order by its ID
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>The order or null if not found</returns>
        public static Order GetOrderById(int orderId)
        {
            const string query = @"
                SELECT o.OrderID, o.CustomerID, c.Name as CustomerName, o.OrderDate, o.TotalAmount, o.PaymentMethod
                FROM Orders o
                JOIN Customer c ON o.CustomerID = c.CustomerID
                WHERE o.OrderID = @OrderID
            ";
            
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
        
        /// <summary>
        /// Gets all orders for a customer
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>A list of orders</returns>
        public static List<Order> GetOrdersByCustomerId(int customerId)
        {
            const string query = @"
                SELECT o.OrderID, o.CustomerID, c.Name as CustomerName, o.OrderDate, o.TotalAmount, o.PaymentMethod
                FROM Orders o
                JOIN Customer c ON o.CustomerID = c.CustomerID
                WHERE o.CustomerID = @CustomerID
                ORDER BY o.OrderDate DESC
            ";
            
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
        
        /// <summary>
        /// Adds a new order to the database
        /// </summary>
        /// <param name="order">The order to add</param>
        /// <returns>The ID of the newly added order</returns>
        public static int AddOrder(Order order)
        {
            const string query = @"
                INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod)
                VALUES (@CustomerID, @OrderDate, @TotalAmount, @PaymentMethod);
                SELECT last_insert_rowid();
            ";
            
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", order.CustomerID },
                { "@OrderDate", order.OrderDate },
                { "@TotalAmount", order.TotalAmount },
                { "@PaymentMethod", order.PaymentMethod }
            };
            
            return DataAccessHelper.ExecuteScalar<int>(query, parameters);
        }
        
        /// <summary>
        /// Updates an existing order
        /// </summary>
        /// <param name="order">The order to update</param>
        public static void UpdateOrder(Order order)
        {
            const string query = @"
                UPDATE Orders
                SET CustomerID = @CustomerID,
                    OrderDate = @OrderDate,
                    TotalAmount = @TotalAmount,
                    PaymentMethod = @PaymentMethod
                WHERE OrderID = @OrderID
            ";
            
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

        /// <summary>
        /// Deletes an order and its details
        /// </summary>
        /// <param name="orderId">The ID of the order to delete</param>
        public static void DeleteOrder(int orderId)
        {
            // First delete all order details
            const string deleteDetailsQuery = @"
                DELETE FROM OrderDetails
                WHERE OrderID = @OrderID
            ";
            
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", orderId }
            };
            
            DataAccessHelper.ExecuteNonQuery(deleteDetailsQuery, parameters);
            
            // Then delete the order
            const string deleteOrderQuery = @"
                DELETE FROM Orders
                WHERE OrderID = @OrderID
            ";
            
            DataAccessHelper.ExecuteNonQuery(deleteOrderQuery, parameters);
        }

        /// <summary>
        /// Creates a new order with details using a transaction
        /// </summary>
        /// <param name="order">The order to create</param>
        /// <param name="orderDetails">The order details</param>
        /// <returns>The ID of the newly created order</returns>
        public static int CreateOrderWithDetails(Order order, List<OrderDetail> orderDetails)
        {
            return DatabaseConnection.ExecuteWithTransaction<int>((connection, transaction) =>
            {
                // Insert order
                string insertOrderQuery = @"
                    INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod)
                    VALUES (@CustomerID, @OrderDate, @TotalAmount, @PaymentMethod);
                    SELECT last_insert_rowid();
                ";
                
                using (var cmd = new SqliteCommand(insertOrderQuery, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                    cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    cmd.Parameters.AddWithValue("@PaymentMethod", order.PaymentMethod);
                    
                    int orderId = Convert.ToInt32(cmd.ExecuteScalar());
                    
                    // Insert order details
                    foreach (var detail in orderDetails)
                    {
                        string insertDetailQuery = @"
                            INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice)
                            VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice)
                        ";
                        
                        using (var detailCmd = new SqliteCommand(insertDetailQuery, connection, transaction))
                        {
                            detailCmd.Parameters.AddWithValue("@OrderID", orderId);
                            detailCmd.Parameters.AddWithValue("@ProductID", detail.ProductID);
                            detailCmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                            detailCmd.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
                            
                            detailCmd.ExecuteNonQuery();
                        }
                        
                        // Update product stock
                        UpdateProductStock(connection, transaction, detail.ProductID, detail.Quantity);
                    }
                    
                    return orderId;
                }
            });
        }
        
        /// <summary>
        /// Updates inventory after an order is placed
        /// </summary>
        /// <param name="connection">The DB connection</param>
        /// <param name="transaction">The transaction</param>
        /// <param name="productId">The product ID</param>
        /// <param name="quantity">The quantity to reduce</param>
        private static void UpdateProductStock(SqliteConnection connection, SqliteTransaction transaction, int productId, int quantity)
        {
            string updateStockQuery = @"
                UPDATE Product
                SET Stock = Stock - @Quantity
                WHERE ProductID = @ProductID
            ";
            
            using (var cmd = new SqliteCommand(updateStockQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets all order details for a specific order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>A list of order details</returns>
        public static List<OrderDetail> GetOrderDetails(int orderId)
        {
            string query = @"
                SELECT od.OrderDetailID, od.OrderID, od.ProductID, p.ProductName, od.Quantity, od.UnitPrice
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
                ProductName = reader["ProductName"].ToString(),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                UnitPrice = Convert.ToDecimal(reader["UnitPrice"])
            }, parameters);
        }

        /// <summary>
        /// Gets all order details for a specific order (alias method for GetOrderDetails)
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>A list of order details</returns>
        public static List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return GetOrderDetails(orderId);
        }
    }
}
