using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public decimal? Price { get; set; }
        public string? QuantityPerUnit { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? RestockLevel { get; set; }
        public int ProductStatusId { get; set; }
        public int SupplierId { get; set; }
        public int RestrictionId { get; set; }
        public string? Image { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ProductStatus ProductStatus { get; set; } = null!;
        public virtual Restriction Restriction { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
