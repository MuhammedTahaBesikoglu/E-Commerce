using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Messages.Events
{
    // Kuyrukta taşınacak olan tekil ürün (kalem) verisi
    public class BasketItemMessage
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
