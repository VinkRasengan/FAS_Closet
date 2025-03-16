namespace FASCloset.Models
{
    public class Manufacturer
    {
        public int ManufacturerID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
