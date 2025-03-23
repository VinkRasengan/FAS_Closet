using System;

namespace FASCloset.Models
{
    public class ReportDetail
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
