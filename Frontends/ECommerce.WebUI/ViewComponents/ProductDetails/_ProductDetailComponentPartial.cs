using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.ProductDtos;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.WebUI.ViewComponents
{
    public class _ProductDetailComponentPartial : ViewComponent
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public _ProductDetailComponentPartial(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            // Get product details
            var product = await _httpClient.GetFromJsonAsync<ProductWithCategoryDto>($"https://localhost:7026/api/Products/WithCategory/{productId}");

            // Get user info
            var claimsPrincipal = User as ClaimsPrincipal;
            var email = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;
            var roles = claimsPrincipal?.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList() ?? new List<string>();

            // Create combined model
            var viewModel = new ProductDetailWithUserInfoViewModel
            {
                Product = product,
                UserInfo = new UserInfoDto
                {
                    Email = email,
                    Roles = roles
                }
            };

            return View(viewModel);
        }
    }
}