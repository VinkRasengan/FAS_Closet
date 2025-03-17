// This file defines the Manufacturer class, which represents a manufacturer in the system.

namespace FASCloset.Models
{
    public class Manufacturer
    {
        public int ManufacturerID { get; set; }
        public required string ManufacturerName { get; set; }
        public string? Description { get; set; }
    }
}
