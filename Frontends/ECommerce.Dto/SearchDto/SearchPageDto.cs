using ECommerce.Dto.AppUserDto;
using ECommerce.Dto.CategoryDtos;
using ECommerce.Dto.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.SearchDto
{
    public class SearchPageDto
    {
        public List<GetProductDto> Products { get; set; }
        public List<GetCategoryDto> Categories { get; set; }
        public string SearchTerm { get; set; }
        public string NormalizedSearchTerm { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }
}
