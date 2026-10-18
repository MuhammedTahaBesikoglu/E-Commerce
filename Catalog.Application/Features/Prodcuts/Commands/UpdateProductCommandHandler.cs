using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Application.Features.Products.Commands;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.Features.Products.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IRepository<Product> _productRepository;

        public UpdateProductCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null) throw new Exception("Ürün bulunamadı.");

            // Product entity'sindeki alanları güncelliyoruz
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.ImageUrl = request.ImageUrl;
            product.StockQuantity = request.StockQuantity;
            product.CategoryId = request.CategoryId;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
