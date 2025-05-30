using E_Commerce.Application.Features.Mediator.Commands.OrderItemCommands;
using E_Commerce.Application.Features.Mediator.Queries.OrderItemQueries;
using E_Commerce.Application.Features.Mediator.Queries.ProductQueries;
using ECommerce.Dto.OrderItemDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> OrderItemsList()
        {
            var values = await _mediator.Send(new GetOrderItemQuery());
            return Ok(values);
        }
        [HttpGet("ThirtyLastOrderItemTotalAmountList")]
        public async Task<IActionResult> ThirtyLastOrderItemTotalAmountList()
        {
            var values = await _mediator.Send(new GetOrderItemQuery());
            return Ok(values);
        }

        [HttpGet("GetBestSellerProduct")]
        public async Task<IActionResult> GetBestSellerProduct([FromQuery] int? favoritesCardId = null)
        {
            var values = await _mediator.Send(new GetProductWithTotalQuantityQuery { FavoritesCardId = favoritesCardId });
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItems(int id)
        {
            var value = await _mediator.Send(new GetOrderItemByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItems(CreateOrderItemCommand command)
        {
            await _mediator.Send(command);
            return Ok("Sipariş Başarıyla Eklendi");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveOrderItems(int id)
        {
            await _mediator.Send(new RemoveOrderItemCommand(id));
            return Ok("Sipariş Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderItems(UpdateOrderItemCommand command)
        {
            await _mediator.Send(command);
            return Ok("Sipariş Başarıyla Güncellendi");
        }

        [HttpGet("by-order/{orderId}")]
        public async Task<ActionResult<IEnumerable<GetOrderItemDto>>> GetByOrderId(int orderId)
        {
            var query = new GetOrderItemsByOrderIdQuery(orderId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
