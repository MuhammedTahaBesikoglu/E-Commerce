using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Domain.Entities
{
    public class ShoppingCart
    {
        // Redis'te sepeti ararken kullanacağımız anahtar (genellikle kullanıcının adı veya User ID'si olur)
        public string UserName { get; set; }

        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCart()
        {
        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        // Sepetin toplam fiyatını dinamik olarak hesaplayan özellik
        public decimal TotalPrice
        {
            get
            {
                decimal totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += item.Price * item.Quantity;
                }
                return totalprice;
            }
        }
    }
}
