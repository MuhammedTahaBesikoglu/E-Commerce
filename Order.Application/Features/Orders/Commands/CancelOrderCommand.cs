using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
namespace Order.Application.Features.Orders.Commands
{
    public class CancelOrderCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public CancelOrderCommand(Guid orderId) => OrderId = orderId;
    }
}
