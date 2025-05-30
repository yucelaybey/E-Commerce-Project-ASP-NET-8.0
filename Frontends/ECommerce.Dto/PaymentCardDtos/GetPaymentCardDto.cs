using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Dto.PaymentCardDtos
{
    public class GetPaymentCardDto
    {
        public int PaymentCardId { get; set; }
        public int UserId { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string CVV { get; set; }
        public bool IsDefault { get; set; }
    }
}
