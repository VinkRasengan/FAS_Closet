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
        /// <summary>
        /// Gets all orders from the database
        /// </summary>
        /// <returns>A list of orders</returns>
        public static List<Order> GetOrders()
        {
            string query = @"
                SELECT OrderID, CustomerID, OrderDate, TotalAmount, PaymentMethod 
                FROM Orders
                ORDER BY OrderDate DESC";
                
            return DataAccessHelper.ExecuteReader(query, reader => new Order
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                OrderDate = reader.GetDateTime(2),
                TotalAmount = reader.GetDecimal(3),
                PaymentMethod = reader.GetString(4)
            });
        }
        
        /// <summary>
        /// Gets an order by its ID
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>The order or null if not found</returns>
        public static Order GetOrderById(int orderId)
        {
            string query = @"
                SELECT OrderID, CustomerID, OrderDate, TotalAmount, PaymentMethod 
                FROM Orders 
                WHERE OrderID = @OrderID";
                
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", orderId }
            };
            
            return DataAccessHelper.ExecuteReaderSingle(query, reader => new Order
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                OrderDate = reader.GetDateTime(2),
                TotalAmount = reader.GetDecimal(3),
                PaymentMethod = reader.GetString(4)
            }, parameters);
        }
        
        /// <summary>
        /// Gets all orders for a customer
        /// </summary>
        /// <param name="customerId">The customer ID</param>
        /// <returns>A list of orders</returns>
        public static List<Order> GetOrdersByCustomerId(int customerId)
        {
            string query = @"
                SELECT OrderID, CustomerID, OrderDate, TotalAmount, PaymentMethod 
                FROM Orders 
                WHERE CustomerID = @CustomerID
                ORDER BY OrderDate DESC";
                
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerID", customerId }
            };
            
            return DataAccessHelper.ExecuteReader(query, reader => new Order
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                OrderDate = reader.GetDateTime(2),
                TotalAmount = reader.GetDecimal(3),
                PaymentMethod = reader.GetString(4)
            }, parameters);
        }
        
        /// <summary>
        /// Adds a new order to the database
        /// </summary>
        /// <param name="order">The order to add</param>
        /// <returns>The ID of the newly added order</returns>
        public static int AddOrder(Order order)
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
            return orderId;
        }
        
        /// <summary>
        /// Updates an existing order
        /// </summary>
        /// <param name="order">The order to update</param>
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

        /// <summary>
        /// Deletes an order and its details
        /// </summary>
        /// <param name="orderId">The ID of the order to delete</param>
        public static void DeleteOrder(int orderId)
        {
            // Step 1: Delete all related order details first
            string deleteOrderDetailsQuery = "DELETE FROM OrderDetails WHERE OrderID = @OrderID";

            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", orderId }
            };

            // Execute the query to delete the related order details
            DataAccessHelper.ExecuteNonQuery(deleteOrderDetailsQuery, parameters);

            // Step 2: Delete the order itself
            string deleteOrderQuery = "DELETE FROM Orders WHERE OrderID = @OrderID";

            // Execute the query to delete the order
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
                try
                {
                    // Insert the order
                    string orderQuery = @"
                        INSERT INTO Orders (CustomerID, OrderDate, TotalAmount, PaymentMethod)
                        VALUES (@CustomerID, @OrderDate, @TotalAmount, @PaymentMethod);
                        SELECT last_insert_rowid();";
                    
                    using (var orderCmd = new SqliteCommand(orderQuery, connection, transaction))
                    {
                        orderCmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                        orderCmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        orderCmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                        orderCmd.Parameters.AddWithValue("@PaymentMethod", order.PaymentMethod);
                        
                        int orderId = Convert.ToInt32(orderCmd.ExecuteScalar());
                        order.OrderID = orderId;
                        
                        // Insert each order detail
                        string detailQuery = @"
                            INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice)
                            VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice)";
                        
                        foreach (var detail in orderDetails)
                        {
                            using (var detailCmd = new SqliteCommand(detailQuery, connection, transaction))
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating order with details: {ex.Message}");
                    throw new InvalidOperationException($"Error creating order: {ex.Message}", ex);
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
            string query = @"
                UPDATE Product 
                SET Stock = Stock - @Quantity 
                WHERE ProductID = @ProductID AND Stock >= @Quantity";

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

        /// <summary>
        /// Gets all order details for a specific order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>A list of order details</returns>
        public static List<OrderDetail> GetOrderDetails(int orderId)
        {
            string query = @"
                SELECT od.OrderDetailID, od.OrderID, od.ProductID, od.Quantity, od.UnitPrice, 
                       p.ProductName, p.Stock, p.Description
                FROM OrderDetails od
                JOIN Product p ON od.ProductID = p.ProductID
                WHERE od.OrderID = @OrderID";
                
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", orderId }
            };
            
            return DataAccessHelper.ExecuteReader(query, reader => {
                var detail = new OrderDetail
                {
                    OrderDetailID = reader.GetInt32(0),
                    OrderID = reader.GetInt32(1),
                    ProductID = reader.GetInt32(2),
                    Quantity = reader.GetInt32(3),
                    UnitPrice = reader.GetDecimal(4),
                };
                
                // Add product details if available
                if (!reader.IsDBNull(5))
                    detail.ProductName = reader.GetString(5);
                
                return detail;
            }, parameters);
        }

        /// <summary>
        /// Gets all order details for a specific order (alias method for GetOrderDetails)
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <returns>A list of order details</returns>
        public static List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            // This is an alias method that calls the existing GetOrderDetails method
            // Added to maintain compatibility with code that expects this method name
            return GetOrderDetails(orderId);
        }
    }
}
