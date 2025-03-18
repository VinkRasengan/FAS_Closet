// This file defines the OrderManager class, which handles order-related operations.

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Models;

namespace FASCloset.Services
{
    public static class OrderManager
    {
        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
        }

        public static void AddOrder(Order order)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "INSERT INTO Orders (CustomerID, OrderDate, TotalAmount) VALUES (@CustomerID, @OrderDate, @TotalAmount)";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                        command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while adding order.", ex);
            }
        }

        public static void UpdateOrder(Order order)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "UPDATE Orders SET CustomerID = @CustomerID, OrderDate = @OrderDate, TotalAmount = @TotalAmount WHERE OrderID = @OrderID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                        command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                        command.Parameters.AddWithValue("@OrderID", order.OrderID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while updating order.", ex);
            }
        }

        public static void DeleteOrder(int orderId)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while deleting order.", ex);
            }
        }

        public static List<Order> GetOrders()
        {
            var orders = new List<Order>();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT OrderID, CustomerID, OrderDate, TotalAmount FROM Orders";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orders.Add(MapOrder(reader));
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving orders.", ex);
            }
            return orders;
        }

        public static Order? GetOrderById(int orderId)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT OrderID, CustomerID, OrderDate, TotalAmount, PaymentMethod FROM Orders WHERE OrderID = @OrderID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Order
                                {
                                    OrderID = reader.GetInt32(0),
                                    CustomerID = reader.GetInt32(1),
                                    OrderDate = reader.GetDateTime(2),
                                    TotalAmount = reader.GetDecimal(3),
                                    PaymentMethod = reader.GetString(4)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving order.", ex);
            }
            return null;
        }

        public static List<Order> GetOrdersByCustomerId(int customerId)
        {
            var orders = new List<Order>();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT OrderID, CustomerID, OrderDate, TotalAmount, PaymentMethod FROM Orders WHERE CustomerID = @CustomerID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", customerId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orders.Add(new Order
                                {
                                    OrderID = reader.GetInt32(0),
                                    CustomerID = reader.GetInt32(1),
                                    OrderDate = reader.GetDateTime(2),
                                    TotalAmount = reader.GetDecimal(3),
                                    PaymentMethod = reader.GetString(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving orders by customer ID.", ex);
            }
            return orders;
        }

        private static Order MapOrder(SqliteDataReader reader)
        {
            return new Order
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                OrderDate = reader.GetDateTime(2),
                TotalAmount = reader.GetDecimal(3),
                PaymentMethod = reader.GetString(4)
            };
        }
    }
}
