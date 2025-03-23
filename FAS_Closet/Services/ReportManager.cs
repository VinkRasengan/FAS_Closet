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

        /// <summary>
        /// Gets the best-selling products, either for all time or for a specific date range if provided
        /// </summary>
        public static List<Product> GetBestSellingProducts(DateTime? startDate = null, DateTime? endDate = null)
        {
            string query;
            var parameters = new Dictionary<string, object>();
            
            if (startDate.HasValue && endDate.HasValue)
            {
                query = @"
                    SELECT p.*, c.CategoryName, COUNT(od.ProductID) as SalesCount,
                           SUM(od.Quantity) as TotalQuantity, SUM(od.Quantity * od.UnitPrice) as Revenue
                    FROM Product p
                    JOIN OrderDetails od ON p.ProductID = od.ProductID
                    JOIN Orders o ON od.OrderID = o.OrderID
                    LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                    WHERE o.OrderDate BETWEEN @StartDate AND @EndDate
                    GROUP BY p.ProductID
                    ORDER BY SalesCount DESC
                    LIMIT 10";
                    
                parameters.Add("@StartDate", startDate.Value.ToString("yyyy-MM-dd"));
                parameters.Add("@EndDate", endDate.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                query = @"
                    SELECT p.*, c.CategoryName, COUNT(od.ProductID) as SalesCount,
                           SUM(od.Quantity) as TotalQuantity, SUM(od.Quantity * od.UnitPrice) as Revenue
                    FROM Product p
                    JOIN OrderDetails od ON p.ProductID = od.ProductID
                    JOIN Orders o ON od.OrderID = o.OrderID
                    LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                    GROUP BY p.ProductID
                    ORDER BY SalesCount DESC
                    LIMIT 10";
            }
            
            return DataAccessHelper.ExecuteReader(query, reader => new Product
            {
                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoryName")),
                SalesCount = reader.GetInt32(reader.GetOrdinal("SalesCount")),
                TotalQuantity = reader.GetInt32(reader.GetOrdinal("TotalQuantity")),
                Revenue = reader.GetDecimal(reader.GetOrdinal("Revenue"))
            }, parameters);
        }
    }
}
