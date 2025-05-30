using Microsoft.AspNetCore.Http;

namespace ECommerce.Dto.CategoryDtos
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
