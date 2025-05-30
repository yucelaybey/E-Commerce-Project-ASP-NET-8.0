using E_Commerce.Application.Features.Mediator.Commands.ProductCommands;
using E_Commerce.Application.Features.Mediator.Commands.ShoppingCardItemCommands;
using E_Commerce.Application.Features.Mediator.Queries.ShoppingCardItemQueries;
using ECommerce.Dto.ShoppingCardItem;
using ECommerce.Dto.ShoppingCardItemDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCardItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShoppingCardItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> ShoppingCardList()
        {
            var values = await _mediator.Send(new GetShoppingCardItemQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShoppingCard(int id)
        {
            var value = await _mediator.Send(new GetShoppingCardItemByIdQuery(id));
            return Ok(value);

        }

        [HttpGet("GetFiveAddedProducts")]
        public async Task<ActionResult<List<BestFiveAddShoppingCardItemDto>>> GetFiveAddedProducts()
        {
            var query = new GetBestFiveAddShoppingCardItemQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShoppingCardItemDto itemDto)
        {
            var command = new CreateShoppingCardItemCommand
            {
                ItemDto = itemDto
            };

            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveShoppingCard(int id)
        {
            await _mediator.Send(new RemoveShoppingCardItemCommand(id));
            return Ok("Sepet Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateShoppingCard(UpdateShoppingCardItemCommand command)
        {
            await _mediator.Send(command);
            return Ok("Sepet Başarıyla Güncellendi");
        }

        [HttpGet("exists")]
        public async Task<ActionResult<bool>> CheckItemExists([FromQuery] int productId, [FromQuery] int shoppingCardId)
        {
            var query = new CheckShoppingCartItemExistsQuery
            {
                ProductId = productId,
                ShoppingCardId = shoppingCardId
            };

            var exists = await _mediator.Send(query);
            return Ok(exists);
        }

        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateShoppingCardItemQuantityCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(ShoppingCartWithTotalsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserCartWithTotals(string userId)
        {
            var query = new GetUserCartItemsQuery(userId);
            var result = await _mediator.Send(query);

            if (result.Items == null || !result.Items.Any())
            {
                return NotFound("Shopping cart not found or empty");
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShoppingCardItemQuantity(int id, [FromBody] UpdateShoppingCartItemQuantityCommand command)
        {
            command.ShoppingCartItemId = id;
            await _mediator.Send(command);
            return Ok("Ürün Başarıyla Güncellendi");
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusApiRequest request)
        {
            var command = new UpdateShoppingCartItemStatusCommand
            {
                ShoppingCartItemId = request.ShoppingCartItemId,
                Status = request.Status
            };

            var result = await _mediator.Send(command);

            return result ? Ok() : BadRequest("Güncelleme başarısız oldu.");
        }
    }
}
