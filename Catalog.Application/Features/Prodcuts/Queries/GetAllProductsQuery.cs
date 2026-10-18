using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Prodcuts.Queries
{
    public class GetAllProductsQuery : IRequest<List<ProductResponse>>
    {
    }
}
