// This file defines the Manufacturer class, which represents a manufacturer in the system.

namespace FASCloset.Models
{
    public class Manufacturer
    {
        public int ManufacturerID { get; set; }
        public string ManufacturerName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
