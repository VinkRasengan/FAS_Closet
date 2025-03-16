namespace FASCloset.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public int LoyaltyPoints { get; set; }
    }
}
