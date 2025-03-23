using System;

namespace FASCloset.Models
{
    public class ReportData
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
        public int ItemCount { get; set; }
    }
}
