namespace E_Commerce.Domain.Entities
{
    public class PaymentCard
    {
        public int PaymentCardID { get; set; }
        public int UserId { get; set; } // Kullanıcıya özel kayıt
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string CVV { get; set; }
        public bool IsDefault { get; set; } // Varsayılan kart
    }
}
