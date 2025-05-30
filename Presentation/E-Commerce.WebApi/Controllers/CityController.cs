using E_Commerce.Application.Features.Mediator.Commands.CityCommands;
using E_Commerce.Application.Features.Mediator.Queries.CityQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> CityList()
        {
            var values = await _mediator.Send(new GetCityQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id)
        {
            var value = await _mediator.Send(new GetCityByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<IActionResult> CreateCity(CreateCityCommand command)
        {
            await _mediator.Send(command);
            return Ok("Şehir Başarıyla Eklendi");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCity(int id)
        {
            await _mediator.Send(new RemoveCityCommand(id));
            return Ok("Şehir Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCity(UpdateCityCommand command)
        {
            await _mediator.Send(command);
            return Ok("Şehir Başarıyla Güncellendi");
        }
    }
}
