using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Messages.Events
{
    public class PaymentSucceededEvent
    {
        public Guid OrderId { get; set; }
    }
}
