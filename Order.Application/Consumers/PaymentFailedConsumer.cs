using System;
using System.Collections.Generic;
using System.Text;
using EventBus.Messages.Events;
using MassTransit;
using Order.Domain.Interfaces;

namespace Order.Application.Consumers
{
    public class PaymentFailedConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public PaymentFailedConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order = await _orderRepository.GetOrderByIdAsync(context.Message.OrderId);
            if (order != null)
            {
                order.OrderStatus = $"Ödeme Başarısız - İptal Edildi. Hata: {context.Message.Reason}";

                _orderRepository.UpdateOrder(order);
                await _orderRepository.SaveChangesAsync();
            }
        }
    }
}
