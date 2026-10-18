using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Domain.Interfaces
{
    public interface ICatalogGrpcService
    {
        // gRPC'den dönecek veriyi taşıyacak basit bir sözleşme (Tuple kullanıyoruz)
        Task<(decimal Price, int StockQuantity, string Name)> GetProductInfoAsync(string productId);
    }
}
