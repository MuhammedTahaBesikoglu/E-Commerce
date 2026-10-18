using Catalog.Application.Responses;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Prodcuts.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IRepository<Product> _productRepository;

        public CreateProductCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // API'den gelen request verisi ile yeni bir veritabanı Entity'si oluşturuyoruz
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId
            };

            
            await _productRepository.AddAsync(newProduct);
            await _productRepository.SaveChangesAsync(cancellationToken);

            // Eklenen ürünün DTO'sunu geriye dönüyoruz.
            return new ProductResponse(
                newProduct.Id,
                newProduct.Name,
                newProduct.Description,
                newProduct.Price,
                newProduct.ImageUrl,
                newProduct.StockQuantity,
                newProduct.CategoryId);
        }
    }
}
