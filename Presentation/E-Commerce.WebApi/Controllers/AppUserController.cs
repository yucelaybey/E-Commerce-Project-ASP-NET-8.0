using E_Commerce.Application.Features.Mediator.Commands.AppUserCommands;
using E_Commerce.Application.Features.Mediator.Commands.CategoryCommands;
using E_Commerce.Application.Features.Mediator.Commands.ProductCommands;
using E_Commerce.Application.Features.Mediator.Queries.AppUsersQueries;
using E_Commerce.Application.Features.Mediator.Queries.CityQueries;
using E_Commerce.Application.Features.Mediator.Queries.ProductQueries;
using E_Commerce.Application.Tools;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> AppUserList()
        {
            var values = await _mediator.Send(new GetAppUserQuery());
            return Ok(values);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateAppUserPasswordResetCommand command)
        {
            command.AppUserId = id;
            await _mediator.Send(command);
            return Ok("Kişi Bilgisi Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppUser(int id)
        {
            var value = await _mediator.Send(new GetByIdProfilesQuery(id));
            return Ok(value);

        }

        [HttpPut("UpdateAppUser/{id}")]
        public async Task<IActionResult> UpdateAppUser(int id, [FromBody] UpdateByIdProfilesCommand command)
        {
            command.AppUserId = id;
            await _mediator.Send(command);
            return Ok("Kişi Bilgileri Başarıyla Güncellendi");
        }
    }
}
