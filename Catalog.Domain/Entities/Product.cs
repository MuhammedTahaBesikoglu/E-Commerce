using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }

        // Kategori ile ilişki (Foreign Key)
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
