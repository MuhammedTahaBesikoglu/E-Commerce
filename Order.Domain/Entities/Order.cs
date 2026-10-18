using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        // Müşteri ve Adres Bilgileri
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        // Ödeme Bilgileri
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }

        // Siparişin Durumu (Örn: Alındı, Hazırlanıyor, Kargoya Verildi)
        public string OrderStatus { get; set; }

        // Bir siparişin birden fazla ürünü (kalemi) olabilir
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
