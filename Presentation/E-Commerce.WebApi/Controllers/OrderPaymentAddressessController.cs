using E_Commerce.Application.Features.Mediator.Commands.OrderCommands;
using E_Commerce.Application.Features.Mediator.Commands.OrderPaymentAddressCommands;
using E_Commerce.Application.Features.Mediator.Queries.OrderPaymentAddressQueries;
using E_Commerce.Application.Features.Mediator.Queries.OrderQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPaymentAddressessController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderPaymentAddressessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrderPaymentAddress([FromBody] CreateOrderPaymentAddressCommand command)
        {
            var orderId = await _mediator.Send(command);
            return Ok(orderId);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderPaymentAddress(int id)
        {
            var value = await _mediator.Send(new GetOrderPaymentAddressByIdQuery(id));
            return Ok(value);

        }
    }
}
