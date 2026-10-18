namespace EventBus.Messages.Events
{
    // Kuyruğa bırakılacak olan sipariş onay mesajımızın şablonu
    public class BasketCheckoutEvent
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

        // Ödeme (Kart) Bilgileri
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }

        // Siparişteki Ürünler
        public List<BasketItemMessage> Items { get; set; } = new List<BasketItemMessage>();
    }
}