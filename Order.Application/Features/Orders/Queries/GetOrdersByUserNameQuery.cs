using MassTransit.Mediator;
using MediatR;
using Order.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Queries
{
    public class GetOrdersByUserNameQuery : IRequest<List<OrderResponse>>
    {
        public string UserName { get; set; }
        public GetOrdersByUserNameQuery(string userName) => UserName = userName;
    }
}
