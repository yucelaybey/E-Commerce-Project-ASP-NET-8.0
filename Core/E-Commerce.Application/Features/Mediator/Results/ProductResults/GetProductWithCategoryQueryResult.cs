namespace E_Commerce.Application.Features.Mediator.Results.ProductResults
{
    public class GetProductWithCategoryQueryResult
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }
        public int? SalePrice { get; set; }
        public double? DiscountRate { get; set; }
        public string UserId { get; set; }
        public bool SaleSeason { get; set; }
        public int Stock { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
