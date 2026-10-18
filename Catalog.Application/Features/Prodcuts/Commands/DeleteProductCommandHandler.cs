using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Application.Features.Products.Commands;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using MediatR;

namespace Catalog.Application.Features.Products.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IRepository<Product> _productRepository;

        public DeleteProductCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null) throw new Exception("Silinecek ürün bulunamadı.");

            _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
