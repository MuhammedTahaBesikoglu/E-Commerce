using System;
using System.Collections.Generic;
using System.Text;
using Payment.Domain.Entities;

namespace Payment.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddTransactionAsync(PaymentTransaction transaction);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
