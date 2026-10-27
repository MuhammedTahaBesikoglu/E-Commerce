using System;
using System.Collections.Generic;
using System.Text;
using EventBus.Messages.Events;
using MassTransit;
using Order.Domain.Interfaces;

namespace Order.Application.Consumers
{
    public class PaymentSucceededConsumer : IConsumer<PaymentSucceededEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public PaymentSucceededConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<PaymentSucceededEvent> context)
        {
            var order = await _orderRepository.GetOrderByIdAsync(context.Message.OrderId);
            if (order != null)
            {
                order.OrderStatus = "Ödeme Başarılı - Hazırlanıyor";

                _orderRepository.UpdateOrder(order);
                await _orderRepository.SaveChangesAsync();
            }
        }
    }
}
