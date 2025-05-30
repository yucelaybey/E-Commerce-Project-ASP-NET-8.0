using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.CategoryDtos;

namespace ECommerce.WebUI.Models
{
    public class GetCategoryAndUserInfoModel
    {
        public List<GetCategoryDto> Categories { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }
}
