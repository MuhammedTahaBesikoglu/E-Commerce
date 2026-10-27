using System;
using System.Collections.Generic;
using System.Text;
using EventBus.Messages.Events;
using MassTransit;
using Payment.Domain.Entities;
using Payment.Domain.Interfaces;

namespace Payment.Application.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IPaymentRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedConsumer(IPaymentRepository repository, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var message = context.Message;
            bool isSuccess = !message.CardNumber.EndsWith("0000");

            var transaction = new PaymentTransaction
            {
                Id = Guid.NewGuid(),
                OrderId = message.OrderId,
                TotalPrice = message.TotalPrice,
                CardName = message.CardName,
                CardNumber = message.CardNumber,
                Status = isSuccess ? "Success" : "Failed",
                ErrorMessage = isSuccess ? null : "Bakiye yetersiz/Kart reddedildi.",
                CreatedDate = DateTime.UtcNow
            };

            await _repository.AddTransactionAsync(transaction);
            await _repository.SaveChangesAsync();

            if (isSuccess)
            {
                await _publishEndpoint.Publish(new PaymentSucceededEvent { OrderId = message.OrderId });
            }
            else
            {
                await _publishEndpoint.Publish(new PaymentFailedEvent
                {
                    OrderId = message.OrderId,
                    Reason = transaction.ErrorMessage,
                    Items = message.Items
                });
            }
        }
    }
}
