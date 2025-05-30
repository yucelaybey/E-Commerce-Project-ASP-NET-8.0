using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.PaymentCardDtos
{
    public class CreatedPaymentCardResponseDto
    {
        public int PaymentCardId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
