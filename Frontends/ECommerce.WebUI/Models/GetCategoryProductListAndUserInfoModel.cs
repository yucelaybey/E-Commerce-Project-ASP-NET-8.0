using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.CategoryProductList;

namespace ECommerce.WebUI.Models
{
    public class GetCategoryProductListAndUserInfoModel
    {
        public List<CategoryProductListDto> Products { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }
}
