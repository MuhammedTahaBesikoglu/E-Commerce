using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<global::Order.Domain.Entities.Order> AddOrderAsync(global::Order.Domain.Entities.Order order);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<global::Order.Domain.Entities.Order>> GetOrdersByUserNameAsync(string userName);
    }
}
