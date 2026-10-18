using Catalog.API.Protos;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Grpc.Core;

namespace Catalog.API.GrpcServices
{
    // Protos namespace'inden gelen ve otomatik üretilen Base sınıfı miras alıyoruz
    public class ProductGrpcService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly IRepository<Product> _productRepository;

        public ProductGrpcService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<ProductModel> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            // Gelen string ID'yi Guid formatına çeviriyoruz
            if (!Guid.TryParse(request.Id, out Guid productId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Geçersiz ürün ID formatı."));
            }

            // Repository üzerinden ürünü MySQL'den çekiyoruz
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"ID'si {request.Id} olan ürün bulunamadı."));
            }

            // Gelen Product entity'sini gRPC'nin beklediği ProductModel sözleşmesine dönüştürüp (Mapleyip) dönüyoruz
            return new ProductModel
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = (double)product.Price, // gRPC'de decimal desteği olmadığı için double kullanıyoruz
                StockQuantity = product.StockQuantity
            };
        }
    }
}