using E_Commerce.Application.Features.Mediator.Commands.OrderPaymentCardCommands;
using E_Commerce.Application.Features.Mediator.Commands.OrderPaymentOrderCommands;
using E_Commerce.Application.Features.Mediator.Queries.OrderPaymentOtherQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPaymentOthersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderPaymentOthersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> OrderPaymentOthersList()
        {
            var values = await _mediator.Send(new GetOrderPaymentOtherQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderPaymentOthers(int id)
        {
            var value = await _mediator.Send(new GetOrderPaymentOtherByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrderPaymentOther([FromBody] CreateOrderPaymentOtherCommand command)
        {
            var orderPaymentOtherId = await _mediator.Send(command);
            return Ok(orderPaymentOtherId);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveOrderPaymentOthers(int id)
        {
            await _mediator.Send(new RemoveOrderPaymentOtherCommand(id));
            return Ok("Şehir Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderPaymentOthers(UpdateOrderPaymentOtherCommand command)
        {
            await _mediator.Send(command);
            return Ok("Şehir Başarıyla Güncellendi");
        }
    }
}
