using Basket.API.Protos;
using Basket.Domain.Interfaces;

namespace Basket.API.Services
{
    // Domain'deki sözleşmeyi burada (gRPC client'ı tanıyan API katmanında) implemente ediyoruz
    public class CatalogGrpcService : ICatalogGrpcService
    {
        private readonly ProductProtoService.ProductProtoServiceClient _grpcClient;

        public CatalogGrpcService(ProductProtoService.ProductProtoServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public async Task<(decimal Price, int StockQuantity, string Name)> GetProductInfoAsync(string productId)
        {
            // Catalog servisine gRPC üzerinden anlık istek atıyoruz
            var request = new GetProductRequest { Id = productId };
            var response = await _grpcClient.GetProductAsync(request);

            return ((decimal)response.Price, response.StockQuantity, response.Name);
        }
    }
}