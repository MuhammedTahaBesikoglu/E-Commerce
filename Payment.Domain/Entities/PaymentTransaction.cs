using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Domain.Entities
{
    public class PaymentTransaction
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
