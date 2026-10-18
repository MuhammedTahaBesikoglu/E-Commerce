using Microsoft.EntityFrameworkCore;
using Order.Domain.Interfaces;
using Order.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<global::Order.Domain.Entities.Order> AddOrderAsync(global::Order.Domain.Entities.Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            return order;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<global::Order.Domain.Entities.Order>> GetOrdersByUserNameAsync(string userName)
        {
            return await _dbContext.Orders
                .Include(o => o.Items) 
                .Where(o => o.UserName == userName)
                .ToListAsync();
        }
    }
}
