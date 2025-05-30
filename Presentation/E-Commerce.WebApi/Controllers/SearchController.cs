using E_Commerce.Application.Features.Mediator.Queries.SearchQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProduct([FromQuery] string searchTerm)
        {
            var query = new SearchProductsQuery { SearchTerm = searchTerm };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("SearchCategory")]
        public async Task<IActionResult> SearchCategory([FromQuery] string searchTerm)
        {
            var query = new SearchCategoriesQuery { SearchTerm = searchTerm };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("SearchProductAll")]
        public async Task<IActionResult> SearchProducts([FromQuery] string searchTerm, [FromQuery] string userId = null)
        {
            try
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    return BadRequest("Search term cannot be empty.");
                }

                var query = new SearchProductAllQuery(searchTerm, userId);
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("SearchCategoryAll")]
        public async Task<IActionResult> SearchCategoryAll([FromQuery] string searchTerm)
        {
            var query = new SearchCategoriesQuery { SearchTerm = searchTerm };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
