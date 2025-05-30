using Microsoft.AspNetCore.Http;

namespace ECommerce.Dto.CategoryDtos
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
