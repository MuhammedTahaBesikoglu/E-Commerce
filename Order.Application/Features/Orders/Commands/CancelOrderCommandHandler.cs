using System;
using System.Collections.Generic;
using System.Text;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Order.Domain.Interfaces;

namespace Order.Application.Features.Orders.Commands
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public CancelOrderCommandHandler(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            // İlgili siparişi bul (içindeki ürünlerle beraber)
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            var product = await _orderRepository.GetOrderItemByIdAsync(request.OrderId);
            if (order == null || order.OrderStatus == "İptal Edildi") return false;

            // Sipariş durumunu iptal olarak güncelle
            order.OrderStatus = "İptal Edildi";
            _orderRepository.UpdateOrder(order);
            await _orderRepository.SaveChangesAsync(cancellationToken);

            // Katalog servisine "Stokları geri yükle" mesajı fırlat (Compensation)
            var cancelledEvent = new OrderCancelledEvent
            {
                OrderId = product.Id,
                Items = order.Items.Select(x => new OrderItemPayload { ProductId = x.ProductId, Quantity = x.Quantity }).ToList()
            };
            await _publishEndpoint.Publish(cancelledEvent, cancellationToken);

            return true;
        }
    }
}
