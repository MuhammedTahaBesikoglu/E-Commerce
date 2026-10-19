using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Order.Application.Responses;
using Order.Domain.Interfaces;

namespace Order.Application.Features.Orders.Queries
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;
        public GetOrderByIdQueryHandler(IOrderRepository orderRepository) => _orderRepository = orderRepository;

        public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null) return null;

            return new OrderResponse(
                order.Id, order.UserName, order.TotalPrice, order.FirstName, order.LastName,
                order.EmailAddress, order.AddressLine, order.OrderStatus,
                order.Items.Select(item => new OrderItemResponse(item.ProductId, item.ProductName, item.Price, item.Quantity, item.ImageUrl)).ToList()
            );
        }
    }
}
