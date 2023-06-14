using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Order
    {
        public Order()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }

        public int OrderId { get; set; }
        public string CustomerId { get; set; } = null!;
        public DateTime? OrderDate { get; set; }
        public string? ShipToName { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipZip { get; set; }
        public string? ShipState { get; set; }
        public string? ShipCountry { get; set; }

        public virtual UserDetail Customer { get; set; } = null!;
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
