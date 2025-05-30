using E_Commerce.Application.Features.Mediator.Commands.OrderCommands;
using E_Commerce.Application.Features.Mediator.Queries.OrderQueries;
using ECommerce.Dto.OrderDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
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
        [HttpGet]
        public async Task<IActionResult> OrderList()
        {
            var values = await _mediator.Send(new GetOrderQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var value = await _mediator.Send(new GetOrderByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);
            return Ok(orderId);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveOrder(int id)
        {
            await _mediator.Send(new RemoveOrderCommand(id));
            return Ok("Siparişler Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(UpdateOrderCommand command)
        {
            await _mediator.Send(command);
            return Ok("Siparişler Başarıyla Güncellendi");
        }
        [HttpGet("ThirtyLastOrderTotalAmountList")]
        public async Task<IActionResult> ThirtyLastOrderTotalAmountList()
        {
            var values = await _mediator.Send(new GetThirtyLastOrderTotalAmountQuery());
            return Ok(values);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<GetOrderDto>>> GetOrdersByUserId(string userId)
        {
            var query = new GetOrdersByUserIdQuery(userId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
