using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Messages.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public List<OrderItemPayload> Items { get; set; } = new List<OrderItemPayload>();
    }

    public class OrderItemPayload
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
