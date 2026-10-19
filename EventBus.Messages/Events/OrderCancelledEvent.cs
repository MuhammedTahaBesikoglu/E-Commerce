using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Messages.Events
{
    public class OrderCancelledEvent
    {
        public Guid OrderId { get; set; }
        public List<OrderItemPayload> Items { get; set; } = new List<OrderItemPayload>();
    }
}
