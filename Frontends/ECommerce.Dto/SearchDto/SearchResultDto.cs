using ECommerce.Dto.CategoryDtos;
using ECommerce.Dto.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.SearchDto
{
    public class SearchResultDto
    {
        public List<GetProductDto> Products { get; set; }
        public List<GetCategoryDto> Categories { get; set; }
        public int TotalResults { get; set; }
    }
}
