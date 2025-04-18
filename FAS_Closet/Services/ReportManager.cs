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
            var query = @"
                SELECT 
                    p.ProductID, 
                    p.ProductName, 
                    p.CategoryID, 
                    p.Price, 
                    p.Stock,
                    p.Description,
                    c.CategoryName,
                    SUM(od.Quantity) as TotalQuantity,
                    COUNT(DISTINCT o.OrderID) as SalesCount,
                    SUM(od.Quantity * od.UnitPrice) as Revenue
                FROM Product p
                JOIN OrderDetails od ON p.ProductID = od.ProductID
                JOIN Orders o ON od.OrderID = o.OrderID
                LEFT JOIN Category c ON p.CategoryID = c.CategoryID
                WHERE 1=1 ";

            if (startDate.HasValue)
            {
                query += " AND o.OrderDate >= @StartDate";
            }

            if (endDate.HasValue)
            {
                query += " AND o.OrderDate <= @EndDate";
            }

            query += @" 
                GROUP BY p.ProductID 
                ORDER BY TotalQuantity DESC, Revenue DESC
                LIMIT @Limit";

            var parameters = new Dictionary<string, object>();
            if (startDate.HasValue)
                parameters.Add("@StartDate", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate.HasValue)
                parameters.Add("@EndDate", endDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            parameters.Add("@Limit", limit);

            return DataAccessHelper.ExecuteReader(query, reader => new Product
            {
                ProductID = Convert.ToInt32(reader["ProductID"]),
                ProductName = reader["ProductName"].ToString(),
                CategoryID = Convert.ToInt32(reader["CategoryID"]),
                Price = Convert.ToDecimal(reader["Price"]),
                Stock = Convert.ToInt32(reader["Stock"]),
                Description = reader["Description"].ToString(),
                CategoryName = reader["CategoryName"]?.ToString() ?? "",
                TotalQuantity = Convert.ToInt32(reader["TotalQuantity"]),
                SalesCount = Convert.ToInt32(reader["SalesCount"]),
                Revenue = Convert.ToDecimal(reader["Revenue"])
            }, parameters);
        }
        
        public static List<Customer> GetTopCustomers(DateTime? startDate = null, DateTime? endDate = null, int limit = 10)
        {
            var query = @"
                SELECT 
                    c.CustomerID, 
                    c.Name, 
                    c.Email, 
                    c.Phone,
                    c.Address,
                    SUM(o.TotalAmount) as TotalSpent,
                    COUNT(o.OrderID) as OrderCount
                FROM Customer c
                JOIN Orders o ON c.CustomerID = o.CustomerID
                WHERE 1=1 ";

            if (startDate.HasValue)
            {
                query += " AND o.OrderDate >= @StartDate";
            }

            if (endDate.HasValue)
            {
                query += " AND o.OrderDate <= @EndDate";
            }

            query += @" 
                GROUP BY c.CustomerID 
                ORDER BY TotalSpent DESC
                LIMIT @Limit";

            var parameters = new Dictionary<string, object>();
            if (startDate.HasValue)
                parameters.Add("@StartDate", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate.HasValue)
                parameters.Add("@EndDate", endDate.Value.ToString("yyyy-MM-dd 23:59:59"));
            parameters.Add("@Limit", limit);

            return DataAccessHelper.ExecuteReader(query, reader =>
            {
                var customer = new Customer
                {
                    CustomerID = Convert.ToInt32(reader["CustomerID"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Address = reader["Address"].ToString(),
                    TotalSpent = Convert.ToDecimal(reader["TotalSpent"])
                };
                
                return customer;
            }, parameters);
        }
        
        // Enhanced compatibility methods for UC_RevenueReport.cs
        public static DataTable GenerateSalesReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            var reportData = GetSalesReport(startDate, endDate);
            DataTable dt = new DataTable();
            
            // Add columns with Vietnamese headers
            dt.Columns.Add("Ngày", typeof(DateTime));
            dt.Columns.Add("Mã đơn hàng", typeof(int));
            dt.Columns.Add("Khách hàng", typeof(string));
            dt.Columns.Add("Tổng tiền", typeof(decimal));
            dt.Columns.Add("Thanh toán", typeof(string));
            
            // Add rows
            foreach (var item in reportData)
            {
                var row = dt.NewRow();
                row["Ngày"] = item.OrderDate;
                row["Mã đơn hàng"] = item.OrderID;
                row["Khách hàng"] = item.CustomerName;
                row["Tổng tiền"] = item.TotalAmount;
                row["Thanh toán"] = item.PaymentMethod;
                dt.Rows.Add(row);
            }
            
            return dt;
        }
        
        public static DataTable GenerateDetailedSalesReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            var reportData = GetDetailedSalesReport(startDate, endDate);
            DataTable dt = new DataTable();
            
            // Add columns with Vietnamese headers
            dt.Columns.Add("Ngày", typeof(DateTime));
            dt.Columns.Add("Mã đơn hàng", typeof(int));
            dt.Columns.Add("Khách hàng", typeof(string));
            dt.Columns.Add("Sản phẩm", typeof(string));
            dt.Columns.Add("Số lượng", typeof(int));
            dt.Columns.Add("Đơn giá", typeof(decimal));
            dt.Columns.Add("Thành tiền", typeof(decimal));
            dt.Columns.Add("Thanh toán", typeof(string));
            
            // Add rows
            foreach (var item in reportData)
            {
                var row = dt.NewRow();
                row["Ngày"] = item.OrderDate;
                row["Mã đơn hàng"] = item.OrderID;
                row["Khách hàng"] = item.CustomerName;
                row["Sản phẩm"] = item.ProductName;
                row["Số lượng"] = item.Quantity;
                row["Đơn giá"] = item.UnitPrice;
                row["Thành tiền"] = item.LineTotal;
                row["Thanh toán"] = item.PaymentMethod;
                dt.Rows.Add(row);
            }
            
            return dt;
        }
        
        public static DataTable GenerateProductSalesReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            var products = GetBestSellingProducts(startDate, endDate, 100); // Get up to 100 products
            
            DataTable dt = new DataTable();
            
            // Add columns with Vietnamese headers
            dt.Columns.Add("Mã SP", typeof(int));
            dt.Columns.Add("Tên sản phẩm", typeof(string));
            dt.Columns.Add("Danh mục", typeof(string));
            dt.Columns.Add("Số lượng bán", typeof(int));
            dt.Columns.Add("Số đơn hàng", typeof(int));
            dt.Columns.Add("Doanh thu", typeof(decimal));
            
            // Add rows
            foreach (var product in products)
            {
                var row = dt.NewRow();
                row["Mã SP"] = product.ProductID;
                row["Tên sản phẩm"] = product.ProductName;
                row["Danh mục"] = product.CategoryName;
                row["Số lượng bán"] = product.TotalQuantity;
                row["Số đơn hàng"] = product.SalesCount;
                row["Doanh thu"] = product.Revenue;
                dt.Rows.Add(row);
            }
            
            return dt;
        }
        
        public static DataTable GenerateCustomerSalesReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            var customers = GetTopCustomers(startDate, endDate, 100); // Get up to 100 customers
            
            DataTable dt = new DataTable();
            
            // Add columns with Vietnamese headers
            dt.Columns.Add("Mã KH", typeof(int));
            dt.Columns.Add("Tên khách hàng", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Điện thoại", typeof(string));
            dt.Columns.Add("Tổng chi tiêu", typeof(decimal));
            
            // Add rows
            foreach (var customer in customers)
            {
                var row = dt.NewRow();
                row["Mã KH"] = customer.CustomerID;
                row["Tên khách hàng"] = customer.Name;
                row["Email"] = customer.Email;
                row["Điện thoại"] = customer.Phone;
                row["Tổng chi tiêu"] = customer.TotalSpent;
                dt.Rows.Add(row);
            }
            
            return dt;
        }

        private static List<ReportData> GetSalesReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = @"
                SELECT 
                    o.OrderDate, 
                    o.OrderID, 
                    o.CustomerID, 
                    c.Name as CustomerName,
                    o.TotalAmount, 
                    o.PaymentMethod
                FROM Orders o
                JOIN Customer c ON o.CustomerID = c.CustomerID
                WHERE 1=1 ";

            if (startDate.HasValue)
            {
                query += " AND o.OrderDate >= @StartDate";
            }

            if (endDate.HasValue)
            {
                query += " AND o.OrderDate <= @EndDate";
            }

            query += " ORDER BY o.OrderDate DESC";

            var parameters = new Dictionary<string, object>();
            if (startDate.HasValue)
                parameters.Add("@StartDate", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate.HasValue)
                parameters.Add("@EndDate", endDate.Value.ToString("yyyy-MM-dd 23:59:59"));

            return DataAccessHelper.ExecuteReader(query, reader => new ReportData
            {
                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                OrderID = Convert.ToInt32(reader["OrderID"]),
                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                CustomerName = reader["CustomerName"].ToString(),
                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                PaymentMethod = reader["PaymentMethod"].ToString()
            }, parameters);
        }
        
        private static List<ReportDetail> GetDetailedSalesReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = @"
                SELECT 
                    o.OrderDate, 
                    o.OrderID, 
                    c.Name as CustomerName,
                    p.ProductName, 
                    od.Quantity, 
                    od.UnitPrice,
                    (od.Quantity * od.UnitPrice) as LineTotal,
                    o.PaymentMethod
                FROM Orders o
                JOIN Customer c ON o.CustomerID = c.CustomerID
                JOIN OrderDetails od ON o.OrderID = od.OrderID
                JOIN Product p ON od.ProductID = p.ProductID
                WHERE 1=1 ";

            if (startDate.HasValue)
            {
                query += " AND o.OrderDate >= @StartDate";
            }

            if (endDate.HasValue)
            {
                query += " AND o.OrderDate <= @EndDate";
            }

            query += " ORDER BY o.OrderDate DESC, o.OrderID, p.ProductName";

            var parameters = new Dictionary<string, object>();
            if (startDate.HasValue)
                parameters.Add("@StartDate", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate.HasValue)
                parameters.Add("@EndDate", endDate.Value.ToString("yyyy-MM-dd 23:59:59"));

            return DataAccessHelper.ExecuteReader(query, reader => new ReportDetail
            {
                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                OrderID = Convert.ToInt32(reader["OrderID"]),
                CustomerName = reader["CustomerName"].ToString(),
                ProductName = reader["ProductName"].ToString(),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                LineTotal = Convert.ToDecimal(reader["LineTotal"]),
                PaymentMethod = reader["PaymentMethod"].ToString()
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
