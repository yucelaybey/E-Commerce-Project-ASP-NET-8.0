using E_Commerce.Application.Features.Mediator.Commands.FavoritesCommands;
using E_Commerce.Application.Features.Mediator.Queries.FavoritesCardQueries;
using E_Commerce.Application.Features.Mediator.Queries.FavoritesQueries;
using ECommerce.Dto.FavoritesCardDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesCardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FavoritesCardController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> FavoritesCardList()
        {
            var values = await _mediator.Send(new GetFavoritesCardQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFavoritesCard(int id)
        {
            var value = await _mediator.Send(new GetFavoritesCardByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<IActionResult> CreateFavoritesCard(CreateFavoritesCardCommand command)
        {
            await _mediator.Send(command);
            return Ok("Ürün Favorilere Başarıyla Eklendi");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFavoritesCard(int id)
        {
            await _mediator.Send(new RemoveFavoritesCardCommand(id));
            return Ok("Ürün Favorilerden Başarıyla Silindi");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFavoritesCard(int id, [FromBody] UpdateFavoritesCardCommand command)
        {
            command.FavoritesCardID = id;
            await _mediator.Send(command);
            return Ok("Ürün Favorileri Başarıyla Güncellendi");
        }

        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<List<FavoritesCardDto>>> GetByUserId(int userId)
        {
            var query = new GetFavoritesCardsByUserIdQuery { UserId = userId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
