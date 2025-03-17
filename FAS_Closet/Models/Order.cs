// This file defines the Order class, which represents an order in the system.

namespace FASCloset.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public required string PaymentMethod { get; set; }
    }
}
