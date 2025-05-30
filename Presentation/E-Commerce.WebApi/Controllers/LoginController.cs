using E_Commerce.Application.Features.Mediator.Queries.AppUsersQueries;
using E_Commerce.Application.Tools;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Index(GetCheckAppUserQuery query)
        {
            var values = await _mediator.Send(query);
            if (values.IsExist)
            {
                return Created("", JwtTokenGenerator.GenerateToken(values));
            }
            else
            {
                return BadRequest("Kullanıcı Adı veya Şifre hatalıdır.");
            }
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetLoginInformation(string email)
        {
            var value = await _mediator.Send(new GetAppUserByIdQuery(email));
            return Ok(value);
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email adresi boş olamaz.");
            }

            var query = new CheckEmailExistsQuery { Email = email };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
