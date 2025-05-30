using E_Commerce.Application.Features.Mediator.Commands.FavoritesCardItemCommands;
using E_Commerce.Application.Features.Mediator.Commands.FavoritesCommands;
using E_Commerce.Application.Features.Mediator.Queries.FavoritesCardItemQueries;
using E_Commerce.Application.Features.Mediator.Queries.FavoritesQueries;
using ECommerce.Dto.FavoritesCardItemDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesCardItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FavoritesCardItemController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> FavoritesCardItemList()
        {
            var values = await _mediator.Send(new GetFavoritesCardItemQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFavoritesCardItem(int id)
        {
            var value = await _mediator.Send(new GetFavoritesCardItemByIdQuery(id));
            if (value.FavoritesCardItemID !=null)
            {
                return Ok(value);
            }
            return StatusCode(404);
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] FavoritesCardItemCheckDto request)
        {
            var result = await _mediator.Send( new CreateFavoritesCardItemCommand(request.FavoritesCardID, request.ProductID));

            return Ok(new
            {
                favoritesCardItemId = result.FavoritesCardItemID,
                favoritesCardId = result.FavoritesCardID
            });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFavoritesCardItem(int id)
        {
            await _mediator.Send(new RemoveFavoritesCardItemCommand(id));
            return Ok("Ürün Favorilerden Başarıyla Silindi");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFavoritesCardItem(int id, [FromBody] UpdateFavoritesCardItemCommand command)
        {
            command.FavoritesCardItemID = id;
            await _mediator.Send(command);
            return Ok("Ürün Favorileri Başarıyla Güncellendi");
        }

        [HttpGet("by-favorites-card/{favoritesCardId}")]
        public async Task<ActionResult<List<FavoritesCardItemWithProductDto>>> GetByFavoritesCardId(int favoritesCardId)
        {
            var query = new GetFavoritesCardItemsWithProductsQuery { FavoritesCardId = favoritesCardId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("check-item")]
        public async Task<ActionResult<bool>> CheckFavoriteItem([FromBody] FavoritesCardItemCheckDto request)
        {
            var result = await _mediator.Send(new CheckFavoriteItemQuery
            {
                FavoritesCardID = request.FavoritesCardID,
                ProductID = request.ProductID
            });

            return Ok(result);
        }
    }
}
