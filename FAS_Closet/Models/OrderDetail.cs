// This file defines the OrderDetail class, which represents the details of an order.

namespace FASCloset.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        
        // Add property required by OrderManager.cs
        public string ProductName { get; set; } = string.Empty;
        
        // Calculated property for the total price of this order detail
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
