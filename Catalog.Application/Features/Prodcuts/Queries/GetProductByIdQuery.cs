using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Features.Products.Queries
{
    public class GetProductByIdQuery : IRequest<ProductResponse>
    {
        public Guid Id { get; set; }
        public GetProductByIdQuery(Guid id) => Id = id;
    }
}