using Basket.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Features.Baskets.Command
{
    public class UpdateBasketCommand : IRequest<ShoppingCart>
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; }

        public UpdateBasketCommand(string userName, List<ShoppingCartItem> items)
        {
            UserName = userName;
            Items = items;
        }
    }
}
