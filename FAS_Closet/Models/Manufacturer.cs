// This file defines the Manufacturer class, which represents a manufacturer in the system.

namespace FASCloset.Models
{
    public class Manufacturer
    {
        /// <summary>
        /// The unique identifier for the manufacturer
        /// </summary>
        public int ManufacturerID { get; set; }
        
        /// <summary>
        /// The name of the manufacturer
        /// </summary>
        public string ManufacturerName { get; set; } = string.Empty;
        
        /// <summary>
        /// Optional description of the manufacturer
        /// </summary>
        public string? Description { get; set; }
    }
}
