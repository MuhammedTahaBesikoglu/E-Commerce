using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public Guid OrderId { get; set; }
    }
}
