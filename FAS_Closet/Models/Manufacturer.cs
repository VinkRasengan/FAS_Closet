using System.ComponentModel.DataAnnotations;

namespace FASCloset.Models
{
    public class Manufacturer
    {
        [Key]
        public int ManufacturerID { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string ContactInfo { get; set; }
    }
}
