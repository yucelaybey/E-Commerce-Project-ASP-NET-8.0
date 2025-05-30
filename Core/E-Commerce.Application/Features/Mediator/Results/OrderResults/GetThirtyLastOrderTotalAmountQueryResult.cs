namespace E_Commerce.Application.Features.Mediator.Results.OrderResults
{
    public class GetThirtyLastOrderTotalAmountQueryResult
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; } // Kredi Kartı veya Nakit
        public int TotalAmount { get; set; } // Toplam tutar
        public string OrderStatus { get; set; } // Sipariş durumu
    }
}
