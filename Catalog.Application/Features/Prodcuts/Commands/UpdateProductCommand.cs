using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Catalog.Application.Features.Products.Commands
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
    }
}