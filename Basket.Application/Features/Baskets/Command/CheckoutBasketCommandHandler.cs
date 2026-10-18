using System;
using System.Collections.Generic;
using System.Text;
using Basket.Application.Features.Baskets.Commands;
using Basket.Domain.Interfaces;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;

namespace Basket.Application.Features.Baskets.Handlers
{
    public class CheckoutBasketCommandHandler : IRequestHandler<CheckoutBasketCommand, bool>
    {
        private readonly IBasketRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        // MassTransit'in IPublishEndpoint arayüzü ile mesajı kuyruğa fırlatacağız
        public CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcının sepetini Redis'ten getir
            var basket = await _repository.GetBasketAsync(request.UserName);
            if (basket == null)
            {
                return false; // Sepet yoksa işlemi iptal et
            }

            // 2. Event nesnesini oluştur ve kullanıcıdan gelen bilgilerle doldur
            var eventMessage = new BasketCheckoutEvent
            {
                UserName = request.UserName,
                // Sepetteki ürünlerin fiyatlarını toplayarak TotalPrice'ı manuel hesaplıyoruz
                TotalPrice = basket.Items.Sum(item => item.Price * item.Quantity),
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                AddressLine = request.AddressLine,
                Country = request.Country,
                ZipCode = request.ZipCode,
                CardName = request.CardName,
                CardNumber = request.CardNumber,
                Expiration = request.Expiration,
                CVV = request.CVV,
                PaymentMethod = request.PaymentMethod,
                Items = basket.Items.Select(x => new BasketItemMessage
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    ImageUrl = x.ImageUrl
                }).ToList()
            };

            // 3. Mesajı RabbitMQ kuyruğuna fırlat
            await _publishEndpoint.Publish(eventMessage, cancellationToken);

            // 4. Sipariş onaylandığı için mevcut sepeti Redis'ten sil
            await _repository.DeleteBasketAsync(request.UserName);

            return true;
        }
    }
}
