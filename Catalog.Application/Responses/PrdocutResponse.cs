using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Responses
{
    public record ProductResponse(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string ImageUrl,
        int StockQuantity,
        Guid CategoryId
     );
}
