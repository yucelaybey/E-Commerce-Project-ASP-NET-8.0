using E_Commerce.Application.Features.Mediator.Commands.ShoppingCardCommands;
using E_Commerce.Application.Features.Mediator.Queries.ShoppingCardQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCardsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShoppingCardsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> ShoppingCardList()
        {
            var values = await _mediator.Send(new GetShoppingCardQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShoppingCard(int id)
        {
            var value = await _mediator.Send(new GetShoppingCardByIdQuery(id));
            return Ok(value);

        }

        [HttpGet("check-user-has-cart/{userId}")]
        public async Task<IActionResult> CheckUserHasShoppingCard(string userId)
        {
            var result = await _mediator.Send(new GetShoppingCardUserIdQuery(userId));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShoppingCard(CreateShoppingCardCommand command)
        {
            await _mediator.Send(command);
            return Ok("Sepete Başarıyla Eklendi");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveShoppingCard(int id)
        {
            await _mediator.Send(new RemoveShoppingCardCommand(id));
            return Ok("Sepet Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateShoppingCard(UpdateShoppingCardCommand command)
        {
            await _mediator.Send(command);
            return Ok("Sepet Başarıyla Güncellendi");
        }

        [HttpGet("GetTotalQuantity")]
        public async Task<IActionResult> GetTotalQuantity(
        [FromQuery] string userId) // Query string'den alıyoruz
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId query parametresi gereklidir");

            var query = new GetTotalQuantityByUserIdQuery { UserId = userId };
            var totalQuantity = await _mediator.Send(query);

            return Ok(new
            {
                Success = true,
                UserId = userId, // Gönderdiğiniz UserId'yi echo yapıyoruz
                TotalQuantity = totalQuantity ?? 0
            });
        }
    }
}
