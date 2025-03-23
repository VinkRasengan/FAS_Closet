// This file defines the ReportManager class, which handles report generation.

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using FASCloset.Models;

namespace FASCloset.Services
{
    public static class ReportManager
    {
        private static string GetConnectionString()
        {
            return DatabaseConnection.GetConnectionString();
        }

        public static DataTable GenerateSalesReport(DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
                        SELECT Orders.OrderID, Orders.OrderDate, Customers.Name AS CustomerName, Orders.TotalAmount
                        FROM Orders
                        JOIN Customers ON Orders.CustomerID = Customers.CustomerID
                        WHERE Orders.OrderDate BETWEEN @StartDate AND @EndDate";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        using (var reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while generating report.", ex);
            }
            return dataTable;
        }

        public static DataTable GenerateDetailedSalesReport(DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
                        SELECT Orders.OrderID, Orders.OrderDate, Customers.Name AS CustomerName, Products.ProductName, OrderDetails.Quantity, OrderDetails.UnitPrice
                        FROM Orders
                        JOIN Customers ON Orders.CustomerID = Customers.CustomerID
                        JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
                        JOIN Products ON OrderDetails.ProductID = Products.ProductID
                        WHERE Orders.OrderDate BETWEEN @StartDate AND @EndDate";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        using (var reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while generating detailed sales report.", ex);
            }
            return dataTable;
        }

        public static List<Product> GetBestSellingProducts()
        {
            var products = new List<Product>();
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
                        SELECT Products.ProductID, Products.ProductName, SUM(OrderDetails.Quantity) AS TotalQuantity
                        FROM OrderDetails
                        JOIN Products ON OrderDetails.ProductID = Products.ProductID
                        GROUP BY Products.ProductID, Products.ProductName
                        ORDER BY TotalQuantity DESC
                        LIMIT 10";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product
                                {
                                    ProductID = reader.GetInt32(0),
                                    ProductName = reader.GetString(1),
                                    Stock = reader.GetInt32(2),
                                    Description = string.Empty // Ensure Description is set
                                });
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException("Database error occurred while retrieving best-selling products.", ex);
            }
            return products;
        }
    }
}
