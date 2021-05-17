using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TennisBookings.APITests.Models
{
    public class ExpectedProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string InternalReference { get; set; }
        public int StockCount { get; set; }
        public ICollection<int> Ratings { get; set; } = new List<int>();
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
