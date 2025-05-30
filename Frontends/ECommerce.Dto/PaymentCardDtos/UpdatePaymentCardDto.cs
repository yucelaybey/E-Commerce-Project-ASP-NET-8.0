using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.PaymentCardDtos
{
    public class UpdatePaymentCardDto
    {
        public string PaymentCardID { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV { get; set; }
        public bool IsDefault { get; set; }
    }
}
