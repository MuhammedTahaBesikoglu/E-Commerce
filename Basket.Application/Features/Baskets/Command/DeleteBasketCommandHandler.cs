using Basket.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Features.Baskets.Command
{
    public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, Unit>
    {
        private readonly IBasketRepository _repository;

        public DeleteBasketCommandHandler(IBasketRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteBasketAsync(request.UserName);
            return Unit.Value;
        }
    }
}
