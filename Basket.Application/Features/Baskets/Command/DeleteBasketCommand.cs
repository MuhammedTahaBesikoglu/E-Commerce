using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Features.Baskets.Command
{
    public class DeleteBasketCommand : IRequest<Unit>
    {
        public string UserName { get; set; }
        public DeleteBasketCommand(string userName) => UserName = userName;
    }
}
