using Basket.Domain.Entities;
using Basket.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Features.Baskets.Query
{
    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, ShoppingCart>
    {
        private readonly IBasketRepository _repository;

        public GetBasketQueryHandler(IBasketRepository repository)
        {
            _repository = repository;
        }

        public async Task<ShoppingCart> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await _repository.GetBasketAsync(request.UserName);

            // Eğer Redis'te bu kullanıcıya ait sepet yoksa yeni boş bir sepet dönüyoruz
            return basket ?? new ShoppingCart(request.UserName);
        }
    }
}
