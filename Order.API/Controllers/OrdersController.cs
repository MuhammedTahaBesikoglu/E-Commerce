using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.Orders.Commands;
using Order.Application.Features.Orders.Queries;
using Order.Application.Responses;
using System.Net;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrdersByUserNameQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrderById(Guid id)
        {
            var query = new GetOrderByIdQuery(id);
            var order = await _mediator.Send(query);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var command = new CancelOrderCommand(id);
            var result = await _mediator.Send(command);
            if (!result) return BadRequest("Sipariş iptal edilemedi veya bulunamadı.");
            return Ok("Sipariş başarıyla iptal edildi ve iade süreci başlatıldı.");
        }
    }
}
