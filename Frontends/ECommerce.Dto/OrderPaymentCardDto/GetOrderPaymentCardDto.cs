using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.OrderPaymentCardDto
{
    public class GetOrderPaymentCardDto
    {
        public int OrderPaymentCardId { get; set; }
        public string OrderCardNumber { get; set; }
        public string OrderCardName { get; set; }
        public string OrderCardCVV { get; set; }
        public DateTime OrderCardDate { get; set; }
    }
}
