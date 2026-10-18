using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(IRepository<Product> productRepository, ILogger<OrderCreatedConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            foreach (var item in context.Message.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    // Stoğu düşür
                    product.StockQuantity -= item.Quantity;

                    // Eksi stoğa düşmemesi için basit bir güvenlik
                    if (product.StockQuantity < 0) product.StockQuantity = 0;

                    _productRepository.Update(product);
                }
            }

            await _productRepository.SaveChangesAsync();
            _logger.LogInformation($"Sipariş ID: {context.Message.OrderId} için stoklar başarıyla düşüldü.");
        }
    }
}
