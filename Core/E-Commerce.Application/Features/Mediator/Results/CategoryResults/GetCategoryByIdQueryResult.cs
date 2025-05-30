namespace E_Commerce.Application.Features.Mediator.Results.CategoryResults
{
    public class GetCategoryByIdQueryResult
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
