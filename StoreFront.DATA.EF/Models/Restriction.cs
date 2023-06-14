using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Restriction
    {
        public Restriction()
        {
            Products = new HashSet<Product>();
        }

        public int RestrictionId { get; set; }
        public string? RestrictionType { get; set; }
        public string? PermitNeeded { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
