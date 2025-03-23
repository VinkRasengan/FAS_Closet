using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
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
            DataTable reportTable = new DataTable();
            
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            o.OrderID, 
                            c.Name AS CustomerName, 
                            o.OrderDate, 
                            o.TotalAmount,
                            o.PaymentMethod
                        FROM Orders o
                        JOIN Customer c ON o.CustomerID = c.CustomerID
                        WHERE o.OrderDate BETWEEN @StartDate AND @EndDate
                        ORDER BY o.OrderDate DESC";
                    
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
                        
                        using (var reader = command.ExecuteReader())
                        {
                            // Load schema
                            reportTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error generating sales report: " + ex.Message, ex);
            }
            
            return reportTable;
        }

        public static DataTable GenerateDetailedSalesReport(DateTime startDate, DateTime endDate)
        {
            DataTable reportTable = new DataTable();
            
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            o.OrderID, 
                            c.Name AS CustomerName,
                            p.ProductName,
                            od.Quantity,
                            od.UnitPrice,
                            (od.Quantity * od.UnitPrice) AS Subtotal,
                            o.OrderDate, 
                            o.TotalAmount,
                            o.PaymentMethod
                        FROM Orders o
                        JOIN Customer c ON o.CustomerID = c.CustomerID
                        JOIN OrderDetails od ON o.OrderID = od.OrderID
                        JOIN Product p ON od.ProductID = p.ProductID
                        WHERE o.OrderDate BETWEEN @StartDate AND @EndDate
                        ORDER BY o.OrderDate DESC, o.OrderID";
                    
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
                        
                        using (var reader = command.ExecuteReader())
                        {
                            // Load schema
                            reportTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error generating detailed sales report: " + ex.Message, ex);
            }
            
            return reportTable;
        }

        // New method to get best selling products for dashboard
        public static DataTable GetBestSellingProducts(int top = 5)
        {
            DataTable reportTable = new DataTable();
            
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            p.ProductName,
                            SUM(od.Quantity) AS TotalSold,
                            SUM(od.Quantity * od.UnitPrice) AS TotalRevenue
                        FROM OrderDetails od
                        JOIN Product p ON od.ProductID = p.ProductID
                        GROUP BY p.ProductID
                        ORDER BY TotalSold DESC
                        LIMIT @Top";
                    
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Top", top);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            // Load schema
                            reportTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error getting best selling products: " + ex.Message, ex);
            }
            
            return reportTable;
        }

        public static DataTable GetRevenueByCategory(DateTime startDate, DateTime endDate)
        {
            DataTable reportData = new DataTable();
            
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            c.CategoryID,
                            c.CategoryName,
                            SUM(od.Quantity * od.UnitPrice) as TotalRevenue,
                            COUNT(DISTINCT o.OrderID) as OrderCount
                        FROM 
                            OrderDetails od
                        JOIN 
                            Product p ON od.ProductID = p.ProductID
                        JOIN
                            Category c ON p.CategoryID = c.CategoryID
                        JOIN
                            Orders o ON od.OrderID = o.OrderID
                        WHERE 
                            o.OrderDate BETWEEN @StartDate AND @EndDate
                        GROUP BY 
                            c.CategoryID
                        ORDER BY 
                            TotalRevenue DESC";
                    
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd 23:59:59"));
                        
                        using (var reader = command.ExecuteReader())
                        {
                            // Load schema
                            reportData.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to get revenue by category", ex);
            }
            
            return reportData;
        }

        // New method to get best selling products for dashboard
        public static DataTable GetBestSellingProducts()
        {
            var result = new DataTable();
            result.Columns.Add("ProductID", typeof(int));
            result.Columns.Add("ProductName", typeof(string));
            result.Columns.Add("TotalSold", typeof(int));
            result.Columns.Add("Revenue", typeof(decimal));
            
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            p.ProductID,
                            p.ProductName,
                            SUM(od.Quantity) as TotalSold,
                            SUM(od.Quantity * od.UnitPrice) as Revenue
                        FROM 
                            Product p
                            INNER JOIN OrderDetails od ON p.ProductID = od.ProductID
                            INNER JOIN Orders o ON od.OrderID = o.OrderID
                        WHERE 
                            o.OrderDate >= date('now', '-30 day')
                        GROUP BY 
                            p.ProductID, p.ProductName
                        ORDER BY 
                            TotalSold DESC
                        LIMIT 10";
                    
                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Rows.Add(
                                    reader.GetInt32(0),                  // ProductID
                                    reader.GetString(1),                 // ProductName
                                    reader.GetInt32(2),                  // TotalSold
                                    reader.GetDecimal(3)                 // Revenue
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting best selling products: {ex.Message}");
                // If an error occurs, return an empty result
            }
            
            return result;
        }
    }
}
