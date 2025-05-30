using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.ProductDtos;

namespace ECommerce.WebUI.Models
{
    public class ProductRecommendationViewModel
    {
        public List<GetProductDto> Products { get; set; }
        public UserInfoDto? UserInfo { get; set; }
    }
}
