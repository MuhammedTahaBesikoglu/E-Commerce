using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Messages.Events
{
    public class PaymentFailedEvent
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
        public List<OrderItemPayload> Items { get; set; } = new List<OrderItemPayload>();
    }
}
