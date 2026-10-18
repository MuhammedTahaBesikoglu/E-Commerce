using Basket.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Features.Baskets.Query
{
    public class GetBasketQuery : IRequest<ShoppingCart>
    {
        public string UserName { get; set; }
        public GetBasketQuery(string userName) => UserName = userName;
    }
}
