using Basket.Application.Features.Baskets.Command;
using Basket.Domain.Entities;
using Basket.Domain.Interfaces;
using MediatR;

namespace Basket.Application.Features.Baskets.Handlers
{
    public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, ShoppingCart>
    {
        private readonly IBasketRepository _repository;
        private readonly ICatalogGrpcService _catalogGrpcService; // Eklenen gRPC servisimiz

        public UpdateBasketCommandHandler(IBasketRepository repository, ICatalogGrpcService catalogGrpcService)
        {
            _repository = repository;
            _catalogGrpcService = catalogGrpcService;
        }

        public async Task<ShoppingCart> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
        {
            var existingBasket = await _repository.GetBasketAsync(request.UserName);
            if (existingBasket == null)
            {
                existingBasket = new ShoppingCart(request.UserName);
            }

            foreach (var newItem in request.Items)
            {
                // 1. gRPC İLE CATALOG SERVİSİNDEN ANLIK BİLGİ ÇEKİYORUZ
                var catalogInfo = await _catalogGrpcService.GetProductInfoAsync(newItem.ProductId.ToString());

                // 2. STOK KONTROLÜ
                if (catalogInfo.StockQuantity < newItem.Quantity)
                {
                    throw new Exception($"'{catalogInfo.Name}' ürünü için yeterli stok yok! Mevcut stok: {catalogInfo.StockQuantity}");
                }

                // 3. GÜVENLİK: Kullanıcının (frontend) sepete koyduğu sahte fiyatı ezip, veritabanındaki GÜNCEL fiyatı atıyoruz
                newItem.Price = catalogInfo.Price;
                newItem.ProductName = catalogInfo.Name;

                // 4. Miktar Birleştirme Mantığı
                var existingItem = existingBasket.Items.FirstOrDefault(x => x.ProductId == newItem.ProductId);

                if (existingItem != null)
                {
                    existingItem.Quantity += newItem.Quantity;
                    existingItem.Price = newItem.Price; // Fiyatını güncelliyoruz
                }
                else
                {
                    existingBasket.Items.Add(newItem);
                }
            }

            return await _repository.UpdateBasketAsync(existingBasket);
        }
    }
}