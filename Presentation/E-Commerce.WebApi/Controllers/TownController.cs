using E_Commerce.Application.Features.Mediator.Commands.TownCommands;
using E_Commerce.Application.Features.Mediator.Queries.TownQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TownController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> TownList()
        {
            var values = await _mediator.Send(new GetTownQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTown(int id)
        {
            var value = await _mediator.Send(new GetTownByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<IActionResult> CreateTown(CreateTownCommand command)
        {
            await _mediator.Send(command);
            return Ok("İlçe Başarıyla Eklendi");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveTown(int id)
        {
            await _mediator.Send(new RemoveTownCommand(id));
            return Ok("İlçe Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTown(UpdateTownCommand command)
        {
            await _mediator.Send(command);
            return Ok("İlçe Başarıyla Güncellendi");
        }
    }
}
