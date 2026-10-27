using System;
using System.Collections.Generic;
using System.Text;
using EventBus.Messages.Events;
using MassTransit;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Consumers
{
    public class PaymentFailedConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly ILogger<PaymentFailedConsumer> _logger;

        public PaymentFailedConsumer(IRepository<Product> productRepository, ILogger<PaymentFailedConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            _logger.LogWarning($"Ödeme başarısız oldu. Sipariş ID: {context.Message.OrderId}. Stoklar geri yükleniyor...");

            // Mesajın içindeki ürün listesini (Items) dönüyoruz
            foreach (var item in context.Message.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    // Düşülen stoğu iade ediyoruz (Telafi / Compensation)
                    product.StockQuantity += item.Quantity;
                    _productRepository.Update(product);
                }
            }

            // Değişiklikleri veritabanına yansıtıyoruz
            await _productRepository.SaveChangesAsync();

            _logger.LogInformation($"Sipariş ID: {context.Message.OrderId} için stoklar başarıyla geri yüklendi.");
        }
    }
}
