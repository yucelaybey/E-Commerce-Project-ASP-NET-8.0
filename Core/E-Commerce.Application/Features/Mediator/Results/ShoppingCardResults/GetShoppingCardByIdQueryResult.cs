namespace E_Commerce.Application.Features.Mediator.Results.ShoppingCardResults
{
    public class GetShoppingCardByIdQueryResult
    {
        public int ShoppingCardID { get; set; }
        public string UserId { get; set; } // Kullanıcı adı ile ilişkilendirme
    }
}
