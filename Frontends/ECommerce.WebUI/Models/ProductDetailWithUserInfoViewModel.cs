using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.ProductDtos;

namespace ECommerce.WebUI.Models
{
    public class ProductDetailWithUserInfoViewModel
    {
        public ProductWithCategoryDto Product { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }
}
