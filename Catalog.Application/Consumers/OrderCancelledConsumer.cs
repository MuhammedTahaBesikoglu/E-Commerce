using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using EventBus.Messages.Events;
using MassTransit;

namespace Catalog.Application.Consumers
{
    public class OrderCancelledConsumer : IConsumer<OrderCancelledEvent>
    {
        private readonly IRepository<Product> _productRepository;
        public OrderCancelledConsumer(IRepository<Product> productRepository) => _productRepository = productRepository;

        public async Task Consume(ConsumeContext<OrderCancelledEvent> context)
        {
            foreach (var item in context.Message.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity += item.Quantity; // Stoğu geri ekle
                    _productRepository.Update(product);
                }
            }
            await _productRepository.SaveChangesAsync();
        }
    }
}
