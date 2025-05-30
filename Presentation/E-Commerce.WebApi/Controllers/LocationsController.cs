using E_Commerce.Application.Features.Mediator.Queries.DistrictQueries;
using E_Commerce.Application.Features.Mediator.Queries.QuarterQueries;
using E_Commerce.Application.Features.Mediator.Queries.TownQueries;
using ECommerce.Dto.DistrictDto;
using ECommerce.Dto.QuarterDto;
using ECommerce.Dto.TownDto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("towns/{cityId}")]
        public async Task<ActionResult<List<GetTownDto>>> GetTownsByCityId(int cityId)
        {
            var query = new GetTownsByCityIdQuery { CityId = cityId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("quarters/{townId}")]
        public async Task<ActionResult<List<GetQuarterDto>>> GetQuartersByTownId(int townId)
        {
            var query = new GetQuartersByTownIdQuery { TownId = townId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("districts/{quarterId}")]
        public async Task<ActionResult<List<GetDistrictDto>>> GetDistrictsByQuarterId(int quarterId)
        {
            var query = new GetDistrictsByQuarterIdQuery { QuarterId = quarterId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
