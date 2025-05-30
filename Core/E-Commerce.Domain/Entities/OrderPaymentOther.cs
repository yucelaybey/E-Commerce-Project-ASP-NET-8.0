using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class OrderPaymentOther
    {
        public int OrderPaymentOtherId { get; set; }
        public string PaymentName { get; set; }
        public string PaymentNumber { get; set; }
    }
}
