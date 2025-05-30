using E_Commerce.Application.Features.Mediator.Commands.OrderCommands;
using E_Commerce.Application.Features.Mediator.Commands.OrderPaymentCardCommands;
using E_Commerce.Application.Features.Mediator.Queries.OrderPaymentCardQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPaymentCardsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderPaymentCardsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> OrderPaymentCardsList()
        {
            var values = await _mediator.Send(new GetOrderPaymentCardQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderPaymentCards(int id)
        {
            var value = await _mediator.Send(new GetOrderPaymentCardByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrderPaymentCards([FromBody] CreateOrderPaymentCardCommand command)
        {
            var orderPaymentCardId = await _mediator.Send(command);
            return Ok(orderPaymentCardId);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveOrderPaymentCards(int id)
        {
            await _mediator.Send(new RemoveOrderPaymentCardCommand(id));
            return Ok("Sipariş Edilirken Kullanılan Kart Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderPaymentCards(UpdateOrderPaymentCardCommand command)
        {
            await _mediator.Send(command);
            return Ok("Sipariş Edilirken Kullanılan Kart Başarıyla Güncellendi");
        }
    }
}
