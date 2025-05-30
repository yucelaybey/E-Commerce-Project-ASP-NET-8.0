using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Dto.PaymentCardDtos
{
    public class PaymentDataDto
    {
        /* Kredi Kartı (Yeni) */
        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("expiryDate")]
        public string ExpiryDate { get; set; }

        [JsonProperty("cvv")]
        public string CVV { get; set; }

        [JsonProperty("cardHolderName")]
        public string CardHolderName { get; set; }

        [JsonProperty("saveCard")]
        public bool SaveCard { get; set; }

        /* Kayıtlı Kart */
        [JsonProperty("cardId")]
        public string CardId { get; set; }

        /* PayPal/Apple Pay */
        [JsonProperty("paymentNumber")]
        public string PaymentNumber { get; set; }
    }
}
