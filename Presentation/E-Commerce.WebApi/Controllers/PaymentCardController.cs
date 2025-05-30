using E_Commerce.Application.Features.Mediator.Commands.PaymentCardCommands;
using E_Commerce.Application.Features.Mediator.Queries.PaymentCardQueries;
using ECommerce.Dto.PaymentCardDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentCardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentCardController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> PaymentCardsList()
        {
            var values = await _mediator.Send(new GetPaymentCardQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentCards(int id)
        {
            var value = await _mediator.Send(new GetPaymentCardByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<ActionResult<CreatedPaymentCardResponseDto>> CreatePaymentCard(CreatePaymentCardDto createPaymentCardDto)
        {
            var command = new CreatePaymentCardCommand { CreatePaymentCardDto = createPaymentCardDto };
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> RemovePaymentCards(int id)
        {
            await _mediator.Send(new RemovePaymentCardCommand(id));
            return Ok("Kart Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePaymentCards(UpdatePaymentCardCommand command)
        {
            await _mediator.Send(command);
            return Ok("Kart Başarıyla Güncellendi");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPaymentCardsByUserId(int userId)
        {
            var query = new GetUserPaymentCardsResult { UserId = userId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("set-default")]
        public async Task<IActionResult> SetDefaultCard([FromBody] SetDefaultPaymentCardCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest("Payment card not found or doesn't belong to user");
            }

            return Ok("Default payment card updated successfully");
        }
    }
}
