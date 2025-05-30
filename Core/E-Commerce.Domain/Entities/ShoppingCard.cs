namespace E_Commerce.Domain.Entities
{
    public class ShoppingCard
    {
        public int ShoppingCardID { get; set; }
        public string UserId { get; set; } // Kullanıcı adı ile ilişkilendirme
        public List<ShoppingCardItem> Items { get; set; }
    }
}
