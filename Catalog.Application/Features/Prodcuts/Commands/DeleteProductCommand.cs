using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Catalog.Application.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public DeleteProductCommand(Guid id) => Id = id;
    }
}
