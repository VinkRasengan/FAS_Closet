// This file defines the Customer class, which represents a customer in the system.

namespace FASCloset.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }

    }
}
