using Catalog.Application.Features.Products.Queries;
using Catalog.Application.Responses;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.Features.Products.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        private readonly IRepository<Product> _productRepository;

        public GetProductByIdQueryHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Domain katmanında tanımladığımız GetByIdAsync metodunu kullanıyoruz.
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null) return null;

            return new ProductResponse(
                product.Id, product.Name, product.Description,
                product.Price, product.ImageUrl, product.StockQuantity, product.CategoryId);
        }
    }
}