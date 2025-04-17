using System;

namespace FASCloset.Models
{
    public class ReportData
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
        public int ItemCount { get; set; }
        
        // Add properties required by UC_RevenueReport.cs
        public DateTime OrderDate { get; set; }
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
