using E_Commerce.Application.Features.Mediator.Commands.QuarterCommands;
using E_Commerce.Application.Features.Mediator.Queries.QuarterQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuarterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuarterController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> QuarterList()
        {
            var values = await _mediator.Send(new GetQuarterQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuarter(int id)
        {
            var value = await _mediator.Send(new GetQuarterByIdQuery(id));
            return Ok(value);

        }

        [HttpPost]
        public async Task<IActionResult> CreateQuarter(CreateQuarterCommand command)
        {
            await _mediator.Send(command);
            return Ok("Semt Başarıyla Eklendi");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveQuarter(int id)
        {
            await _mediator.Send(new RemoveQuarterCommand(id));
            return Ok("Semt Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuarter(UpdateQuarterCommand command)
        {
            await _mediator.Send(command);
            return Ok("Semt Başarıyla Güncellendi");
        }
    }
}
