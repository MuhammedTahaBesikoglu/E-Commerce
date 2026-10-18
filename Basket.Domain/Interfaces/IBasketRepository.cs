using Basket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Domain.Interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string userName);
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);
        Task DeleteBasketAsync(string userName);
    }
}
