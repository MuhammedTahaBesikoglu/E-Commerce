using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Domain.Entities;
using Order.Domain.Interfaces;

namespace Order.Application.Consumers
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<BasketCheckoutConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketCheckoutConsumer(IOrderRepository orderRepository, ILogger<BasketCheckoutConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var eventMessage = context.Message;

            // Manuel Eşleştirme: Hem ana sipariş hem de içindeki ürünler (Items)
            var newOrder = new global::Order.Domain.Entities.Order
            {
                Id = Guid.NewGuid(),
                UserName = eventMessage.UserName,
                TotalPrice = eventMessage.TotalPrice,
                FirstName = eventMessage.FirstName,
                LastName = eventMessage.LastName,
                EmailAddress = eventMessage.EmailAddress,
                AddressLine = eventMessage.AddressLine,
                Country = eventMessage.Country,
                ZipCode = eventMessage.ZipCode,
                CardName = eventMessage.CardName,
                CardNumber = eventMessage.CardNumber,
                Expiration = eventMessage.Expiration,
                CVV = eventMessage.CVV,
                PaymentMethod = eventMessage.PaymentMethod,
                OrderStatus = "Sipariş Alındı",

                // Kuyruktan gelen ürün listesini, veritabanındaki OrderItem nesnesine dönüştürüyoruz
                Items = eventMessage.Items.Select(x => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    ImageUrl = x.ImageUrl
                }).ToList()
            };

            await _orderRepository.AddOrderAsync(newOrder);
            await _orderRepository.SaveChangesAsync();

            var orderCreatedEvent = new OrderCreatedEvent
            {
                OrderId = newOrder.Id,
                TotalPrice = newOrder.TotalPrice,
                CardName = newOrder.CardName,
                CardNumber = newOrder.CardNumber, 
                Expiration = newOrder.Expiration,
                CVV = newOrder.CVV,
                Items = newOrder.Items.Select(x => new OrderItemPayload
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };

            await _publishEndpoint.Publish(orderCreatedEvent);
            _logger.LogInformation($"OrderCreatedEvent kuyruğa fırlatıldı. Sipariş ID: {newOrder.Id}");

            _logger.LogInformation($"Sipariş başarıyla oluşturuldu ve veritabanına kaydedildi. Sipariş ID: {newOrder.Id}");
        }
    }
}