using E_Commerce.Application.Features.Mediator.Commands.DistrictCommands;
using E_Commerce.Application.Features.Mediator.Queries.DistrictQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DistrictController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> DistrictList()
        {
            var values = await _mediator.Send(new GetDistrictQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDistrict(int id)
        {
            var value = await _mediator.Send(new GetDistrictByIdQuery(id));
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDistrict(CreateDistrictCommand command)
        {
            await _mediator.Send(command);
            return Ok("Mahalle Başarıyla Eklendi");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveDistrict(int id)
        {
            await _mediator.Send(new RemoveDistrictCommand(id));
            return Ok("Mahalle Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDistrict(UpdateDistrictCommand command)
        {
            await _mediator.Send(command);
            return Ok("Mahalle Başarıyla Güncellendi");
        }
    }
}
