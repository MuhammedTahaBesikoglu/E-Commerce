using Catalog.Application.Responses;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Prodcuts.Queries
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductResponse>>
    {
        private readonly IRepository<Product> _productRepository;

        // Daha önce altyapısını kurduğumuz Repository'yi buraya enjekte ediyoruz.
        public GetAllProductsQueryHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();

            // Veritabanından gelen Product (Entity) listesini, dışarıya açacağımız ProductResponse listesine dönüştürüyoruz (Mapping).
            return products.Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.ImageUrl,
                p.StockQuantity,
                p.CategoryId)).ToList();
        }
    }
}
