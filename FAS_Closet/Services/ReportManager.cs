using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using FASCloset.Data;
using FASCloset.Models;

namespace FASCloset.Services
{
    public class ReportManager
    {
        public static List<Product> GetBestSellingProducts(DateTime? startDate = null, DateTime? endDate = null, int limit = 10)
        {
            string dateFilter = "";
            var parameters = new Dictionary<string, object>();
            
            if (startDate.HasValue)
            {
                dateFilter += " AND o.OrderDate >= @StartDate";
                parameters.Add("@StartDate", startDate.Value.ToString("yyyy-MM-dd"));
            }
            
            if (endDate.HasValue)
            {
                dateFilter += " AND o.OrderDate <= @EndDate";
                parameters.Add("@EndDate", endDate.Value.ToString("yyyy-MM-dd"));
            }
            
            parameters.Add("@Limit", limit);
            
            string query = $@"
                SELECT 
                    p.ProductID, 
                    p.ProductName,
                    c.CategoryName,
                    p.Price,
                    SUM(od.Quantity) as TotalQuantity,
                    SUM(od.Quantity * od.UnitPrice) as TotalRevenue
                FROM 
                    Product p
                JOIN 
                    OrderDetails od ON p.ProductID = od.ProductID
                JOIN 
                    Orders o ON od.OrderID = o.OrderID
                JOIN
                    Category c ON p.CategoryID = c.CategoryID
                WHERE 
                    1=1 {dateFilter}
                GROUP BY 
                    p.ProductID, p.ProductName, c.CategoryName, p.Price
                ORDER BY 
                    TotalQuantity DESC
                LIMIT @Limit";
                
            return DataAccessHelper.ExecuteReader(query, reader => new Product
            {
                ProductID = Convert.ToInt32(reader["ProductID"]),
                ProductName = reader["ProductName"].ToString() ?? string.Empty,
                CategoryName = reader["CategoryName"].ToString() ?? string.Empty,
                Price = Convert.ToDecimal(reader["Price"]),
                Stock = Convert.ToInt32(reader["TotalQuantity"])
            }, parameters);
        }
        
        public static List<Customer> GetTopCustomers(DateTime? startDate = null, DateTime? endDate = null, int limit = 10)
        {
            string dateFilter = "";
            var parameters = new Dictionary<string, object>();
            
            if (startDate.HasValue)
            {
                dateFilter += " AND o.OrderDate >= @StartDate";
                parameters.Add("@StartDate", startDate.Value.ToString("yyyy-MM-dd"));
            }
            
            if (endDate.HasValue)
            {
                dateFilter += " AND o.OrderDate <= @EndDate";
                parameters.Add("@EndDate", endDate.Value.ToString("yyyy-MM-dd"));
            }
            
            parameters.Add("@Limit", limit);
            
            string query = $@"
                SELECT 
                    c.CustomerID, 
                    c.Name, 
                    c.Email,
                    COUNT(o.OrderID) as OrderCount,
                    SUM(o.TotalAmount) as TotalSpent
                FROM 
                    Customer c
                JOIN 
                    Orders o ON c.CustomerID = o.CustomerID
                WHERE 
                    1=1 {dateFilter}
                GROUP BY 
                    c.CustomerID, c.Name, c.Email
                ORDER BY 
                    TotalSpent DESC
                LIMIT @Limit";
                
            return DataAccessHelper.ExecuteReader(query, reader => new Customer
            {
                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty
            }, parameters);
        }
        
        // Enhanced compatibility methods for UC_RevenueReport.cs
        public static DataTable GenerateSalesReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            // Get the list data first
            var reportData = GetSalesReport(startDate, endDate);
            
            // Convert to DataTable
            return ConvertToDataTable(reportData);
        }
        
        public static DataTable GenerateDetailedSalesReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            // Get the detailed report data
            var detailData = GetDetailedSalesReport(startDate, endDate);
            
            // Convert to DataTable
            return ConvertToDataTable(detailData);
        }
        
        private static List<ReportData> GetSalesReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            string dateFilter = "";
            var parameters = new Dictionary<string, object>();
            
            if (startDate.HasValue)
            {
                dateFilter += " AND o.OrderDate >= @StartDate";
                parameters.Add("@StartDate", startDate.Value.ToString("yyyy-MM-dd"));
            }
            
            if (endDate.HasValue)
            {
                dateFilter += " AND o.OrderDate <= @EndDate";
                parameters.Add("@EndDate", endDate.Value.ToString("yyyy-MM-dd"));
            }
            
            string query = $@"
                SELECT 
                    o.OrderDate,
                    SUM(od.Quantity * od.UnitPrice) as Revenue,
                    COUNT(DISTINCT o.OrderID) as OrderCount,
                    COUNT(od.OrderDetailID) as ItemCount
                FROM 
                    Orders o
                JOIN 
                    OrderDetails od ON o.OrderID = od.OrderID
                WHERE 
                    1=1 {dateFilter}
                GROUP BY 
                    DATE(o.OrderDate)
                ORDER BY 
                    o.OrderDate";
                
            return DataAccessHelper.ExecuteReader(query, reader => new ReportData
            {
                Date = Convert.ToDateTime(reader["OrderDate"]),
                Revenue = Convert.ToDecimal(reader["Revenue"]),
                OrderCount = Convert.ToInt32(reader["OrderCount"]),
                ItemCount = Convert.ToInt32(reader["ItemCount"])
            }, parameters);
        }
        
        private static List<ReportDetail> GetDetailedSalesReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            string dateFilter = "";
            var parameters = new Dictionary<string, object>();
            
            if (startDate.HasValue)
            {
                dateFilter += " AND o.OrderDate >= @StartDate";
                parameters.Add("@StartDate", startDate.Value.ToString("yyyy-MM-dd"));
            }
            
            if (endDate.HasValue)
            {
                dateFilter += " AND o.OrderDate <= @EndDate";
                parameters.Add("@EndDate", endDate.Value.ToString("yyyy-MM-dd"));
            }
            
            string query = $@"
                SELECT 
                    o.OrderID,
                    o.OrderDate,
                    c.Name as CustomerName,
                    p.ProductName,
                    od.Quantity,
                    od.UnitPrice,
                    (od.Quantity * od.UnitPrice) as LineTotal,
                    o.PaymentMethod
                FROM 
                    Orders o
                JOIN 
                    OrderDetails od ON o.OrderID = od.OrderID
                JOIN 
                    Product p ON od.ProductID = p.ProductID
                JOIN 
                    Customer c ON o.CustomerID = c.CustomerID
                WHERE 
                    1=1 {dateFilter}
                ORDER BY 
                    o.OrderDate DESC, o.OrderID";
                
            return DataAccessHelper.ExecuteReader(query, reader => new ReportDetail
            {
                OrderID = Convert.ToInt32(reader["OrderID"]),
                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                CustomerName = reader["CustomerName"].ToString() ?? string.Empty,
                ProductName = reader["ProductName"].ToString() ?? string.Empty,
                Quantity = Convert.ToInt32(reader["Quantity"]),
                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                LineTotal = Convert.ToDecimal(reader["LineTotal"]),
                PaymentMethod = reader["PaymentMethod"].ToString() ?? string.Empty
            }, parameters);
        }
        
        // Generic method to convert List<T> to DataTable
        private static DataTable ConvertToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            
            // Get all the properties of T
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            // Create columns in the DataTable
            foreach (PropertyInfo prop in properties)
            {
                // Add the property type as a column in the DataTable
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            
            // Add the property values to the DataTable
            foreach (T item in items)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    // Get the value of the property in the object
                    values[i] = properties[i].GetValue(item) ?? DBNull.Value;
                }
                
                dataTable.Rows.Add(values);
            }
            
            return dataTable;
        }
    }
}
