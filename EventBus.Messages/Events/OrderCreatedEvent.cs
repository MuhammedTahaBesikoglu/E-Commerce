using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Messages.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public List<OrderItemPayload> Items { get; set; } = new List<OrderItemPayload>();
        public decimal TotalPrice { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
    }

    public class OrderItemPayload
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
