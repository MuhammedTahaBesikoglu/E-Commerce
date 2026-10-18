using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Order.Application.Responses;
using Order.Domain.Interfaces;

namespace Order.Application.Features.Orders.Queries
{
    public class GetOrdersByUserNameQueryHandler : IRequestHandler<GetOrdersByUserNameQuery, List<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersByUserNameQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderResponse>> Handle(GetOrdersByUserNameQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserNameAsync(request.UserName);

            return orderList.Select(order => new OrderResponse(
                order.Id,
                order.UserName,
                order.TotalPrice,
                order.FirstName,
                order.LastName,
                order.EmailAddress,
                order.AddressLine,
                order.OrderStatus,
                order.Items.Select(item => new OrderItemResponse(
                    item.ProductId,
                    item.ProductName,
                    item.Price,
                    item.Quantity,
                    item.ImageUrl
                )).ToList()
            )).ToList();
        }
    }
}
