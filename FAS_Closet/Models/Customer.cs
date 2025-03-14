using System.ComponentModel.DataAnnotations;

namespace FASCloset.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public int LoyaltyPoints { get; set; }
    }
}
