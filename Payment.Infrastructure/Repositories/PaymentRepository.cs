using System;
using System.Collections.Generic;
using System.Text;
using Payment.Domain.Entities;
using Payment.Domain.Interfaces;
using Payment.Infrastructure.Context;

namespace Payment.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _dbContext;

        public PaymentRepository(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTransactionAsync(PaymentTransaction transaction)
        {
            await _dbContext.PaymentTransactions.AddAsync(transaction);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
