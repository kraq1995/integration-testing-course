using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TennisBookings.APITests.Models
{
    public class TestProductInputModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal? Price { get; set; }
        public string InternalReference { get; set; }

        public TestProductInputModel CloneWith(Action<TestProductInputModel> changes)
        {
            var clone = (TestProductInputModel)MemberwiseClone();

            changes(clone);

            return clone;
        }
    }
}
