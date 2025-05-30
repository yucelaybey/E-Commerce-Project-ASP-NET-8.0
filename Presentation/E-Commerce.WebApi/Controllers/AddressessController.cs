using E_Commerce.Application.Features.Mediator.Commands.AddressCommands;
using E_Commerce.Application.Features.Mediator.Queries.AddressQueries;
using ECommerce.Dto.AddressDto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressessController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressessController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> AddresssList()
        {
            var values = await _mediator.Send(new GetAddressQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddresss(int id)
        {
            var value = await _mediator.Send(new GetAddressByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDto createAddressDto)
        {
            var command = new CreateAddressCommand(createAddressDto);
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = result.Message,
                    Data = new { AddressId = result.Data }
                });
            }

            return BadRequest(new { Success = false, result.Message });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAddresss(int id)
        {
            await _mediator.Send(new RemoveAddressCommand(id));
            return Ok("Adres Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAddresss(UpdateAddressCommand command)
        {
            await _mediator.Send(command);
            return Ok("Adres Başarıyla Güncellendi");
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<GetAddressDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var query = new GetAddressesByUserIdQuery(userId);
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
                return NotFound("Kullanıcıya ait adres bulunamadı");

            return Ok(result);
        }

        [HttpPost("set-default")]
        public async Task<IActionResult> SetDefaultAddress([FromBody] SetDefaultAddressDto dto)
        {
            var result = await _mediator.Send(new SetDefaultAddressCommand
            {
                AddressId = dto.AddressId,
                UserId = dto.UserId
            });

            return result.Succeeded
                ? Ok(new { success = true })
                : BadRequest(new { error = result.Error });
        }
    }
}
