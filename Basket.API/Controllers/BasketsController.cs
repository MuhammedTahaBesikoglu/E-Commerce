using Basket.Application.Features.Baskets.Command;
using Basket.Application.Features.Baskets.Commands;
using Basket.Application.Features.Baskets.Query;
using Basket.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var query = new GetBasketQuery(userName);
            var basket = await _mediator.Send(query);

            return Ok(basket);
        }

  
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] UpdateBasketCommand command)
        {
            var updatedBasket = await _mediator.Send(command);

            return Ok(updatedBasket);
        }


        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            var command = new DeleteBasketCommand(userName);
            await _mediator.Send(command);

            return Ok();
        }

        // POST: api/baskets/checkout
        [HttpPost("checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] CheckoutBasketCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
            {
                return BadRequest("Sepet bulunamadı veya onaylanamadı.");
            }

            return Accepted(); // 202 Accepted: İşlem alındı, arka planda (kuyrukta) işleniyor.
        }
    }
}
