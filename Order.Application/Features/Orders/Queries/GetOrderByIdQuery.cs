using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Order.Application.Responses;
namespace Order.Application.Features.Orders.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderResponse>
    {
        public Guid OrderId { get; set; }
        public GetOrderByIdQuery(Guid orderId) => OrderId = orderId;
    }
}
