using Catalog.Application.Responses;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Command
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
    {
        private readonly IRepository<Category> _categoryRepository;

        public CreateCategoryCommandHandler(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description
            };

            // Kategoriyi belleğe al ve ardından veritabanına fiziksel olarak kaydet
            await _categoryRepository.AddAsync(newCategory);
            await _categoryRepository.SaveChangesAsync(cancellationToken);

            return new CategoryResponse(newCategory.Id, newCategory.Name, newCategory.Description);
        }
    }
}
