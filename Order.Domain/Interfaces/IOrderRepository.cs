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
        Task<global::Order.Domain.Entities.Order> GetOrderByIdAsync(Guid id);
        void UpdateOrder(global::Order.Domain.Entities.Order order);
        Task<global::Order.Domain.Entities.OrderItem> GetOrderItemByIdAsync(Guid id);

    }
}
