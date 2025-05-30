namespace E_Commerce.Domain.Entities
{
    public class Order
    {
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; } // Siparişi veren kullanıcı
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string PaymentMethod { get; set; }
        public OrderPaymentCard OrderPaymentCard { get; set; }
        public OrderPaymentAddress OrderPaymentAddress { get; set; }
        public OrderPaymentOther OrderPaymentOther { get; set; }
        public int? OrderPaymentCardId { get; set; }
        public int? OrderPaymentOtherId { get; set; }
        public int OrderPaymentAddressId { get; set; }
        public int TotalAmount { get; set; } // Toplam tutar
        public string OrderStatus { get; set; } // Sipariş durumu
    }
}
