using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Features.Mediator.Results.OrderPaymentCardResults
{
    public class GetOrderPaymentCardQueryResult
    {
        public int OrderPaymentCardId { get; set; }
        public string OrderCardNumber { get; set; }
        public string OrderCardName { get; set; }
        public string OrderCardCVV { get; set; }
        public DateTime OrderCardDate { get; set; }
    }
}
