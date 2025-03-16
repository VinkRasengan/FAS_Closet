using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Models;

namespace FASCloset.Services
{
    public static class OrderDetailManager
    {
        private static string GetConnectionString()
        {
            string? baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (baseDirectory == null)
            {
                throw new InvalidOperationException("Base directory is null.");
            }

            string? projectDir = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;
            if (projectDir == null)
            {
                throw new InvalidOperationException("Project directory is null.");
            }

            string dbPath = Path.Combine(projectDir, "Data", "FASClosetDB.sqlite");
            return $"Data Source={dbPath};";
        }

        public static void AddOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Subtotal) VALUES (@OrderID, @ProductID, @Quantity, @Subtotal)";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderDetail.OrderID);
                        command.Parameters.AddWithValue("@ProductID", orderDetail.ProductID);
                        command.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);
                        command.Parameters.AddWithValue("@Subtotal", orderDetail.Subtotal);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while adding order detail.", ex);
            }
        }

        public static void UpdateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "UPDATE OrderDetails SET OrderID = @OrderID, ProductID = @ProductID, Quantity = @Quantity, Subtotal = @Subtotal WHERE OrderDetailID = @OrderDetailID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderDetail.OrderID);
                        command.Parameters.AddWithValue("@ProductID", orderDetail.ProductID);
                        command.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);
                        command.Parameters.AddWithValue("@Subtotal", orderDetail.Subtotal);
                        command.Parameters.AddWithValue("@OrderDetailID", orderDetail.OrderDetailID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while updating order detail.", ex);
            }
        }

        public static void DeleteOrderDetail(int orderDetailId)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "DELETE FROM OrderDetails WHERE OrderDetailID = @OrderDetailID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderDetailID", orderDetailId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while deleting order detail.", ex);
            }
        }

        public static List<OrderDetail> GetOrderDetails(int orderId)
        {
            var orderDetails = new List<OrderDetail>();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT OrderDetailID, OrderID, ProductID, Quantity, Subtotal FROM OrderDetails WHERE OrderID = @OrderID";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orderDetails.Add(new OrderDetail
                                {
                                    OrderDetailID = reader.GetInt32(0),
                                    OrderID = reader.GetInt32(1),
                                    ProductID = reader.GetInt32(2),
                                    Quantity = reader.GetInt32(3),
                                    Subtotal = reader.GetDecimal(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving order details.", ex);
            }
            return orderDetails;
        }
    }
}
