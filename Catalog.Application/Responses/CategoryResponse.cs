using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Responses
{
    public record CategoryResponse
    (
        Guid Id, 
        string Name, 
        string Description
    );
}
