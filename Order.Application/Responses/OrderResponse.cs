using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Responses
{
    public record OrderItemResponse(Guid ProductId, string ProductName, decimal Price, int Quantity, string ImageUrl);

    public record OrderResponse(
        Guid Id,
        string UserName,
        decimal TotalPrice,
        string FirstName,
        string LastName,
        string EmailAddress,
        string AddressLine,
        string OrderStatus,
        List<OrderItemResponse> Items);
}
